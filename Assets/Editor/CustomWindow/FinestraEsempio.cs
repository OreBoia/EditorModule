using UnityEngine;
using UnityEditor;
public class FinestraEsempio : EditorWindow
{
    string testo = "Hello World";
    bool opzione = true;
    float valore = 0.5f;

    // Crea una voce di menu per aprire la finestra
    [MenuItem("Tools/Finestra Esempio")]
    public static void ShowWindow()
    {
        // Apre la finestra (o la porta in primo piano se già esistente)
        EditorWindow.GetWindow<FinestraEsempio>("Finestra Esempio");
    }

    // Disegna l'interfaccia grafica dentro la finestra
    private void OnGUI()
    {
        GUILayout.Label("Esempio di finestra", EditorStyles.boldLabel);
        testo = EditorGUILayout.TextField("Testo:", testo);
        opzione = EditorGUILayout.Toggle("Opzione", opzione);
        valore = EditorGUILayout.Slider("Valore", valore, 0f, 1f);

        if (GUILayout.Button("Premi per azione"))
        {
            Debug.Log($"Bottone premuto! Testo corrente: {testo}");
            // Esempio: potremmo eseguire qui qualche azione utile
        }
        
        
    }
}