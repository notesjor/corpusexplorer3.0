using CorpusExplorer.Sdk.Aspect;
using Metalama.Framework.Aspects;
using Metalama.Framework.Fabrics;

namespace CorpusExplorer.v3.Sdk
{
  public class GlobalAspects : ProjectFabric
  {
    public override void AmendProject(IProjectAmender amender)
    {
      amender
        .SelectTypes()
        .SelectMany(type => type.Methods)
        .AddAspectIfEligible<ErrorReportingAspect>();
    }
  }
}
