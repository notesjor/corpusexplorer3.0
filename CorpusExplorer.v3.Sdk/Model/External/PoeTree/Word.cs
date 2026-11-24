using Newtonsoft.Json;

namespace CorpusExplorer.Sdk.Extern.Json.PoeTree.Model
{
  public class Word
  {

    ///// <summary>
    ///// Examples: 1, 2, 3, 4, 5
    ///// </summary>
    //[JsonProperty("id")]
    //public int Id { get; set; }

    /// <summary>
    /// Examples: "Der", "Jüngling", "steht", "auf", "dem"
    /// </summary>
    [JsonProperty("form")]
    public string Form { get; set; }

    /// <summary>
    /// Examples: "Der", "Jüngling", "stehen", "auf", "dem"
    /// </summary>
    [JsonProperty("lemma")]
    public string Lemma { get; set; }

    /// <summary>
    /// Examples: "DET", "NOUN", "VERB", "ADP", "PUNCT"
    /// </summary>
    [JsonProperty("upos")]
    public string Upos { get; set; }

    /// <summary>
    /// Examples: "ART", "NN", "VVFIN", "APPR", "$,"
    /// </summary>
    [JsonProperty("xpos")]
    public string Xpos { get; set; }

    /// <summary>
    /// Examples: "Case=Nom|Gender=Masc|Number=Sing|PronType=Art", "Gender=Masc|Number=Sing|Person=3", "Mood=Ind|Number=Sing|Person=3|Tense=Pres|VerbForm=Fin", "AdpType=Prep|Case=Dat", "Case=Dat|Gender=Masc|Number=Sing|PronType=Art"
    /// </summary>
    [JsonProperty("feats")]
    public string Feats { get; set; }

    ///// <summary>
    ///// Examples: 2, 3, 0, 6, 8
    ///// </summary>
    //[JsonProperty("head")]
    //public int Head { get; set; }

    /// <summary>
    /// Examples: "det", "nsubj", "root", "case", "obl"
    /// </summary>
    [JsonProperty("deprel")]
    public string Deprel { get; set; }

    /// <summary>
    /// Examples: "_"
    /// </summary>
    [JsonProperty("deps")]
    public string Deps { get; set; }    
  }
}