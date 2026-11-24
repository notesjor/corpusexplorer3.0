using System.Collections.Generic;
using System.Linq;
using CorpusExplorer.Sdk.Model.Adapter.Corpus.Abstract;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Abstract;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Builder;

namespace CorpusExplorer.Sdk.Utils.CorpusManipulation
{
  public static class LayerValueFilterCompundValues
  {
    public static AbstractCorpusAdapter FilterCompundLayerValues(this AbstractCorpusAdapter corpus, string layerDisplayname, string compundLayerValue, string compundSeparator = "|", string compundAssignmentOperator = "=", AbstractCorpusBuilder builder = null)
    {
      if (builder == null)
        builder = new CorpusBuilderWriteDirect();

      var layer = corpus.GetLayers(layerDisplayname).FirstOrDefault();
      var values = layer.Values.ToArray();
      
      var nvalues = new Dictionary<string, int>();
      nvalues.Add("", layer[""]);
      var changes = new Dictionary<int, int>();

      foreach (var v in values)
      {
        var split = v.Split(new string[] { compundSeparator }, System.StringSplitOptions.None)?.Select(x => x.Trim()).ToArray();
        if (split == null || split.Length == 0)
          continue;

        var replacement = "";
        foreach(var s in split)
        {
          var assign = s.Split(new string[] { compundAssignmentOperator }, System.StringSplitOptions.None)?.Select(x => x.Trim()).ToArray();
          if (assign == null || assign.Length != 2)
            continue;
          if (assign[0] == compundLayerValue)
            replacement = assign[1].Trim();
        }

        var oldIndex = layer[v];
        if (!nvalues.ContainsKey(replacement))
          nvalues.Add(replacement, nvalues.Count);
        var newIndex = nvalues[replacement];
        changes.Add(oldIndex, newIndex);
      }

      layer.ResetRawLayerDictionary(nvalues);

      foreach(var dsel in layer.DocumentGuids)
      {
        var doc = layer[dsel];
        for (var i = 0; i < doc.Length; i++)
        for (var j = 0; j < doc[i].Length; j++)
          doc[i][j] = changes[doc[i][j]];
        layer.AddDocumentByForce(dsel, doc);
      }

      return corpus;
    }
  }
}