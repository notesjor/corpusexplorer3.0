#region

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using CorpusExplorer.Sdk._3rdParty.Commercial.Metaphone3.Action;
using CorpusExplorer.Sdk.Action;
using CorpusExplorer.Sdk.Addon;
using CorpusExplorer.Sdk.Blocks.Cooccurrence;
using CorpusExplorer.Sdk.Blocks.Measure;
using CorpusExplorer.Sdk.Blocks.Measure.Abstract;
using CorpusExplorer.Sdk.Blocks.ReadingEase;
using CorpusExplorer.Sdk.Blocks.Similarity;
using CorpusExplorer.Sdk.Blocks.VocabularyComplaxity;
using CorpusExplorer.Sdk.Diagnostic;
using CorpusExplorer.Sdk.Helper;
using CorpusExplorer.Sdk.Model.Cache;
using CorpusExplorer.Sdk.Model.Cache.Abstract;
using CorpusExplorer.Sdk.Model.Localization;
using CorpusExplorer.Sdk.Properties;
using CorpusExplorer.Sdk.Utils.DataTableWriter;
using CorpusExplorer.Sdk.Utils.DataTableWriter.Abstract;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Abstract;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Builder;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Crawler;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Crawler.Abstract;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Exporter;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Exporter.Abstract;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Importer;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Importer.Abstract;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Importer.CorpusExplorerV6;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Scraper;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Scraper.Abstract;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Scraper.Txt;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Tagger.Abstract;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Tagger.AdditionalTaggerWrapper;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Tagger.RawText;

#endregion

namespace CorpusExplorer.Sdk.Ecosystem.Model
{
  /// <summary>
  ///   Class <see cref="Configuration" />
  /// </summary>
  public static class Configuration
  {
    private static Encoding _encoding = Encoding.UTF8;
    private static bool? _rightToLeftSupport;
    private static ISignificance _significance = new PoissonSignificance();

    /// <summary>
    ///   Zusätzliche Tagger
    /// </summary>
    public static IEnumerable<AbstractAdditionalTagger> AddonAdditionalTaggers => _addonAdditionalTaggers;

    public static void AddonAdditionalTaggersAdd(IEnumerable<AbstractAdditionalTagger> list)
    {
      if (list != null)
        _addonAdditionalTaggers.AddRange(list);
    }

    /// <summary>
    ///   Verfügbare Backends
    /// </summary>
    /// <value>The addon backends.</value>
    public static IEnumerable<KeyValuePair<string, AbstractCorpusBuilder>> AddonBackends
    {
      get
      {
        var list = _addonBackends.OrderBy(x => x.Key).ToList();
        var cec6 =
          (from x in list where x.Value.GetType() == typeof(CorpusBuilderWriteDirect) select x).FirstOrDefault();
        list.Remove(cec6);
        list.Insert(0, cec6);
        return list;
      }
    }

    public static void AddonBackendsAdd(IEnumerable<KeyValuePair<string, AbstractCorpusBuilder>> list)
    {
      if (list == null)
        return;

      foreach (var x in list.Where(x => !_addonImporters.ContainsKey(x.Key)))
        _addonBackends.Add(x.Key, x.Value);
    }

    public static IEnumerable<T> GetSideloadFeature<T>()
    {
      return _addonSideLoadFeatures.OfType<T>();
    }

    public static void AddSideloadFeature(object feature)
    {
      _addonSideLoadFeatures.Add(feature);
    }

    public static IEnumerable<IAction> AddonConsoleActions => _addonConsoleActions;

    public static IAction GetConsoleAction(string actionName)
    {
      return (from x in AddonConsoleActions where x.Action == actionName select x).FirstOrDefault();
    }

    public static void AddonConsoleActionsAdd(IEnumerable<IAction> list)
    {
      if (list != null)
        _addonConsoleActions.AddRange(list);
    }

    /// <summary>
    ///   Zusätzliche Crawler
    /// </summary>
    public static IEnumerable<AbstractCrawler> AddonCrawlers => _addonCrawlers;

    public static void AddonCrawlersAdd(IEnumerable<AbstractCrawler> list)
    {
      if (list != null)
        _addonCrawlers.AddRange(list);
    }

    public static IEnumerable<KeyValuePair<string, AbstractExporter>> AddonExporters
    {
      get
      {
        var list = _addonExporters.OrderBy(x => x.Key).ToList();
        var cec6 = (from x in list where x.Value.GetType() == typeof(ExporterCec6) select x).FirstOrDefault();
        list.Remove(cec6);
        list.Insert(0, cec6);
        return list;
      }
    }

    public static AbstractExporter GetExporter(string exporter)
      => (from x in _addonExporters where x.Value.GetType().Name.EndsWith(exporter) select x.Value).FirstOrDefault();

    public static void AddonExportersAdd(IEnumerable<KeyValuePair<string, AbstractExporter>> list)
    {
      if (list != null)
        foreach (var x in list.Where(x => !_addonImporters.ContainsKey(x.Key)))
          _addonExporters.Add(x.Key, x.Value);
    }

    /// <summary>
    ///   Liste mit Scrapern die lokale Dateien bestehender Korpora importieren (z. B. XML, EXMERaLDA).
    ///   Für Dateien MIT Annotation.
    /// </summary>
    public static IEnumerable<KeyValuePair<string, AbstractImporter>> AddonImporters
    {
      get
      {
        var list = _addonImporters.OrderBy(x => x.Key).ToList();
        var cec6 = (from x in list where x.Value.GetType() == typeof(ImporterCec6) select x).FirstOrDefault();
        list.Remove(cec6);
        list.Insert(0, cec6);
        return list;
      }
    }

    public static void AddonImportersAdd(IEnumerable<KeyValuePair<string, AbstractImporter>> list)
    {
      if (list == null)
        return;

      foreach (var x in list.Where(x => !_addonImporters.ContainsKey(x.Key)))
        _addonImporters.Add(x.Key, x.Value);
    }

    /// <summary>
    ///   Liste mit Scrapern die lokale Dateien (z. B. TXT, RTF, DOCX, PDF) in Korpusdokumente konvertieren.
    ///   Für Dateien OHNE Annotation.
    /// </summary>
    public static IEnumerable<KeyValuePair<string, AbstractScraper>> AddonScrapers =>
      _addonScrapers.OrderBy(x => x.Key);

    public static void AddonScrapersAdd(IEnumerable<KeyValuePair<string, AbstractScraper>> list)
    {
      if (list != null)
        foreach (var x in list.Where(x => !_addonImporters.ContainsKey(x.Key)))
          _addonScrapers.Add(x.Key, x.Value);
    }

    /// <summary>
    /// Export für Analysedaten im Tabellenformat
    /// </summary>
    public static IEnumerable<KeyValuePair<string, AbstractTableWriter>> AddonTableWriter =>
      _addonTableWriters.OrderBy(x => x.Key);

    public static AbstractTableWriter GetTableWriter(string tableWriterTag)
    {
      if (tableWriterTag.StartsWith("F:"))
      {
        GetTableWriterAdditionalPath(tableWriterTag, out tableWriterTag, out var path);

        var res = _addonTableWriters.Where(x => x.Value.TableWriterTag == tableWriterTag)
          .Select(x => x.Value)
          .FirstOrDefault() ?? new TsvTableWriter();

        if (path == null)
          return res;

        res.OutputStream = new FileStream(path, FileMode.Create, FileAccess.Write);
        res.Path = path;
        return res;
      }

      // ReSharper disable once InvertIf
      if (tableWriterTag.StartsWith("FNT:"))
      {
        GetTableWriterAdditionalPath(tableWriterTag, out tableWriterTag, out var path);

        var f = tableWriterTag.Replace("FNT:", "F:");

        var res = _addonTableWriters.Where(x => x.Value.TableWriterTag == f)
          .Select(x => x.Value)
          .FirstOrDefault() ?? _addonTableWriters.First().Value;

        res.WriteTid = false;
        if (path == null)
          return res;

        res.OutputStream = new FileStream(path, FileMode.Create, FileAccess.Write);
        res.Path = path;
        return res;
      }

      return null;
    }

    public static void AddonTableWriterAdd(IEnumerable<KeyValuePair<string, AbstractTableWriter>> list)
    {
      if (list != null)
        foreach (var x in list.Where(x => !_addonImporters.ContainsKey(x.Key)))
          _addonTableWriters.Add(x.Key, x.Value);
    }

    public static void GetTableWriterAdditionalPath(string tableWriterTag, out string tag, out string path)
    {
      if (tableWriterTag.Contains("#"))
      {
        var split = tableWriterTag.Split('#');
        tag = split[0];
        path = split[1];
      }
      else
      {
        tag = tableWriterTag;
        path = null;
      }
    }

    /// <summary>
    ///   Tagger
    /// </summary>
    public static IEnumerable<AbstractTagger> AddonTaggers => _addonTaggers;

    public static void AddonTaggersAdd(IEnumerable<AbstractTagger> list)
    {
      if (list != null)
        _addonTaggers.AddRange(list);
    }

    /// <summary>
    /// Gets the layer display name localization.
    /// </summary>
    /// <value>The layer display name localization.</value>
    public static ModelLocalization LayerDisplayNameLocalization { get; } = new ModelLocalization();

    /// <summary>
    /// Gets the document metadata localization.
    /// </summary>
    /// <value>The document metadata localization.</value>
    public static ModelLocalization DocumentMetadataLocalization { get; } = new ModelLocalization();

    public static void LoadLocalizations(string directory)
    {
      try
      {
        var path = Path.Combine(directory, "layer.local");
        if (File.Exists(path))
          LayerDisplayNameLocalization.LoadLocalization(path);
      }
      catch
      {
        //ignore
      }
      try
      {
        var path = Path.Combine(directory, "meta.local");
        if (File.Exists(path))
          DocumentMetadataLocalization.LoadLocalization(path);
      }
      catch
      {
        // ignore
      }
    }

    /// <summary>
    ///   Gets the app config path.
    /// </summary>
    public static string AppConfigPath { get; private set; }

    /// <summary>
    ///   Gets the app path.
    /// </summary>
    public static string AppPath
    {
      get
      {
        try
        {
          return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
        catch
        {
          return "";
        }
      }
    }

    public static AbstractCacheStrategy Cache { get; set; } = new CacheStrategyDisableCaching();

    public static Encoding Encoding
    {
      get
      {
        if (_encoding != null)
          return _encoding;

        try
        {
          _encoding = Encoding.GetEncoding((int)GetSetting("Encoding (CodePage)", Encoding.UTF8.CodePage));
        }
        catch
        {
          _encoding = Encoding.UTF8;
        }

        return _encoding;
      }
      set
      {
        _encoding = value ?? Encoding.UTF8;
        SetSetting("Encoding (CodePage)", _encoding.CodePage);
      }
    }

    /// <summary>
    ///   Gets the extern app path.
    /// </summary>
    public static string ExternAppPath { get; private set; }

    public static bool IsInitialized { get; private set; }

    /// <summary>
    ///   Gets or sets the measure.
    /// </summary>
    public static IMeasure Measure { get; set; } = new DiceCoefficient();

    /// <summary>
    ///   Gets or sets the minimum frequency.
    /// </summary>
    public static int MinimumFrequency { get; set; } = 1;

    /// <summary>
    ///   Gets or sets the minimum significance.
    /// </summary>
    public static double MinimumSignificance { get; set; } = 0.9;

    /// <summary>
    ///   Gibt das Verzeichnis zurück in dem der Benutzer Crawler/Addons ablegen kann ohne diese Installieren zu müssen.
    /// </summary>
    public static string MyAddons { get; private set; }

    /// <summary>
    ///   Pfad zu den Korpora
    /// </summary>
    public static string MyCorpora { get; private set; }

    public static string MyDataSources { get; private set; }

    /// <summary>
    ///   Pfad zum Projektordner
    /// </summary>
    public static string MyProjects { get; private set; }

    public static ParallelOptions ParallelOptions { get; set; } =
      new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

    public static bool ProtectMemoryOverflow
    {
      get => (bool)GetSetting("RAM-Selbstschutz", true);
      set => SetSetting("RAM-Selbstschutz", value);
    }

    public static double DefaultFontSize
    {
      get => (double)GetSetting("Schriftgröße", 12.0);
      set => SetSetting("Schriftgröße", value);
    }

    public static Random Random { get; private set; } = new Random();

    public static bool RightToLeftSupport
    {
      get
      {
        if (_rightToLeftSupport.HasValue)
          return _rightToLeftSupport.Value;

        try
        {
          _rightToLeftSupport = (bool)GetSetting("R/L-Support", false);
        }
        catch
        {
          _rightToLeftSupport = false;
        }

        return _rightToLeftSupport.Value;
      }
      set
      {
        _rightToLeftSupport = value;
        SetSetting("R/L-Support", value);
      }
    }

    /// <summary>
    /// Ändrn um eine andere Metaangabe als Bezeichnung für ein Dokument zu verwenden.
    /// </summary>
    public static string TitleMetaKey { get; set; } = Resources.Title;

    /// <summary>
    ///   Pfad zu den temporären Dateien
    /// </summary>
    public static string TempPath
    {
      get
      {
        try
        {
          return Path.Combine(Path.GetTempPath(), "CorpusExplorer");
        }
        catch
        {
          return "TEMP";
        }
      }
    }

    /// <summary>
    ///   Dateipfad der Einstellungsdatei
    /// </summary>
    private static string SettingsAppPath { get; set; }

    public static int IOBufferSize
    {
      get => (int)GetSetting("Puffergröße", 1024 * 1024 * 100);
      set => SetSetting("Puffergröße", value);
    }

    public static void SetSignificance(ISignificance significance)
    {
      _significance = significance;
      MinimumSignificance = significance.MinimumSignificance;
    }

    public static ISignificance GetSignificance(double a, double n)
    {
      return _significance.PreCalculationSetup(a, n);
    }

    public static Type GetSignificanceType()
    {
      return _significance.GetType();
    }

    /// <summary>
    ///   Gibt eine Einstellung zurück (safety cast)
    /// </summary>
    /// <param name="settingName">
    ///   Name der Einstellung
    /// </param>
    /// <param name="defaultValue">
    ///   Wert, der gesetzt wird, wenn der Eintrag noch nicht exsistiert
    /// </param>
    /// <returns>
    ///   Wert
    /// </returns>
    public static object GetSetting(string settingName, object defaultValue)
    {
      try
      {
        var obj = GetSettings();
        if (obj != null && obj.ContainsKey(settingName))
          return obj[settingName];

        SetSetting(settingName, defaultValue);
        return defaultValue;
      }
      catch
      {
        return defaultValue;
      }
    }

    public static Dictionary<string, object>? GetSettings(string? alternativePath = null)
    {
      try
      {
        return File.Exists(alternativePath ?? SettingsAppPath)
                 ? Serializer.Deserialize<Dictionary<string, object>>(alternativePath ?? SettingsAppPath)
                 : new Dictionary<string, object>();
      }
      catch
      {
        return new Dictionary<string, object>();
      }
    }

    /// <summary>
    ///   Speichert eine Einstellung
    /// </summary>
    /// <param name="settingName">
    ///   Name der Einstellung
    /// </param>
    /// <param name="value">
    ///   Wert
    /// </param>
    public static void RemoveSetting(string settingName)
    {
      var obj = GetSettings();

      if (!obj.ContainsKey(settingName))
        return;

      obj.Remove(settingName);
      SetSettings(obj);
    }

    /// <summary>
    ///   Speichert eine Einstellung
    /// </summary>
    /// <param name="settingName">
    ///   Name der Einstellung
    /// </param>
    /// <param name="value">
    ///   Wert
    /// </param>
    public static void SetSetting(string settingName, object value)
    {
      var obj = GetSettings() ?? new Dictionary<string, object>();
      if (obj.ContainsKey(settingName))
        obj[settingName] = value;
      else
        obj.Add(settingName, value);

      SetSettings(obj);
    }

    public static void SetSettings(Dictionary<string, object> settings, string? alternativePath = null)
    {
      try
      {
        Serializer.Serialize(settings, alternativePath ?? SettingsAppPath);
      }
      catch
      {
        // ignore
      }
    }

    /// <summary>
    ///   The initialize.
    /// </summary>
    internal static void Initialize()
    {
      if (IsInitialized)
        return;

      InitializeMinimal();

      IsInitialized = true;
    }

    /// <summary>
    ///   Stellt sicher, das der Pfad exsisitert
    /// </summary>
    /// <param name="path">
    ///   Pfad
    /// </param>
    /// <returns>
    ///   Gibt den sichergestellten Pfad zurück
    /// </returns>
    private static string EnsurePath(string path)
    {
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);

      return path;
    }

    private static string GetRelativAppDirectory(string directory)
    {
      try
      {
        var res = string.IsNullOrEmpty(directory) ? AppPath : Path.Combine(AppPath, directory);
        if (res != null && !Directory.Exists(res))
          Directory.CreateDirectory(res);
        return res;
      }
      catch
      {
        return directory;
      }
    }

    /// <summary>
    ///   Wandelt einen relativen Pfad in einen absoluten Pfad um
    /// </summary>
    /// <param name="path">
    ///   Pfad
    /// </param>
    /// <returns>
    ///   Absoluter Pfad (Anwendungsdaten\CorpusExplorer\Application\ + path)
    /// </returns>
    private static string GetRelativAppFilePath(string path)
    {
      try
      {
        var res = string.IsNullOrEmpty(path) ? AppPath : Path.Combine(AppPath, path);
        var dir = Path.GetDirectoryName(res);
        if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
          Directory.CreateDirectory(dir);
        return res;
      }
      catch
      {
        return path;
      }
    }

    /// <summary>
    ///   The get relativ path.
    /// </summary>
    /// <param name="path">
    ///   The path.
    /// </param>
    /// <returns>
    ///   The <see cref="string" />.
    /// </returns>
    private static string GetRelativPath(string path)
    {
      try
      {
        var res = Path.Combine(
                             Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                             "CorpusExplorer\\" + path);
        return EnsurePath(res);
      }
      catch
      {
        return EnsurePath(path);
      }
    }

    private static void InitializeMinimal()
    {
      Random = new Random();

      SetSignificance(new PoissonSignificance());

      try // notwendig z. B. unter AZURE
      {
        AppConfigPath = GetRelativAppDirectory("myConfig");
        ExternAppPath = GetRelativAppDirectory("extern");
      }
      catch
      {
        // ignore
      }

      try // notwendig z. B. unter AZURE
      {
        MyCorpora = GetRelativPath(Resources.MyCorpora);
        MyAddons = GetRelativPath(Resources.MyExtension);
        MyDataSources = GetRelativPath(Resources.MyDataSources);
        MyProjects = GetRelativPath(Resources.MyProjects);
      }
      catch
      {
        // ignore
      }

      // Anwendungskonfiguration
      try // notwendig z. B. unter AZURE
      {
        SettingsAppPath = GetRelativAppFilePath("settings.bin");
      }
      catch
      {
        // ignore
      }

      ParallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * 2 };

      try // notwendig z. B. unter AZURE
      {
        if (!Directory.Exists(TempPath))
          Directory.CreateDirectory(TempPath);

        var files = Directory.GetFiles(TempPath, "*.*");
        foreach (var file in files)
          try
          {
            File.Delete(file);
          }
          catch
          {
            // ignore
          }
      }
      catch
      {
        // ignore
      }

      // notwendig - Aufruf get > set setzt lädt bzw. initialisiert die Eigenschaften
      Encoding = Encoding;
      RightToLeftSupport = RightToLeftSupport;
      ProtectMemoryOverflow = ProtectMemoryOverflow;
      DefaultFontSize = DefaultFontSize;

      IsInitialized = true;
    }

    private static List<AbstractAdditionalTagger> _addonAdditionalTaggers =
    [
      new AdditionalTsvValueTagger(),
      new AdditionalTsvValueTagger()
    ];
    private static Dictionary<string, AbstractCorpusBuilder> _addonBackends = new()
      {
        { "CorpusExplorer v6", new CorpusBuilderWriteDirect() }
      };
    private static List<object> _addonSideLoadFeatures =
      [
        new FelschKincaidGradeIndex(),
        new FelschReadingEaseIndex(),
        new GunningFogIndexIndex(),
        new SmogIndexIndex(),
        new WienerSachtextV1Index(),
        new WienerSachtextV2Index(),
        new WienerSachtextV3Index(),
        new WienerSachtextV4Index(),

        new VocabularyComplexityByBrunet1978(),
        new VocabularyComplexityByHonore1979(),
        new VocabularyComplexityBySichel1975(),
        new VocabularyComplexityByYule1938(),
        new VocabularyComplexityCarrollsCorrectedTTR(),
        new VocabularyComplexityGuiraud1954(),
        new VocabularyComplexityHerdan1960(),
        new VocabularyComplexitySummersIndex(),
        new VocabularyComplexityTypeTokenRatio(),

        new CosineMeasure(),
        new DistanceEuclidean(),

        new ChiSquaredSignificance(),
        new LogLikelihoodSignificance(),
        new PoissonSignificance(),

        new BraunMeasure(),
        new DiceCoefficient(),
        new HamannMeasure(),
        new JaccardMeasure(),
        new KappaMeasure(),
        new KulczynskiMeasure(),
        new MCoefficientMeasure(),
        new MutalInformation(),
        new OchiaiMeasure(),
        new PhiMeasure(),
        new RusselRaoMeasure(),
        new SimpsonMeasure(),
        new SneathMeasure(),
        new TanimotoMeasure(),
        new YuleMeasure()
      ];
    private static List<IAction> _addonConsoleActions =
      [
        new AddTaggerAction(),

        new BasicInformationAction(),

        new CentralSnipAction(),
        new CentralSnipTreeAction(),
        new ClusterAction(),
        new ClusterListAction(),
        new ConvertAction(),
        new CooccurrenceAction(),
        new CooccurrenceCorrespondingAction(),
        new CooccurrenceCrossAction(),
        new CooccurrenceCrossFullAction(),
        new CooccurrencePolarisationAction(),
        new CooccurrenceProfileAction(),
        new CooccurrenceSelectedAction(),
        new CooccurrenceSelectedCorrespondingAction(),
        new CorrespondingMetaphoneValuesAction(),
        new CorrespondingValuesAction(),
        new CrossFrequencyAction(),
        new CrossFrequencyCorrespondingAction(),
        new CutOffPhraseAction(),

        new DisambiguationeAction(),
        new DispersionAction(),
        new DispersionCorrespondingAction(),
        new DocumentCountAction(),
        new DocumentHashAction(),
        new DocumentSimilarityAction(),
        new DocumentTermFrequencyAction(),

        new EditDistanceAction(),

        new Frequency1Action(),
        new Frequency1RawAction(),
        new Frequency1RawSelectAction(),
        new Frequency1SelectAction(),
        new Frequency2Action(),
        new Frequency2RawAction(),
        new Phrase2LayerCountAction(),
        new Frequency3Action(),
        new Frequency3RawAction(),

        new GetDocumentAction(),
        new GetDocumentDisplaynamesAction(),
        new GetDocumentMetadataAction(),

        new InverseDocumentFrequencyAction(),

        new KeywordAction(),
        new KeywordCorrespondingAction(),
        new KwicAllInDocumentFilterAction(),
        new KwicAllInSentenceFilterAction(),
        new KwicAnyFilterAction(),
        new KwicExactPhraseFilterAction(),
        new KwicExportAction(),
        new KwicFirstAnyFilterAction(),
        new KwicMdaFilterAction(),
        new KwicNamedEntityAction(),
        new KwicSignificantFilterAction(),
        new KwitAction(),
        new KwitSelectAction(),

        new LayerCloneAction(),
        new LayerNamesAction(),
        new LayerNewAction(),
        new LayerRemoveAction(),
        new LayerRenameAction(),
        new LayerValuesAction(),

        new MetaAction(),
        new MetaCategoriesAction(),
        new MetaDocumentAction(),
        new MetaDocumentSelectAction(),
        new MetaExportAction(),
        new MetaImportAction(),
        new MetaphoneValuesAction(),
        new MetaSelectedAction(),
        new MetaSelectedDomainAction(),
        new MetaUniqueAction(),
        new MetaUniqueSelectAction(),
        new MetaUniqueSelectValuesAction(),
        new MetaUniqueValuesAction(),

        new NamedEntityAction(),
        new NGramAction(),
        new NGramCooccurrenceAction(),
        new NGramCorrespondingAction(),
        new NGramSelectedAction(),
        new NGramSelectedCooccurrenceAction(),

        new PhraseCountAction(),
        new PositionFrequencyAction(),
        new PositionFrequencyCorrespondingAction(),

        new QueryAction(),
        new QueryCountAllTokenMatchesAction(),
        new QueryCountDocumentsAction(),
        new QueryCountSentencesAction(),
        new QueryListAction(),

        new RemoveLayerAction(),
        new RemoveMetaAction(),

        new SentenceCountAction(),
        new SizeAction(),
        new StopwordListAction(),
        new StyleBurrowsDeltaAction(),
        new StyleNgramAction(),

        new TermFrequencyInverseDocumentFrequencyAction(),
        new TokenCountAction(),
        new TokenListAction(),
        new TokenListSelectAction(),
        new TypeCountAction(),

        new ValidateAction(),
        new VocabularyComplexityAction(),
      ];
    private static List<AbstractCrawler> _addonCrawlers = [];
    private static Dictionary<string, AbstractExporter> _addonExporters = new()
    {
      { "JSON-Export (*.json)|*.json", new ExporterJson() },
      { "JSON-Export + ZIP (*.zip)|*.zip", new ExporterJsonZip() },
      { "TLV-XML-Export (*.xml)|*.xml", new ExporterTlv() },
      { "CorpusExplorer v6 (*.cec6)|*.cec6", new ExporterCec6() },
      { "CorpusExplorer v6 + LZ4 (*.cec6.lz4)|*.cec6.lz4", new ExporterCec6 { UseCompression = true } },
      { "CorpusExplorer v6 + GZip (*.cec6.gz)|*.cec6.gz", new ExporterCec6 { UseCompression = true } },
      { "Plaintext-Export (*.txt)|*.txt", new ExporterPlaintext() },
      { "Plaintext-Export [Nur Wort-Layer] (*.txt)|*.txt", new ExporterPlaintextPure() },
      { "Plaintext-Export [Nur Wort-Layer - Eine Datei] (*.txt)|*.txt", new ExporterPlaintextPureInOneFile() },
      { "Einfacher HTML-Export [Nur Wort-Layer] (*.txt)|*.txt", new ExporterHtmlPure() },
      { "CSV-Export [Nur Metadatan] (*.csv)|*.csv", new ExporterCsvMetadataOnly() },
      { "CSV-Export [Metadaten + Wort-Layer] (*.csv)|*.csv", new ExporterCsv() },
      { "Abfragen-Export [Nur für Schnappschüsse] (*.ceusd)|*.ceusd", new ExporterQuery() },
      { "CoNLL (*.conll)|*.conll", new ExporterConll() },
      { "CoNLL + ZIP (*.zip)|*.zip", new ExporterConllZip() },
      { "TreeTagger (*.treetagger)|*.treetagger", new ExporterTreeTagger() },
      { "TreeTagger + Satzgrenze (*.treetagger)|*.treetagger", new ExporterTreeTagger { UseSentenceTag = true } },
      { "TreeTagger + ZIP (*.zip)|*.zip", new ExporterTreeTaggerZip { UseSentenceTag = true } },
      { "CorpusWorkBench bis 2021(*.vrt)|*.vrt", new ExporterCorpusWorkBench { UseSentenceTag = false } },
      { "CorpusWorkBench bis 2021 inkl. Satzgrene (*.vrt, *.vrt.xml)|*.vrt;*.vrt.xml", new ExporterCorpusWorkBench { UseSentenceTag = true } },
      { "CorpusWorkBench ab 2022 (*.vrt, *.vrt.xml)|*.vrt;*.vrt.xml", new ExporterCorpusWorkBench2022 { UseSentenceTag = true } },
      { "Sketch Engine VERT (*.vert)|*.vert", new ExporterSketchEngine() },
    };
    private static Dictionary<string, AbstractImporter> _addonImporters = new()
      {
        {"CorpusExplorer v6 (*.cec6, *.cec6.lz4, *.cec6.gz)|*.cec6;*.cec6.gz;*.cec6.lz4", new ImporterCec6()},
        {"CorpusExplorer v6 [mit DRM geschützt] (*.cec6.drm)|*.cec6.drm", new ImporterCec6Drm()},
        {"CorpusExplorer v6 [STREAM] (*.cec6)|*.cec6", new ImporterCec6Stream()},
        {"TLV-XML (*.xml)|*.xml", new ImporterTlv()},
        {"CoNLL (*.conll)|*.conll", new ImporterConll()},
        {"TreeTagger (*.txt)|*.txt", new ImporterTreeTagger()},
        {"CorpusWorkBench VRT (*.vrt)|*.vrt", new ImporterCorpusWorkBench()},
        {"Sketch Engine VERT (*.vert)|*.vert", new ImporterSketchEngine()},
      };
    private static Dictionary<string, AbstractScraper> _addonScrapers = new()
    {
      {"Plain-TXT (*.txt)|*.txt", new TxtScraper()},
      {"COSMAS-TXT (*.rtf)|*.rtf", new CosmasScraper()},
      {"Nur Text - Re-Annotation (*.cec5, *.cec6, *.cec6.gz)|*.cec5;*.cec6;*.cec6.gz", new Cec6Scraper()},
    };
    private static Dictionary<string, AbstractTableWriter> _addonTableWriters = new()
        {
          {"CSV-Tabelle (*.csv)|*.csv", new CsvTableWriter() },
          {"HTML-Dokument (*.html)|*.html", new HtmlTableWriter() },
          {"HTML-Tabelle (*.html)|*.html", new HtmlTableSnippetTableWriter() },
          {"JSON-Dokument (*.json)|*.json", new JsonTableWriter() },
          {"JSON-Dokument (gerundet) (*.json)|*.json", new JsonRoundedTableWriter() },
          {"SQL-Query nur Daten (*.sql)|*.sql", new SqlDataOnlyTableWriter() },
          {"SQL-Query nur Schema (*.sql)|*.sql", new SqlSchemaOnlyTableWriter() },
          {"SQL-Query Schema und Daten (*.sql)|*.sql", new SqlTableWriter() },
          {"TSV-Tabelle (*.tsv)|*.tsv", new TsvTableWriter() },
          {"TSV-Tabelle (gerundet) (*.tsv)|*.tsv", new TsvRoundedTableWriter() },
          {"XML-Dokument (*.xml)|*.xml", new XmlTableWriter() }
        };
    private static List<AbstractTagger> _addonTaggers =
      [
        new RawTextTagger(),
      ];
  }
}