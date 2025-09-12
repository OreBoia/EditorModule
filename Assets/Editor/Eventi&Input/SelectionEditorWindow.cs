using UnityEditor;
using UnityEngine;

public class SelectionEditorWindow : EditorWindow
{
    [MenuItem("Tools/Selection Editor")]
    public static void ShowWindow()
    {
        GetWindow<SelectionEditorWindow>("Selection Editor");
    }

    private bool _isSelecting;
    private Vector2 _selectStart, _selectEnd;

    private void OnGUI()
    {
        var canvas = GUILayoutUtility.GetRect(position.width, position.height - 40);
        EditorGUI.DrawRect(canvas, new Color(0.1f, 0.1f, 0.1f));

        Event e = Event.current;

        if (canvas.Contains(e.mousePosition))
        {
            if (e.type == EventType.MouseDown && e.button == 0)
            {
                if (e.clickCount == 2)
                {
                    Debug.Log("Doppio click a " + e.mousePosition);
                    e.Use();
                }
                else
                {
                    _isSelecting = true;
                    _selectStart = _selectEnd = e.mousePosition;
                    e.Use();
                }
            }
            else if (e.type == EventType.MouseDrag && _isSelecting)
            {
                _selectEnd = e.mousePosition;
                Repaint();
                e.Use();
            }
            else if (e.type == EventType.MouseUp && _isSelecting)
            {
                _isSelecting = false;
                var r = Utils.MakeRect(_selectStart, _selectEnd);
                Debug.Log("Selezione rettangolo: " + r);
                // Applica selezione agli oggetti nel r
                e.Use();
            }
        }

        // Disegna il rettangolo durante Repaint
        if (Event.current.type == EventType.Repaint && _isSelecting)
        {
            var r = Utils.MakeRect(_selectStart, _selectEnd);
            Handles.BeginGUI();
            Color c = new Color(0.3f, 0.6f, 1f, 0.2f);
            EditorGUI.DrawRect(r, c);
            Handles.DrawSolidRectangleWithOutline(r, new Color(0, 0, 0, 0), new Color(0.3f, 0.6f, 1f, 0.9f));
            Handles.EndGUI();
        }

        GUILayout.FlexibleSpace();
        EditorGUILayout.HelpBox("Doppio click per azione, drag LMB per selezione.", MessageType.Info);
    }
}

static class Utils
{
    public static Rect MakeRect(Vector2 a, Vector2 b)
    {
        float x = Mathf.Min(a.x, b.x);
        float y = Mathf.Min(a.y, b.y);
        float w = Mathf.Abs(a.x - b.x);
        float h = Mathf.Abs(a.y - b.y);
        return new Rect(x, y, w, h);
    }
}
