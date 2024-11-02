#region

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using CorpusExplorer.Sdk.Ecosystem.Model;
using CorpusExplorer.Sdk.Helper;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Scraper.Abstract;
using CorpusExplorer.v3.Sdk.Plus.Model.Abstract;
using HtmlAgilityPack;

#endregion

namespace CorpusExplorer.Sdk.Extern.Pandoc.Pandoc
{
  public class PandocScraper : AbstractScraper
  {
    public enum InputFormat
    {
      commonmark,
      docbook,
      docx,
      epub,
      haddock,
      html,
      json,
      latex,
      markdown,
      markdown_github,
      markdown_mmd,
      markdown_phpextra,
      markdown_strict,
      mediawiki,
      native,
      odt,
      opml,
      org,
      rst,
      t2t,
      textile,
      twiki
    }

    public override string DisplayName => "PANDOC";

    public InputFormat Format { get; set; } = InputFormat.docx;

    public Feature _pandocInstallation = new Feature()

    protected override IEnumerable<Dictionary<string, object>> Execute(string file)
    {
      using (var outputTemp = new TemporaryFile(Configuration.TempPath))
      {
        var pandoc = new Process
        {
          StartInfo =
            new ProcessStartInfo
            {
              FileName = Configuration.GetDependencyPath(@"pandoc\pandoc.exe"),
              Arguments =
                $"-s -S --normalize --no-highlight --wrap=none -f {Format} -t html \"{file}\" -o \"{outputTemp.Path}\"",
              CreateNoWindow = false,
              WindowStyle = ProcessWindowStyle.Hidden
            }
        };
        pandoc.Start();
        pandoc.WaitForExit();

        var doc = new HtmlDocument();
        using (var fs = new FileStream(outputTemp.Path, FileMode.Open, FileAccess.Read))
        using (var bs = new BufferedStream(fs))
          doc.Load(bs, Configuration.Encoding);

        return new[]
        {
          new Dictionary<string, object>
          {
            { "Text", doc.DocumentNode.SelectSingleNode("//body").InnerText },
            { "Datei", file }
          }
        };
      }
    }
  }
}