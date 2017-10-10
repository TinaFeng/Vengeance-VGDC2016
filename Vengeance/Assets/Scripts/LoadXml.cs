using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Xml;
using System.IO;

public class Type_Dialogue
{
    //load an entire file into a list of character name, character protrait, and the dialogue

    //make an empty game object and name it loader and just throw this script in and add the xml file

    //Currently just for straight up playing dialogues, figuring out triggers later

    public string name;
    public string icon;
    public string text;

}

public class LoadXml : MonoBehaviour {

    public TextAsset script;

    public List<Type_Dialogue> dialogues = new List<Type_Dialogue>();

    private void Start()
    {
        GetScene();

    }
    public void GetScene()
    {
        XmlDocument xmlDoc = new XmlDocument();//create an xml
        xmlDoc.LoadXml(script.text);//load?
        XmlNodeList SceneList = xmlDoc.GetElementsByTagName("Scene");//array of scene Nodes

        foreach(XmlNode SceneInfo in SceneList)
        {
            XmlNodeList SceneContent = SceneInfo.ChildNodes;

           
            foreach(XmlNode SceneItems in SceneContent)
            {
                if(SceneItems.Name == "object")
                {
                    Type_Dialogue line = new Type_Dialogue();

                    line.name = SceneItems.Attributes["name"].Value;//person speaking

                    string[] two_other = new string[2];//split the icon and text
                    two_other =SceneItems.InnerText.Split('*');
                
                    line.icon = two_other[0];
                    line.text = two_other[1];

                    //Debug.Log(line.name);
                    //Debug.Log(line.icon);
                    //Debug.Log(line.text);
                    dialogues.Add(line);
                }

            }
            
        }
        
    }
}
