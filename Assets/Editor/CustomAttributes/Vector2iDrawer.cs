using UnityEditor;
using UnityEngine;

/// <summary>
/// Property Drawer per il tipo Vector2i
/// Disegna due campi int affiancati su una singola riga
/// </summary>
[CustomPropertyDrawer(typeof(Vector2i))]
public class Vector2iDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty xProp = property.FindPropertyRelative("x");
        SerializedProperty yProp = property.FindPropertyRelative("y");

        if (xProp == null || yProp == null)
        {
            EditorGUI.LabelField(position, label.text, "Vector2i must have 'x' and 'y' int fields.");
            return;
        }

        // Inizia il gruppo di properties
        EditorGUI.BeginProperty(position, label, property);

        // Calcola le posizioni
        Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
        float fieldWidth = (position.width - EditorGUIUtility.labelWidth - 25) / 2f; // 25 pixel per spacing
        Rect xRect = new Rect(position.x + EditorGUIUtility.labelWidth, position.y, fieldWidth, position.height);
        Rect yRect = new Rect(xRect.xMax + 5, position.y, fieldWidth, position.height);

        // Disegna la label principale
        EditorGUI.LabelField(labelRect, label);

        // Salva la larghezza della label per i sub-field
        float oldLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 15f; // Larghezza per "X" e "Y"

        // Disegna i campi X e Y
        EditorGUI.PropertyField(xRect, xProp, new GUIContent("X"));
        EditorGUI.PropertyField(yRect, yProp, new GUIContent("Y"));

        // Ripristina la larghezza della label
        EditorGUIUtility.labelWidth = oldLabelWidth;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }
}