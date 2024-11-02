#region

using System;
using System.Collections.Generic;
using CorpusExplorer.Sdk._3rdParty.Commercial.Metaphone3;
using CorpusExplorer.Sdk.Blocks.Abstract;
using CorpusExplorer.Sdk.Model.Adapter.Layer.Abstract;

#endregion

namespace CorpusExplorer.Sdk.Blocks
{
  public class MetaphoneLayerValueBlock : AbstractLayerValueBlock
  {
    private readonly object _clvLock = new object();

    public Dictionary<string, HashSet<string>> CorrespondingLayerValues { get; set; }

    protected override void CalculateCall(Guid dsel, AbstractLayerAdapter layer)
    {
      var tmp = new Dictionary<string, HashSet<string>>();
      var mp = new Metaphone3Deutsch();

      foreach (var v in layer.Values)
      {
        var m = mp.Encode(v);
        if (tmp.ContainsKey(m))
          tmp[m].Add(v);
        else
          tmp.Add(m, new HashSet<string> { v });
      }

      lock (_clvLock)
        foreach (var v in tmp)
          if (CorrespondingLayerValues.ContainsKey(v.Key))
            foreach (var x in v.Value)
              CorrespondingLayerValues[v.Key].Add(x);
          else
            CorrespondingLayerValues.Add(v.Key, v.Value);
    }
  }
}