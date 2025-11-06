# Unity Custom Property Drawers - Guida Completa

## Indice

1. [Introduzione ai Property Drawers](#introduzione)
2. [Struttura del Sistema](#struttura)
3. [Property Drawers Analizzati](#property-drawers)
   - [ColoredHeaderDrawer](#coloredheaderdrawer)
   - [MinMaxRangeDrawer](#minmaxrangedrawer)
   - [PercentDrawer](#percentdrawer)
   - [ReadOnlyDrawer](#readonlydrawer)
   - [Vector2iDrawer](#vector2idrawer)
   - [HiddenBoolDrawer](#hiddenbooldrawer)
4. [Custom Editor](#custom-editor)
5. [Best Practices](#best-practices)

---

## Introduzione {#introduzione}

I **Property Drawers** in Unity sono classi che permettono di personalizzare l'aspetto e il comportamento dei campi nell'Inspector. Esistono due tipi principali:

- **PropertyDrawer**: Per personalizzare la visualizzazione di tipi specifici o proprietà con attributi
- **DecoratorDrawer**: Per aggiungere elementi decorativi (header, separatori) senza modificare i valori

### Concetti Chiave

- **SerializedProperty**: Rappresentazione serializzata di una proprietà Unity
- **PropertyAttribute**: Classe base per tutti gli attributi personalizzati
- **CustomPropertyDrawer**: Attributo che associa un drawer a un tipo o attributo
- **Rect**: Struttura che definisce posizione e dimensioni nell'GUI

---

## Struttura del Sistema {#struttura}

```
Assets/
├── Scripts/CustomTypes/
│   ├── Attributes/          # Definizioni degli attributi
│   ├── MinMaxRange.cs       # Tipi personalizzati
│   └── Vector2i.cs
└── Editor/CustomAttributes/ # Property Drawers (solo in Editor)
    ├── ColoredHeaderDrawer.cs
    ├── MinMaxRangeDrawer.cs
    └── ...
```

**Importante**: I Property Drawers devono sempre essere nella cartella `Editor` o in una sottocartella con file `Editor.asmdef`.

---

## Property Drawers {#property-drawers}

### ColoredHeaderDrawer {#coloredheaderdrawer}

#### Scopo

Crea header colorati con separatori per organizzare visivamente l'Inspector.

#### Attributo Associato

```csharp
[System.Serializable]
public class ColoredHeaderAttribute : PropertyAttribute
{
    public string title;    // Testo dell'header
    public Color color;     // Colore dell'header
    public bool showLine;   // Mostra linea separatrice
    public float height;    // Altezza dell'header
}
```

#### Property Drawer

```csharp
[CustomPropertyDrawer(typeof(ColoredHeaderAttribute))]
public class ColoredHeaderDrawer : DecoratorDrawer
```

**Tipo**: `DecoratorDrawer` - Non modifica valori, solo decorazione visuale.

#### Metodi Principali

##### `GetHeight()`

```csharp
public override float GetHeight()
{
    return coloredHeader.height + (coloredHeader.showLine ? 8f : 3f);
}
```

- **Scopo**: Calcola l'altezza necessaria per l'header
- **Parametri**: Nessuno
- **Return**: `float` - Altezza in pixel
- **Logica**: Altezza base + spazio extra per la linea se presente

##### `OnGUI(Rect position)`

```csharp
public override void OnGUI(Rect position)
{
    // Salva colori originali
    Color originalColor = GUI.color;
    Color originalBackgroundColor = GUI.backgroundColor;
    
    // Calcola posizioni
    float textHeight = coloredHeader.height - (coloredHeader.showLine ? lineHeight + 3f : 0f);
    Rect headerRect = new Rect(position.x, position.y, position.width, textHeight);
    
    // Disegna sfondo colorato
    Color backgroundColor = coloredHeader.color * 0.3f;
    backgroundColor.a = 0.3f;
    EditorGUI.DrawRect(headerRect, backgroundColor);
    
    // Disegna bordi
    // ... codice per bordi ...
    
    // Disegna testo
    GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel);
    GUI.Label(headerRect, coloredHeader.title.ToUpper(), headerStyle);
}
```

#### Componenti GUI Utilizzati

1. **EditorGUI.DrawRect()**: Disegna rettangoli colorati per sfondo e bordi
2. **GUIStyle**: Personalizza l'aspetto del testo
3. **GUI.Label()**: Renderizza il testo dell'header
4. **Color manipulation**: Crea variazioni di colore per sfondo e bordi

#### Esempio d'Uso

```csharp
[ColoredHeader("⚔️ WEAPON STATS", "red")]
public WeaponStats weapon;

[ColoredHeader("🛡️ DEFENSE", "blue", true, 25f)]
public DefenseStats defense;
```

#### Limitazioni

- **DecoratorDrawer** non funziona bene con array/liste
- **Soluzione**: Usare campi dummy o Custom Editor completo

---

### MinMaxRangeDrawer {#minmaxrangedrawer}

#### Scopo

Crea un controllo slider bidirezionale per gestire range di valori min/max.

#### Tipo Personalizzato

```csharp
[System.Serializable]
public struct MinMaxRange
{
    public float min;
    public float max;
    
    public MinMaxRange(float min, float max) { /* ... */ }
    public float RandomValue => Random.Range(min, max);
    public bool Contains(float value) => value >= min && value <= max;
}
```

#### Attributo Associato

```csharp
public class MinMaxRangeAttribute : PropertyAttribute
{
    public float min;  // Limite minimo del range
    public float max;  // Limite massimo del range
    
    public MinMaxRangeAttribute(float min, float max) { /* ... */ }
}
```

#### Property Drawer

```csharp
[CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
public class MinMaxRangeDrawer : PropertyDrawer
```

**Tipo**: `PropertyDrawer` - Modifica i valori della proprietà.

#### Metodi Principali

##### `OnGUI(Rect position, SerializedProperty property, GUIContent label)`

```csharp
public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
{
    MinMaxRangeAttribute minMaxRangeAttribute = (MinMaxRangeAttribute)attribute;
    
    // Validazione del tipo
    if (property.type != "MinMaxRange")
    {
        EditorGUI.LabelField(position, label.text, "Use [MinMaxRange] with MinMaxRange fields only.");
        return;
    }
    
    // Accesso ai campi interni
    SerializedProperty minProp = property.FindPropertyRelative("min");
    SerializedProperty maxProp = property.FindPropertyRelative("max");
    
    // Layout del controllo
    Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
    Rect sliderRect = new Rect(/* ... calcoli posizione ... */);
    Rect minFieldRect = new Rect(/* ... */);
    Rect maxFieldRect = new Rect(/* ... */);
    
    // Controlli GUI
    EditorGUI.BeginChangeCheck();
    EditorGUI.MinMaxSlider(sliderRect, ref minValue, ref maxValue, minLimit, maxLimit);
    minValue = EditorGUI.FloatField(minFieldRect, minValue);
    maxValue = EditorGUI.FloatField(maxFieldRect, maxValue);
    
    if (EditorGUI.EndChangeCheck())
    {
        // Validazione e salvataggio
        minValue = Mathf.Clamp(minValue, minLimit, maxLimit);
        maxValue = Mathf.Clamp(maxValue, minLimit, maxLimit);
        minProp.floatValue = minValue;
        maxProp.floatValue = maxValue;
    }
}
```

#### Componenti GUI Utilizzati

1. **property.FindPropertyRelative()**: Accede ai campi interni della struct
2. **EditorGUI.MinMaxSlider()**: Slider bidirezionale
3. **EditorGUI.FloatField()**: Campi numerici per input preciso
4. **EditorGUI.BeginChangeCheck()/EndChangeCheck()**: Rileva modifiche
5. **Mathf.Clamp()**: Limita i valori nei range consentiti

#### Calcolo Layout

```csharp
// Posizioni calcolate per layout orizzontale
Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
Rect sliderRect = new Rect(
    position.x + EditorGUIUtility.labelWidth, 
    position.y, 
    position.width - EditorGUIUtility.labelWidth - 130, 
    position.height
);
Rect minFieldRect = new Rect(sliderRect.xMax + 5, position.y, 60, position.height);
Rect maxFieldRect = new Rect(minFieldRect.xMax + 5, position.y, 60, position.height);
```

#### Esempio d'Uso

```csharp
[MinMaxRange(0f, 100f)]
public MinMaxRange damage = new MinMaxRange(10f, 20f);

[MinMaxRange(-40f, 50f)]
public MinMaxRange temperature = new MinMaxRange(15f, 25f);
```

---

### PercentDrawer {#percentdrawer}

#### Scopo

Visualizza valori float (0-1) come percentuali (0-100%) con slider.

#### Attributo Associato

```csharp
public class PercentAttribute : PropertyAttribute
{
    // Attributo semplice senza parametri
}
```

#### Property Drawer

```csharp
[CustomPropertyDrawer(typeof(PercentAttribute))]
public class PercentDrawer : PropertyDrawer
```

#### Metodi Principali

##### `OnGUI(Rect position, SerializedProperty property, GUIContent label)`

```csharp
public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
{
    // Validazione del tipo
    if (property.propertyType != SerializedPropertyType.Float)
    {
        EditorGUI.LabelField(position, label.text, "Use [Percent] with float fields only.");
        return;
    }
    
    // Conversione da valore normalizzato (0-1) a percentuale (0-100)
    float percentValue = property.floatValue * 100f;
    
    EditorGUI.BeginChangeCheck();
    percentValue = EditorGUI.Slider(position, label, percentValue, 0f, 100f);
    
    if (EditorGUI.EndChangeCheck())
    {
        // Conversione da percentuale a valore normalizzato
        property.floatValue = percentValue / 100f;
    }
}
```

#### Concetti Chiave

1. **Conversione Valori**:
   - **Storage**: 0-1 (normalizzato)
   - **Display**: 0-100 (percentuale)

2. **Validazione Tipo**: Controlla che sia applicato solo a `float`

3. **EditorGUI.Slider()**: Crea slider con range personalizzato

#### Esempio d'Uso

```csharp
[Percent]
public float accuracy = 0.85f;  // Mostrato come 85%

[Percent] 
public float criticalChance = 0.15f;  // Mostrato come 15%
```

---

### ReadOnlyDrawer {#readonlydrawer}

#### Scopo

Rende i campi non modificabili nell'Inspector (sola lettura).

#### Attributo Associato

```csharp
public class ReadOnlyAttribute : PropertyAttribute 
{ 
    // Attributo marker senza parametri
}
```

#### Property Drawer

```csharp
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class MyReadOnlyDrawer : PropertyDrawer
```

#### Metodi Principali

##### `GetPropertyHeight(SerializedProperty property, GUIContent label)`

```csharp
public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
{
    // Mantiene l'altezza standard del campo
    return EditorGUI.GetPropertyHeight(property, label, true);
}
```

##### `OnGUI(Rect position, SerializedProperty property, GUIContent label)`

```csharp
public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
{
    // Disabilita temporaneamente l'input
    GUI.enabled = false;
    
    // Disegna il campo normalmente (ma disabilitato)
    EditorGUI.PropertyField(position, property, label, true);
    
    // Riabilita l'input per altri controlli
    GUI.enabled = true;
}
```

#### Concetti Chiave

1. **GUI.enabled**: Flag globale che controlla l'interattività
2. **EditorGUI.PropertyField()**: Disegna il campo usando il drawer standard
3. **Stato Temporaneo**: Importante ripristinare `GUI.enabled = true`

#### Esempio d'Uso

```csharp
[ReadOnly]
public float calculatedValue;

[ReadOnly]
public string generatedID;
```

---

### Vector2iDrawer {#vector2idrawer}

#### Scopo

Visualizza il tipo personalizzato `Vector2i` con due campi integer affiancati.

#### Tipo Personalizzato

```csharp
[System.Serializable]
public struct Vector2i
{
    public int x;
    public int y;
    
    public Vector2i(int x, int y) { /* ... */ }
    public static Vector2i zero => new Vector2i(0, 0);
    public static Vector2i one => new Vector2i(1, 1);
}
```

#### Property Drawer

```csharp
[CustomPropertyDrawer(typeof(Vector2i))]
public class Vector2iDrawer : PropertyDrawer
```

**Nota**: Questo drawer è associato al **tipo** `Vector2i`, non a un attributo.

#### Metodi Principali

##### `OnGUI(Rect position, SerializedProperty property, GUIContent label)`

```csharp
public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
{
    // Accesso ai campi interni
    SerializedProperty xProp = property.FindPropertyRelative("x");
    SerializedProperty yProp = property.FindPropertyRelative("y");
    
    // Validazione
    if (xProp == null || yProp == null)
    {
        EditorGUI.LabelField(position, label.text, "Vector2i must have 'x' and 'y' int fields.");
        return;
    }
    
    // Raggruppa le proprietà correlate
    EditorGUI.BeginProperty(position, label, property);
    
    // Calcolo layout
    Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
    float fieldWidth = (position.width - EditorGUIUtility.labelWidth - 25) / 2f;
    Rect xRect = new Rect(position.x + EditorGUIUtility.labelWidth, position.y, fieldWidth, position.height);
    Rect yRect = new Rect(xRect.xMax + 5, position.y, fieldWidth, position.height);
    
    // Disegna i controlli
    EditorGUI.LabelField(labelRect, label);
    
    float oldLabelWidth = EditorGUIUtility.labelWidth;
    EditorGUIUtility.labelWidth = 15f;  // Label compatte per "X" e "Y"
    
    EditorGUI.PropertyField(xRect, xProp, new GUIContent("X"));
    EditorGUI.PropertyField(yRect, yProp, new GUIContent("Y"));
    
    EditorGUIUtility.labelWidth = oldLabelWidth;
    
    EditorGUI.EndProperty();
}
```

#### Componenti GUI Utilizzati

1. **EditorGUI.BeginProperty()/EndProperty()**: Raggruppa proprietà correlate
2. **property.FindPropertyRelative()**: Accede ai campi della struct
3. **EditorGUIUtility.labelWidth**: Controlla la larghezza delle label
4. **Layout Calculation**: Divide lo spazio disponibile tra i campi

#### Calcolo Layout Dettagliato

```csharp
// Larghezza disponibile per i campi (escludendo label principale e spacing)
float fieldWidth = (position.width - EditorGUIUtility.labelWidth - 25) / 2f;

// Posizionamento dei campi X e Y
Rect xRect = new Rect(
    position.x + EditorGUIUtility.labelWidth,  // Dopo la label principale
    position.y, 
    fieldWidth, 
    position.height
);
Rect yRect = new Rect(
    xRect.xMax + 5,  // 5 pixel di spacing
    position.y, 
    fieldWidth, 
    position.height
);
```

#### Esempio d'Uso

```csharp
public Vector2i gridPosition = new Vector2i(5, 10);
public Vector2i mapSize = new Vector2i(100, 100);
```

---

### HiddenBoolDrawer {#hiddenbooldrawer}

#### Scopo

Nasconde automaticamente i campi `bool` che terminano con "Header" (usati come dummy per ColoredHeader).

#### Property Drawer

```csharp
[CustomPropertyDrawer(typeof(bool))]
public class HiddenBoolDrawer : PropertyDrawer
```

**Nota**: Questo drawer si applica a **tutti** i campi `bool`, usa logica condizionale per nascondere solo quelli specifici.

#### Metodi Principali

##### `GetPropertyHeight(SerializedProperty property, GUIContent label)`

```csharp
public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
{
    // Se il nome del campo finisce con "Header", non riservare spazio
    if (property.name.EndsWith("Header"))
    {
        return 0f;
    }
    
    // Altrimenti usa l'altezza standard
    return EditorGUI.GetPropertyHeight(property, label, true);
}
```

##### `OnGUI(Rect position, SerializedProperty property, GUIContent label)`

```csharp
public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
{
    // Se il nome del campo finisce con "Header", non disegnarlo
    if (property.name.EndsWith("Header"))
    {
        return;
    }
    
    // Altrimenti disegna il campo bool normalmente
    EditorGUI.PropertyField(position, property, label, true);
}
```

#### Logica di Filtering

- **Pattern Matching**: Usa `property.name.EndsWith("Header")`
- **Zero Height**: Campi nascosti non occupano spazio
- **Conditional Rendering**: Disegna solo i bool "normali"

#### Esempio d'Uso

```csharp
// Questi campi saranno nascosti
[SerializeField] private bool _weaponStatsHeader = true;
[SerializeField] private bool _playerStatsHeader = true;

// Questi campi saranno visibili normalmente
public bool isActive = true;
public bool debugMode = false;
```

---

## Custom Editor {#custom-editor}

### AdvancedPropertyDrawerExampleEditor

#### Scopo

Fornisce controllo completo sul rendering dell'Inspector, risolvendo le limitazioni dei DecoratorDrawer.

#### Struttura

```csharp
[CustomEditor(typeof(AdvancedPropertyDrawerExample))]
public class AdvancedPropertyDrawerExampleEditor : Editor
```

#### Metodi Principali

##### `OnInspectorGUI()`

```csharp
public override void OnInspectorGUI()
{
    serializedObject.Update();
    
    AdvancedPropertyDrawerExample script = (AdvancedPropertyDrawerExample)target;
    
    // Usa Reflection per ottenere tutti i campi
    FieldInfo[] fields = typeof(AdvancedPropertyDrawerExample)
        .GetFields(BindingFlags.Public | BindingFlags.Instance);
    
    foreach (FieldInfo field in fields)
    {
        // Salta i campi header dummy
        if (field.name.EndsWith("Header"))
            continue;
        
        // Cerca ColoredHeaderAttribute
        ColoredHeaderAttribute headerAttr = GetHeaderAttribute(field);
        
        // Disegna header se presente
        if (headerAttr != null)
        {
            DrawColoredHeader(headerAttr);
        }
        
        // Disegna il campo
        SerializedProperty prop = serializedObject.FindProperty(field.name);
        if (prop != null)
        {
            EditorGUILayout.PropertyField(prop, true);
        }
    }
    
    serializedObject.ApplyModifiedProperties();
}
```

#### Reflection per Header Discovery

```csharp
private ColoredHeaderAttribute GetHeaderAttribute(FieldInfo field)
{
    // Prima cerca campo header dummy
    string headerFieldName = "_" + field.Name + "Header";
    FieldInfo headerField = typeof(AdvancedPropertyDrawerExample)
        .GetField(headerFieldName, BindingFlags.NonPublic | BindingFlags.Instance);
    
    if (headerField != null)
    {
        return headerField.GetCustomAttribute<ColoredHeaderAttribute>();
    }
    
    // Altrimenti cerca direttamente sul campo
    return field.GetCustomAttribute<ColoredHeaderAttribute>();
}
```

#### Rendering Header Personalizzato

```csharp
private void DrawColoredHeader(ColoredHeaderAttribute header)
{
    GUILayout.Space(5f);
    
    // Ottieni rect per l'header
    Rect rect = GUILayoutUtility.GetRect(0f, header.height, GUILayout.ExpandWidth(true));
    
    // Disegna sfondo e bordi
    Color backgroundColor = header.color * 0.3f;
    backgroundColor.a = 0.3f;
    EditorGUI.DrawRect(rect, backgroundColor);
    
    // Bordi colorati
    Color borderColor = header.color * 0.6f;
    borderColor.a = 0.8f;
    EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, 1f), borderColor);
    // ... altri bordi ...
    
    // Testo dell'header
    GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel)
    {
        fontSize = 12,
        alignment = TextAnchor.MiddleCenter,
        normal = { textColor = header.color }
    };
    GUI.Label(rect, header.title.ToUpper(), headerStyle);
}
```

#### Vantaggi del Custom Editor

1. **Controllo Completo**: Gestisce ogni aspetto del rendering
2. **Nessuna Limitazione**: Funziona con array, liste, oggetti complessi
3. **Reflection**: Scoperta automatica degli attributi
4. **Layout Flessibile**: Può riorganizzare completamente l'Inspector

---

## Best Practices {#best-practices}

### 1. Validazione dei Tipi

```csharp
// Sempre validare il tipo di proprietà
if (property.propertyType != SerializedPropertyType.Float)
{
    EditorGUI.LabelField(position, label.text, "Use [Percent] with float fields only.");
    return;
}
```

### 2. Gestione degli Stati GUI

```csharp
// Salvare e ripristinare stati GUI
Color originalColor = GUI.color;
GUI.enabled = false;
// ... operazioni GUI ...
GUI.color = originalColor;
GUI.enabled = true;
```

### 3. Layout Calculations

```csharp
// Usare EditorGUIUtility per dimensioni standard
float labelWidth = EditorGUIUtility.labelWidth;
float lineHeight = EditorGUIUtility.singleLineHeight;
float spacing = EditorGUIUtility.standardVerticalSpacing;
```

### 4. Change Detection

```csharp
// Rileva modifiche per performance
EditorGUI.BeginChangeCheck();
// ... controlli GUI ...
if (EditorGUI.EndChangeCheck())
{
    // Applica modifiche solo se necessario
    property.floatValue = newValue;
}
```

### 5. Organizzazione File

```
Editor/
├── CustomAttributes/     # Property Drawers
├── CustomEditors/       # Custom Editors
└── Utilities/          # Utility per Editor
```

### 6. Performance

- **Evita allocazioni** in `OnGUI()`
- **Cache GUIStyle** quando possibile
- **Usa BeginChangeCheck()** per evitare aggiornamenti inutili

### 7. Accessibility

- **Tooltip informativi** sui controlli
- **Messaggi di errore chiari** per tipi sbagliati
- **Layout consistente** tra diversi drawer

### 8. Testing

- **Testa con diversi tipi** di proprietà
- **Verifica il comportamento** con array/liste
- **Controlla la compatibilità** con altri drawer

---

## Conclusione

Questo sistema di Property Drawers fornisce:

- ✅ **Controlli GUI personalizzati** per tipi specifici
- ✅ **Validazione automatica** dei tipi
- ✅ **Layout responsivo** che si adatta alle dimensioni
- ✅ **Integrazione seamless** con l'Inspector Unity
- ✅ **Codice riutilizzabile** e modulare

I Property Drawers sono uno strumento potente per migliorare l'esperienza di sviluppo in Unity, rendendo l'Inspector più intuitivo e funzionale.
