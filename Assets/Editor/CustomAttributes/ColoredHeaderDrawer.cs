using UnityEngine;
using UnityEditor;

/// <summary>
/// Property Drawer per ColoredHeaderAttribute che crea header colorati con separatori
/// </summary>
[CustomPropertyDrawer(typeof(ColoredHeaderAttribute))]
public class ColoredHeaderDrawer : DecoratorDrawer
{
    private ColoredHeaderAttribute coloredHeader => (ColoredHeaderAttribute)attribute;
    
    public override float GetHeight()
    {
        return coloredHeader.height + (coloredHeader.showLine ? 5f : 0f);
    }
    
    public override void OnGUI(Rect position)
    {
        // Salva il colore originale della GUI
        Color originalColor = GUI.color;
        Color originalBackgroundColor = GUI.backgroundColor;
        
        // Calcola le posizioni
        float lineHeight = 2f;
        float textHeight = coloredHeader.height - (coloredHeader.showLine ? lineHeight + 3f : 0f);
        
        Rect headerRect = new Rect(position.x, position.y, position.width, textHeight);
        Rect lineRect = new Rect(position.x, position.y + textHeight + 2f, position.width, lineHeight);
        
        // Disegna lo sfondo dell'header con colore più scuro
        Color backgroundColor = coloredHeader.color * 0.3f;
        backgroundColor.a = 0.3f;
        EditorGUI.DrawRect(headerRect, backgroundColor);
        
        // Stile per il testo dell'header
        GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 12,
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = coloredHeader.color }
        };
        
        // Aggiungi un bordo più scuro intorno all'header
        Color borderColor = coloredHeader.color * 0.6f;
        borderColor.a = 0.8f;
        
        // Bordi laterali
        EditorGUI.DrawRect(new Rect(headerRect.x, headerRect.y, 2f, headerRect.height), borderColor);
        EditorGUI.DrawRect(new Rect(headerRect.xMax - 2f, headerRect.y, 2f, headerRect.height), borderColor);
        
        // Bordi superiore e inferiore
        EditorGUI.DrawRect(new Rect(headerRect.x, headerRect.y, headerRect.width, 1f), borderColor);
        EditorGUI.DrawRect(new Rect(headerRect.x, headerRect.yMax - 1f, headerRect.width, 1f), borderColor);
        
        // Disegna il testo dell'header
        GUI.Label(headerRect, coloredHeader.title.ToUpper(), headerStyle);
        
        // Disegna la linea separatrice se richiesta
        if (coloredHeader.showLine)
        {
            Color lineColor = coloredHeader.color;
            lineColor.a = 0.8f;
            EditorGUI.DrawRect(lineRect, lineColor);
            
            // Aggiungi un effetto gradiente alla linea
            Color fadeColor = lineColor;
            fadeColor.a = 0.2f;
            
            // Gradiente sinistro
            for (int i = 0; i < 20; i++)
            {
                Color gradColor = Color.Lerp(fadeColor, lineColor, i / 20f);
                EditorGUI.DrawRect(new Rect(lineRect.x + i, lineRect.y, 1f, lineRect.height), gradColor);
            }
            
            // Gradiente destro
            for (int i = 0; i < 20; i++)
            {
                Color gradColor = Color.Lerp(lineColor, fadeColor, i / 20f);
                EditorGUI.DrawRect(new Rect(lineRect.xMax - 20 + i, lineRect.y, 1f, lineRect.height), gradColor);
            }
        }
        
        // Ripristina i colori originali
        GUI.color = originalColor;
        GUI.backgroundColor = originalBackgroundColor;
    }
}