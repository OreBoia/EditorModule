
using Unity.AppUI.Core;
using UnityEditor;
using UnityEngine;

public class EditorGUILayoutExamples : EditorWindow
{

    Types types;
    Color color;

    int layer;

    private bool showSettings;
    private float speed;

    int toolbarIndex;

    Vector2 scrollPos;

    Vector2 scroll; 
    Rect area = new Rect(10, 210, 260, 90);
    Rect winRect = new Rect(300, 40, 240, 120);

    string[] toolbarTitles = { "OpzioneA", "OpzioneB" };

    int toolbarIdx = 0; 
    string[] tabs = {"Home","Edit","View"};
    int gridIdx = 0;
    string[] grid = { "Uno", "Due", "Tre", "Quattro", "Cinque", "Sei" };

    GUIStyle big; 
    GUIStyle warn;

    [MenuItem("Tools/EditorGUILayoutExample")]
    public static void ShowWindow()
    {
        GetWindow<EditorGUILayoutExamples>("Esempio Editor/GUILayout");
    }

    void OnEnable()
    {
        big = new GUIStyle(EditorStyles.label){
            fontSize = 16,
            normal = { textColor = Color.aquamarine, },


        };
        warn = new GUIStyle(EditorStyles.helpBox){ wordWrap = true, alignment = TextAnchor.MiddleLeft };
    }

    void OnGUI()
    {
        types = (Types)EditorGUILayout.EnumPopup("Tipi di dato", types);
        color = EditorGUILayout.ColorField(color);

        EditorGUILayout.Separator();

        layer = EditorGUILayout.LayerField("Layer", layer);

        showSettings = EditorGUILayout.Foldout(showSettings, "Impostazioni");
        if (showSettings)
        {
            speed = EditorGUILayout.FloatField("Velocità", speed);
        }
        layer = EditorGUILayout.LayerField("Layer", layer);

        // EditorGUILayout.HelpBox("Attenzione: dati incompleti!", MessageType.Warning);

        toolbarIndex = GUILayout.Toolbar(toolbarIndex, toolbarTitles);

        // scrollPos = GUILayout.BeginScrollView(scrollPos);

        // GUILayout.Label("BeginHorizontal / BeginVertical", EditorStyles.boldLabel);

        // GUILayout.BeginHorizontal();
        // if (GUILayout.Button("Sinistra", GUILayout.Width(80))) {}
        // GUILayout.FlexibleSpace(); // spinge a dx
        // if (GUILayout.Button("Destra", GUILayout.Width(80))) {}
        // GUILayout.EndHorizontal();

        // GUILayout.Space(6);

        // GUILayout.BeginVertical("box");
        // GUILayout.Label("Colonna 1");
        // GUILayout.Button("A");
        // GUILayout.Button("B");
        // GUILayout.EndVertical();

        // GUILayout.Space(6);

        // GUILayout.BeginHorizontal("box");
        // GUILayout.Label("Riga con spazi:");
        // GUILayout.Space(10);
        // GUILayout.Button("C");
        // GUILayout.Space(10);
        // GUILayout.Button("D");
        // GUILayout.EndHorizontal();

        // GUILayout.Label("ScrollView:", EditorStyles.boldLabel);
        // scroll = GUILayout.BeginScrollView(scroll, GUILayout.Height(100));
        //     for (int i = 0; i < 20; i++) GUILayout.Button("Elemento " + i);
        // GUILayout.EndScrollView();

        // GUILayout.Label("BeginArea:", EditorStyles.boldLabel);
        // GUILayout.BeginArea(area, "Area delimitata", "window");
        // GUILayout.Label("Contenuto area");
        // if (GUILayout.Button("Ping Console")) Debug.Log("Clic in Area");
        // GUILayout.EndArea();

        // GUILayout.Label("Window:", EditorStyles.boldLabel);
        // winRect = GUILayout.Window(12345, winRect, DrawInnerWindow, "Finestra flottante");

        // GUILayout.Label("Toolbar:", EditorStyles.boldLabel);
        //     toolbarIdx = GUILayout.Toolbar(toolbarIdx, tabs);
        // GUILayout.Label($"Tab attuale: {tabs[toolbarIdx]}");

        // GUILayout.Space(6);
        // GUILayout.Label("SelectionGrid:", EditorStyles.boldLabel);
        // gridIdx = GUILayout.SelectionGrid(gridIdx, grid, 5); // 3 colonne
        // GUILayout.Label($"Scelto: {grid[gridIdx]}");

        GUILayout.Label("Dimensioni controlli con GUILayoutOption", big);
        if (GUILayout.Button("Largo 200, Alto 40", GUILayout.Width(200), GUILayout.Height(40)))
            Debug.Log("Click");

        GUILayout.BeginHorizontal();
        GUILayout.Button("ExpandWidth true", GUILayout.ExpandWidth(true));
        GUILayout.Button("Fixed 100", GUILayout.Width(100));
        GUILayout.EndHorizontal();

        GUILayout.Space(6);
        GUILayout.Label("Stili personalizzati", big);
        GUILayout.Label("Messaggio d'aiuto con helpBox style", warn);
        GUILayout.TextArea("TextArea con min/max", GUILayout.MinHeight(60), GUILayout.MaxHeight(120));
    }
    
    void DrawInnerWindow(int id)
    {
        GUILayout.Label("UI interna alla finestra");
        if (GUILayout.Button("Drag me")) {}
        GUI.DragWindow(); // permette di trascinare la window
    }
}

public enum Types
{
    Prefabs,
    Files,
    Sprites
}

