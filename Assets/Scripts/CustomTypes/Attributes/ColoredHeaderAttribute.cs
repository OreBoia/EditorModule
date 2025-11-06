using UnityEngine;

/// <summary>
/// Attributo per creare header colorati con separatori nell'Inspector
/// </summary>
public class ColoredHeaderAttribute : PropertyAttribute
{
    public string title;
    public Color color;
    public bool showLine;
    public float height;
    
    /// <summary>
    /// Crea un header colorato
    /// </summary>
    /// <param name="title">Testo dell'header</param>
    /// <param name="r">Componente rosso (0-1)</param>
    /// <param name="g">Componente verde (0-1)</param>
    /// <param name="b">Componente blu (0-1)</param>
    /// <param name="showLine">Mostra linea separatrice</param>
    /// <param name="height">Altezza dell'header</param>
    public ColoredHeaderAttribute(string title, float r = 0.7f, float g = 0.7f, float b = 0.7f, bool showLine = true, float height = 20f)
    {
        this.title = title;
        this.color = new Color(r, g, b, 1f);
        this.showLine = showLine;
        this.height = height;
    }
    
    /// <summary>
    /// Crea un header con colore predefinito
    /// </summary>
    /// <param name="title">Testo dell'header</param>
    /// <param name="colorName">Nome del colore predefinito</param>
    /// <param name="showLine">Mostra linea separatrice</param>
    /// <param name="height">Altezza dell'header</param>
    public ColoredHeaderAttribute(string title, string colorName, bool showLine = true, float height = 20f)
    {
        this.title = title;
        this.showLine = showLine;
        this.height = height;
        
        switch (colorName.ToLower())
        {
            case "red":
                color = new Color(0.8f, 0.3f, 0.3f, 1f);
                break;
            case "green":
                color = new Color(0.3f, 0.8f, 0.3f, 1f);
                break;
            case "blue":
                color = new Color(0.3f, 0.5f, 0.8f, 1f);
                break;
            case "yellow":
                color = new Color(0.8f, 0.8f, 0.3f, 1f);
                break;
            case "orange":
                color = new Color(0.8f, 0.5f, 0.2f, 1f);
                break;
            case "purple":
                color = new Color(0.6f, 0.3f, 0.8f, 1f);
                break;
            case "cyan":
                color = new Color(0.3f, 0.8f, 0.8f, 1f);
                break;
            case "pink":
                color = new Color(0.8f, 0.4f, 0.6f, 1f);
                break;
            default:
                color = new Color(0.7f, 0.7f, 0.7f, 1f);
                break;
        }
    }
}