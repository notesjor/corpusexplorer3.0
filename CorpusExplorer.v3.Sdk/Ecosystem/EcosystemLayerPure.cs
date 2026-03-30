using CorpusExplorer.Sdk._3rdParty.Commercial.Metaphone3.Action;
using CorpusExplorer.Sdk.Action;
using CorpusExplorer.Sdk.Ecosystem.Model;
using CorpusExplorer.Sdk.Extern.FuzzyCloneDetection.Action;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Builder;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Exporter;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Importer.CorpusExplorerV6;
using CorpusExplorer.v3.Sdk.Ecosystem.Model;
using CorpusExplorer.Sdk.Blocks.ReadingEase;
using CorpusExplorer.Sdk.Blocks.Cooccurrence;
using CorpusExplorer.Sdk.Blocks.Measure;
using CorpusExplorer.Sdk.Blocks.Similarity;
using CorpusExplorer.Sdk.Blocks.VocabularyComplaxity;
using CorpusExplorer.Sdk.Utils.DataTableWriter;

namespace CorpusExplorer.v3.Sdk.Ecosystem
{
  public class EcosystemLayerPure : AbstractEcosystemLayer
  {
    public override void Initialize()
    {
      #region Backend
      Configuration.AddBackend("CorpusExplorer v6", new CorpusBuilderWriteDirect());
      #endregion

      #region ConsoleActions
      Configuration.AddConsoleAction(new AddTaggerAction());
      Configuration.AddConsoleAction(new BasicInformationAction());
      Configuration.AddConsoleAction(new BurrowsDeltaAction());
      Configuration.AddConsoleAction(new CentralSnipAction());
      Configuration.AddConsoleAction(new CentralSnipTreeAction());
      Configuration.AddConsoleAction(new ClusterAction());
      Configuration.AddConsoleAction(new ClusterListAction());
      Configuration.AddConsoleAction(new ConvertAction());
      Configuration.AddConsoleAction(new CooccurrenceAction());
      Configuration.AddConsoleAction(new CooccurrenceClassicAction());
      Configuration.AddConsoleAction(new CooccurrenceCorrespondingAction());
      Configuration.AddConsoleAction(new CooccurrenceCrossAction());
      Configuration.AddConsoleAction(new CooccurrenceCrossFullAction());
      Configuration.AddConsoleAction(new CooccurrenceDiversityAction());
      Configuration.AddConsoleAction(new CooccurrenceNetworkAction());
      Configuration.AddConsoleAction(new CooccurrenceNetworkSelectAction());
      Configuration.AddConsoleAction(new CooccurrencePolarisationAction());
      Configuration.AddConsoleAction(new CooccurrenceProfileAction());
      Configuration.AddConsoleAction(new CooccurrenceSelectedAction());
      Configuration.AddConsoleAction(new CooccurrenceSelectedCorrespondingAction());
      Configuration.AddConsoleAction(new CorpusFiniteStateMachineAction());
      Configuration.AddConsoleAction(new CorrespondingMetaphoneValuesAction());
      Configuration.AddConsoleAction(new CorrespondingValuesAction());
      Configuration.AddConsoleAction(new CrossFrequencyAction());
      Configuration.AddConsoleAction(new CrossFrequencyCorrespondingAction());
      Configuration.AddConsoleAction(new CrossFrequencySelectAction());
      Configuration.AddConsoleAction(new CrossFrequencySelectRangeAction());
      Configuration.AddConsoleAction(new CutOffPhraseAction());
      Configuration.AddConsoleAction(new DisambiguationeAction());
      Configuration.AddConsoleAction(new DispersionAction());
      Configuration.AddConsoleAction(new DispersionCorrespondingAction());
      Configuration.AddConsoleAction(new DocumentCountAction());
      Configuration.AddConsoleAction(new DocumentHashAction());
      Configuration.AddConsoleAction(new DocumentRollingHashAction());
      Configuration.AddConsoleAction(new DocumentSimilarityAction());
      Configuration.AddConsoleAction(new DocumentTermFrequencyAction());
      Configuration.AddConsoleAction(new EditDistanceAction());
      Configuration.AddConsoleAction(new Frequency1Action());
      Configuration.AddConsoleAction(new Frequency1PerDocumentAction());
      Configuration.AddConsoleAction(new Frequency1PerSentenceAction());
      Configuration.AddConsoleAction(new Frequency1RawAction());
      Configuration.AddConsoleAction(new Frequency1RawSelectAction());
      Configuration.AddConsoleAction(new Frequency1SelectAction());
      Configuration.AddConsoleAction(new Frequency2Action());
      Configuration.AddConsoleAction(new Frequency2RawAction());
      Configuration.AddConsoleAction(new Frequency3Action());
      Configuration.AddConsoleAction(new Frequency3RawAction());
      Configuration.AddConsoleAction(new GuidOfCorpusAction());
      Configuration.AddConsoleAction(new GuidOfLayerAction());
      Configuration.AddConsoleAction(new GuidOfDocumentAction());
      Configuration.AddConsoleAction(new GuidOfSelectionAction());
      Configuration.AddConsoleAction(new GetDocumentAction());
      Configuration.AddConsoleAction(new GetDocumentDisplaynamesAction());
      Configuration.AddConsoleAction(new GetDocumentMetadataAction());
      Configuration.AddConsoleAction(new InverseDocumentFrequencyAction());
      Configuration.AddConsoleAction(new KeywordAction());
      Configuration.AddConsoleAction(new KeywordCorrespondingAction());
      Configuration.AddConsoleAction(new KwicAllInDocumentFilterAction());
      Configuration.AddConsoleAction(new KwicAllInSentenceFilterAction());
      Configuration.AddConsoleAction(new KwicAnyFilterAction());
      Configuration.AddConsoleAction(new KwicExactPhraseFilterAction());
      Configuration.AddConsoleAction(new KwicExportAction());
      Configuration.AddConsoleAction(new KwicFirstAnyFilterAction());
      Configuration.AddConsoleAction(new KwicMdaFilterAction());
      Configuration.AddConsoleAction(new KwicNamedEntityAction());
      Configuration.AddConsoleAction(new KwicSignificantFilterAction());
      Configuration.AddConsoleAction(new KwitAction());
      Configuration.AddConsoleAction(new KwitFrequencyAction());
      Configuration.AddConsoleAction(new KwitSelectAction());
      Configuration.AddConsoleAction(new LayerCloneAction());
      Configuration.AddConsoleAction(new LayerNamesAction());
      Configuration.AddConsoleAction(new LayerNewAction());
      Configuration.AddConsoleAction(new LayerRemoveAction());
      Configuration.AddConsoleAction(new LayerRenameAction());
      Configuration.AddConsoleAction(new LayerValuesAction());
      Configuration.AddConsoleAction(new MetaAction());
      Configuration.AddConsoleAction(new MetaCategoriesAction());
      Configuration.AddConsoleAction(new MetaDocumentAction());
      Configuration.AddConsoleAction(new MetaDocumentSelectAction());
      Configuration.AddConsoleAction(new MetaExportAction());
      Configuration.AddConsoleAction(new MetaImportAction());
      Configuration.AddConsoleAction(new MetaphoneValuesAction());
      Configuration.AddConsoleAction(new MetaSelectedAction());
      Configuration.AddConsoleAction(new MetaSelectedDomainAction());
      Configuration.AddConsoleAction(new MetaUniqueAction());
      Configuration.AddConsoleAction(new MetaUniqueSelectAction());
      Configuration.AddConsoleAction(new MetaUniqueSelectValuesAction());
      Configuration.AddConsoleAction(new MetaUniqueValuesAction());
      Configuration.AddConsoleAction(new NamedEntityAction());
      Configuration.AddConsoleAction(new NGramAction());
      Configuration.AddConsoleAction(new NGramCharAction());
      Configuration.AddConsoleAction(new NGramCooccurrenceAction());
      Configuration.AddConsoleAction(new NGramCorrespondingAction());
      Configuration.AddConsoleAction(new NGramSelectedAction());
      Configuration.AddConsoleAction(new NGramSelectedCooccurrenceAction());
      Configuration.AddConsoleAction(new Phrase2LayerCountAction());
      Configuration.AddConsoleAction(new PhraseCountAction());
      Configuration.AddConsoleAction(new PositionFrequencyAction());
      Configuration.AddConsoleAction(new PositionFrequencyCorrespondingAction());
      Configuration.AddConsoleAction(new QueryAction());
      Configuration.AddConsoleAction(new QueryCountAllTokenMatchesAction());
      Configuration.AddConsoleAction(new QueryCountDocumentsAction());
      Configuration.AddConsoleAction(new QueryCountSentencesAction());
      Configuration.AddConsoleAction(new QueryListAction());
      Configuration.AddConsoleAction(new RemoveLayerAction());
      Configuration.AddConsoleAction(new RemoveMetaAction());
      Configuration.AddConsoleAction(new SentenceCountAction());
      Configuration.AddConsoleAction(new SizeAction());
      Configuration.AddConsoleAction(new SkipgramAction());
      Configuration.AddConsoleAction(new StopwordListAction());
      Configuration.AddConsoleAction(new StyleBurrowsDeltaAction());
      Configuration.AddConsoleAction(new StyleNgramAction());
      Configuration.AddConsoleAction(new TermDocumentMatrixAction());
      Configuration.AddConsoleAction(new TermFrequencyInverseDocumentFrequencyAction());
      Configuration.AddConsoleAction(new TokenCountAction());
      Configuration.AddConsoleAction(new TokenListAction());
      Configuration.AddConsoleAction(new TokenListSelectAction());
      Configuration.AddConsoleAction(new TypeCountAction());
      Configuration.AddConsoleAction(new ValidateAction());
      Configuration.AddConsoleAction(new VocabularyComplexityAction());
      #endregion

      #region Exporter
      Configuration.AddExporter("CorpusExplorer v6 (*.cec6)|*.cec6", new ExporterCec6());
      Configuration.AddExporter("CorpusExplorer v6 + LZ4 (*.cec6.lz4)|*.cec6.lz4", new ExporterCec6 { UseCompression = true });
      Configuration.AddExporter("CorpusExplorer v6 + GZip (*.cec6.gz)|*.cec6.gz", new ExporterCec6 { UseCompression = true });      
      #endregion

      #region Importer
      Configuration.AddImporter("CorpusExplorer v6 (*.cec6, *.cec6.lz4, *.cec6.gz)|*.cec6;*.cec6.gz;*.cec6.lz4", new ImporterCec6());
      Configuration.AddImporter("CorpusExplorer v6 [mit DRM geschützt] (*.cec6.drm)|*.cec6.drm", new ImporterCec6Drm());
      Configuration.AddImporter("CorpusExplorer v6 [STREAM] (*.cec6)|*.cec6", new ImporterCec6Stream());
      #endregion

      #region SideloadFeature
      Configuration.AddSideloadFeature(new FelschKincaidGradeIndex());
      Configuration.AddSideloadFeature(new FelschReadingEaseIndex());
      Configuration.AddSideloadFeature(new GunningFogIndexIndex());
      Configuration.AddSideloadFeature(new SmogIndexIndex());
      Configuration.AddSideloadFeature(new WienerSachtextV1Index());
      Configuration.AddSideloadFeature(new WienerSachtextV2Index());
      Configuration.AddSideloadFeature(new WienerSachtextV3Index());
      Configuration.AddSideloadFeature(new WienerSachtextV4Index());
      Configuration.AddSideloadFeature(new VocabularyComplexityByBrunet1978());
      Configuration.AddSideloadFeature(new VocabularyComplexityByHonore1979());
      Configuration.AddSideloadFeature(new VocabularyComplexityBySichel1975());
      Configuration.AddSideloadFeature(new VocabularyComplexityByYule1938());
      Configuration.AddSideloadFeature(new VocabularyComplexityCarrollsCorrectedTTR());
      Configuration.AddSideloadFeature(new VocabularyComplexityGuiraud1954());
      Configuration.AddSideloadFeature(new VocabularyComplexityHerdan1960());
      Configuration.AddSideloadFeature(new VocabularyComplexitySummersIndex());
      Configuration.AddSideloadFeature(new VocabularyComplexityTypeTokenRatio());
      Configuration.AddSideloadFeature(new CosineMeasure());
      Configuration.AddSideloadFeature(new DistanceEuclidean());
      Configuration.AddSideloadFeature(new ChiSquaredSignificance());
      Configuration.AddSideloadFeature(new LogLikelihoodSignificance());
      Configuration.AddSideloadFeature(new PoissonSignificance());
      Configuration.AddSideloadFeature(new BraunMeasure());
      Configuration.AddSideloadFeature(new DiceCoefficient());
      Configuration.AddSideloadFeature(new HamannMeasure());
      Configuration.AddSideloadFeature(new JaccardMeasure());
      Configuration.AddSideloadFeature(new KappaMeasure());
      Configuration.AddSideloadFeature(new KulczynskiMeasure());
      Configuration.AddSideloadFeature(new MCoefficientMeasure());
      Configuration.AddSideloadFeature(new MutalInformation());
      Configuration.AddSideloadFeature(new OchiaiMeasure());
      Configuration.AddSideloadFeature(new PhiMeasure());
      Configuration.AddSideloadFeature(new RusselRaoMeasure());
      Configuration.AddSideloadFeature(new SimpsonMeasure());
      Configuration.AddSideloadFeature(new SneathMeasure());
      Configuration.AddSideloadFeature(new TanimotoMeasure());
      Configuration.AddSideloadFeature(new YuleMeasure());
      #endregion

      #region TableWriter
      Configuration.AddTableWriter("CSV-Tabelle (*.csv)|*.csv", new CsvTableWriter());
      Configuration.AddTableWriter("HTML-Dokument (*.html)|*.html", new HtmlTableWriter());
      Configuration.AddTableWriter("HTML-Tabelle (*.html)|*.html", new HtmlTableSnippetTableWriter());
      Configuration.AddTableWriter("JSON-Dokument (*.json)|*.json", new JsonTableWriter());
      Configuration.AddTableWriter("JSON-Dokument (gerundet) (*.json)|*.json", new JsonRoundedTableWriter());
      Configuration.AddTableWriter("SQL-Query nur Daten (*.sql)|*.sql", new SqlDataOnlyTableWriter());
      Configuration.AddTableWriter("SQL-Query nur Schema (*.sql)|*.sql", new SqlSchemaOnlyTableWriter());
      Configuration.AddTableWriter("SQL-Query Schema und Daten (*.sql)|*.sql", new SqlTableWriter());
      Configuration.AddTableWriter("TSV-Tabelle (*.tsv)|*.tsv", new TsvTableWriter());
      Configuration.AddTableWriter("TSV-Tabelle (gerundet) (*.tsv)|*.tsv", new TsvRoundedTableWriter());
      Configuration.AddTableWriter("XML-Dokument (*.xml)|*.xml", new XmlTableWriter());
      #endregion
    }
  }
}
