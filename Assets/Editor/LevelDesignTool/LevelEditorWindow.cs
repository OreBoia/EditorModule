using UnityEngine;
using UnityEditor;
using System;
using Random = UnityEngine.Random;
// [1] Definizione della classe EditorWindow personalizzata
public class LevelEditorWindow : EditorWindow
{
    // [2] Aggiunge una voce di menu per aprire la finestra
    [MenuItem("Tools/Level Design Editor %#d")]
    public static void ShowWindow()
    {
        // Crea o attiva la finestra denominandola "Level Editor"
        GetWindow<LevelEditorWindow>("Level Editor");
    }

    // [3] Variabili per controlli UI (seguiamo l'inizializzazione più avanti)
    private enum ShapeType { Stanza, Corridoio }
    private ShapeType tipoForma = ShapeType.Stanza;
    private int numeroElementi = 1;
    private float larghezzaStanza = 5f, profonditaStanza = 5f;
    private float lunghezzaCorr = 8f, larghezzaCorr = 2f;
    private bool randomizza = false;

    // Riferimenti a prefab opzionali
    public GameObject prefabStanza;
    public GameObject prefabCorridoio;

    // [4] Metodo principale per disegnare la GUI della finestra
    private void OnGUI()
    {
        // Titolo e descrizione
        GUILayout.Label("Generatore di Stanze e Corridoi",
        EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Configura i parametri e genera il layout nella scena corrente.", MessageType.Info);

        // Selettore forma (stanza o corridoio)
        tipoForma = (ShapeType)EditorGUILayout.EnumPopup("Forma da Generare",tipoForma);

        // Campo per il numero di elementi da creare
        numeroElementi = EditorGUILayout.IntField("Numero di " + (tipoForma ==
        ShapeType.Stanza ? "stanze" : "corridoi"), numeroElementi);
        if (numeroElementi < 1) numeroElementi = 1; // minimo 1 elemento
                                                    // Parametri dimensionali a seconda del tipo selezionato

        if (tipoForma == ShapeType.Stanza)
        {
            GUILayout.Label("Dimensioni Stanza (unità di scena)",
            EditorStyles.boldLabel);
            larghezzaStanza = EditorGUILayout.FloatField("Larghezza (X)",
            larghezzaStanza);
            profonditaStanza = EditorGUILayout.FloatField("Profondità (Z)",
            profonditaStanza);
        }
        else // Corridoio
        {
            GUILayout.Label("Dimensioni Corridoio (unità di scena)",
            EditorStyles.boldLabel);
            lunghezzaCorr = EditorGUILayout.FloatField("Lunghezza (X)",
            lunghezzaCorr);
            larghezzaCorr = EditorGUILayout.FloatField("Larghezza (Z)",
            larghezzaCorr);
        }

        // Opzione di randomizzazione
        randomizza = EditorGUILayout.Toggle(new GUIContent("Randomizza dimensioni / posizioni"), randomizza);

        // Campi per prefab personalizzati
        prefabStanza = (GameObject)EditorGUILayout.ObjectField("Prefab Stanza",
        prefabStanza, typeof(GameObject), false);
        prefabCorridoio = (GameObject)EditorGUILayout.ObjectField("Prefab Corridoio", prefabCorridoio, typeof(GameObject), false);
        EditorGUILayout.Space(); // spazio di separazione
                                 // Pulsanti di azione

        if (GUILayout.Button("Genera"))
        {
            GeneraElementi(); // chiamata al metodo che esegue la generazione
        }

        if (GUILayout.Button("Reset"))
        {
            ResetLayout(); // chiamata al metodo per ripulire la scena dagli oggetti generati
        }
        
        if (GUILayout.Button("Salva Layout come Prefab"))
        {
            SalvaLayoutPrefab(); // chiamata al metodo per salvare il layout come prefab
        }
    }

    private void GeneraElementi()
    {
        // Se esiste già un layout generato in scena, chiediamo conferma o lo rimuoviamo
        GameObject precedente = GameObject.Find("GeneratedLayout");
        if (precedente != null)
        {
            // Rimuove il layout precedente per evitare duplicati (registro in Undo per permettere annulla)
            Undo.DestroyObjectImmediate(precedente);
        }
        
        // Crea un oggetto vuoto come contenitore principale per il nuovo layout
        GameObject parent = new GameObject("GeneratedLayout");
        Undo.RegisterCreatedObjectUndo(parent, "Genera Layout"); // supporto Undo

        // Determina parametri base in base al tipo di forma selezionato
        float baseLarghezza, baseProfondita;
        if (tipoForma == ShapeType.Stanza)
        {
            baseLarghezza = larghezzaStanza;
            baseProfondita = profonditaStanza;
        }
        else // Corridoio
        {
            baseLarghezza = larghezzaCorr;
            baseProfondita = lunghezzaCorr;
        }
        
        // Calcola un range di dispersione per posizionare casualmente gli elementi se randomizzazione attiva
        float maxDim = Mathf.Max(baseLarghezza, baseProfondita);
        float rangePos = numeroElementi * maxDim * 1.5f; // fattore di spread basato su quantità e dimensione max

        // Loop di generazione degli elementi
        for (int i = 0; i < numeroElementi; i++)
        {
            // 1. Creazione dell'oggetto (prefab o primitivo)
            GameObject nuovo;
            if (tipoForma == ShapeType.Stanza)
            {
                // Usa prefab se assegnato, altrimenti crea un Cubo
                if (prefabStanza != null)
                    nuovo = (GameObject)PrefabUtility.InstantiatePrefab(prefabStanza);
                else
                    nuovo = GameObject.CreatePrimitive(PrimitiveType.Cube);

                nuovo.name = "Stanza_" + (i + 1);
            }
            else // Corridoio
            {
                if (prefabCorridoio != null)
                    nuovo = (GameObject)PrefabUtility.InstantiatePrefab(prefabCorridoio);
                else
                    nuovo = GameObject.CreatePrimitive(PrimitiveType.Cube);

                nuovo.name = "Corridoio_" + (i + 1);
            }

            // Registra l'operazione di creazione per Undo
            Undo.RegisterCreatedObjectUndo(nuovo, "Genera Layout");
            
            // 2. Parenting: rende il nuovo oggetto figlio del container principale
            nuovo.transform.SetParent(parent.transform);

            // 3. Posizionamento
            Vector3 pos = Vector3.zero;
            if (randomizza)
            {
            // posizione casuale entro un range quadrato centrato 'origine
                pos.x = Random.Range(-rangePos, rangePos);
                pos.z = Random.Range(-rangePos, rangePos);
            }
            else
            {
                // disposizione a griglia: calcola colonne e righe per distribuire gli elementi
                int cols = Mathf.CeilToInt(Mathf.Sqrt(numeroElementi));
                int row = i / cols;
                int col = i % cols;
                pos.x = col * (baseLarghezza + 2f);
                pos.z = row * (baseProfondita + 2f);
            }

            pos.y = 0f;
            nuovo.transform.position = pos;

            // 4. Scala (dimensioni) dell'oggetto
            float larghezza = baseLarghezza;
            float profondita = baseProfondita;

            if (randomizza)
            {// varia le dimensioni di ±50% circa
                larghezza *= Random.Range(0.5f, 1.5f);
                profondita *= Random.Range(0.5f, 1.5f);
            }
            
            // Applica la scala:
            // - per la "stanza" intendiamo larghezza = asse X, profondità = asse Z.
            // - per il "corridoio" lunghezza = asse X, larghezzaCorr = asse Z.
            nuovo.transform.localScale = new Vector3(larghezza, 1f, profondita);
        }

        // (Opzionale) Seleziona il parent appena creato nella Hierarchy per comodità
        Selection.activeObject = parent;
    }

    private void SalvaLayoutPrefab()
    {
        GameObject layoutGO = GameObject.Find("GeneratedLayout");
        if (layoutGO == null)
        {
            EditorUtility.DisplayDialog("Nessun Layout da salvare", "Non è presente alcun 'GeneratedLayout' nella scena.Genera un layout prima di salvarlo.", "OK");
            return;
        }

        // Apri finestra di dialogo per scegliere il nome e percorso del prefab
        string path = EditorUtility.SaveFilePanelInProject(
                        "Salva Layout come Prefab",
                        "LevelLayout", // nome di default
                        "prefab",
                        "Scegli il nome e la cartella in cui salvare il prefab del layout"
        );

        if (string.IsNullOrEmpty(path))
        {
            // Operazione annullata dall'utente
            return;
        }

        // Esegue il salvataggio del prefab
        bool success;
        PrefabUtility.SaveAsPrefabAsset(layoutGO, path, out success);
        if (success)
        {
            Debug.Log("Layout salvato con successo nel prefab: " + path);
        }
        else
        {
            Debug.LogError("Errore: impossibile salvare il prefab del layout.");
        }
    }

    private void ResetLayout()
    {
        GameObject precedente = GameObject.Find("GeneratedLayout");
        if (precedente != null)
        {
            // Elimina il gameObject contenitore del layout generato (con Undo registrato)
            Undo.DestroyObjectImmediate(precedente);
        }
    }

}


