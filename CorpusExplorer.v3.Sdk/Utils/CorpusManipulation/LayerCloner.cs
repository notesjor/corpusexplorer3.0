using System.Linq;
using CorpusExplorer.Sdk.Model.Adapter.Corpus.Abstract;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Abstract;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Builder;

namespace CorpusExplorer.Sdk.Utils.CorpusManipulation
{
  public static class LayerCloner
  {
    public static AbstractCorpusAdapter CloneLayer(this AbstractCorpusAdapter corpus, string layerDisplayname, string newLayerDisplayname, AbstractCorpusBuilder builder = null)
    {
      if (builder == null)
        builder = new CorpusBuilderWriteDirect();

      var layers = corpus.Layers.Select(x => x.ToLayerState()).ToList();
      layers.Add(corpus.GetLayers(layerDisplayname).FirstOrDefault().ToLayerState(newLayerDisplayname, corpus.Layers.Count()));

      return builder.Create(layers,
        corpus.DocumentMetadata.ToDictionary(x => x.Key, x => x.Value),
        corpus.GetCorpusMetadata().ToDictionary(x => x.Key, x => x.Value),
        corpus.Concepts?.ToList())
        .FirstOrDefault();
    }
  }
}
