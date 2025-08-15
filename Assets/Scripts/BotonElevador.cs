using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BotonElevador : MonoBehaviour
{
    public enum Direccion { Subir, Bajar }
    public Direccion tipo;

    public ControlElevador elevador;
    public PuertaElevador puertas;
    public Renderer botonRenderer;

    public Color colorEncendido = Color.green;
    public Color colorApagado = Color.black;

    private Material botonMaterial;
    private XRBaseInteractable interactable;

    void Start()
    {
        botonMaterial = botonRenderer.material;
        interactable = GetComponent<XRBaseInteractable>();
        interactable.selectEntered.AddListener(_ => EjecutarAccion());
    }

    void Update()
    {
        if (PuedeEjecutar())
        {
            botonMaterial.EnableKeyword("_EMISSION");
            botonMaterial.SetColor("_EmissionColor", colorEncendido);
        }
        else
        {
            botonMaterial.SetColor("_EmissionColor", colorApagado);
        }
    }

    bool PuedeEjecutar()
    {
        if (puertas.EstanAbiertas() || puertas.EstaMoviendose()) return false;

        return (tipo == Direccion.Subir && puertas.GetPisoActual() == PuertaElevador.Piso.Abajo) ||
               (tipo == Direccion.Bajar && puertas.GetPisoActual() == PuertaElevador.Piso.Arriba);
    }

    void EjecutarAccion()
    {
        if (!PuedeEjecutar()) return;

        if (tipo == Direccion.Subir)
            elevador.Subir();
        else
            elevador.Bajar();
    }
}
