using CorpusExplorer.v3.Sdk.Ecosystem.Model;

namespace CorpusExplorer.v3.Sdk.Full
{
  public class EcosystemLayerFull : AbstractEcosystemLayer
  {
    public override void Initialize()
    {
      (new EcosystemLayerNativeFunctions()).Initialize();
      (new EcosystemLayerNativeBinaryFormats()).Initialize();      
    }
  }
}
