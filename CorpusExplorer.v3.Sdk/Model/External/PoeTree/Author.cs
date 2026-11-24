using Newtonsoft.Json;

namespace CorpusExplorer.Sdk.Extern.Json.PoeTree.Model
{
  public class Author
  {

    /// <summary>
    /// Examples: "Uhland, Ludwig"
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Examples: "Q61058"
    /// </summary>
    [JsonProperty("wiki")]
    public string Wiki { get; set; }

    /// <summary>
    /// Examples: "73943536"
    /// </summary>
    [JsonProperty("viaf")]
    public string Viaf { get; set; }

    /// <summary>
    /// Examples: 1787
    /// </summary>
    [JsonProperty("born")]
    public string Born { get; set; }

    /// <summary>
    /// Examples: 1862
    /// </summary>
    [JsonProperty("died")]
    public string Died { get; set; }

    /// <summary>
    /// Examples: "de"
    /// </summary>
    [JsonProperty("country")]
    public string Country { get; set; }
  }
}
