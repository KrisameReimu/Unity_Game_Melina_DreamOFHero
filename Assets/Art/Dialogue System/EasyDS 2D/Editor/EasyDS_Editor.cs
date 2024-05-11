using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EasyDS))]
public class EasyDS_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space(10);
               
        if (GUILayout.Button("Create Dialogue"))
        {
            CreateDialogue();
        }

    }
    public void CreateDialogue()
    {
        EasyDS script = (EasyDS)target;
        Dialogue dialogue = ScriptableObject.CreateInstance<Dialogue>();
        string assetName = script.Name + "_Dialogue_";
        string path = "Assets/EasyDS 2D/Nodes/" + assetName + ".asset";
        script.dialogue.Add(dialogue);
        AssetDatabase.CreateAsset(dialogue, AssetDatabase.GenerateUniqueAssetPath(path));
    }
}
