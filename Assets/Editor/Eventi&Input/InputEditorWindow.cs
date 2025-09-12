using UnityEditor;
using UnityEngine;

public class InputEditorWindow : EditorWindow
{
    private System.Collections.Generic.List<string> droppedObjectNames = new System.Collections.Generic.List<string>();

    [MenuItem("Tools/Input Editor")]
    public static void ShowWindow()
    {
        GetWindow<InputEditorWindow>("Input Editor");
    }

    private void OnGUI()
    {
        Event e = Event.current;
        var rectCanvas = GUILayoutUtility.GetRect(position.width, position.height - 60);

        // Sfondo area interattiva
        EditorGUI.DrawRect(rectCanvas, new Color(0.12f, 0.12f, 0.12f));

        // Mostra i nomi degli oggetti trascinati
        if (droppedObjectNames.Count > 0)
        {
            var viewRect = new Rect(rectCanvas.x + 10, rectCanvas.y + 10, rectCanvas.width - 20, rectCanvas.height - 20);
            GUILayout.BeginArea(viewRect);
            foreach (var name in droppedObjectNames)
            {
                GUILayout.Label(name, EditorStyles.whiteLabel);
            }
            GUILayout.EndArea();
        }

        // Mouse & tasti solo se dentro l’area interattiva
        if (rectCanvas.Contains(e.mousePosition))
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        // Click sinistro
                        Debug.Log("Click L su " + e.mousePosition);
                        e.Use(); // segna evento come gestito
                    }
                    else if (e.button == 1)
                    {
                        // Click destro: apri context menu
                        var menu = new GenericMenu();
                        menu.AddItem(new GUIContent("Azione 1"), false, () => Debug.Log("Azione 1"));
                        menu.AddItem(new GUIContent("Azione 2"), false, () => Debug.Log("Azione 2"));
                        menu.ShowAsContext();
                        e.Use();
                    }
                    break;

                case EventType.DragUpdated:
                    // Mostra che è possibile copiare l'oggetto
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    e.Use();
                    break;

                case EventType.DragPerform:
                    DragAndDrop.AcceptDrag();
                    
                    foreach (Object draggedObject in DragAndDrop.objectReferences)
                    {
                        droppedObjectNames.Add(draggedObject.name);
                    }
                    
                    e.Use();
                    break;

                case EventType.MouseDrag:
                    // Trascinamento (es. pan di una viewport)
                    // pan += e.delta;
                    e.Use();
                    break;

                case EventType.ScrollWheel:
                    // Zoom con scroll (tipico: CTRL/CMD + wheel)
                    // float zoomDelta = -e.delta.y * 0.1f;
                    e.Use();
                    break;

                case EventType.KeyDown:
                    if (e.keyCode == KeyCode.F && e.control)
                    {
                        Debug.Log("CTRL+F → trova");
                        e.Use();
                    }
                    break;

                case EventType.ContextClick:
                    // Alternativa al tasto destro
                    // Mostra context menu
                    e.Use();
                    break;
            }
        }

        // Disegna UI standard sotto
        GUILayout.FlexibleSpace();
        using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
        {
            GUILayout.Label("Suggerimenti: Click destro per menu, CTRL+F per find, Scroll per zoom.");
        }

        // Se logiche cambiano stato che richiedono refresh: Repaint();
        Repaint();
    }
}
