using System.Globalization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Models;
using HtmlAgilityPack;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Document = DocumentFormat.OpenXml.Wordprocessing.Document;

namespace Application.Services;

public class DocumentExportService
{
    public byte[] GeneratePdf(Journal journal)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(journal.Month);

        var document = QuestPDF.Fluent.Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                page.Header()
                    .Text($"{monthName} {journal.Year} Journal")
                    .FontSize(20)
                    .Bold()
                    .AlignCenter();

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        foreach (var entry in journal.JournalEntries.OrderBy(e => e.EntryDate))
                        {
                            column.Item().PaddingBottom(0.5f, Unit.Centimetre).Column(entryColumn =>
                            {
                                // Date
                                entryColumn.Item()
                                    .Text(entry.EntryDate.ToString("dddd, MMMM dd, yyyy"))
                                    .FontSize(14)
                                    .Bold();

                                // Prompt
                                if (entry.Prompt != null)
                                {
                                    entryColumn.Item()
                                        .PaddingTop(0.2f, Unit.Centimetre)
                                        .Text($"Prompt: {entry.Prompt.Text}")
                                        .FontSize(10)
                                        .Italic()
                                        .FontColor(Colors.Grey.Darken2);
                                }

                                // Content with HTML rendering
                                entryColumn.Item()
                                    .PaddingTop(0.3f, Unit.Centimetre)
                                    .Column(contentColumn =>
                                    {
                                        RenderHtmlContent(contentColumn, entry.Content);
                                    });

                                // Separator
                                entryColumn.Item()
                                    .PaddingTop(0.5f, Unit.Centimetre)
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Grey.Lighten2);
                            });
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" of ");
                        x.TotalPages();
                    });
            });
        });

        using var memoryStream = new MemoryStream();
        document.GeneratePdf(memoryStream);
        return memoryStream.ToArray();
    }

    private void RenderHtmlContent(ColumnDescriptor column, string htmlContent)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(htmlContent);

        RenderNode(column, doc.DocumentNode);
    }

    private void RenderNode(ColumnDescriptor column, HtmlNode node)
    {
        foreach (var child in node.ChildNodes)
        {
            if (child.NodeType == HtmlNodeType.Text)
            {
                var content = HtmlEntity.DeEntitize(child.InnerText);
                if (!string.IsNullOrWhiteSpace(content))
                {
                    column.Item().Text(content).LineHeight(1.5f);
                }
            }
            else if (child.NodeType == HtmlNodeType.Element)
            {
                var styleAttr = child.GetAttributeValue("style", string.Empty);
                var styles = ParseStyleAttribute(styleAttr);

                switch (child.Name.ToLower())
                {
                    case "strong":
                    case "b":
                        RenderStyledText(column, child, text => ApplyStyles(text.Bold(), styles));
                        break;

                    case "em":
                    case "i":
                        RenderStyledText(column, child, text => ApplyStyles(text.Italic(), styles));
                        break;

                    case "u":
                        RenderStyledText(column, child, text => ApplyStyles(text.Underline(), styles));
                        break;

                    case "br":
                        column.Item().Text(" ");
                        break;

                    case "p":
                        var pItem = column.Item().PaddingBottom(0.3f, Unit.Centimetre);
                        pItem = ApplyAlignment(pItem, styles);
                        pItem.Column(p =>
                        {
                            RenderNode(p, child);
                        });
                        break;

                    case "ul":
                    case "ol":
                        column.Item().Column(listColumn =>
                        {
                            RenderList(listColumn, child);
                        });
                        break;

                    case "h1":
                    case "h2":
                    case "h3":
                    case "h4":
                    case "h5":
                    case "h6":
                        var hItem = column.Item().PaddingTop(0.2f, Unit.Centimetre);
                        hItem = ApplyAlignment(hItem, styles);
                        hItem.Text(text =>
                        {
                            ApplyStyles(text.Span(GetTextContent(child)).FontSize(14).Bold().LineHeight(1.5f), styles);
                        });
                        break;
                    case "a":
                        var href = child.GetAttributeValue("href", string.Empty);
                        var linkText = GetTextContent(child);
                        if (!string.IsNullOrWhiteSpace(linkText))
                        {
                            column.Item().Text(text =>
                            {
                                var span = !string.IsNullOrWhiteSpace(href)
                                    ? text.Hyperlink(linkText, href).FontColor(Colors.Blue.Medium).Underline()
                                    : text.Span(linkText).FontColor(Colors.Blue.Medium).Underline();
                                ApplyStyles(span, styles);
                            });
                        }
                        break;
                    case "blockquote":
                        column.Item().PaddingLeft(0.5f, Unit.Centimetre).BorderLeft(2).BorderColor(Colors.Grey.Lighten2).Column(bq =>
                        {
                            RenderNode(bq, child);
                        });
                        break;
                    case "pre":
                        var preContent = GetTextContent(child);
                        if (!string.IsNullOrWhiteSpace(preContent))
                        {
                            column.Item().Background(Colors.Grey.Lighten4).Padding(5).Text(preContent).FontFamily("Consolas").FontSize(10).LineHeight(1.2f);
                        }
                        break;
                    case "span":
                        RenderStyledText(column, child, text => ApplyStyles(text, styles));
                        break;
                    case "table":
                        column.Item().Table(table =>
                        {
                            RenderTable(table, child);
                        });
                        break;
                    case "div":
                        var divItem = column.Item();
                        divItem = ApplyAlignment(divItem, styles);
                        divItem.Column(divColumn =>
                        {
                            RenderNode(divColumn, child);
                        });
                        break;
                    default:
                        RenderNode(column, child);
                        break;
                }
            }
        }
    }

    private IContainer ApplyAlignment(IContainer container, Dictionary<string, string> styles)
    {
        if (styles.TryGetValue("text-align", out var alignment))
        {
            return alignment.ToLower() switch
            {
                "center" => container.AlignCenter(),
                "right" => container.AlignRight(),
                "left" => container.AlignLeft(),
                "justify" => container.AlignLeft(),
                _ => container
            };
        }
        return container;
    }

    private void RenderTable(TableDescriptor table, HtmlNode tableNode)
    {
        var rows = tableNode.SelectNodes(".//tr");
        if (rows == null || rows.Count == 0)
            return;

        var firstRow = rows[0];
        var cellCount = firstRow.SelectNodes(".//td | .//th")?.Count ?? 0;
        if (cellCount == 0)
            return;

        table.ColumnsDefinition(columns =>
        {
            for (int i = 0; i < cellCount; i++)
            {
                columns.RelativeColumn();
            }
        });

        foreach (var row in rows)
        {
            var cells = row.SelectNodes(".//td | .//th");
            if (cells == null)
                continue;

            foreach (var cell in cells)
            {
                var isHeader = cell.Name.ToLower() == "th";
                var cellStyles = ParseStyleAttribute(cell.GetAttributeValue("style", string.Empty));

                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Column(cellColumn =>
                {
                    if (isHeader)
                    {
                        cellColumn.Item().Text(GetTextContent(cell)).Bold().FontSize(11);
                    }
                    else
                    {
                        RenderNode(cellColumn, cell);
                    }
                });
            }
        }
    }

    private Dictionary<string, string> ParseStyleAttribute(string styleAttr)
    {
        var styles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        if (string.IsNullOrWhiteSpace(styleAttr))
            return styles;

        var declarations = styleAttr.Split(';', StringSplitOptions.RemoveEmptyEntries);
        foreach (var declaration in declarations)
        {
            var parts = declaration.Split(':', 2);
            if (parts.Length == 2)
            {
                styles[parts[0].Trim()] = parts[1].Trim();
            }
        }

        return styles;
    }

    private TextSpanDescriptor ApplyStyles(TextSpanDescriptor text, Dictionary<string, string> styles)
    {
        if (styles.Count == 0)
            return text;

        if (styles.TryGetValue("color", out var color))
        {
            var parsedColor = ParseColor(color);
            if (parsedColor != null)
            {
                text = text.FontColor(parsedColor);
            }
        }

        if (styles.TryGetValue("background-color", out var bgColor))
        {
            var parsedBgColor = ParseColor(bgColor);
            if (parsedBgColor != null)
            {
                text = text.BackgroundColor(parsedBgColor);
            }
        }

        if (styles.TryGetValue("font-size", out var fontSize))
        {
            if (float.TryParse(fontSize.Replace("px", "").Replace("pt", "").Trim(), out var size))
            {
                text = text.FontSize(size);
            }
        }

        if (styles.TryGetValue("font-weight", out var fontWeight))
        {
            if (fontWeight.Equals("bold", StringComparison.OrdinalIgnoreCase) ||
                (int.TryParse(fontWeight, out var weight) && weight >= 700))
            {
                text = text.Bold();
            }
        }

        if (styles.TryGetValue("font-style", out var fontStyle))
        {
            if (fontStyle.Equals("italic", StringComparison.OrdinalIgnoreCase))
            {
                text = text.Italic();
            }
        }

        if (styles.TryGetValue("text-decoration", out var textDecoration))
        {
            if (textDecoration.Contains("underline", StringComparison.OrdinalIgnoreCase))
            {
                text = text.Underline();
            }
        }

        return text;
    }

    private string? ParseColor(string colorValue)
    {
        if (string.IsNullOrWhiteSpace(colorValue))
            return null;

        colorValue = colorValue.Trim().ToLower();

        if (colorValue.StartsWith('#'))
        {
            return colorValue;
        }

        if (colorValue.StartsWith("rgb"))
        {
            return ConvertRgbToHex(colorValue);
        }

        return colorValue switch
        {
            "red" => "#FF0000",
            "blue" => "#0000FF",
            "green" => "#008000",
            "black" => "#000000",
            "white" => "#FFFFFF",
            "gray" or "grey" => "#808080",
            "yellow" => "#FFFF00",
            "orange" => "#FFA500",
            "purple" => "#800080",
            "pink" => "#FFC0CB",
            _ => null
        };
    }

    private string? ConvertRgbToHex(string rgb)
    {
        var match = System.Text.RegularExpressions.Regex.Match(rgb, @"rgba?\((\d+),\s*(\d+),\s*(\d+)(?:,\s*([\d.]+))?\)");

        if (!match.Success)
            return null;

        if (!int.TryParse(match.Groups[1].Value, out var r) || r < 0 || r > 255)
            return null;
        if (!int.TryParse(match.Groups[2].Value, out var g) || g < 0 || g > 255)
            return null;
        if (!int.TryParse(match.Groups[3].Value, out var b) || b < 0 || b > 255)
            return null;

        if (match.Groups[4].Success && float.TryParse(match.Groups[4].Value, out var alpha))
        {
            var a = (int)(alpha * 255);
            return $"#{a:X2}{r:X2}{g:X2}{b:X2}";
        }

        return $"#{r:X2}{g:X2}{b:X2}";
    }

    private void RenderStyledText(ColumnDescriptor column, HtmlNode node, Func<TextSpanDescriptor, TextSpanDescriptor> styleFunc)
    {
        var content = GetTextContent(node);
        if (!string.IsNullOrWhiteSpace(content))
        {
            column.Item().Text(text =>
            {
                styleFunc(text.Span(content));
            });
        }
    }

    private void RenderList(ColumnDescriptor column, HtmlNode listNode)
    {
        foreach (var li in listNode.ChildNodes.Where(n => n.Name.ToLower() == "li"))
        {
            column.Item().Row(row =>
            {
                row.ConstantItem(20).Text("•");
                row.RelativeItem().Text(GetTextContent(li)).LineHeight(1.5f);
            });
        }
    }

    private string GetTextContent(HtmlNode node)
    {
        return HtmlEntity.DeEntitize(node.InnerText);
    }
}