using CorpusExplorer.Sdk.Model.Adapter.Corpus.Abstract;
using System.Linq;

namespace CorpusExplorer.Sdk.Helper
{
  public static class CorpusAutoFixHelper
  {
    public static void EnsureAllLayersHaveSameDocumentGuids(this AbstractCorpusAdapter corpus)
    {
      var layerCount = corpus.Layers.Count();
      var guids = corpus.Layers.SelectMany(x => x.DocumentGuids).Distinct().ToArray();
      foreach (var guid in guids)
      {
        var count = corpus.Layers.Count(x => x.DocumentGuids.Contains(guid));
        if (count == layerCount)
          continue;

        var first = corpus.Layers.FirstOrDefault(x => x.DocumentGuids.Contains(guid));
        var template = first[guid];
        var empty = new int[template.Length][];
        for (var i = 0; i < template.Length; i++)
        {
          var n = new int[template[i].Length];
          for (var j = 0; j < n.Length; j++)
            n[j] = -1;
          empty[i] = n;
        }

        foreach (var layer in corpus.Layers)
        {
          if (layer.DocumentGuids.Contains(guid))
            continue;
          layer.AddDocumentByForce(guid, empty);
        }
      }
    }
  }
}
