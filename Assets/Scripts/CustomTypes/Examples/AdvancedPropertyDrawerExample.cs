using UnityEngine;

/// <summary>
/// Esempio avanzato che mostra utilizzi più complessi dei custom property drawer
/// </summary>
public class AdvancedPropertyDrawerExample : MonoBehaviour
{
    [System.Serializable]
    public class WeaponStats
    {
        [Header("Damage Settings")]
        [MinMaxRange(1f, 100f)]
        [Tooltip("Range di danno base dell'arma")]
        public MinMaxRange baseDamage = new MinMaxRange(10f, 20f);
        
        [MinMaxRange(0f, 50f)]
        [Tooltip("Danno critico aggiuntivo")]
        public MinMaxRange criticalDamage = new MinMaxRange(5f, 15f);
        
        [Header("Accuracy Settings")]
        [Percent]
        [Tooltip("Precisione base dell'arma")]
        public float baseAccuracy = 0.85f;
        
        [Percent]
        [Tooltip("Possibilità di colpo critico")]
        public float criticalChance = 0.15f;
    }

    [System.Serializable]
    public class GridObject
    {
        [Tooltip("Posizione nella griglia")]
        public Vector2i gridPosition;
        
        [Tooltip("Dimensioni occupate nella griglia")]
        public Vector2i gridSize = Vector2i.one;
        
        [Percent]
        [Tooltip("Probabilità di spawn")]
        public float spawnChance = 0.5f;
    }

    [System.Serializable]
    public class EnvironmentSettings
    {
        [Header("Temperature")]
        [MinMaxRange(-40f, 50f)]
        [Tooltip("Range di temperatura giornaliera")]
        public MinMaxRange dailyTemperature = new MinMaxRange(15f, 25f);
        
        [Header("Lighting")]
        [MinMaxRange(0f, 24f)]
        [Tooltip("Ore di luce solare")]
        public MinMaxRange sunlightHours = new MinMaxRange(6f, 18f);
        
        [Percent]
        [Tooltip("Intensità della luce ambiente")]
        public float ambientLightIntensity = 0.3f;
        
        [Header("Wind")]
        [MinMaxRange(0f, 100f)]
        [Tooltip("Velocità del vento (km/h)")]
        public MinMaxRange windSpeed = new MinMaxRange(5f, 20f);
    }

    [Header("Weapon Configuration")]
    public WeaponStats primaryWeapon = new WeaponStats();
    public WeaponStats secondaryWeapon = new WeaponStats();

    [Header("Grid Objects")]
    public GridObject[] gridObjects = new GridObject[]
    {
        new GridObject { gridPosition = new Vector2i(0, 0), gridSize = new Vector2i(2, 2), spawnChance = 0.8f },
        new GridObject { gridPosition = new Vector2i(3, 1), gridSize = new Vector2i(1, 3), spawnChance = 0.6f }
    };

    [Header("Environment")]
    public EnvironmentSettings environment = new EnvironmentSettings();

    [Header("Player Stats")]
    [Percent]
    [Tooltip("Livello di salute del giocatore")]
    public float playerHealth = 1f;
    
    [Percent]
    [Tooltip("Livello di mana del giocatore")]
    public float playerMana = 0.7f;
    
    [Percent]
    [Tooltip("Livello di stamina del giocatore")]
    public float playerStamina = 0.9f;

    [Header("Map Configuration")]
    [Tooltip("Dimensioni della mappa di gioco")]
    public Vector2i mapSize = new Vector2i(100, 100);
    
    [Tooltip("Posizione di spawn del giocatore")]
    public Vector2i playerSpawnPoint = new Vector2i(50, 50);

    [Header("Performance Settings")]
    [MinMaxRange(30f, 144f)]
    [Tooltip("Range FPS target")]
    public MinMaxRange targetFPS = new MinMaxRange(60f, 120f);
    
    [Percent]
    [Tooltip("Qualità delle ombre")]
    public float shadowQuality = 0.8f;

    private void Start()
    {
        LogAllSettings();
    }

    private void LogAllSettings()
    {
        Debug.Log("=== Weapon Stats ===");
        Debug.Log($"Primary Weapon - Base Damage: {primaryWeapon.baseDamage.min}-{primaryWeapon.baseDamage.max}");
        Debug.Log($"Primary Weapon - Accuracy: {primaryWeapon.baseAccuracy * 100f}%");
        Debug.Log($"Primary Weapon - Crit Chance: {primaryWeapon.criticalChance * 100f}%");

        Debug.Log("=== Environment ===");
        Debug.Log($"Temperature Range: {environment.dailyTemperature.min}°C - {environment.dailyTemperature.max}°C");
        Debug.Log($"Sunlight Hours: {environment.sunlightHours.min}h - {environment.sunlightHours.max}h");
        Debug.Log($"Ambient Light: {environment.ambientLightIntensity * 100f}%");

        Debug.Log("=== Player Stats ===");
        Debug.Log($"Health: {playerHealth * 100f}%");
        Debug.Log($"Mana: {playerMana * 100f}%");
        Debug.Log($"Stamina: {playerStamina * 100f}%");

        Debug.Log("=== Map Info ===");
        Debug.Log($"Map Size: {mapSize}");
        Debug.Log($"Player Spawn: {playerSpawnPoint}");
    }

    // Metodi di utilità per dimostrare l'uso dei valori
    public float CalculateWeaponDamage(WeaponStats weapon)
    {
        float damage = weapon.baseDamage.RandomValue;
        
        if (Random.value < weapon.criticalChance)
        {
            damage += weapon.criticalDamage.RandomValue;
            Debug.Log("Critical hit!");
        }
        
        return damage;
    }

    public bool IsValidGridPosition(Vector2i position)
    {
        return position.x >= 0 && position.x < mapSize.x && 
               position.y >= 0 && position.y < mapSize.y;
    }

    public float GetCurrentTemperature()
    {
        return environment.dailyTemperature.RandomValue;
    }

    public bool ShouldSpawnObject(GridObject obj)
    {
        return Random.value < obj.spawnChance;
    }
}