#region

using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Scraper.Abstract;
using Google.Apis.Blogger.v3.Data;

#endregion

namespace CorpusExplorer.Sdk.Extern.SocialMedia.Blogger.Data
{
  public class BloggerScraper : AbstractScraper
  {
    private readonly Regex _r1 = new Regex(@"<[^>]*>", RegexOptions.Compiled);
    public override string DisplayName => "Blogger";

    protected override IEnumerable<Dictionary<string, object>> Execute(string file)
    {
      Post post;
      var serializer = new DataContractSerializer(typeof(Post));
      using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
        post = serializer.ReadObject(fs) as Post;

      if (post == null)
        return null;

      return new[]
      {
        new Dictionary<string, object>
        {
          { "Autor", post.Author?.DisplayName },
          { "AutorId", post.Author?.Id },
          { "BlogId", post.Blog?.Id },
          { "Text", _r1.Replace(post.Content, string.Empty) },
          { "ETag", post.ETag },
          { "Id", post.Id },
          { "Typ", post.Kind },
          { "Schlagworte", post.Labels == null ? "" : string.Join("|", post.Labels) },
          { "Latitude", post.Location?.Lat },
          { "Longitude", post.Location?.Lng },
          { "Datum", post.Published },
          { "Titel", post.Title },
          { "Url", post.Url },
        }
      };
    }
  }
}