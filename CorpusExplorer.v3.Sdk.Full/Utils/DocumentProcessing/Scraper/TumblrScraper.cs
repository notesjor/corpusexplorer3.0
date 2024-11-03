#region

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Compat.Runtime.Serialization;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Scraper.Abstract;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;

#endregion

namespace CorpusExplorer.Sdk.Extern.SocialMedia.Tumblr.Data
{
  public class TumblrScraper : AbstractScraper
  {
    private readonly Regex _r1 = new Regex(@"<[^>]*>", RegexOptions.Compiled);
    public override string DisplayName => "Tumblr";

    protected override IEnumerable<Dictionary<string, object>> Execute(string file)
    {
      BasePost post;
      var serializer = new NetDataContractSerializer();
      using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
        post = serializer.Deserialize(fs) as BasePost;

      if (post == null)
        return null;

      var content = post.Reblog?.TreeHtml;
      if (string.IsNullOrEmpty(content))
        content = post.Trails?[0].ContentRaw;

      return new[]
      {
        new Dictionary<string, object>
        {
          { "BlogName", post.BlogName },
          { "Id", post.Id },
          { "Slug", post.Slug },
          { "Url", post.Url },
          { "Likes", post.Notes?.Where(x => x.Type   == NoteType.Like) },
          { "Reblogs", post.Notes?.Where(x => x.Type == NoteType.Reblog) },
          { "Datum", post.Timestamp },
          { "Tags", post.Tags == null ? "" : string.Join("|", post.Tags) },
          { "Comment", post.Reblog?.Comment },
          { "Text", _r1.Replace(content, string.Empty) },
        }
      };
    }
  }
}