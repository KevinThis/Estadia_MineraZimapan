/**using UnityEngine;
using System.Collections;

public class ControlElevador : MonoBehaviour
{
    public Transform elevador;

    // 📌 Coordenadas exactas de los pisos
    public Vector3 posicionPisoBase = new Vector3(0, 0, 0);
    public Vector3 posicionPisoBajo = new Vector3(0, -3, 0);

    public float velocidad = 2f;

    public PuertaElevador controladorPuerta;

    private bool enMovimiento = false;

    public void Subir()
    {
        if (!PuedeMover() || controladorPuerta.GetPisoActual() != PuertaElevador.Piso.Abajo) return;
        StartCoroutine(MoverElevador(PuertaElevador.Piso.Arriba));
    }

    public void Bajar()
    {
        if (!PuedeMover() || controladorPuerta.GetPisoActual() != PuertaElevador.Piso.Arriba) return;
        StartCoroutine(MoverElevador(PuertaElevador.Piso.Abajo));
    }

    bool PuedeMover()
    {
        return !enMovimiento && !controladorPuerta.EstanAbiertas();
    }

    IEnumerator MoverElevador(PuertaElevador.Piso destino)
    {
        enMovimiento = true;
        controladorPuerta.elevadorMoviendose = true;

        Vector3 destinoFinal = (destino == PuertaElevador.Piso.Arriba) ?
            posicionPisoBase :
            posicionPisoBajo;

        while (Vector3.Distance(elevador.position, destinoFinal) > 0.01f)
        {
            elevador.position = Vector3.MoveTowards(elevador.position, destinoFinal, Time.deltaTime * velocidad);
            yield return null;
        }

        elevador.position = destinoFinal;
        controladorPuerta.pisoActual = destino;
        controladorPuerta.elevadorMoviendose = false;
        enMovimiento = false;
    }

    public bool EstaMoviendose() => enMovimiento;
}**/

using UnityEngine;
using System.Collections;

public class ControlElevador : MonoBehaviour
{
    public Transform elevador;

    // 📌 Coordenadas exactas de los pisos
    public Vector3 posicionPisoBase = new Vector3(0, 0, 0);
    public Vector3 posicionPisoBajo = new Vector3(0, -3, 0);

    public float velocidad = 2f;

    public PuertaElevador controladorPuerta;

    private bool enMovimiento = false;

    // 📌 AudioSource asignado desde el Inspector
    public AudioSource audioSource;

    public void Subir()
    {
        if (!PuedeMover() || controladorPuerta.GetPisoActual() != PuertaElevador.Piso.Abajo) return;
        StartCoroutine(MoverElevador(PuertaElevador.Piso.Arriba));
    }

    public void Bajar()
    {
        if (!PuedeMover() || controladorPuerta.GetPisoActual() != PuertaElevador.Piso.Arriba) return;
        StartCoroutine(MoverElevador(PuertaElevador.Piso.Abajo));
    }

    bool PuedeMover()
    {
        return !enMovimiento && !controladorPuerta.EstanAbiertas();
    }

    IEnumerator MoverElevador(PuertaElevador.Piso destino)
    {
        enMovimiento = true;
        controladorPuerta.elevadorMoviendose = true;

        Vector3 destinoFinal = (destino == PuertaElevador.Piso.Arriba) ? 
            posicionPisoBase : 
            posicionPisoBajo;

        // Reproduce el sonido al comenzar a moverse
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play(); // Comienza el sonido
        }

        while (Vector3.Distance(elevador.position, destinoFinal) > 0.01f)
        {
            elevador.position = Vector3.MoveTowards(elevador.position, destinoFinal, Time.deltaTime * velocidad);
            yield return null;
        }

        elevador.position = destinoFinal;
        controladorPuerta.pisoActual = destino;
        controladorPuerta.elevadorMoviendose = false;
        enMovimiento = false;

        // Detiene el sonido cuando el elevador ha llegado a su destino
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop(); // Detiene el sonido
        }
    }

    public bool EstaMoviendose() => enMovimiento;
}