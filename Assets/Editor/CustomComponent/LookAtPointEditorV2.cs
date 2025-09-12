using UnityEngine;
using UnityEditor;

// Indica che questo Editor personalizzato si applica al tipo LookAtPoint
[CustomEditor(typeof(LookAtPoint)), CanEditMultipleObjects]
public class LookAtPointEditorV2 : Editor
{
    // Riferimento a un SerializedProperty per il campo lookAtPoint
    SerializedProperty lookAtProp;
    void OnEnable()
    {
        // Collega la SerializedProperty al campo "lookAtPoint" del target
        lookAtProp = serializedObject.FindProperty("lookAtPoint");
    }

    public override void OnInspectorGUI()
    {
        // Aggiorna i dati del SerializedObject
        serializedObject.Update();
        // Disegna il campo lookAtPoint usando il sistema serializzato
        EditorGUILayout.PropertyField(lookAtProp);
        // Aggiunge messaggi come prima, usando lookAtProp anziché target diretto
        
        if (lookAtProp.vector3Value.y > ((LookAtPoint)target).transform.position.y)
        {
            EditorGUILayout.HelpBox("Il punto di destinazione è sopra questo oggetto.", MessageType.Info);
        }
        else if (lookAtProp.vector3Value.y < ((LookAtPoint)target).transform.position.y)
        {
            EditorGUILayout.HelpBox("Il punto di destinazione è sotto questo oggetto.", MessageType.Info);
        }
        else
        {
            EditorGUILayout.LabelField("Il punto è allo stesso livello dell'oggetto.");
        }

        // Pulsante Reset che opera sul SerializedProperty
        if (GUILayout.Button("Reset Point to Origin"))
        {
            lookAtProp.vector3Value = Vector3.zero;
        }
        
        // Applica le modifiche serializzate (incluse quelle fatte tramite PropertyField o manualmente)
        serializedObject.ApplyModifiedProperties();
    }
}

