using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Cleanup.Abstract;

namespace CorpusExplorer.Sdk.Extern.NReadabilityHtmlCleanup
{
  public class NReadabilityCleanup : AbstractCleanup
  {
    private NReadability.NReadabilityTranscoder _transcoder;

    public NReadabilityCleanup()
    {
      _transcoder = new NReadability.NReadabilityTranscoder();
    }

    public override string DisplayName => "NReadability";
    
    protected override string Execute(string key, string input) => _transcoder.Transcode(input, out bool _);
  }
}
