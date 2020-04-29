using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class xmlReader : MonoBehaviour
{
    public static xmlReader xmlReaderAccess;

    void Awake()
    {
        xmlReaderAccess = this;
    }

    public void ReadXml(string FilePath, string NodeName, string[] FieldName, List<string[]> ResultList)
    {
        TextAsset textAsset = (TextAsset)Resources.Load(FilePath);

        XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.LoadXml(textAsset.text);

        XmlNodeList Nodes = XmlDoc.SelectNodes(NodeName);

        foreach(XmlNode Node in Nodes)
        {
            string[] Result = new string[FieldName.Length];
            for(int i = 0; i < FieldName.Length; i++)
            {
                Result[i] = Node.SelectSingleNode(FieldName[i]).InnerText;
            }
            ResultList.Add(Result);
        }
    }
}
