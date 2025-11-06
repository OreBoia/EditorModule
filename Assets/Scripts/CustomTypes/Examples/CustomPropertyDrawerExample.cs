using UnityEngine;

/// <summary>
/// Esempio di MonoBehaviour che dimostra l'utilizzo dei custom property drawer
/// </summary>
public class CustomPropertyDrawerExample : MonoBehaviour
{
    [Header("Percent Attribute Examples")]
    [Percent]
    [Tooltip("Percentuale di salute (0-100%)")]
    public float healthPercentage = 0.8f; // Verrà mostrato come 80%
    
    [Percent]
    [Tooltip("Percentuale di esperienza (0-100%)")]
    public float experiencePercentage = 0.25f; // Verrà mostrato come 25%
    
    [Percent]
    [Tooltip("Volume dell'audio (0-100%)")]
    public float audioVolume = 0.5f; // Verrà mostrato come 50%

    [Header("MinMaxRange Attribute Examples")]
    [MinMaxRange(0f, 100f)]
    [Tooltip("Range di danno dell'arma")]
    public MinMaxRange weaponDamage = new MinMaxRange(10f, 25f);
    
    [MinMaxRange(-10f, 10f)]
    [Tooltip("Range di temperatura")]
    public MinMaxRange temperatureRange = new MinMaxRange(-5f, 8f);
    
    [MinMaxRange(0f, 1f)]
    [Tooltip("Range di spawn casuale")]
    public MinMaxRange spawnTimeRange = new MinMaxRange(0.5f, 2f);

    [Header("Vector2i Custom Type Examples")]
    [Tooltip("Posizione della griglia")]
    public Vector2i gridPosition = new Vector2i(5, 3);
    
    [Tooltip("Dimensioni della texture")]
    public Vector2i textureSize = new Vector2i(512, 256);
    
    [Tooltip("Coordinate del tile")]
    public Vector2i tileCoordinates = Vector2i.zero;

    [Header("ReadOnly Example (già esistente)")]
    [ReadOnly]
    [Tooltip("Valore calcolato automaticamente")]
    public float calculatedValue;

    private void Start()
    {
        // Esempio di utilizzo dei valori
        Debug.Log($"Health: {healthPercentage * 100f}%");
        Debug.Log($"Weapon damage range: {weaponDamage.min} - {weaponDamage.max}");
        Debug.Log($"Grid position: {gridPosition}");
        
        // Calcola un valore per il campo ReadOnly
        calculatedValue = healthPercentage * 42f;
    }

    private void Update()
    {
        // Aggiorna il valore calcolato ogni frame per mostrare che è ReadOnly
        calculatedValue = Time.time % 100f;
    }

    // Metodi di esempio per utilizzare i valori
    public float GetRandomDamage()
    {
        return weaponDamage.RandomValue;
    }

    public bool IsInTemperatureRange(float temperature)
    {
        return temperatureRange.Contains(temperature);
    }

    public Vector3 GetWorldPositionFromGrid()
    {
        return new Vector3(gridPosition.x, 0f, gridPosition.y);
    }
}