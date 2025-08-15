using UnityEngine;

public class ZonaSonido : MonoBehaviour
{
    public AudioSource audioZona;  // El AudioSource que debe reproducir el sonido de la zona
    public string zonaNombre;      // El nombre de la zona (Pasto, Cueva, etc.)

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el jugador entra en la zona
        if (other.CompareTag("Player"))
        {
            // Activar el sonido de esta zona
            audioZona.Play();
            Debug.Log("Entraste en la zona de: " + zonaNombre);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Verifica si el jugador sale de la zona
        if (other.CompareTag("Player"))
        {
            // Detener el sonido cuando el jugador salga de la zona
            audioZona.Stop();
            Debug.Log("Saliste de la zona de: " + zonaNombre);
        }
    }
}