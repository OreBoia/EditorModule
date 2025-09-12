using UnityEngine;


// Questo componente fa ruotare l'oggetto verso un punto specificato ogni frame
public class LookAtPoint : MonoBehaviour
{
    [ReadOnly] public Vector3 lookAtPoint = Vector3.zero;
    void Update()
    {
        // Ruota l'oggetto in modo che "guardi" verso lookAtPoint nello worldspace
        transform.LookAt(lookAtPoint);
    }
}