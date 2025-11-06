using UnityEngine;
using UnityEditor;
using System.Reflection;

/// <summary>
/// Custom Editor per AdvancedPropertyDrawerExample che gestisce correttamente gli header colorati
/// </summary>
[CustomEditor(typeof(AdvancedPropertyDrawerExample))]
public class AdvancedPropertyDrawerExampleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        AdvancedPropertyDrawerExample script = (AdvancedPropertyDrawerExample)target;
        
        // Ottieni tutti i campi
        FieldInfo[] fields = typeof(AdvancedPropertyDrawerExample).GetFields(BindingFlags.Public | BindingFlags.Instance);
        
        foreach (FieldInfo field in fields)
        {
            // Salta i campi header dummy
            if (field.Name.EndsWith("Header"))
                continue;
                
            // Cerca se c'è un ColoredHeaderAttribute
            ColoredHeaderAttribute headerAttr = null;
            
            // Prima controlla se c'è un campo header dummy corrispondente
            string headerFieldName = "_" + field.Name + "Header";
            FieldInfo headerField = typeof(AdvancedPropertyDrawerExample).GetField(headerFieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            
            if (headerField != null)
            {
                headerAttr = headerField.GetCustomAttribute<ColoredHeaderAttribute>();
            }
            else
            {
                // Altrimenti cerca direttamente sul campo
                headerAttr = field.GetCustomAttribute<ColoredHeaderAttribute>();
            }
            
            // Disegna l'header se presente
            if (headerAttr != null)
            {
                DrawColoredHeader(headerAttr);
            }
            
            // Disegna il campo
            SerializedProperty prop = serializedObject.FindProperty(field.Name);
            if (prop != null)
            {
                EditorGUILayout.PropertyField(prop, true);
            }
        }
        
        serializedObject.ApplyModifiedProperties();
    }
    
    private void DrawColoredHeader(ColoredHeaderAttribute header)
    {
        GUILayout.Space(5f);
        
        Color originalColor = GUI.color;
        Color originalBackgroundColor = GUI.backgroundColor;
        
        // Calcola il rect per l'header
        Rect rect = GUILayoutUtility.GetRect(0f, header.height, GUILayout.ExpandWidth(true));
        
        // Disegna lo sfondo
        Color backgroundColor = header.color * 0.3f;
        backgroundColor.a = 0.3f;
        EditorGUI.DrawRect(rect, backgroundColor);
        
        // Bordi
        Color borderColor = header.color * 0.6f;
        borderColor.a = 0.8f;
        
        EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, 1f), borderColor);
        EditorGUI.DrawRect(new Rect(rect.x, rect.yMax - 1f, rect.width, 1f), borderColor);
        EditorGUI.DrawRect(new Rect(rect.x, rect.y, 2f, rect.height), borderColor);
        EditorGUI.DrawRect(new Rect(rect.xMax - 2f, rect.y, 2f, rect.height), borderColor);
        
        // Stile testo
        GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 12,
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = header.color }
        };
        
        GUI.Label(rect, header.title.ToUpper(), headerStyle);
        
        // Linea separatrice
        if (header.showLine)
        {
            GUILayout.Space(3f);
            Rect lineRect = GUILayoutUtility.GetRect(0f, 2f, GUILayout.ExpandWidth(true));
            
            Color lineColor = header.color;
            lineColor.a = 0.8f;
            EditorGUI.DrawRect(lineRect, lineColor);
            
            // Effetto gradiente
            Color fadeColor = lineColor;
            fadeColor.a = 0.2f;
            
            for (int i = 0; i < 20; i++)
            {
                Color gradColor = Color.Lerp(fadeColor, lineColor, i / 20f);
                EditorGUI.DrawRect(new Rect(lineRect.x + i, lineRect.y, 1f, lineRect.height), gradColor);
            }
            
            for (int i = 0; i < 20; i++)
            {
                Color gradColor = Color.Lerp(lineColor, fadeColor, i / 20f);
                EditorGUI.DrawRect(new Rect(lineRect.xMax - 20 + i, lineRect.y, 1f, lineRect.height), gradColor);
            }
        }
        
        GUI.color = originalColor;
        GUI.backgroundColor = originalBackgroundColor;
        
        GUILayout.Space(2f);
    }
}