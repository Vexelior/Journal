using System;
using System.Globalization;
using System.Text;
using Application.Repository;
using Domain.Models;

namespace Application.Services;

public class JournalExportService(IRepository<Journal> journalRepository)
{
    public byte[] GenerateTextExport(Journal journal)
    {
        var sb = new StringBuilder();
        var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(journal.Month);

        sb.AppendLine($"{monthName} {journal.Year} Journal");
        sb.AppendLine(new string('=', 50));
        sb.AppendLine();

        foreach (var entry in journal.JournalEntries.OrderBy(e => e.EntryDate))
        {
            sb.AppendLine($"Date: {entry.EntryDate:dddd, MMMM dd, yyyy}");
            sb.AppendLine(new string('-', 50));

            if (entry.Prompt != null)
            {
                sb.AppendLine($"Prompt: {entry.Prompt.Text}");
                sb.AppendLine();
            }

            sb.AppendLine(entry.Content);
            sb.AppendLine();
            sb.AppendLine(new string('=', 50));
            sb.AppendLine();
        }

        return Encoding.UTF8.GetBytes(sb.ToString());
    }
}
