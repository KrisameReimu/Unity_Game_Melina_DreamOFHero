using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/DialogueObject", order = 1)]
public class Dialogue : ScriptableObject
{
    [TextArea(1,4)]
    public List<string> dialogue;
}
