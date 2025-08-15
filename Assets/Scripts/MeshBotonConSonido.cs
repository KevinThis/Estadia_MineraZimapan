using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MeshBotonConSonido : MonoBehaviour
{
    // Hacemos que AudioSource sea público para asignarlo desde el Inspector
    public AudioSource audioSource;  // AudioSource que puedes asignar manualmente en el Inspector

    void Start()
    {
        // Aseguramos que el AudioSource no sea null antes de usarlo
        if (audioSource == null)
        {
            Debug.LogError("¡AudioSource no asignado en el Inspector!");
        }
    }

    // Este método se llama cuando el XR Controller presiona el botón
    public void OnInteract()
    {
        // Imprime un mensaje en la consola para indicar que se ha presionado el botón
        Debug.Log("¡Botón presionado! Reproduciendo sonido...");

        // Si el AudioSource está asignado, reproducimos el sonido
        if (audioSource != null)
        {
            audioSource.Play(); // Reproduce el sonido cuando el Mesh es presionado
        }
    }
}