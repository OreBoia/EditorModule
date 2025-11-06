using UnityEngine;
using UnityEditor;

/// <summary>
/// Property Drawer che nasconde i campi bool usati come dummy per gli header
/// </summary>
[CustomPropertyDrawer(typeof(bool))]
public class HiddenBoolDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Se il nome del campo finisce con "Header", non mostrarlo
        if (property.name.EndsWith("Header"))
        {
            return 0f;
        }
        
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Se il nome del campo finisce con "Header", non disegnarlo
        if (property.name.EndsWith("Header"))
        {
            return;
        }
        
        EditorGUI.PropertyField(position, property, label, true);
    }
}