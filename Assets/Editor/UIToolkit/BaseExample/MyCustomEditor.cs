using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MyCustomEditor : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Tools/UI Toolkit/MyCustomEditor")]
    public static void ShowExample()
    {
        MyCustomEditor wnd = GetWindow<MyCustomEditor>();
        wnd.titleContent = new GUIContent("MyCustomEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);
        
        Button myButton = rootVisualElement.Q<Button>("button2");
        Label myLabel = rootVisualElement.Q<Label>().GetFirstOfType<Label>();
        
        myButton.RegisterCallback<ClickEvent>(evt => Debug.Log("Pulsante cliccato!"));
    }
}
