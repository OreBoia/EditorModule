using UnityEditor;
using UnityEngine;

/// <summary>
/// Property Drawer per l'attributo [Percent]
/// Mostra un campo float come slider percentuale (0-100%)
/// </summary>
[CustomPropertyDrawer(typeof(PercentAttribute))]
public class PercentDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.Float)
        {
            EditorGUI.LabelField(position, label.text, "Use [Percent] with float fields only.");
            return;
        }

        // Converti il valore da 0-1 a 0-100 per la visualizzazione
        float percentValue = property.floatValue * 100f;
        
        EditorGUI.BeginChangeCheck();
        
        // Crea uno slider che va da 0 a 100
        percentValue = EditorGUI.Slider(position, label, percentValue, 0f, 100f);
        
        if (EditorGUI.EndChangeCheck())
        {
            // Converti il valore da 0-100 a 0-1 per il salvataggio
            property.floatValue = percentValue / 100f;
        }
    }
}