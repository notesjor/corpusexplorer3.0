using CorpusExplorer.Sdk.Ecosystem.Model;
using CorpusExplorer.v3.Sdk.Ecosystem.Model;
using iText.Kernel.Geom;

namespace CorpusExplorer.v3.Sdk.Full
{
  public class EcosystemLayerFull : AbstractEcosystemLayer
  {
    public override void Initialize()
    {
      // Beispiel für chained initialization eines EcosystemLayers
      (new EcosystemLayerNativeBinaryFormats()).Initialize();      
    }
  }
}
