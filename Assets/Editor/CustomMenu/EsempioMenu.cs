using UnityEditor;
using UnityEngine;

public class EsempioMenu
{

    // Aggiunge una voce di menu "Tools/Crea Oggetto"
    [MenuItem("Tools/Crea Oggetto Personalizzato %l")] //ctrl+g
    private static void CreateCustomGameObject()
    {
        // Crea un nuovo GameObject in scena con un nome predefinito
        GameObject go = new GameObject("OggettoPersonalizzato");
        go.transform.position = Vector3.zero;
        Debug.Log("Oggetto creato in posizione (0,0,0)");
    }

    // Aggiunge una voce di menu al menu "Assets/Create".
    [MenuItem("Assets/Create/My Custom Asset %#\\")]
    private static void CreateCustomAsset()
    {
        // Crea un'istanza di un oggetto ScriptableObject (puoi definire la tua classe).
        // Per questo esempio, creiamo un semplice oggetto ScriptableObject.
        ScriptableObject asset = ScriptableObject.CreateInstance<ScriptableObject>();

        // Crea un percorso univoco per il nuovo asset.
        string path = AssetDatabase.GenerateUniqueAssetPath("Assets/New My Custom Asset.asset");

        // Crea l'asset nel database degli asset di Unity.
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();

        // Evidenzia il nuovo asset nel pannello Project.
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;

        Debug.Log($"Creato nuovo asset personalizzato in: {path}");
    }

    // Aggiunge una voce di menu al menu contestuale del componente Transform.
    // Questa funzione viene chiamata quando l'utente seleziona "Reset Y" dal menu contestuale di un Transform.
    [MenuItem("CONTEXT/Transform/Reset Y %r")]
    private static void ResetY(MenuCommand command)
    {
        // command.context è il componente Transform su cui si è fatto clic con il pulsante destro del mouse.
        Transform transform = (Transform)command.context;

        // Registra l'oggetto per la funzionalità di undo.
        Undo.RecordObject(transform, "Reset Y Position");

        // Imposta la posizione y a 0, mantenendo x e z.
        Vector3 position = transform.position;
        position.y = 0;
        transform.position = position;

        Debug.Log($"Posizione Y del GameObject '{transform.gameObject.name}' resettata a 0.");
    }
}