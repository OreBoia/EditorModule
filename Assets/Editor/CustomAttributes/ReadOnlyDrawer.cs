using UnityEditor;
using UnityEngine;

// Associa questo Drawer all'attributo ReadOnlyAttribute
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class MyReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Mantiene l'altezza predefinita del property field (una riga normalmente)
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Salva lo stato GUI corrente e imposta il campo come disabilitato (grigio / non editabile)
        GUI.enabled = false;

        // Disegna il field normalmente
        EditorGUI.PropertyField(position, property, label, true);

        // Ripristina lo stato abilitato
        GUI.enabled = true;
    }
}