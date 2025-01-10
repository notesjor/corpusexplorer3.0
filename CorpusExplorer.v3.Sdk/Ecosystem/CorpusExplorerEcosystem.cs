#region

using System.Net;
using CorpusExplorer.Sdk.Ecosystem.Model;
using CorpusExplorer.Sdk.Model;
using CorpusExplorer.Sdk.Model.Cache;
using CorpusExplorer.Sdk.Model.Cache.Abstract;
using CorpusExplorer.Sdk.Terminal;
using CorpusExplorer.v3.Sdk;
using CorpusExplorer.v3.Sdk.Ecosystem.Model;

#endregion

namespace CorpusExplorer.Sdk.Ecosystem
{
  public static class CorpusExplorerEcosystem
  {
    public static TerminalController CurrentTerminalController { get; private set; }
    public static Project CurrentProject => CurrentTerminalController?.Project;
    public static bool RunSslFix { get; set; } = true;

    /// <summary>
    ///   Initialisiert ein vollständiges CorpusExplorer-Ökosystem.
    /// </summary>
    /// <param name="ecosystemLayers">Lädt zusätzliche Ecosystem-Layer</param>
    /// <param name="cacheStrategy">
    ///   Erlaubt es das Block-Caching-Verhalten zu ändern. Für Initialize(null) ist
    ///   CacheStrategyCurrentSelection festgelegt.
    /// </param>
    /// <returns>TerminalController - erlaubt das Verwalten von Projekten</returns>
    public static TerminalController Initialize(AbstractEcosystemLayer[]? ecosystemLayers = null, AbstractCacheStrategy? cacheStrategy = null)
    {
      SslFix();

      Configuration.Initialize();
      Configuration.Cache = cacheStrategy ?? new CacheStrategyCurrentSelection();
      if (ecosystemLayers != null)
        foreach (var layer in ecosystemLayers)
          layer.Initialize();

      CurrentTerminalController = new TerminalController();
      CurrentTerminalController.ProjectNew();

      return CurrentTerminalController;
    }

    private static void SslFix()
    {
      if (!RunSslFix)
        return;
      try
      {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
      }
      catch
      {
        // ignore
      }
    }
  }
}