using UnityEngine;

public class PuertaElevador : MonoBehaviour
{
    // Puertas del piso de arriba
    public Transform puertaArribaIzquierda;
    public Transform puertaArribaDerecha;

    public Vector3 rotacionAbiertaArribaIzq;
    public Vector3 rotacionCerradaArribaIzq;
    public Vector3 rotacionAbiertaArribaDer;
    public Vector3 rotacionCerradaArribaDer;

    // Puertas del piso de abajo
    public Transform puertaAbajoIzquierda;
    public Transform puertaAbajoDerecha;

    public Vector3 rotacionAbiertaAbajoIzq;
    public Vector3 rotacionCerradaAbajoIzq;
    public Vector3 rotacionAbiertaAbajoDer;
    public Vector3 rotacionCerradaAbajoDer;

    public float velocidadRotacion = 2f;

    public enum Piso { Arriba, Abajo }
    public Piso pisoActual = Piso.Arriba;

    public bool puertasAbiertas = true;
    public bool elevadorMoviendose = false;

    // Referencias para el sonido
    public AudioSource audioSource;  // El AudioSource para reproducir el sonido
    public AudioClip sonidoCierrePuertas;  // El sonido que se reproducirá al cerrar las puertas
    public AudioClip sonidoAperturaPuertas; // El sonido que se reproducirá al abrir las puertas

    void Update()
    {
        if (pisoActual == Piso.Arriba)
        {
            RotarPuerta(puertaArribaIzquierda, puertasAbiertas ? rotacionAbiertaArribaIzq : rotacionCerradaArribaIzq);
            RotarPuerta(puertaArribaDerecha, puertasAbiertas ? rotacionAbiertaArribaDer : rotacionCerradaArribaDer);
        }
        else
        {
            RotarPuerta(puertaAbajoIzquierda, puertasAbiertas ? rotacionAbiertaAbajoIzq : rotacionCerradaAbajoIzq);
            RotarPuerta(puertaAbajoDerecha, puertasAbiertas ? rotacionAbiertaAbajoDer : rotacionCerradaAbajoDer);
        }
    }

    void RotarPuerta(Transform puerta, Vector3 rotacionObjetivo)
    {
        Quaternion rotObjetivo = Quaternion.Euler(rotacionObjetivo);
        puerta.localRotation = Quaternion.Lerp(puerta.localRotation, rotObjetivo, Time.deltaTime * velocidadRotacion);
    }

    public void AlternarPuertas()
    {
        if (elevadorMoviendose) return;

        puertasAbiertas = !puertasAbiertas;

        // Reproducir sonido al abrir o cerrar
        if (puertasAbiertas)
        {
            // Reproduce el sonido de apertura
            if (audioSource != null && sonidoAperturaPuertas != null)
            {
                audioSource.PlayOneShot(sonidoAperturaPuertas);
            }
        }
        else
        {
            // Reproduce el sonido de cierre
            if (audioSource != null && sonidoCierrePuertas != null)
            {
                audioSource.PlayOneShot(sonidoCierrePuertas);
            }
        }
    }

    public bool EstanAbiertas() => puertasAbiertas;
    public bool EstaMoviendose() => elevadorMoviendose;
    public Piso GetPisoActual() => pisoActual;
}