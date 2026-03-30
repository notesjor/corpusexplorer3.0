using System.Collections.Generic;
using System.IO;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Scraper.Abstract;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace CorpusExplorer.Sdk.Extern.TextSharp.PDF
{
  public class TextSharpPdfSinglePageScraper : AbstractScraper
  {
    public enum TextSharpPdfScraperStrategy
    {
      Location,
      Simple
    }

    public override string DisplayName => "iTextSharp (single page)";

    public TextSharpPdfScraperStrategy Strategy { get; set; } = TextSharpPdfScraperStrategy.Location;

    protected override IEnumerable<Dictionary<string, object>> Execute(string file)
    {
      var res = new List<Dictionary<string, object>>();

      using (var pdfReader = new PdfReader(file))
      using (var pdfDocument = new PdfDocument(pdfReader))
      {
        for (var i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
        {
          var page = pdfDocument.GetPage(i);
          ITextExtractionStrategy strategy = Strategy == TextSharpPdfScraperStrategy.Simple
            ? new SimpleTextExtractionStrategy()
            : new LocationTextExtractionStrategy();

          var text = PdfTextExtractor.GetTextFromPage(page, strategy);

          if (!string.IsNullOrWhiteSpace(text))
          {
            res.Add(new Dictionary<string, object>
            {
              { "Text", text },
              { "Datei", file },
              { "Seite", i }
            });
          }
        }
      }

      return res;
    }
  }
}