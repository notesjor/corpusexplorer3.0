using System;
using System.Collections.Generic;
using System.Linq;
using Bcs.IO;
using CorpusExplorer.Sdk.Extern.Json.PoeTree.Model;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Scraper.Abstract;
using Newtonsoft.Json;

namespace CorpusExplorer.Sdk.Extern.Json.PoeTree
{
  public class PoeTreeScraper : AbstractScraper
  {
    public override string DisplayName => "PoeTree";

    protected override IEnumerable<Dictionary<string, object>> Execute(string file)
    {
      var root = JsonConvert.DeserializeObject<Root>(FileIO.ReadText(file));

      if (root.Body == null)
        return Array.Empty<Dictionary<string, object>>();

      var res = new Dictionary<string, object>();
      if (root.Author != null)
      {
        res.Add("Autor", root.Author.Name);
        res.Add("Autor (Wikidata QID)", root.Author.Wiki);
        res.Add("Autor (VIAF)", root.Author.Viaf);
        res.Add("Autor (Geboren)", root.Author.Born);
        res.Add("Autor (Gestorben)", root.Author.Died);
        res.Add("Autor (Land)", root.Author.Country);
      }

      if (root.Source != null)
      {
        res.Add("Quelle (Id)", root.Source.Id);
        res.Add("Quelle (Titel)", root.Source.Title);
        res.Add("Quelle (Publiziert - Wann?)", root.Source.YearPublished);
        res.Add("Quelle (Publiziert - Von wem?)", root.Source.Publisher);
        res.Add("Quelle (Publiziert - Wo?)", root.Source.Place);
        res.Add("Quelle (Korpus)", root.Source.Corpus);
      }

      res.Add("Titel", root.Title);
      res.Add("Jahr", root.YearCreated);
      res.Add("Text", string.Join("\r\n", root.Body.Select(x => x.Text)));
      
      return new[] { res };
    }
  }
}
