#region

using System.Data;
using CorpusExplorer.Sdk._3rdParty.Commercial.Metaphone3.Blocks;
using CorpusExplorer.Sdk.Addon;
using CorpusExplorer.Sdk.Model;
using CorpusExplorer.Sdk.Utils.DataTableWriter.Abstract;

#endregion

namespace CorpusExplorer.Sdk._3rdParty.Commercial.Metaphone3.Action
{
  public class CorrespondingMetaphoneValuesAction : IAction
  {
    public string Action => "corresponding-metaphone";

    public string Description =>
      "corresponding-metaphone [LAYER1] [LAYER2] - find all corresponding types betweet LAYER1 & LAYER2 (e. g. 1: Lemma / 2: Wort) based on metaphone";

    public void Execute(Selection selection, string[] args, AbstractTableWriter writer)
    {
      if (args == null || args.Length != 2)
        return;

      var block = selection.CreateBlock<CorrespondingMetaphoneLayerValueBlock>();
      block.Layer1Displayname = args[0];
      block.Layer2Displayname = args[1];
      block.Calculate();

      var dt = new DataTable();
      dt.Columns.Add(args[0], typeof(string));
      dt.Columns.Add(args[1], typeof(string));

      dt.BeginLoadData();
      foreach (var x in block.CorrespondingLayerValues)
        dt.Rows.Add(x.Key, string.Join(" | ", x.Value));
      dt.EndLoadData();

      writer.WriteTable(selection.Displayname, dt);
    }
  }
}