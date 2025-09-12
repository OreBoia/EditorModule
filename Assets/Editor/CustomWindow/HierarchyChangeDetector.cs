using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Questa classe EditorWindow rileva e registra le modifiche nella gerarchia di Unity.
/// Dimostra come intercettare l'evento EditorApplication.hierarchyChanged e determinare
/// quali oggetti sono stati creati, eliminati, rinominati o spostati.
/// </summary>
public class HierarchyChangeDetector : EditorWindow
{
    // Dizionario per memorizzare lo stato precedente della gerarchia.
    // La chiave è l'InstanceID del GameObject, il valore è uno snapshot del suo stato.
    private static Dictionary<int, GameObjectState> hierarchyState = new Dictionary<int, GameObjectState>();
    private static bool isFirstRun = true;
    private Vector2 scrollPosition;
    private static List<string> detectedChanges = new List<string>();

    // Struttura per memorizzare le informazioni rilevanti di un GameObject.
    private struct GameObjectState
    {
        public string name;
        public int parentInstanceID;
        public int siblingIndex;
    }

    [MenuItem("Tools/Hierarchy Change Detector")]
    public static void ShowWindow()
    {
        GetWindow<HierarchyChangeDetector>("Hierarchy Changes");
    }

    private void OnEnable()
    {
        // Registra il metodo OnHierarchyChanged all'evento hierarchyChanged.
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
        Debug.Log("Rilevatore di modifiche della gerarchia abilitato.");

        // Esegui un'analisi iniziale per popolare lo stato.
        isFirstRun = true;
        OnHierarchyChanged();
    }

    private void OnDisable()
    {
        // Rimuovi la registrazione per evitare memory leak.
        EditorApplication.hierarchyChanged -= OnHierarchyChanged;
        Debug.Log("Rilevatore di modifiche della gerarchia disabilitato.");
    }

    private void OnGUI()
    {
        GUILayout.Label("Log delle Modifiche nella Gerarchia", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Apri questa finestra e modifica la gerarchia (crea, elimina, rinomina, sposta oggetti) per vedere i log. I log appaiono anche nella Console.", MessageType.Info);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(position.height - 50));
        
        // Mostra le modifiche in ordine inverso (le più recenti in alto).
        for (int i = detectedChanges.Count - 1; i >= 0; i--)
        {
            GUILayout.Label(detectedChanges[i], EditorStyles.wordWrappedLabel);
            EditorGUILayout.Space();
        }

        EditorGUILayout.EndScrollView();
    }

    private static void OnHierarchyChanged()
    {
        // Se è la prima esecuzione, ci limitiamo a salvare lo stato iniziale.
        if (isFirstRun)
        {
            LogChange("Stato iniziale della gerarchia catturato.");
            UpdateHierarchyState();
            isFirstRun = false;
            return;
        }

        // Ottieni lo stato attuale di tutti i GameObject nella scena.
        var currentObjects = FindObjectsOfType<GameObject>();
        var currentIds = new HashSet<int>(currentObjects.Select(o => o.GetInstanceID()));
        var previousIds = new HashSet<int>(hierarchyState.Keys);

        // 1. Rileva oggetti eliminati
        var deletedIds = previousIds.Except(currentIds);
        foreach (var id in deletedIds)
        {
            LogChange($"[ELIMINATO] Oggetto: '{hierarchyState[id].name}' (ID: {id})");
            hierarchyState.Remove(id);
        }

        // 2. Rileva oggetti creati
        var createdIds = currentIds.Except(previousIds);
        foreach (var id in createdIds)
        {
            var obj = EditorUtility.InstanceIDToObject(id) as GameObject;
            if (obj != null)
            {
                LogChange($"[CREATO] Oggetto: '{obj.name}' (ID: {id})");
                hierarchyState[id] = GetGameObjectState(obj);
            }
        }

        // 3. Rileva modifiche (rinomina, spostamento) negli oggetti esistenti
        var existingIds = currentIds.Intersect(previousIds);
        foreach (var id in existingIds)
        {
            var obj = EditorUtility.InstanceIDToObject(id) as GameObject;
            if (obj == null) continue;

            var previousState = hierarchyState[id];
            var currentState = GetGameObjectState(obj);

            if (previousState.name != currentState.name)
            {
                LogChange($"[RINOMINATO] Oggetto: da '{previousState.name}' a '{currentState.name}' (ID: {id})");
            }
            if (previousState.parentInstanceID != currentState.parentInstanceID)
            {
                var oldParentName = previousState.parentInstanceID == 0 ? "root" : (EditorUtility.InstanceIDToObject(previousState.parentInstanceID)?.name ?? "genitore eliminato");
                var newParentName = currentState.parentInstanceID == 0 ? "root" : (obj.transform.parent?.name ?? "root");
                LogChange($"[SPOSTATO] Oggetto: '{obj.name}' ha cambiato genitore da '{oldParentName}' a '{newParentName}'");
            }
            else if (previousState.siblingIndex != currentState.siblingIndex)
            {
                LogChange($"[RIORDINATO] Oggetto: '{obj.name}' ha cambiato posizione nella gerarchia.");
            }

            // Aggiorna lo stato per la prossima comparazione.
            hierarchyState[id] = currentState;
        }
        
        // Forza il ridisegno della finestra per mostrare le nuove modifiche.
        if (HasOpenInstances<HierarchyChangeDetector>())
        {
            GetWindow<HierarchyChangeDetector>().Repaint();
        }
    }

    /// <summary>
    /// Aggiorna l'intero stato della gerarchia. Utile per l'inizializzazione.
    /// </summary>
    private static void UpdateHierarchyState()
    {
        hierarchyState.Clear();
        var allObjects = FindObjectsOfType<GameObject>();
        foreach (var obj in allObjects)
        {
            hierarchyState[obj.GetInstanceID()] = GetGameObjectState(obj);
        }
    }

    /// <summary>
    /// Crea uno snapshot dello stato di un GameObject.
    /// </summary>
    private static GameObjectState GetGameObjectState(GameObject obj)
    {
        return new GameObjectState
        {
            name = obj.name,
            parentInstanceID = obj.transform.parent?.GetInstanceID() ?? 0,
            siblingIndex = obj.transform.GetSiblingIndex()
        };
    }

    /// <summary>
    /// Aggiunge un messaggio di log alla lista e lo stampa nella console.
    /// </summary>
    private static void LogChange(string change)
    {
        string message = $"{System.DateTime.Now:HH:mm:ss}: {change}";
        Debug.Log(message);
        detectedChanges.Add(message);
    }
}
