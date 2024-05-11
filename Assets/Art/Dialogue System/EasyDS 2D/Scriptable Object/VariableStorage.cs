using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Variable", menuName = "ScriptableObjects/VariableObject", order = 2)]
public class VariableStorage : ScriptableObject
{
    public Dictionary<string, object> variables;
}
