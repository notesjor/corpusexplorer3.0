using Newtonsoft.Json;

namespace CorpusExplorer.Sdk.Extern.Json.PoeTree.Model
{
  public class Source
  {

    /// <summary>
    /// Examples: "wpnh.0"
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// Examples: "Ludwig Uhland: Werke. Herausgegeben von Hartmut Fröschle und Walter Scheffler, Band 1–4, München: Winkler, 1980."
    /// </summary>
    [JsonProperty("title")]
    public string Title { get; set; }

    /// <summary>
    /// Examples: 1980
    /// </summary>
    [JsonProperty("year_published")]
    public string YearPublished { get; set; }

    /// <summary>
    /// Examples: null
    /// </summary>
    [JsonProperty("publisher")]
    public string Publisher { get; set; }

    /// <summary>
    /// Examples: "München"
    /// </summary>
    [JsonProperty("place")]
    public string Place { get; set; }

    /// <summary>
    /// Examples: "metricalizer"
    /// </summary>
    [JsonProperty("corpus")]
    public string Corpus { get; set; }
  }
}