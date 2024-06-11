using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TextDialogue
{
    [TextArea(1,2)]
    public string[] sentences;
    public Sprite[] dialogueWindow;
}
