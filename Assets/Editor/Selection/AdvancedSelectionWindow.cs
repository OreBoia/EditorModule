using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using Object = UnityEngine.Object;

public class AdvancedSelectionWindow : EditorWindow
{
    private Object objectToCheck;
    private SelectionMode getTransformsMode = SelectionMode.TopLevel;
    private SelectionMode getFilteredMode = SelectionMode.TopLevel;
    private string typeNameToFilter = "GameObject";
    private Vector2 scrollPosition;

    [MenuItem("Tools/Advanced Selection Explorer")]
    public static void ShowWindow()
    {
        GetWindow<AdvancedSelectionWindow>("Advanced Selection");
    }

    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        EditorGUILayout.HelpBox("Seleziona oggetti nella scena o nel progetto per vedere i risultati.", MessageType.Info);

        // --- Sezione per Selection.Contains ---
        EditorGUILayout.LabelField("Selection.Contains(Object obj)", EditorStyles.boldLabel);
        objectToCheck = EditorGUILayout.ObjectField("Oggetto da Verificare", objectToCheck, typeof(Object), true);

        if (GUILayout.Button("L'oggetto è nella selezione?"))
        {
            if (objectToCheck != null)
            {
                // Controlla se l'oggetto specificato è parte della selezione corrente.
                bool isSelected = Selection.Contains(objectToCheck);
                string message = $"L'oggetto '{objectToCheck.name}' è nella selezione corrente? {isSelected}";
                Debug.Log(message);
                ShowNotification(new GUIContent(message));
            }
            else
            {
                ShowNotification(new GUIContent("Per favore, assegna un oggetto da verificare."));
            }
        }

        EditorGUILayout.Space(20);

        // --- Sezione per Selection.GetFiltered ---
        EditorGUILayout.LabelField("Selection.GetFiltered(Type type, SelectionMode mode)", EditorStyles.boldLabel);
        typeNameToFilter = EditorGUILayout.TextField("Filtra per Tipo (es. GameObject, Material)", typeNameToFilter);
        getFilteredMode = (SelectionMode)EditorGUILayout.EnumPopup("Modalità di Selezione", getFilteredMode);

        if (GUILayout.Button("Ottieni Selezione Filtrata"))
        {
            // Prova a trovare il tipo negli assembly di Unity e dell'Editor.
            Type filterType = Type.GetType($"{typeNameToFilter}, UnityEngine") ?? Type.GetType($"{typeNameToFilter}, UnityEditor");
            
            // Se non trovato, cerca in tutti gli assembly caricati.
            if (filterType == null)
            {
                filterType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t => t.Name.Equals(typeNameToFilter, StringComparison.OrdinalIgnoreCase));
            }

            if (filterType != null)
            {
                // Ottiene un array di oggetti dalla selezione che corrispondono al tipo e alla modalità specificati.
                var filteredSelection = Selection.GetFiltered(filterType, getFilteredMode);
                Debug.Log($"--- Oggetti Filtrati (Tipo: {filterType.Name}, Modalità: {getFilteredMode}) ---");
                if (filteredSelection.Length == 0)
                {
                    Debug.Log("Nessun oggetto corrisponde ai criteri.");
                }
                else
                {
                    foreach (var obj in filteredSelection)
                    {
                        Debug.Log($"- {obj.name}", obj);
                    }
                }
            }
            else
            {
                Debug.LogError($"Tipo '{typeNameToFilter}' non trovato. Assicurati che il nome sia corretto e che l'assembly sia caricato.");
            }
        }

        EditorGUILayout.Space(20);

        // --- Sezione per Selection.GetTransforms ---
        EditorGUILayout.LabelField("Selection.GetTransforms(SelectionMode mode)", EditorStyles.boldLabel);
        getTransformsMode = (SelectionMode)EditorGUILayout.EnumPopup("Modalità di Selezione", getTransformsMode);

        if (GUILayout.Button("Ottieni Transform Selezionati"))
        {
            // Restituisce i Transform di tutti gli oggetti selezionati, filtrati dalla SelectionMode.
            var selectedTransforms = Selection.GetTransforms(getTransformsMode);
            Debug.Log($"--- Transform Selezionati (Modalità: {getTransformsMode}) ---");
            if (selectedTransforms.Length == 0)
            {
                Debug.Log("Nessun transform trovato con questa modalità.");
            }
            else
            {
                foreach (var t in selectedTransforms)
                {
                    Debug.Log($"- {t.name}", t);
                }
            }
        }

        EditorGUILayout.EndScrollView();
    }
}
