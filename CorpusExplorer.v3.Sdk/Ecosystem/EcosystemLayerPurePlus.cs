using CorpusExplorer.Sdk.Ecosystem.Model;
using CorpusExplorer.Sdk.Extern.Json.SimpleStandoff;
using CorpusExplorer.Sdk.Extern.Json.Speedy;
using CorpusExplorer.Sdk.Extern.Plaintext.ClanChildes;
using CorpusExplorer.Sdk.Extern.Plaintext.LeipzigerWortschatz;
using CorpusExplorer.Sdk.Extern.Plaintext.Redewiedergabe;
using CorpusExplorer.Sdk.Extern.Plaintext.RelAnnis;
using CorpusExplorer.Sdk.Extern.Plaintext.Tsv;
using CorpusExplorer.Sdk.Extern.Xml.AnnotationPro;
using CorpusExplorer.Sdk.Extern.Xml.Bnc;
using CorpusExplorer.Sdk.Extern.Xml.Catma._5._0;
using CorpusExplorer.Sdk.Extern.Xml.Catma._6._0;
using CorpusExplorer.Sdk.Extern.Xml.CoraXml._0._8;
using CorpusExplorer.Sdk.Extern.Xml.CoraXml._1._0;
using CorpusExplorer.Sdk.Extern.Xml.Dewac;
using CorpusExplorer.Sdk.Extern.Xml.Dta.Tcf;
using CorpusExplorer.Sdk.Extern.Xml.Dta.Tcf2017;
using CorpusExplorer.Sdk.Extern.Xml.FnhdC;
using CorpusExplorer.Sdk.Extern.Xml.FoLiA;
using CorpusExplorer.Sdk.Extern.Xml.Folker.Fln;
using CorpusExplorer.Sdk.Extern.Xml.Ids.I5Xml;
using CorpusExplorer.Sdk.Extern.Xml.Ids.KorAP;
using CorpusExplorer.Sdk.Extern.Xml.Opus;
using CorpusExplorer.Sdk.Extern.Xml.Refi;
using CorpusExplorer.Sdk.Extern.Xml.SaltXml;
using CorpusExplorer.Sdk.Extern.Xml.SlashA;
using CorpusExplorer.Sdk.Extern.Xml.Tei.Dwds;
using CorpusExplorer.Sdk.Extern.Xml.Tiger.Importer;
using CorpusExplorer.Sdk.Extern.Xml.Txm;
using CorpusExplorer.Sdk.Extern.Xml.Weblicht;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Exporter;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Importer;
using CorpusExplorer.v3.Sdk.Ecosystem.Model;

namespace CorpusExplorer.v3.Sdk.Ecosystem;

public class EcosystemLayerPurePlus : AbstractEcosystemLayer
{
  public override void Initialize()
  {
    new EcosystemLayerPure().Initialize();

    #region Exporter

    // Configuration.AddExporter("CorpusExplorer v6 (*.cec6)|*.cec6", new ExporterCec6());
    // Configuration.AddExporter("CorpusExplorer v6 + LZ4 (*.cec6.lz4)|*.cec6.lz4", new ExporterCec6 { UseCompression = true });
    // Configuration.AddExporter("CorpusExplorer v6 + GZip (*.cec6.gz)|*.cec6.gz", new ExporterCec6 { UseCompression = true });
    Configuration.AddExporter("AnnotationPro (*.ant)|*.ant", new ExporterAnnoationPro());
    Configuration.AddExporter("CATMA 6 (*.xml/*.txt)|*", new ExporterCatma());
    Configuration.AddExporter("CoNLL (*.conll)|*.conll", new ExporterConll());
    Configuration.AddExporter("CoNLL + ZIP (*.zip)|*.zip", new ExporterConllZip());
    Configuration.AddExporter("CorpusWorkBench bis 2021(*.vrt)|*.vrt",
      new ExporterCorpusWorkBench { UseSentenceTag = false });
    Configuration.AddExporter("CorpusWorkBench bis 2021 inkl. Satzgrene (*.vrt, *.vrt.xml)|*.vrt;*.vrt.xml",
      new ExporterCorpusWorkBench { UseSentenceTag = true });
    Configuration.AddExporter("CorpusWorkBench ab 2022 (*.vrt, *.vrt.xml)|*.vrt;*.vrt.xml",
      new ExporterCorpusWorkBench2022 { UseSentenceTag = true });
    Configuration.AddExporter("CSV-Export [Metadaten + Wort-Layer] (*.csv)|*.csv", new ExporterCsv());
    Configuration.AddExporter("CSV-Export [Nur Metadatan] (*.csv)|*.csv", new ExporterCsvMetadataOnly());
    Configuration.AddExporter("DTA-TCF (*.tcf.xml)|*.tcf.xml", new ExporterDta());
    Configuration.AddExporter("DTA-TCF bis 2017 (*.tcf.xml)|*.tcf.xml", new ExporterDta2017());
    Configuration.AddExporter("DTA-TCF + ZIP (*.zip)|*.zip", new ExporterDtaZip());
    Configuration.AddExporter("DWDS TEI (*.xml)|*.xml", new ExporterDwdsTei());
    Configuration.AddExporter("FoLiA-XML (*.xml)|*.xml", new ExporterFolia());
    Configuration.AddExporter("Einfacher HTML-Export [Nur Wort-Layer] (*.txt)|*.txt", new ExporterHtmlPure());
    Configuration.AddExporter("IDS I5 (*.xml)|*.xml", new ExporterI5());
    Configuration.AddExporter("JSON-Export (*.json)|*.json", new ExporterJson());
    Configuration.AddExporter("JSON-Export [Nur Wort-Layer] (*.json)|*.json", new ExporterJsonPure());
    Configuration.AddExporter("JSON-Export + ZIP (*.zip)|*.zip", new ExporterJsonZip());
    Configuration.AddExporter("IDS KorAP (*.zip)|*.zip", new ExporterKorap());
    Configuration.AddExporter("XCES-XML (https://opus.nlpl.eu/) (*.xml)|*.xml", new ExporterOpusXces());
    Configuration.AddExporter("Plaintext-Export (*.txt)|*.txt", new ExporterPlaintext());
    Configuration.AddExporter("Plaintext-Export [Nur Wort-Layer] (*.txt)|*.txt", new ExporterPlaintextPure());
    Configuration.AddExporter("Plaintext-Export [Nur Wort-Layer - Eine Datei] (*.txt)|*.txt",
      new ExporterPlaintextPureInOneFile());
    Configuration.AddExporter("R-Freundlicher Korpus-Export (*.tsv)|*.tsv", new ExporterRFriendly());
    Configuration.AddExporter("Abfragen-Export [Nur für Schnappschüsse] (*.ceusd)|*.ceusd", new ExporterQuery());
    Configuration.AddExporter("SaltXML (*.xml, *.salt)|*.salt;*.xml", new ExporterSaltXml());
    Configuration.AddExporter("Sketch Engine VERT (*.vert)|*.vert", new ExporterSketchEngine());
    Configuration.AddExporter("Slash/A-XML (*.xml)|*.xml", new ExporterSlashA());
    Configuration.AddExporter("SPEEDy/CODEX (*.json)|*.json", new ExporterSpeedy());
    Configuration.AddExporter("TLV-XML-Export (*.xml)|*.xml", new ExporterTlv());
    Configuration.AddExporter("TreeTagger (*.treetagger)|*.treetagger", new ExporterTreeTagger());
    Configuration.AddExporter("TreeTagger + Satzgrenze (*.treetagger)|*.treetagger",
      new ExporterTreeTagger { UseSentenceTag = true });
    Configuration.AddExporter("TreeTagger + ZIP (*.zip)|*.zip", new ExporterTreeTaggerZip());
    Configuration.AddExporter("TXM TEI-XML (*.xml)|*.xml", new ExporterTxm());
    Configuration.AddExporter("TXM TEI-XML + ZIP (*.zip)|*.zip", new ExporterTxmZip());
    Configuration.AddExporter("WebLicht (*.xml)|*.xml", new ExporterWeblicht());

    #endregion

    #region Importer

    // Configuration.AddImporter("CorpusExplorer v6 (*.cec6, *.cec6.lz4, *.cec6.gz)|*.cec6;*.cec6.gz;*.cec6.lz4", new ImporterCec6());
    // Configuration.AddImporter("CorpusExplorer v6 [mit DRM geschützt] (*.cec6.drm)|*.cec6.drm", new ImporterCec6Drm());
    // Configuration.AddImporter("CorpusExplorer v6 [STREAM] (*.cec6)|*.cec6", new ImporterCec6Stream());
    Configuration.AddImporter("BNC TEI (*.xml)|*.xml", new ImporterBnc());
    Configuration.AddImporter("CATMA 5.0 (*.xml)|*.xml", new ImporterCatma());
    Configuration.AddImporter("CLAN/Childes (*.cex)|*.cex", new ImporterClanChildes());
    Configuration.AddImporter("CoNLL (*.conll)|*.conll", new ImporterConll());
    Configuration.AddImporter("CoraXML 0.8 (*.xml)|*.xml", new ImporterCoraXml08());
    Configuration.AddImporter("CoraXML 1.0 (*.xml)|*.xml", new ImporterCoraXml10());
    Configuration.AddImporter("CorpusWorkBench VRT (*.vrt)|*.vrt", new ImporterCorpusWorkBench());
    Configuration.AddImporter("DEWAC (DEWAC-*.xml)|DEWAC-*.xml", new ImporterDewac());
    Configuration.AddImporter("DTA-TCF (*.tcf.xml)|*.tcf.xml", new ImporterDta());
    Configuration.AddImporter("DTA-TCF bis 2017 (*.tcf.xml)|*.tcf.xml", new ImporterDta2017());
    Configuration.AddImporter("FnhdC (*.xml)|*.xml", new ImporterFnhdC());
    Configuration.AddImporter("FoLiA-XML (*.xml)|*.xml", new ImporterFolia());
    Configuration.AddImporter("FOLKER / OrthoNormal (*.fln)|*.fln", new ImporterFolkerFln());
    Configuration.AddImporter("IDS KorAP (*.zip)|*.zip", new ImporterKorap());
    Configuration.AddImporter("IDS KorAP ab 2021 (*.zip)|*.zip", new ImporterKorap2021());
    Configuration.AddImporter("Wortschatz Leipzig (*-sentences.txt)|*-sentences.txt",
      new ImporterLeipzigerWortschatz());
    Configuration.AddImporter("XCES-XML (https://opus.nlpl.eu/) (*.xml)|*.xml", new ImporterOpusXces());
    Configuration.AddImporter("Projekt: http://www.redewiedergabe.de/ (metadata.tsv)|metadata.tsv",
      new ImporterRedewiedergabe());
    Configuration.AddImporter("Rotterdam Exchange Format Initiative (*.qdpx, *.zip)|*.qdpx;*.zip", new ImporterRefi());
    Configuration.AddImporter("ANNIS v3.3 (corpus.annis)|corpus.annis", new ImporterRelAnnis());
    Configuration.AddImporter("ANNIS v3.3 (corpus.annis) - Layer-Alternative|corpus.annis",
      new ImporterRelAnnisAlternative());
    Configuration.AddImporter("SaltXML (*.xml, *.salt)|*.salt;*.xml", new ImporterSaltXml());
    Configuration.AddImporter("JSON Stand-Off Universal (*.json)|*.json", new ImporterSimpleJsonStandoff());
    Configuration.AddImporter("Sketch Engine VERT (*.vert)|*.vert", new ImporterSketchEngine());
    Configuration.AddImporter("SPEEDy/CODEX (*.json)|*.json", new ImporterSpeedy());
    Configuration.AddImporter("TiGER-XML (*.xml)|*.xml", new ImporterTiger());
    Configuration.AddImporter("TLV-XML (*.xml)|*.xml", new ImporterTlv());
    Configuration.AddImporter("TreeTagger (*.txt)|*.txt", new ImporterTreeTagger());
    Configuration.AddImporter("Universelles TSV-Korpus (*.tsv)|*.tsv", new ImporterTsv());
    Configuration.AddImporter("TXM TEI-XML (*.xml)|*.xml", new ImporterTxm());
    Configuration.AddImporter("WebLicht (*.xml)|*.xml", new ImporterWeblicht());

    #endregion
  }
}