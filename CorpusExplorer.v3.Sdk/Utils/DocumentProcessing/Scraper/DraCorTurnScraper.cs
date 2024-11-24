using System.Collections.Generic;
using System.Linq;
using CorpusExplorer.Sdk.Extern.Xml.Helper;
using CorpusExplorer.Sdk.Utils.DocumentProcessing.Scraper.Abstract;

namespace CorpusExplorer.Sdk.Extern.Xml.DraCor
{
  public class DraCorTurnScraper : AbstractScraper
  {
    public override string DisplayName => "DraCorTurn";

    protected override IEnumerable<Dictionary<string, object>> Execute(string file)
    {
      try
      {
        var html = new HtmlAgilityPackHelper(file);
        var castList = GetCastList(html);

        var doc = new Dictionary<string, object>
        {
          {"Titel", GetTitle(html)?.Trim()},
          {"Autor", GetAuthor(html)?.Trim()},
          {"Datum (Druck)", GetDatePrint(html)?.Trim()},
          {"Datum (Premiere)", GetDatePremiere(html)?.Trim()}
        };

        return GetTurns(html, castList, doc);
      }
      catch
      {
        // ignore
      }

      return null;
    }

    private IEnumerable<Dictionary<string, object>> GetTurns(HtmlAgilityPackHelper html,
      Dictionary<string, string> castList, Dictionary<string, object> doc)
    {
      var turns = html.GetNodes("//sp");
      var res = new List<Dictionary<string, object>>();

      foreach (var turn in turns)
      {
        var speaker = turn.GetAttributeValue("who", "");
        if(castList.ContainsKey(speaker))
          speaker = castList[speaker];

        var div = turn.ParentNode;
        var Szene = "";
        if (div != null && div.Name == "div" && div.GetAttributeValue("type", "") == "scene")
          Szene = div.GetAttributeValue("n", "?");

        var lines = turn.ChildNodes.Where(x => x.Name == "l")?.Select(x=>x.InnerText);
        var text = string.Join("\r\n", lines);

        var temp = doc.ToDictionary(x => x.Key, x => x.Value);
        temp.Add("Sprecher", speaker?.Trim());
        temp.Add("Text", text?.Trim().Replace("\t", " ").Replace("  ", " ").Replace("  ", " "));
        temp.Add("Szene", Szene.Trim());
        res.Add(temp);
      }

      return res;
    }

    private Dictionary<string, string> GetCastList(HtmlAgilityPackHelper html)
    {
      var roles = html.GetNodes("//role");
      var castList = new Dictionary<string, string>();
      foreach (var role in roles)
      {
        var key = role.GetAttributeValue("corresp", "");
        var value = role.InnerText;
        try
        {
          if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
            castList.Add(key, value);
        }
        catch
        {
          // ignore
        }
      }
      return castList;
    }

    private string GetDatePremiere(HtmlAgilityPackHelper html)
    {
      var tmp = html.GetNodes("//event[@type=\"premiere\"]")?.FirstOrDefault();
      return tmp?.GetAttributeValue("when", "");
    }

    private string GetDatePrint(HtmlAgilityPackHelper html)
    {
      var tmp = html.GetNodes("//event[@type=\"print\"]")?.FirstOrDefault();
      return tmp?.GetAttributeValue("when", "");
    }

    private string GetAuthor(HtmlAgilityPackHelper html)
    {
      var tmp = html.GetNodes("//author/persname");
      return tmp == null ? "" : string.Join("; ", tmp.Select(x => string.Join(" ", x.InnerText)));
    }

    private string GetTitle(HtmlAgilityPackHelper html)
    {
      var tmp = html.GetNodes("//title");
      return tmp == null ? "" : string.Join(" - ", tmp.Select(x=>string.Join(" ", x.InnerText)));
    }
  }
}