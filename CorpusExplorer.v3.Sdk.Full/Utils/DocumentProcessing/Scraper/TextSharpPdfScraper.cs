using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Scraper.Abstract;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace CorpusExplorer.Sdk.Extern.TextSharp.PDF
{
  public class TextSharpPdfScraper : AbstractScraper
  {
    public enum TextSharpPdfScraperStrategy
    {
      Location,
      Simple
    }

    public override string DisplayName => "iText";

    public ITextExtractionStrategy Strategy { get; set; }

    protected override IEnumerable<Dictionary<string, object>> Execute(string file)
    {
      using (var pdfReader = new PdfReader(file))
      using (var pdfDocument = new PdfDocument(pdfReader))
      {
        var output = new StringBuilder();

        for (var i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
        {
          var page = pdfDocument.GetPage(i);
          var strategy = Strategy ?? new SimpleTextExtractionStrategy();
          var text = PdfTextExtractor.GetTextFromPage(page, strategy);
          output.AppendLine(text);
        }

        var res = new Dictionary<string, object> { { "Text", output.ToString() }, { "Datei", file } };
        var info = pdfDocument.GetDocumentInfo();
        res.Add("Autor", info.GetAuthor());
        res.Add("Titel", info.GetTitle());
        res.Add("Ersteller", info.GetCreator());
        res.Add("Schlagworte", info.GetKeywords());
        res.Add("Betreff", info.GetSubject());

        return new[] { res };
      }
    }
  }
}