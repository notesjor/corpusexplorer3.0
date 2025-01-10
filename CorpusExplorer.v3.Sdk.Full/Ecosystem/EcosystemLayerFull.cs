using CorpusExplorer.v3.Sdk.Ecosystem;
using CorpusExplorer.v3.Sdk.Ecosystem.Model;

namespace CorpusExplorer.v3.Sdk.Full.Ecosystem
{
  public class EcosystemLayerFull : AbstractEcosystemLayer
  {
    public override void Initialize()
    {
      // Beispiel für chained initialization eines EcosystemLayers
      new EcosystemLayerDefault().Initialize();
      new EcosystemLayerNativeBinaryFormats().Initialize();
    }
  }
}
