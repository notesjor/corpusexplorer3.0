using CorpusExplorer.Sdk.Ecosystem.Model;
using CorpusExplorer.Sdk.Extern.FuzzyCloneDetection.Action;
using CorpusExplorer.Sdk.Model.Adapter.Corpus;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Builder;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Tagger.RawText;
using CorpusExplorer.v3.Sdk.Ecosystem.Model;

namespace CorpusExplorer.v3.Sdk
{
  public class EcosystemLayerNativeFunctions : AbstractEcosystemLayer
  {
    public override void Initialize()
    {
      Configuration.AddAdditionalTaggers(new AdditionalTsvValueTagger());

      Configuration.AddBackend("CorpusExplorer v6", new CorpusBuilderWriteDirect());           

      
    }
  }
}
