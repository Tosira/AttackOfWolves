using UnityEngine;

public class EfectoRaton : MonoBehaviour
{
    private Vector3 originalScale;
    public Vector3 enlargedScale; // Lo que aumenta de tama�o el objeto

    void Start()
    {
        // La escala original
        originalScale = transform.localScale;
        enlargedScale = transform.localScale*1.2f;
    }

    // Este metodo es cuando el raton entra al objeto (sin clickear)
    void OnMouseEnter()
    {
        // Aumentar el tama�o del objeto
        transform.localScale = enlargedScale;
    }

    // El raton sale del objeto
    void OnMouseExit()
    {
        // Restaurar el tama�o original del objeto
        transform.localScale = originalScale;
    }
}
