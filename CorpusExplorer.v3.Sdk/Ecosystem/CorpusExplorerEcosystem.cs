#region

using System.Net;
using CorpusExplorer.Sdk.Ecosystem.Model;
using CorpusExplorer.Sdk.Model;
using CorpusExplorer.Sdk.Model.Cache;
using CorpusExplorer.Sdk.Model.Cache.Abstract;
using CorpusExplorer.Sdk.Terminal;

#endregion

namespace CorpusExplorer.Sdk.Ecosystem
{
  public static class CorpusExplorerEcosystem
  {
    public static TerminalController CurrentTerminalController { get;private set; }
    public static Project CurrentProject => CurrentTerminalController?.Project;

    /// <summary>
    ///   Initialisiert ein vollständiges CorpusExplorer-Ökosystem.
    /// </summary>
    /// <param name="cacheStrategy">
    ///   Erlaubt es das Block-Caching-Verhalten zu ändern. Für Initialize(null) ist
    ///   CacheStrategyCurrentSelection festgelegt.
    /// </param>
    /// <returns>TerminalController - erlaubt das Verwalten von Projekten</returns>
    public static TerminalController Initialize(AbstractCacheStrategy? cacheStrategy = null)
    {
      SslFix();
      Configuration.Initialize();
      Configuration.Cache = cacheStrategy ?? new CacheStrategyCurrentSelection();
      CurrentTerminalController = new TerminalController();
      CurrentTerminalController.ProjectNew();

      return CurrentTerminalController;
    }

    private static void SslFix()
    {
      try
      {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = (SecurityProtocolType) 3072;
      }
      catch
      {
        // ignore
      }
    }
  }
}