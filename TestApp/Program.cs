using CorpusExplorer.Sdk.Ecosystem;
using CorpusExplorer.Sdk.Model.Adapter.Corpus;

namespace TestApp
{
  internal class Program
  {
    static void Main(string[] args)
    {
      CorpusExplorerEcosystem.Initialize([new CorpusExplorer.v3.Sdk.Full.EcosystemLayerFull()]);
      //CorpusExplorerEcosystem.Initialize();
      var corpus =
        CorpusAdapterWriteDirect.Create(@"C:\Users\Jan\Documents\CorpusExplorer\Meine Korpora\Bundespraesident.cec6");

      Console.WriteLine(corpus.CountToken);
    }
  }
}
