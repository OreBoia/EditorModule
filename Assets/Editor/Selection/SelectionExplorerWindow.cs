using UnityEditor;
using UnityEngine;

public class SelectionExplorerWindow : EditorWindow
{
    private Vector2 scrollPosition;

    [MenuItem("Tools/Selection Explorer")]
    public static void ShowWindow()
    {
        GetWindow<SelectionExplorerWindow>("Selection Explorer");
    }

    // Questo metodo è chiamato quando la finestra viene abilitata.
    private void OnEnable()
    {
        // Registra il metodo OnSelectionChange al delegate selectionChanged.
        // Questo significa che ogni volta che la selezione cambia nell'editor, il metodo OnSelectionChange sarà chiamato.
        Selection.selectionChanged += OnSelectionChange;
    }

    // Questo metodo è chiamato quando la finestra viene disabilitata.
    private void OnDisable()
    {
        // Rimuove la registrazione del metodo OnSelectionChange dal delegate selectionChanged.
        // È importante fare pulizia quando la finestra viene chiusa.
        Selection.selectionChanged -= OnSelectionChange;
    }

    private void OnSelectionChange()
    {
        // Questo metodo è chiamato dal delegate Selection.selectionChanged.
        var obj = Selection.activeObject;
        if (obj != null)
        {
            Debug.Log("Selezione cambiata: " + obj.name + " di tipo " + obj.GetType());
        }
        else
        {
            Debug.Log("Selezione cambiata: Nessun oggetto selezionato.");
        }
        Repaint(); // Ridisegna la finestra per mostrare la nuova selezione.
    }

    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // --- Selection.activeObject ---
        GUILayout.Label("Selection.activeObject", EditorStyles.boldLabel);
        Object activeObj = Selection.activeObject;
        if (activeObj == null)
        {
            GUILayout.Label("Nessun oggetto attivo selezionato.");
        }
        else
        {
            GUILayout.Label("Nome: " + activeObj.name);
            GUILayout.Label("Tipo: " + activeObj.GetType().Name);
        }

        EditorGUILayout.Space();

        // --- Selection.activeGameObject ---
        GUILayout.Label("Selection.activeGameObject", EditorStyles.boldLabel);
        GameObject activeGO = Selection.activeGameObject;
        if (activeGO == null)
        {
            GUILayout.Label("Nessun GameObject attivo selezionato.");
            EditorGUILayout.HelpBox("Selection.activeObject non è un GameObject o è nullo.", MessageType.Info);
        }
        else
        {
            GUILayout.Label("Nome: " + activeGO.name);
            GUILayout.Label("Tag: " + activeGO.tag);
            GUILayout.Label("Layer: " + LayerMask.LayerToName(activeGO.layer));
        }

        EditorGUILayout.Space();

        // --- Selection.objects ---
        GUILayout.Label("Selection.objects", EditorStyles.boldLabel);
        Object[] selectedObjects = Selection.objects;
        if (selectedObjects.Length == 0)
        {
            GUILayout.Label("Nessun oggetto selezionato.");
        }
        else
        {
            GUILayout.Label("Oggetti selezionati: " + selectedObjects.Length);
            foreach (var obj in selectedObjects)
            {
                EditorGUILayout.ObjectField(obj, obj.GetType(), true);
            }
        }

        EditorGUILayout.EndScrollView();
    }
}