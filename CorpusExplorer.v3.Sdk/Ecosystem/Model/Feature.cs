using System.IO.Compression;
using CorpusExplorer.Sdk.Ecosystem.Model;
using CorpusExplorer.Sdk.Helper;
using CorpusExplorer.v3.Sdk.Helper;
using RestSharp;

namespace CorpusExplorer.v3.Sdk.Plus.Model.Abstract
{
  public class Feature
  {
    private string _latestVersion = "";

    public string UrlPackage { get; }
    public string UrlVersion { get; }
    public string PathPackage { get; }
    public string PathVersion { get; }
    private string Result { get; }

    public Feature(string urlPackage, string urlVersion, string pathPackage, string pathVersion, string result)
    {
      UrlPackage = urlPackage.Replace("{cpu}", OSPlatformHelper.GetOSPlatformArchitecture());
      UrlVersion = urlVersion;
      PathPackage = pathPackage;
      PathVersion = pathVersion;
      Result = result;
    }

    public bool IsInstalled => Directory.Exists(PathPackage);

    public bool CheckUpdate()
    {
      try
      {
        var currentVersion = File.ReadAllText(PathVersion);
        using (var client = new RestClient())
        {
          var response = client.Execute(new RestRequest(UrlVersion));
          if (response.IsSuccessful)
          {
            _latestVersion = response.Content;
            if (string.IsNullOrWhiteSpace(_latestVersion))
              return false;
            return currentVersion != _latestVersion;
          }
        }
      }
      catch
      {
        // ignore
      }

      return false;
    }

    public void Install()
    {
      if (string.IsNullOrWhiteSpace(_latestVersion))
        CheckUpdate();

      using (var tmp = new TemporaryFile(Configuration.TempPath))
      {
        using (var client = new RestClient())
          File.WriteAllBytes(tmp.Path, client.DownloadData(new RestRequest(UrlPackage)));
        ZipFile.ExtractToDirectory(tmp.Path, PathPackage);
        File.WriteAllText(PathVersion, _latestVersion);
      }
    }

    public void Uninstall()
    {
      using (var tmp = new TemporaryFile(Configuration.TempPath))
      {
        using (var client = new RestClient())
          File.WriteAllBytes(tmp.Path, client.DownloadData(new RestRequest(UrlPackage)));

        using (var zip = ZipFile.OpenRead(tmp.Path))
        {
          foreach (var entry in zip.Entries)
          {
            var path = Path.Combine(PathPackage, entry.FullName);
            if (File.Exists(path))
              File.Delete(path);
          }
        }

        foreach (var dir in Directory.GetDirectories(PathPackage))
          if (Directory.GetFiles(dir).Length == 0)
            Directory.Delete(dir, true);
      }
    }

    public string Ensure()
    {
      if (!IsInstalled)
        Install();
      else
      {
        if (CheckUpdate())
          Install();
      }

      return Result;
    }
  }
}