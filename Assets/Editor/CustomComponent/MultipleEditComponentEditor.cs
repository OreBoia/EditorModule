using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor personalizzato per la classe MyComponent.
/// L'attributo [CustomEditor(typeof(MyComponent))] dice a Unity che questa classe
/// è un editor per il componente MyComponent.
/// </summary>
[CustomEditor(typeof(MultipleEditComponent))]
/// <summary>
/// L'attributo [CanEditMultipleObjects] abilita questo editor a gestire la selezione
/// di più oggetti che hanno il componente MyComponent.
/// </summary>
[CanEditMultipleObjects]
public class MultipleEditComponentEditor : Editor
{
    // SerializedProperty è il modo corretto in Unity per modificare le variabili 
    // di un componente in un editor personalizzato. Gestisce automaticamente 
    // il multi-editing, l'undo/redo e altre funzionalità dell'editor.
    private SerializedProperty myValueProp;

    void OnEnable()
    {
        // In OnEnable, otteniamo un riferimento alla proprietà "myValue"
        // del nostro oggetto (o oggetti) serializzato.
        myValueProp = serializedObject.FindProperty("myValue");
    }

    public override void OnInspectorGUI()
    {
        // serializedObject.Update() deve essere chiamato all'inizio di OnInspectorGUI.
        // Prepara l'oggetto per essere ispezionato e modificato.
        serializedObject.Update();

        // Mostriamo un'etichetta che indica quanti oggetti sono attualmente selezionati.
        // La proprietà 'targets' (plurale) è un array di tutti gli oggetti selezionati.
        // È disponibile proprio grazie a [CanEditMultipleObjects].
        EditorGUILayout.LabelField("Oggetti selezionati:", targets.Length.ToString());

        // EditorGUILayout.PropertyField disegna il campo predefinito per la nostra proprietà.
        // Quando modifichi questo campo nell'Inspector, la modifica verrà applicata
        // a 'myValue' su TUTTI gli oggetti selezionati.
        EditorGUILayout.PropertyField(myValueProp, new GUIContent("My Value"));

        // Esempio di come accedere ai singoli oggetti, se necessario.
        if (targets.Length > 1)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Accesso ai singoli valori (sola lettura):");
            
            // Iteriamo attraverso l'array 'targets' per ispezionare ogni oggetto.
            foreach (Object obj in targets)
            {
                // Eseguiamo il cast dell'oggetto al nostro tipo di componente.
                MultipleEditComponent comp = (MultipleEditComponent)obj;
                // Mostriamo il nome del GameObject e il valore di 'myValue' per quell'oggetto specifico.
                EditorGUILayout.LabelField($"- {comp.gameObject.name}:", comp.myValue.ToString());
            }
        }

        // serializedObject.ApplyModifiedProperties() deve essere chiamato alla fine.
        // Applica tutte le modifiche fatte alle proprietà e le salva.
        // Se non lo chiami, le modifiche non verranno salvate.
        serializedObject.ApplyModifiedProperties();
    }
}
