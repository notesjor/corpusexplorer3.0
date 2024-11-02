#region

using System.Data;
using CorpusExplorer.Sdk.Addon;
using CorpusExplorer.Sdk.Blocks;
using CorpusExplorer.Sdk.Model;
using CorpusExplorer.Sdk.Utils.DataTableWriter.Abstract;

#endregion

namespace CorpusExplorer.Sdk._3rdParty.Commercial.Metaphone3.Action
{
  public class MetaphoneValuesAction : IAction
  {
    public string Action => "metaphone";
    public string Description => "metaphone [LAYER] - convert all types (in layer) into metaphone representations.";

    public void Execute(Selection selection, string[] args, AbstractTableWriter writer)
    {
      if (args == null || args.Length != 2)
        return;

      var block = selection.CreateBlock<MetaphoneLayerValueBlock>();
      block.LayerDisplayname = args[0];
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