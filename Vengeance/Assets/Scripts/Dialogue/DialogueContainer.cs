using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("DialogueCollection")] //points to the root of our XML tree, which represents a collection of dialogue

public class DialogueContainer {

    [XmlArray("LevelDialogue")]
    [XmlArrayItem("Dialogue")]
    public List<Dialogue> levelDialogue = new List<Dialogue>();
}
