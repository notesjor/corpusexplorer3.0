#region

using System.Collections.Generic;
using System.Linq;
using CorpusExplorer.Sdk.Model.Adapter.Layer.Abstract;
using CorpusExplorer.Sdk.Utils.Filter.Abstract;

#endregion

namespace CorpusExplorer.Sdk._3rdParty.Commercial.Metaphone3.Filter
{
  public class FilterQuerySingleLayerFuzzy : AbstractFilterQuerySingleLayer
  {
    public override string Verbal
    {
      get
      {
        var values = string.Join(", ", LayerQueries);
        if (values.Length > 50)
          values = values.Substring(0, 47) + "...";
        return $"Min. ein Fuzzy-Wert im Dokument ({values}).";
      }
    }

    public override object Clone()
    {
      return new FilterQuerySingleLayerFuzzy
      {
        Inverse = Inverse,
        LayerDisplayname = LayerDisplayname,
        LayerQueries = LayerQueries,
        OrFilterQueries = OrFilterQueries.Select(q => q.Clone() as AbstractFilterQuery)
      };
    }

    protected override HashSet<int> GetCachedIndicesCall(AbstractLayerAdapter layer, IEnumerable<string> layerQueries)
    {
      var metaphone = new Metaphone3Deutsch();

      var valid = new HashSet<string>(layerQueries.Select(query => metaphone.Encode(query)));
      return new HashSet<int>(layer.ReciveRawLayerDictionary()
                                   .Where(x => valid.Contains(metaphone.Encode(x.Key)))
                                   .Select(x => x.Value));
    }

    protected override IEnumerable<int> GetSentencesCall(int[][] document, HashSet<int> indices)
    {
      var res = new List<int>();
      for (var i = 0; i < document.Length; i++)
        res.AddRange(from w in document[i] where indices.Contains(w) select i);

      return res;
    }

    protected override bool ValidateCallCall(int[][] document, HashSet<int> indices)
    {
      return document.SelectMany(s => s).Any(indices.Contains);
    }
  }
}