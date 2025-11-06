using UnityEditor;
using UnityEngine;

/// <summary>
/// Property Drawer per l'attributo [MinMaxRange]
/// Crea un slider bidirezionale per controllare i valori min e max di una struct MinMaxRange
/// </summary>
[CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
public class MinMaxRangeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        MinMaxRangeAttribute minMaxRangeAttribute = (MinMaxRangeAttribute)attribute;
        
        if (property.type != "MinMaxRange")
        {
            EditorGUI.LabelField(position, label.text, "Use [MinMaxRange] with MinMaxRange fields only.");
            return;
        }

        SerializedProperty minProp = property.FindPropertyRelative("min");
        SerializedProperty maxProp = property.FindPropertyRelative("max");

        if (minProp == null || maxProp == null)
        {
            EditorGUI.LabelField(position, label.text, "MinMaxRange must have 'min' and 'max' float fields.");
            return;
        }

        float minValue = minProp.floatValue;
        float maxValue = maxProp.floatValue;
        float minLimit = minMaxRangeAttribute.min;
        float maxLimit = minMaxRangeAttribute.max;

        // Calcola le posizioni per il layout
        Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
        Rect sliderRect = new Rect(position.x + EditorGUIUtility.labelWidth, position.y, 
                                position.width - EditorGUIUtility.labelWidth - 130, position.height);
        Rect minFieldRect = new Rect(sliderRect.xMax + 5, position.y, 60, position.height);
        Rect maxFieldRect = new Rect(minFieldRect.xMax + 5, position.y, 60, position.height);

        // Disegna la label
        EditorGUI.LabelField(labelRect, label);

        EditorGUI.BeginChangeCheck();
        
        // Disegna il range slider
        EditorGUI.MinMaxSlider(sliderRect, ref minValue, ref maxValue, minLimit, maxLimit);
        
        // Disegna i campi numerici per i valori
        minValue = EditorGUI.FloatField(minFieldRect, minValue);
        maxValue = EditorGUI.FloatField(maxFieldRect, maxValue);

        if (EditorGUI.EndChangeCheck())
        {
            // Assicurati che min non sia maggiore di max
            if (minValue > maxValue)
                minValue = maxValue;
            if (maxValue < minValue)
                maxValue = minValue;

            // Clamp ai limiti dell'attributo
            minValue = Mathf.Clamp(minValue, minLimit, maxLimit);
            maxValue = Mathf.Clamp(maxValue, minLimit, maxLimit);

            minProp.floatValue = minValue;
            maxProp.floatValue = maxValue;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }
}