using UnityEditor;
using UnityEngine;

/// <summary>
/// Custom Editor che dimostra come utilizzare i property drawer in un inspector personalizzato
/// </summary>
[CustomEditor(typeof(CustomPropertyDrawerExample))]
public class CustomPropertyDrawerExampleEditor : Editor
{
    private SerializedProperty healthPercentage;
    private SerializedProperty weaponDamage;
    private SerializedProperty gridPosition;
    private SerializedProperty calculatedValue;

    private void OnEnable()
    {
        healthPercentage = serializedObject.FindProperty("healthPercentage");
        weaponDamage = serializedObject.FindProperty("weaponDamage");
        gridPosition = serializedObject.FindProperty("gridPosition");
        calculatedValue = serializedObject.FindProperty("calculatedValue");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CustomPropertyDrawerExample example = (CustomPropertyDrawerExample)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Custom Property Drawer Demo", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // Disegna tutti i property con i loro drawer personalizzati
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Runtime Values", EditorStyles.boldLabel);

        // Mostra alcuni valori calcolati in tempo reale
        GUI.enabled = false;
        EditorGUILayout.FloatField("Health (0-1 value)", example.healthPercentage);
        EditorGUILayout.FloatField("Random Damage", example.GetRandomDamage());
        EditorGUILayout.Vector2Field("Grid as Vector2", new Vector2(example.gridPosition.x, example.gridPosition.y));
        GUI.enabled = true;

        EditorGUILayout.Space();

        // Bottoni di utilità
        EditorGUILayout.LabelField("Utility Buttons", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Set Random Values"))
        {
            Undo.RecordObject(target, "Set Random Values");
            
            healthPercentage.floatValue = Random.Range(0f, 1f);
            weaponDamage.FindPropertyRelative("min").floatValue = Random.Range(5f, 15f);
            weaponDamage.FindPropertyRelative("max").floatValue = Random.Range(20f, 30f);
            gridPosition.FindPropertyRelative("x").intValue = Random.Range(0, 10);
            gridPosition.FindPropertyRelative("y").intValue = Random.Range(0, 10);
            
            serializedObject.ApplyModifiedProperties();
        }

        if (GUILayout.Button("Reset to Default"))
        {
            Undo.RecordObject(target, "Reset to Default");
            
            healthPercentage.floatValue = 0.8f;
            weaponDamage.FindPropertyRelative("min").floatValue = 10f;
            weaponDamage.FindPropertyRelative("max").floatValue = 25f;
            gridPosition.FindPropertyRelative("x").intValue = 5;
            gridPosition.FindPropertyRelative("y").intValue = 3;
            
            serializedObject.ApplyModifiedProperties();
        }

        serializedObject.ApplyModifiedProperties();
    }
}