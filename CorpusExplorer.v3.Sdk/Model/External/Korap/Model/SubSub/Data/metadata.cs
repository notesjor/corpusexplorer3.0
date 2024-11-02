#region

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#endregion

namespace CorpusExplorer.Sdk.Extern.Xml.Ids.KorAP.Model.SubSub.Data
{
  /// <remarks />
  [GeneratedCode("xsd", "4.8.3928.0")]
  [Serializable]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true, Namespace = "http://ids-mannheim.de/ns/KorAP")]
  [XmlRoot(Namespace = "http://ids-mannheim.de/ns/KorAP", IsNullable = false)]
  public class metadata
  {
    private string fileField;

    /// <remarks />
    [XmlAttribute(DataType = "NCName")]
    public string file
    {
      get => fileField;
      set => fileField = value;
    }
  }
}