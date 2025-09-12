#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class LifecycleSample : EditorWindow
{
    bool autoRefresh;
    double lastRefresh;

    [MenuItem("Tools/Lifecycle Sample")]
    static void Open() => GetWindow<LifecycleSample>("Lifecycle");

    void OnEnable()
    {
        // Leggi preferenze salvate
        autoRefresh = EditorPrefs.GetBool("LifecycleSample.AutoRefresh", true);

        // Imposta la finestra per ricevere movimenti del mouse
        wantsMouseMove = true;
    }

    void OnDisable()
    {
        // Salva le preferenze
        EditorPrefs.SetBool("LifecycleSample.AutoRefresh", autoRefresh);
    }

    void OnFocus()  => Debug.Log("Finestra in primo piano");
    void OnLostFocus() => Debug.Log("Finestra ha perso il focus");
    void OnDestroy() => Debug.Log("Finestra distrutta");

    void Update()
    {
        // Refresh periodico: ogni 0,25 s forza Repaint
        if (autoRefresh && EditorApplication.timeSinceStartup - lastRefresh > 0.25)
        {
            lastRefresh = EditorApplication.timeSinceStartup;
            Repaint();
        }
    }
    
    void OnInspectorUpdate()
    {
        // Aggiornamenti a bassa frequenza (10 fps)
        // Esempio: aggiornare contatori, monitorare selezione
    }

    void OnGUI()
    {
        GUILayout.Label("Esempio ciclo di vita", EditorStyles.boldLabel);
        autoRefresh = EditorGUILayout.ToggleLeft("Auto-refresh", autoRefresh);
        GUILayout.Label($"Time: {EditorApplication.timeSinceStartup:F2}");
    }

    void OnHierarchyChange() => Debug.Log("Gerarchia modificata");
    void OnProjectChange() => Debug.Log("Progetto modificato");
    void OnSelectionChange() => Debug.Log($"Selezione: {(Selection.activeObject ? Selection.activeObject.name : "null")}");
}
#endif