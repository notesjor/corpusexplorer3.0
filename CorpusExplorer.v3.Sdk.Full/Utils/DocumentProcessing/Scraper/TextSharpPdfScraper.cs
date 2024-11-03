using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CorpusExplorer.Sdk.Extern.Xml.Exmaralda.Simple.Model;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Scraper.Abstract;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace CorpusExplorer.Sdk.Extern.TextSharp.PDF
{
  public class TextSharpPdfScraper : AbstractScraper
  {
    public override string DisplayName => "iTextSharp";

    public ITextExtractionStrategy Strategy { get; set; } = new SimpleTextExtractionStrategy();

    protected override IEnumerable<Dictionary<string, object>> Execute(string file)
    {
      using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
      using (var bs = new BufferedStream(fs))
      {
        var pdfDocument = new PdfDocument(new PdfReader(bs));

        var output = new StringBuilder();
        for (var i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
        {
          var page = pdfDocument.GetPage(i);
          var text = PdfTextExtractor.GetTextFromPage(page, Strategy);
          output.AppendLine(text);
        }

        var res = new Dictionary<string, object> { { "Text", output.ToString() }, { "Datei", file } };
        var info = pdfDocument.GetDocumentInfo();
        res.Add("Autor", info.GetAuthor());
        res.Add("Titel", info.GetTitle());

        return new[] { res };
      }
    }
  }
}