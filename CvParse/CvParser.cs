using ClinicWebApi.Models;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System.Text.RegularExpressions;

namespace ClinicWebApi.CvParse
{
    public class CvParser
    {
        public List<ExperienceEntry> ExtractExperience(byte[] fileBytes)
        {
            // Extract text from the PDF file
            string extractedText = ExtractTextFromPdf(fileBytes);

            // Extract and structure experience data
            return ExtractExperienceFromText(extractedText);
        }

        private string ExtractTextFromPdf(byte[] fileBytes)
        {
            using var memoryStream = new MemoryStream(fileBytes);
            var pdfReader = new PdfReader(memoryStream);
            var pdfDocument = new PdfDocument(pdfReader);

            var textBuilder = new System.Text.StringBuilder();

            for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
            {
                var page = pdfDocument.GetPage(i);
                textBuilder.Append(PdfTextExtractor.GetTextFromPage(page));
            }

            return textBuilder.ToString();
        }

        private List<ExperienceEntry> ExtractExperienceFromText(string text)
        {
            // Split the text into sections for each role
            var experienceBlocks = Regex.Split(text, @"\n\s*(?=[A-Z\s]+\s{3,})");

            var experiences = new List<ExperienceEntry>();

            foreach (var block in experienceBlocks)
            {
                var lines = block.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToList();
                if (lines.Count < 2) continue;

                // Extract company name and position
                var organization = lines[0].Trim();
                var positionLine = lines[1].Trim();

                // Extract start and end dates
                var dateRegex = new Regex(@"\b(\d{4}) - (present|\d{4})", RegexOptions.IgnoreCase);
                var dateMatch = dateRegex.Match(positionLine);

                if (!dateMatch.Success) continue;

                var startDate = dateMatch.Groups[1].Value;
                var endDate = dateMatch.Groups[2].Value.ToLower() == "present" ? "დღემდე" : dateMatch.Groups[2].Value;

                // Extract position (removing dates)
                var position = positionLine.Replace(dateMatch.Value, "").Trim();

                // Add to the list
                experiences.Add(new ExperienceEntry
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    Position = position,
                    Organization = organization
                });
            }

            return experiences;
        }
    }
}
