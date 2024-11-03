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
  public class TextSharpPdfSinglePageScraper : AbstractScraper
  {
    public override string DisplayName => "iTextSharp (single page)";

    public ITextExtractionStrategy Strategy { get; set; } = new SimpleTextExtractionStrategy();

    protected override IEnumerable<Dictionary<string, object>> Execute(string file)
    {
      var res = new List<Dictionary<string, object>>();

      using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
      using (var bs = new BufferedStream(fs))
      {
        var pdfDocument = new PdfDocument(new PdfReader(bs));
        for (var i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
        {
          var page = pdfDocument.GetPage(i);

          var text = PdfTextExtractor.GetTextFromPage(page, Strategy);

          if (!string.IsNullOrWhiteSpace(text))
            res.Add(new Dictionary<string, object>
              {
                {"Text", text},
                {"Datei", file},
                {"Seite", i }
              });
        }

        return res;
      }
    }
  }
}