using System.Collections.Generic;
using Bcs.IO;
using CorpusExplorer.Sdk.Extern.Json.PoeTree.Model;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Importer.Abstract;
using Newtonsoft.Json;

namespace CorpusExplorer.Sdk.Extern.Json.PoeTree
{
  public class ImporterPoeTree : AbstractImporterBase
  {
    protected override void ExecuteCall(string path)
    {
      var root = JsonConvert.DeserializeObject<Root>(FileIO.ReadText(path));

      if (root.Body == null)
        return;

      var guid = System.Guid.NewGuid();

      var meta = new Dictionary<string, object>();
      if (root.Author != null)
      {
        meta.Add("Autor", root.Author.Name);
        meta.Add("Autor (Wikidata QID)", root.Author.Wiki);
        meta.Add("Autor (VIAF)", root.Author.Viaf);
        meta.Add("Autor (Geboren)", root.Author.Born);
        meta.Add("Autor (Gestorben)", root.Author.Died);
        meta.Add("Autor (Land)", root.Author.Country);
      }

      if (root.Source != null)
      {
        meta.Add("Quelle (Id)", root.Source.Id);
        meta.Add("Quelle (Titel)", root.Source.Title);
        meta.Add("Quelle (Publiziert - Wann?)", root.Source.YearPublished);
        meta.Add("Quelle (Publiziert - Von wem?)", root.Source.Publisher);
        meta.Add("Quelle (Publiziert - Wo?)", root.Source.Place);
        meta.Add("Quelle (Korpus)", root.Source.Corpus);
      }

      meta.Add("Titel", root.Title);
      meta.Add("Jahr", root.YearCreated);
      AddDocumentMetadata(guid, meta);

      var layer_w = new List<string[]>();
      var layer_l = new List<string[]>();
      var layer_u = new List<string[]>();
      var layer_x = new List<string[]>();
      var layer_f = new List<string[]>();
      var layer_d = new List<string[]>();

      foreach (var sentence in root.Body)
      {
        var s_w = new List<string>();
        var s_l = new List<string>();
        var s_u = new List<string>();
        var s_x = new List<string>();
        var s_f = new List<string>();
        var s_d = new List<string>();

        foreach (var word in sentence.Words)
        {
          s_w.Add(word.Form);
          s_l.Add(word.Lemma);
          s_u.Add(word.Upos);
          s_x.Add(word.Xpos);
          s_f.Add(word.Feats);
          s_d.Add(word.Deprel);
        }

        layer_w.Add(s_w.ToArray());
        layer_l.Add(s_l.ToArray());
        layer_u.Add(s_u.ToArray());
        layer_x.Add(s_x.ToArray());
        layer_f.Add(s_f.ToArray());
        layer_d.Add(s_d.ToArray());
      }

      AddDocument("Wort", guid, layer_w.ToArray());
      AddDocument("Lemma", guid, layer_l.ToArray());
      AddDocument("UPOS", guid, layer_u.ToArray());
      AddDocument("XPOS", guid, layer_x.ToArray());
      AddDocument("Feats", guid, layer_f.ToArray());
      AddDocument("Deprel", guid, layer_d.ToArray());
    }
  }
}
