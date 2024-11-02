#region

using System.Text;
using System.Xml;
using System.Xml.Serialization;
using CorpusExplorer.Sdk.Diagnostic;
using CorpusExplorer.Sdk.Ecosystem.Model;
using CorpusExplorer.Sdk.Extern.Xml.Catma._6._0;
using Newtonsoft.Json;

#endregion

namespace CorpusExplorer.Sdk.Helper
{
  /// <summary>
  ///   The serializer.
  /// </summary>
  public static class Serializer
  {
    public static T? Deserialize<T>(string path) where T : class
    {
      try
      {
        return JsonConvert.DeserializeObject<T>(File.ReadAllText(path, Configuration.Encoding));
      }
      catch (Exception ex)
      {
        InMemoryErrorConsole.Log(ex);
        return null;
      }
    }

    public static void Serialize(object obj, string path)
    {
      try
      {
        using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
        using (var bs = new BufferedStream(fs))
        using (var write = new StreamWriter(bs, Configuration.Encoding))
        {
          var json = new JsonSerializer();
          json.Serialize(write, obj);
        }
      }
      catch (Exception ex)
      {
        InMemoryErrorConsole.Log(ex);
      }
    }

    public static T? DeserializeInMemory<T>(string serializedJson) where T : class
    {
      try
      {
        return JsonConvert.DeserializeObject<T>(serializedJson);
      }
      catch (Exception ex)
      {
        InMemoryErrorConsole.Log(ex);
        return null;
      }
    }

    public static string SerializeInMemory(object obj)
    {
      try
      {
        using (var ms = new MemoryStream())
        {
          using (var write = new StreamWriter(ms, Configuration.Encoding))
          {
            var json = new JsonSerializer();
            json.Serialize(write, obj);
          }
          return Encoding.UTF8.GetString(ms.ToArray());
        }
      }
      catch (Exception ex)
      {
        InMemoryErrorConsole.Log(ex);
        return "";
      }
    }

    public static void SerializeXml<T>(T obj, string path)
    {
      try
      {
        using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
        using (var writer = new XmlTextWriter(fs, Configuration.Encoding))
        {
          var xml = new XmlSerializer(typeof(T));
          xml.Serialize(writer, obj);
        }
      }
      catch (Exception ex)
      {
        InMemoryErrorConsole.Log(ex);
      }
    }

    public static string SerializeXmlInMemory<T>(T obj)
    {
      try
      {
        using (var ms = new MemoryStream())
        {
          using (var writer = new XmlTextWriter(ms, Configuration.Encoding))
          {
            var xml = new XmlSerializer(typeof(T));
            xml.Serialize(writer, obj);
          }
          return Configuration.Encoding.GetString(ms.ToArray());
        }
      }
      catch (Exception ex)
      {
        InMemoryErrorConsole.Log(ex);
        return "";
      }
    }
  }
}