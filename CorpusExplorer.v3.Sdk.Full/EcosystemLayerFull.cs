using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpusExplorer.v3.Sdk.Ecosystem.Model;

namespace CorpusExplorer.v3.Sdk.Full
{
  public class EcosystemLayerFull : AbstractEcosystemLayer
  {
    public override void Initialize()
    {
      (new EcosystemLayerNativeBinaryFormats()).Initialize();
      (new EcosystemLayerNativeFunctions()).Initialize();
    }
  }
}
