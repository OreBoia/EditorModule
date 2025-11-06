using UnityEngine;

/// <summary>
/// Esempio di utilizzo completo del ColoredHeaderAttribute
/// </summary>
public class ColoredHeaderExamples : MonoBehaviour
{
    [ColoredHeader("🔴 COLORI PREDEFINITI", "red")]
    public float redValue = 1f;
    
    [ColoredHeader("🟢 GREEN SECTION", "green")]
    public float greenValue = 1f;
    
    [ColoredHeader("🔵 BLUE SECTION", "blue")]
    public float blueValue = 1f;
    
    [ColoredHeader("🟡 YELLOW SECTION", "yellow")]
    public float yellowValue = 1f;
    
    [ColoredHeader("🟠 ORANGE SECTION", "orange")]
    public float orangeValue = 1f;
    
    [ColoredHeader("🟣 PURPLE SECTION", "purple")]
    public float purpleValue = 1f;
    
    [ColoredHeader("🔷 CYAN SECTION", "cyan")]
    public float cyanValue = 1f;
    
    [ColoredHeader("🩷 PINK SECTION", "pink")]
    public float pinkValue = 1f;
    
    [ColoredHeader("COLORE PERSONALIZZATO RGB", 1f, 0.5f, 0.2f)]
    public float customColorValue = 1f;
    
    [ColoredHeader("SENZA LINEA SEPARATRICE", "blue", false)]
    public float noLineValue = 1f;
    
    [ColoredHeader("HEADER ALTO", "green", true, 30f)]
    public float tallHeaderValue = 1f;
    
    [ColoredHeader("⭐ SEZIONE IMPORTANTE ⭐", 1f, 0.8f, 0.3f, true, 25f)]
    [Tooltip("Questo è un campo molto importante!")]
    public string importantField = "Importante";
    
    [System.Serializable]
    public class DatabaseSettings
    {
        [ColoredHeader("🗄️ DATABASE CONFIG", "cyan")]
        public string connectionString = "localhost";
        public int port = 5432;
        public string username = "admin";
        
        [ColoredHeader("🔒 SECURITY", "red")]
        public bool useSSL = true;
        public int timeoutSeconds = 30;
    }
    
    [System.Serializable]
    public class UISettings  
    {
        [ColoredHeader("🎨 VISUAL SETTINGS", "purple")]
        public Color primaryColor = Color.blue;
        public Color secondaryColor = Color.white;
        
        [ColoredHeader("📏 LAYOUT", "green")]
        public int buttonSize = 50;
        public float spacing = 10f;
    }
    
    [ColoredHeader("🛠️ CONFIGURAZIONI AVANZATE", 0.8f, 0.4f, 0.9f)]
    public DatabaseSettings database = new DatabaseSettings();
    public UISettings uiSettings = new UISettings();
    
    [ColoredHeader("🎮 GAMEPLAY FINALE", "orange", true, 35f)]
    [Space(10)]
    public bool enableAdvancedFeatures = false;
    public float difficultyMultiplier = 1.0f;
    
    private void Start()
    {
        Debug.Log("ColoredHeader Examples loaded successfully!");
        Debug.Log($"Database connection: {database.connectionString}:{database.port}");
        Debug.Log($"UI Primary Color: {uiSettings.primaryColor}");
    }
}