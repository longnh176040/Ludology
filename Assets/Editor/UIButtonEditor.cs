using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(UIButton))]
public class UIButtonEditor : ButtonEditor
{
    SerializedProperty blockImg;

    protected override void OnEnable()
    {
        base.OnEnable();
        blockImg = serializedObject.FindProperty("blockImg");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // draw default Button stuff
        serializedObject.Update();

        EditorGUILayout.PropertyField(blockImg); // draw your custom field

        serializedObject.ApplyModifiedProperties();
    }
}
