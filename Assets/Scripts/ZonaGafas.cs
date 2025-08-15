using UnityEngine;

public class ZonaGafas : MonoBehaviour
{
    [Header("Referencia al modelo físico de las gafas")]
    public GameObject gafasModelo;

    [Header("Canvas o efecto visual que simula las gafas")]
    public GameObject efectoVisual;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gafas"))
        {
            // Oculta el modelo físico (opcional)
            if (gafasModelo != null)
                gafasModelo.SetActive(false);

            // Activa el efecto visual (canvas, overlay, etc.)
            if (efectoVisual != null)
                efectoVisual.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Gafas"))
        {
            // Vuelve a mostrar el modelo físico
            if (gafasModelo != null)
                gafasModelo.SetActive(true);

            // Desactiva el efecto visual
            if (efectoVisual != null)
                efectoVisual.SetActive(false);
        }
    }
}
