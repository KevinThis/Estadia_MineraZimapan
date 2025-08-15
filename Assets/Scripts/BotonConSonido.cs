using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BotonConSonido : MonoBehaviour
{
    public AudioSource audioSource;  // Referencia al AudioSource
    public AudioClip sonidoBoton;    // El sonido que se reproducirá al presionar el botón

    private XRBaseInteractable interactable;

    void Start()
    {
        interactable = GetComponent<XRBaseInteractable>();
        interactable.selectEntered.AddListener(_ => ReproducirSonido());
    }

    void ReproducirSonido()
    {
        // Reproduce el sonido al presionar el botón
        if (audioSource != null && sonidoBoton != null)
        {
            audioSource.PlayOneShot(sonidoBoton);
        }
    }
}