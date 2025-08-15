using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BotonTogglePuertas : MonoBehaviour
{
    public PuertaElevador controladorPuerta;
    public Renderer botonRenderer;
    public Color colorEncendido = Color.cyan;
    public Color colorApagado = Color.black;
    public AudioSource audioSource;  // Referencia al AudioSource
    public AudioClip sonidoBoton;    // El sonido que se reproducirá al presionar el botón

    private Material botonMaterial;
    private XRBaseInteractable interactable;

    void Start()
    {
        botonMaterial = botonRenderer.material;
        interactable = GetComponent<XRBaseInteractable>();
        interactable.selectEntered.AddListener(_ => AlternarPuertas());
    }

    void Update()
    {
        // Solo brilla si NO se está moviendo
        if (!controladorPuerta.EstaMoviendose())
        {
            botonMaterial.EnableKeyword("_EMISSION");
            botonMaterial.SetColor("_EmissionColor", colorEncendido);
        }
        else
        {
            botonMaterial.SetColor("_EmissionColor", colorApagado);
        }
    }

    void AlternarPuertas()
    {
        if (!controladorPuerta.EstaMoviendose())
        {
            controladorPuerta.AlternarPuertas();

            // Reproduce el sonido al presionar el botón
            if (audioSource != null && sonidoBoton != null)
            {
                audioSource.PlayOneShot(sonidoBoton);
            }
        }
    }
}