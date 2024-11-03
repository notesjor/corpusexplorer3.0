using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpusExplorer.v3.Sdk.Full.Model.YouTube
{
  public class YouTubeSearchServiceCommentThread : YouTubeSearchServiceComment
  {
    public string ParentId { get; set; }
    public long? ReplyCount { get; set; }
  }
}
