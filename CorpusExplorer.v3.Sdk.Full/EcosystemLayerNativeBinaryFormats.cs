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
      Configuration.AddScraper("YouTube-JSON (*.json)|*.json", new YouTubeScraper());
      Configuration.AddScraper("RSS-Feed (*.xml)|*.xml", new RssFeedItemScraper());
      Configuration.AddExporter("XML-Export (*.xml)|*.xml", new ExporterXml());
      Configuration.AddScraper("Mediawiki/Wikipedia-DUMP (*.xml)|*.xml", new WikipediaScraper());
      Configuration.AddScraper("Mediawiki/Wikipedia-TALK-DUMP (*.xml)|*.xml", new WikipediaMetaTalkScraper());

    }
  }
}
