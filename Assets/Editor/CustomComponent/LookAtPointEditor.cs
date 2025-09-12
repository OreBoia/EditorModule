using UnityEngine;
using UnityEditor;

// Indica che questo Editor personalizzato si applica al tipo LookAtPoint
[CustomEditor(typeof(LookAtPoint))]
public class LookAtPointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Disegna l'interfaccia di default (campo lookAtPoint)
        DrawDefaultInspector();

        // Aggiungi elementi personalizzati sotto
        LookAtPoint script = (LookAtPoint)target;

        // Messaggio condizionale sulla posizione relativa in Y
        if (script.lookAtPoint.y > script.transform.position.y)
        {
            EditorGUILayout.HelpBox("Il punto di destinazione è sopra questo oggetto.", MessageType.Info);
        }
        else if (script.lookAtPoint.y < script.transform.position.y)
        {
            EditorGUILayout.HelpBox("Il punto di destinazione è sotto questo oggetto.", MessageType.Info);
        }
        else
        {
            EditorGUILayout.LabelField("Il punto è allo stesso livello dell'oggetto.");
        }
        
        // Pulsante per reimpostare il punto di destinazione
        if (GUILayout.Button("Reset Point to Origin"))
        {
            Undo.RecordObject(script, "Reset LookAtPoint"); // rende annullabile l'operazione

            script.lookAtPoint = Vector3.zero;
            // Facciamo in modo che Unity sappia della modifica:
            EditorUtility.SetDirty(script);
        }
    }
}
