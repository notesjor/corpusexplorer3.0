using CorpusExplorer.Sdk.Ecosystem;
using CorpusExplorer.Sdk.Model.Adapter.Corpus;

namespace TestApp
{
  internal class Program
  {
    static void Main(string[] args)
    {
      // Print current memory consumption of the app in KB
      System.Console.WriteLine(System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64 / 1024);

      //CorpusExplorerEcosystem.Initialize([new CorpusExplorer.v3.Sdk.Ecosystem.EcosystemLayerPure()]);
      //CorpusExplorerEcosystem.Initialize([new CorpusExplorer.v3.Sdk.Ecosystem.EcosystemLayerDefault()]);
      //CorpusExplorerEcosystem.Initialize([new CorpusExplorer.v3.Sdk.Full.Ecosystem.EcosystemLayerFull()]);
      
      System.Console.WriteLine(System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64 / 1024);

      //CorpusExplorerEcosystem.Initialize();
      var corpus =
        CorpusAdapterWriteDirect.Create(@"C:\Users\Jan\Documents\CorpusExplorer\Meine Korpora\Bundespraesident.cec6");

      Console.WriteLine(corpus.CountToken);
    }
  }
}
