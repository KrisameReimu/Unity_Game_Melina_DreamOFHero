using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parser : MonoBehaviour
{
    public static Parser parser { get; set; }
    DialogueManager manager;
    [HideInInspector]
    public bool lineFinished = false;
    public bool nodeFinished = false;
    void Awake()
    {
        manager = DialogueManager.manager;
        if (parser == null) { parser = this; }
    }

    //removes tags from raw lines and returns the lines
    public string Line(string rawLine)
    {
        string[] lineTags = { "/end", "/line" };
        string newLine = string.Empty;
        foreach (string tag in lineTags)
        {
            if (rawLine.EndsWith(tag))
            {
                newLine = rawLine.Replace(tag, string.Empty).TrimEnd();
            }
        }       
        return newLine;
    }

    //checks for tags and/or punctuation at the end of each raw line
    public void CheckLine(string rawLine)
    {
        //check for tags
        string[] tags = { "/line", "/end" };
        
        if (rawLine.EndsWith(tags[0]))
        {
           lineFinished = true;
        }
        if (rawLine.EndsWith(tags[1]))
        {
           nodeFinished = true;
        }
    }  
}
