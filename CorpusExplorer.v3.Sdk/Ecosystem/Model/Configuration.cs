#region

using System.Reflection;
using System.Text;
using CorpusExplorer.Sdk.Addon;
using CorpusExplorer.Sdk.Blocks.Cooccurrence;
using CorpusExplorer.Sdk.Blocks.Measure;
using CorpusExplorer.Sdk.Blocks.Measure.Abstract;
using CorpusExplorer.Sdk.Helper;
using CorpusExplorer.Sdk.Model.Cache;
using CorpusExplorer.Sdk.Model.Cache.Abstract;
using CorpusExplorer.Sdk.Model.Localization;
using CorpusExplorer.Sdk.Properties;
using CorpusExplorer.Sdk.Utils.DataTableWriter;
using CorpusExplorer.Sdk.Utils.DataTableWriter.Abstract;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Abstract;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Crawler.Abstract;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Exporter.Abstract;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Importer.Abstract;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Scraper.Abstract;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Tagger.Abstract;

#endregion

namespace CorpusExplorer.Sdk.Ecosystem.Model
{
  /// <summary>
  ///   Class <see cref="Configuration" />
  /// </summary>
  public static class Configuration
  {
    public static string GetDependencyPath(string path)
    {
      return Path.Combine(AppPath, "XDependencies", path); // TODO
    }

    #region Addons
    /// <summary>
    ///   Zusätzliche Tagger
    /// </summary>
    public static IEnumerable<AbstractAdditionalTagger> AddonAdditionalTaggers => _addonAdditionalTaggers.OrderBy(x => x.DisplayName);

    public static void AddAdditionalTaggers(AbstractAdditionalTagger additionalTagger) => _addonAdditionalTaggers.Add(additionalTagger);

    /// <summary>
    ///   Verfügbare Backends
    /// </summary>
    /// <value>The addon backends.</value>
    public static IEnumerable<KeyValuePair<string, AbstractCorpusBuilder>> AddonBackends
    {
      get
      {
        if (_addonBackends.Count == 1)
          return _addonBackends;

        var preserve = _addonBackends.Take(1);
        var rest = _addonBackends.Skip(1).OrderBy(x => x.Key);
        return preserve.Concat(rest);
      }
    }

    public static void AddBackend(string name, AbstractCorpusBuilder backend) => _addonBackends.TryAdd(name, backend);

    public static IEnumerable<T> GetSideloadFeature<T>()
    {
      return _addonSideLoadFeatures.OfType<T>();
    }

    public static void AddSideloadFeature(object feature) => _addonSideLoadFeatures.Add(feature);

    public static IEnumerable<IAction> AddonConsoleActions => _addonConsoleActions.Values;

    public static void AddConsoleAction(IAction action) => _addonConsoleActions.TryAdd(action.Action, action);

    public static IAction GetConsoleAction(string actionName)
    {
      return (from x in AddonConsoleActions where x.Action == actionName select x).FirstOrDefault();
    }

    /// <summary>
    ///   Zusätzliche Crawler
    /// </summary>
    public static IEnumerable<AbstractCrawler> AddonCrawlers => _addonCrawlers.OrderBy(x => x.DisplayName);

    public static void AddCrawler(AbstractCrawler crawler) => _addonCrawlers.Add(crawler);

    public static IEnumerable<KeyValuePair<string, AbstractExporter>> AddonExporters
    {
      get
      {
        var preserve = _addonExporters.Take(3);
        var rest = _addonExporters.Skip(3).OrderBy(x => x.Key);
        return preserve.Concat(rest);
      }
    }

    public static void AddExporter(string name, AbstractExporter exporter) => _addonExporters.TryAdd(name, exporter);

    public static AbstractExporter GetExporter(string exporter)
      => (from x in _addonExporters where x.Value.GetType().Name.EndsWith(exporter) select x.Value).FirstOrDefault();

    /// <summary>
    ///   Liste mit Scrapern die lokale Dateien bestehender Korpora importieren (z. B. XML, EXMERaLDA).
    ///   Für Dateien MIT Annotation.
    /// </summary>
    public static IEnumerable<KeyValuePair<string, AbstractImporter>> AddonImporters
    {
      get
      {
        var preserve = _addonImporters.Take(3);
        var rest = _addonImporters.Skip(3).OrderBy(x => x.Key);
        return preserve.Concat(rest);
      }
    }

    public static void AddImporter(string name, AbstractImporter importer) => _addonImporters.TryAdd(name, importer);

    /// <summary>
    ///   Liste mit Scrapern die lokale Dateien (z. B. TXT, RTF, DOCX, PDF) in Korpusdokumente konvertieren.
    ///   Für Dateien OHNE Annotation.
    /// </summary>
    public static IEnumerable<KeyValuePair<string, AbstractScraper>> AddonScrapers => _addonScrapers.OrderBy(x => x.Key);

    public static void AddScraper(string name, AbstractScraper scraper) => _addonScrapers.TryAdd(name, scraper);

    /// <summary>
    /// Export für Analysedaten im Tabellenformat
    /// </summary>
    public static IEnumerable<KeyValuePair<string, AbstractTableWriter>> AddonTableWriter => _addonTableWriters.OrderBy(x => x.Key);

    public static void AddTableWriter(string name, AbstractTableWriter tableWriter) => _addonTableWriters.TryAdd(name, tableWriter);

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
    public static IEnumerable<AbstractTagger> AddonTaggers => _addonTaggers.OrderBy(x => x.DisplayName);

    public static void AddTagger(AbstractTagger tagger) => _addonTaggers.Add(tagger);
    #endregion

    #region ModelLocalization
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
    #endregion

    #region General Configuration
    private static Encoding _encoding = Encoding.UTF8;
    private static bool? _rightToLeftSupport;
    private static ISignificance _significance = new PoissonSignificance();

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
          _encoding = Encoding.GetEncoding(Convert.ToInt32(GetSetting("Encoding (CodePage)", Encoding.UTF8.CodePage)));
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
      // ReSharper disable once RedundantCast
      get => Convert.ToInt32(GetSetting("Puffergröße", 104857600));
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
    #endregion

    #region Static Addon Configuration
    private static List<AbstractAdditionalTagger> _addonAdditionalTaggers = [];
    private static Dictionary<string, AbstractCorpusBuilder> _addonBackends = new();
    private static List<object> _addonSideLoadFeatures = [];
    private static Dictionary<string, IAction> _addonConsoleActions = new();
    private static List<AbstractCrawler> _addonCrawlers = new();
    private static Dictionary<string, AbstractExporter> _addonExporters = new();
    private static Dictionary<string, AbstractImporter> _addonImporters = new();
    private static Dictionary<string, AbstractScraper> _addonScrapers = new();
    private static Dictionary<string, AbstractTableWriter> _addonTableWriters = new();
    private static List<AbstractTagger> _addonTaggers = [];
    #endregion
  }
}