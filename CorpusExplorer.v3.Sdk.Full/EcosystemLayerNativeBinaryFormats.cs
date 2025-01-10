using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpusExplorer.Core.Utils.DocumentProcessing.Scraper.Cosmas;
using CorpusExplorer.Core.Utils.DocumentProcessing.Scraper.Docx;
using CorpusExplorer.Core.Utils.DocumentProcessing.Scraper.Html;
using CorpusExplorer.Core.Utils.DocumentProcessing.Scraper.Pdf;
using CorpusExplorer.Core.Utils.DocumentProcessing.Scraper.Rtf;
using CorpusExplorer.Sdk.Ecosystem.Model;
using CorpusExplorer.Sdk.Extern.Binary.Excel.Kidko;
using CorpusExplorer.Sdk.Extern.Binary.Excel.Universal;
using CorpusExplorer.Sdk.Extern.Binary.NexisNew;
using CorpusExplorer.Sdk.Extern.Epub;
using CorpusExplorer.Sdk.Extern.Pandoc.Pandoc;
using CorpusExplorer.Sdk.Extern.Plaintext.RawMailMsg;
using CorpusExplorer.Sdk.Extern.Plaintext.WET;
using CorpusExplorer.Sdk.Extern.SocialMedia.Blogger.Data;
using CorpusExplorer.Sdk.Extern.SocialMedia.Rss.Format;
using CorpusExplorer.Sdk.Extern.SocialMedia.Tumblr.Data;
using CorpusExplorer.Sdk.Extern.SocialMedia.Youtube;
using CorpusExplorer.Sdk.Extern.TextSharp.APAEK;
using CorpusExplorer.Sdk.Extern.TextSharp.PDF;
using CorpusExplorer.Sdk.Extern.Wiki.Wikipedia;
using CorpusExplorer.Sdk.Extern.Wiki.WikipediaMetaTalk;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Exporter;
using CorpusExplorer.v3.Sdk.Ecosystem.Model;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace CorpusExplorer.v3.Sdk.Full
{
  public class EcosystemLayerNativeBinaryFormats : AbstractEcosystemLayer
  {
    public override void Initialize()
    {
      Configuration.AddScraper("COSMAS-TXT ab v4 (*.rtf)|*.rtf", new CosmasScraper());
      Configuration.AddScraper("Nur Text (*.docx; *.doc)|*.docx;*.doc", new SimpleDocxDocumentScraper());
      Configuration.AddScraper("Nur Text (*.rtf)|*.rtf", new SimpleRtfDocumentScraper());
      Configuration.AddScraper("Nur Text (*.html)|*.htm;*.html", new SimpleHtmlDocumentScraper());
      Configuration.AddScraper("Nur Text (*.pdf)|*.pdf", new SimplePdfDocumentScraper());
      //Configuration.AddScraper("CorpusExplorer Rohdaten (*.sdd)|*.sdd", new ListOfScrapDocumentScraper());
      Configuration.AddScraper("Universeller Excel™-Scraper (*.xlsx)|*.xlsx", new UniversalExcelScraper());
      Configuration.AddScraper("Nexis/Uni / Lexis-Nexis (ab 2019) - [Vollständig + MS Word (docx) + separate Dateien] (*.zip)|*.zip", new NexisNewScraper());
      Configuration.AddScraper("KiDKo/E-Scraper (*.xlsx)|*.xlsx", new KidkoScraper());
      Configuration.AddImporter("Universeller Excel™-Importer (*.xlsx)|*.xlsx", new ImporterUniversalExcel());
      Configuration.AddScraper("EPUB-eBook (*.epub)|*.epub", new EpubScraper());
      Configuration.AddScraper("Nur Text (*.pdf - via iTextSharp (simple))|*.pdf", new TextSharpPdfScraper());
      Configuration.AddScraper("Nur Text (*.pdf - via iTextSharp (location))|*.pdf", new TextSharpPdfScraper { Strategy = new LocationTextExtractionStrategy() });
      Configuration.AddScraper("Nur Text (*.pdf - via iTextSharp (glyph))|*.pdf", new TextSharpPdfScraper { Strategy = new GlyphTextEventListener(new LocationTextExtractionStrategy()) });
      Configuration.AddScraper("Nur Text (*.pdf - via iTextSharp (filter))|*.pdf", new TextSharpPdfScraper { Strategy = new FilteredTextEventListener(new LocationTextExtractionStrategy()) });
      Configuration.AddScraper("APAEK-Unterrichtstranskript (*.pdf)|*.pdf", new ApaekScraper());
      Configuration.AddScraper("E-Mail MIME-Format (*.msg; *.eml)|*.msg;*.eml", new RawMsgMsgScraper());
      Configuration.AddScraper("WET-Format http://commoncrawl.org (*.warc.wet)|*.warc.wet", new WetScraper());
      Configuration.AddScraper("Blogger-XML (*.xml)|*.xml", new BloggerScraper());
      Configuration.AddScraper("Tumblr-XML (*.xml)|*.xml", new TumblrScraper());
      Configuration.AddScraper("RSS-Feed (*.xml)|*.xml", new RssFeedItemScraper());
      Configuration.AddExporter("XML-Export (*.xml)|*.xml", new ExporterXml());
      Configuration.AddScraper("Mediawiki/Wikipedia-DUMP (*.xml)|*.xml", new WikipediaScraper());
      Configuration.AddScraper("Mediawiki/Wikipedia-TALK-DUMP (*.xml)|*.xml", new WikipediaMetaTalkScraper());
      Configuration.AddScraper("PANDOC [commonmark] (*.txt; *.*)|*.*", new PandocScraper { Format = PandocScraper.InputFormat.commonmark });
      Configuration.AddScraper("PANDOC [docbook] (*.docbook; *.xml; *.*)|*.*", new PandocScraper { Format = PandocScraper.InputFormat.docbook });
      Configuration.AddScraper("PANDOC [Microsoft Word] (*.docx)|*.docx", new PandocScraper { Format = PandocScraper.InputFormat.docx });
      Configuration.AddScraper("PANDOC [epub] (*.epub)|*.epub", new PandocScraper { Format = PandocScraper.InputFormat.epub });
      Configuration.AddScraper("PANDOC [haddock] (*.txt; *.*)|*.*", new PandocScraper { Format = PandocScraper.InputFormat.haddock });
      Configuration.AddScraper("PANDOC [html] (*.html)|*.html", new PandocScraper { Format = PandocScraper.InputFormat.html });
      Configuration.AddScraper("PANDOC [json] (*.json)|*.json", new PandocScraper { Format = PandocScraper.InputFormat.json });
      Configuration.AddScraper("PANDOC [LaTeX] (*.tex; *.latex; *.*)|*.*", new PandocScraper { Format = PandocScraper.InputFormat.latex });
      Configuration.AddScraper("PANDOC [markdown] (*.txt; *.*)|*.*", new PandocScraper { Format = PandocScraper.InputFormat.markdown });
      Configuration.AddScraper("PANDOC [markdown - github] (*.txt; *.*)|*.*", new PandocScraper { Format = PandocScraper.InputFormat.markdown_github });
      Configuration.AddScraper("PANDOC [markdown - mnd] (*.txt; *.*)|*.*", new PandocScraper { Format = PandocScraper.InputFormat.markdown_mmd });
      Configuration.AddScraper("PANDOC [markdown - phpextra] (*.txt; *.*)|*.*", new PandocScraper { Format = PandocScraper.InputFormat.markdown_phpextra });
      Configuration.AddScraper("PANDOC [markdown - strict] (*.txt; *.*)|*.*", new PandocScraper { Format = PandocScraper.InputFormat.markdown_strict });
      Configuration.AddScraper("PANDOC [wikipedia / mediawiki] (*.txt; *.*)|*.*", new PandocScraper { Format = PandocScraper.InputFormat.mediawiki });
      Configuration.AddScraper("PANDOC [native] (*.txt; *.*)|*.*", new PandocScraper { Format = PandocScraper.InputFormat.native });
      Configuration.AddScraper("PANDOC [OpenOffice / LibreOffice] (*.odt)|*.odt", new PandocScraper { Format = PandocScraper.InputFormat.odt });
      Configuration.AddScraper("PANDOC [opml] (*.opml; *.*)|*.*", new PandocScraper { Format = PandocScraper.InputFormat.opml });
      Configuration.AddScraper("PANDOC [org] (*.txt; *.*)|*.*", new PandocScraper { Format = PandocScraper.InputFormat.org });
      Configuration.AddScraper("PANDOC [rst] (*.rst; *.*)|*.*", new PandocScraper { Format = PandocScraper.InputFormat.rst });
      Configuration.AddScraper("PANDOC [t2t] (*.t2t; *.*)|*.*", new PandocScraper { Format = PandocScraper.InputFormat.t2t });
      Configuration.AddScraper("PANDOC [textile] (*.txt; *.*)|*.*", new PandocScraper { Format = PandocScraper.InputFormat.textile });
      Configuration.AddScraper("PANDOC [twiki] (*.txt; *.*)|*.*", new PandocScraper { Format = PandocScraper.InputFormat.twiki });
    }
  }
}
