using System.Collections.Generic;
using CorpusExplorer.Sdk.Helper;
using CorpusExplorer.Sdk.Model.Adapter.Corpus;
using CorpusExplorer.Sdk.Model.Adapter.Corpus.Abstract;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Importer.Abstract;

namespace CorpusExplorer.Sdk.Utils.DocumentProcessing.Importer.CorpusExplorerV6
{
  public class ImporterCec6 : AbstractImporter
  {
    protected override IEnumerable<AbstractCorpusAdapter> Execute(string importFilePath)
    {
      var corpus = CorpusAdapterWriteDirect.Create(importFilePath);
      corpus.EnsureAllLayersHaveSameDocumentGuids();
      return new[] { corpus };
    }

    public override bool HasStaticGuids => true;
  }
}