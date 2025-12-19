using System.Globalization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Document = DocumentFormat.OpenXml.Wordprocessing.Document;

namespace Application.Services;

public class DocumentExportService
{
    public byte[] GenerateDocx(Journal journal)
    {
        using var memoryStream = new MemoryStream();
        using (var document = WordprocessingDocument.Create(memoryStream, WordprocessingDocumentType.Document))
        {
            var mainPart = document.AddMainDocumentPart();
            mainPart.Document = new Document();
            var body = mainPart.Document.AppendChild(new Body());

            var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(journal.Month);

            // Title
            var titleParagraph = body.AppendChild(new Paragraph());
            var titleRun = titleParagraph.AppendChild(new Run());
            var titleProperties = titleRun.AppendChild(new RunProperties());
            titleProperties.AppendChild(new Bold());
            titleProperties.AppendChild(new FontSize { Val = "32" });
            titleRun.AppendChild(new Text($"{monthName} {journal.Year} Journal"));

            body.AppendChild(new Paragraph(new Run(new Break())));

            // Entries
            foreach (var entry in journal.JournalEntries.OrderBy(e => e.EntryDate))
            {
                // Date header
                var dateParagraph = body.AppendChild(new Paragraph());
                var dateRun = dateParagraph.AppendChild(new Run());
                var dateProperties = dateRun.AppendChild(new RunProperties());
                dateProperties.AppendChild(new Bold());
                dateProperties.AppendChild(new FontSize { Val = "24" });
                dateRun.AppendChild(new Text(entry.EntryDate.ToString("dddd, MMMM dd, yyyy")));

                // Prompt (if exists)
                if (entry.Prompt != null)
                {
                    var promptParagraph = body.AppendChild(new Paragraph());
                    var promptRun = promptParagraph.AppendChild(new Run());
                    var promptProperties = promptRun.AppendChild(new RunProperties());
                    promptProperties.AppendChild(new Italic());
                    promptProperties.AppendChild(new DocumentFormat.OpenXml.Office2013.Word.Color { Val = "808080" });
                    promptRun.AppendChild(new Text($"Prompt: {entry.Prompt.Text}"));
                }

                body.AppendChild(new Paragraph(new Run(new Break())));

                // Content
                var contentLines = entry.Content.Split('\n');
                foreach (var line in contentLines)
                {
                    var contentParagraph = body.AppendChild(new Paragraph());
                    var contentRun = contentParagraph.AppendChild(new Run());
                    contentRun.AppendChild(new Text(line));
                }

                // Separator
                body.AppendChild(new Paragraph(new Run(new Break())));
                var separatorParagraph = body.AppendChild(new Paragraph());
                var separatorRun = separatorParagraph.AppendChild(new Run());
                separatorRun.AppendChild(new Text("___________________________________________"));
                body.AppendChild(new Paragraph(new Run(new Break())));
            }

            mainPart.Document.Save();
        }

        return memoryStream.ToArray();
    }

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

                                // Content
                                entryColumn.Item()
                                    .PaddingTop(0.3f, Unit.Centimetre)
                                    .Text(entry.Content)
                                    .LineHeight(1.5f);

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
}