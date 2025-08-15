/**using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DetonadorDinamita : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject paredADestruir;
    public GameObject particulasExplosionPrefab;
    public GameObject humoPrefab;
    public GameObject escombros;
    public AudioSource sonidoExplosion;

    [Header("Configuración")]
    public Transform puntoCentralExplosion;
    public float tiempoParaDesaparecer = 1.5f;
    public string tagDinamita = "Dinamita";

    [Header("Botón")]
    public Transform botonVisual; // Objeto que se moverá al presionar
    public Vector3 posicionPresionadaOffset = new Vector3(0, -0.01f, 0);
    public float tiempoRetorno = 0.2f;

    [Header("UI")]
    public Text contadorText; // Referencia al texto del temporizador (debe estar en la UI)

    private Vector3 posicionOriginalBoton;
    private bool yaExploto = false;
    private float contadorTiempo = 5f; // Temporizador de 5 segundos

    void Start()
    {
        if (botonVisual != null)
            posicionOriginalBoton = botonVisual.localPosition;

        if (contadorText != null)
            contadorText.gameObject.SetActive(false); // Desactivamos el contador al principio
    }

    public void Detonar()
    {
        if (yaExploto) return;

        if (HayAlMenosUnaDinamita())
        {
            yaExploto = true;
            StartCoroutine(EjecutarExplosion());

            // Simular presión del botón
            if (botonVisual != null)
                StartCoroutine(PresionarBotonVisual());

            // Iniciar el temporizador de explosión
            if (contadorText != null)
                StartCoroutine(IniciarTemporizador());
        }
        else
        {
            Debug.Log($"❌ No hay suficientes hoyos perforados. Actualmente hay {PerforadoraHoyoConDelay.hoyosColocados} / 3 necesarios.");
        }
    }

    private IEnumerator PresionarBotonVisual()
    {
        botonVisual.localPosition = posicionOriginalBoton + posicionPresionadaOffset;
        yield return new WaitForSeconds(tiempoRetorno);
        botonVisual.localPosition = posicionOriginalBoton;
    }

    private bool HayAlMenosUnaDinamita()
    {
        // Verificar si hay suficientes hoyos perforados
        return PerforadoraHoyoConDelay.HaySuficientesHoyos();
    }

    private IEnumerator EjecutarExplosion()
    {
        // Crear la explosión y los efectos
        if (particulasExplosionPrefab != null)
            Instantiate(particulasExplosionPrefab, puntoCentralExplosion.position, Quaternion.identity);

        if (humoPrefab != null)
            Instantiate(humoPrefab, puntoCentralExplosion.position, Quaternion.identity);

        if (sonidoExplosion != null)
            sonidoExplosion.Play();

        // Esperar el tiempo para desaparecer la pared
        yield return new WaitForSeconds(tiempoParaDesaparecer);

        // Desaparecer la pared
        if (paredADestruir != null)
            paredADestruir.SetActive(false);

        // Eliminar la dinamita
        GameObject[] dinamitas = GameObject.FindGameObjectsWithTag(tagDinamita);
        foreach (GameObject dinamita in dinamitas)
        {
            Destroy(dinamita);
        }

        // Generar escombros
        if (escombros != null)
            Instantiate(escombros, puntoCentralExplosion.position, Quaternion.identity);
    }

    private IEnumerator IniciarTemporizador()
    {
        contadorText.gameObject.SetActive(true);
        while (contadorTiempo > 0)
        {
            contadorText.text = Mathf.RoundToInt(contadorTiempo).ToString();
            contadorTiempo -= Time.deltaTime;
            yield return null;
        }
        Detonar();
    }
}**/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DetonadorDinamita : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject paredADestruir;
    public GameObject particulasExplosionPrefab;
    public GameObject humoPrefab;
    public GameObject escombros;
    public AudioSource sonidoExplosion;

    [Header("Configuración")]
    public Transform puntoCentralExplosion;
    public float tiempoParaDesaparecer = 1.5f;
    public string tagDinamita = "Dinamita";

    [Header("Botón")]
    public Transform botonVisual; // Objeto que se moverá al presionar
    public Vector3 posicionPresionadaOffset = new Vector3(0, -0.01f, 0);
    public float tiempoRetorno = 0.2f;

    [Header("UI")]
    public Text contadorText; // Referencia al texto del temporizador (debe estar en la UI)

    private Vector3 posicionOriginalBoton;
    private bool yaExploto = false;
    private float contadorTiempo = 5f; // Temporizador de 5 segundos

    void Start()
    {
        if (botonVisual != null)
            posicionOriginalBoton = botonVisual.localPosition;

        if (contadorText != null)
            contadorText.gameObject.SetActive(false); // Desactivamos el contador al principio
    }

    public void Detonar()
    {
        if (yaExploto) return;

        if (HayAlMenosUnaDinamita())
        {
            yaExploto = true;
            StartCoroutine(EjecutarExplosion());

            // Simular presión del botón
            if (botonVisual != null)
                StartCoroutine(PresionarBotonVisual());

            // Iniciar el temporizador de explosión
            if (contadorText != null)
                StartCoroutine(IniciarTemporizador());
        }
        else
        {
            Debug.Log($"❌ No hay suficientes hoyos perforados. Actualmente hay {PerforadoraHoyoConDelay.hoyosColocados} / 3 necesarios.");
        }
    }

    private IEnumerator PresionarBotonVisual()
    {
        botonVisual.localPosition = posicionOriginalBoton + posicionPresionadaOffset;
        yield return new WaitForSeconds(tiempoRetorno);
        botonVisual.localPosition = posicionOriginalBoton;
    }

    private bool HayAlMenosUnaDinamita()
    {
        // Verificar si hay suficientes hoyos perforados
        return PerforadoraHoyoConDelay.HaySuficientesHoyos();
    }

    private IEnumerator EjecutarExplosion()
    {
        // Crear la explosión y los efectos
        if (particulasExplosionPrefab != null)
            Instantiate(particulasExplosionPrefab, puntoCentralExplosion.position, Quaternion.identity);

        if (humoPrefab != null)
            Instantiate(humoPrefab, puntoCentralExplosion.position, Quaternion.identity);

        if (sonidoExplosion != null)
            sonidoExplosion.Play();

        // Esperar el tiempo para desaparecer la pared
        yield return new WaitForSeconds(tiempoParaDesaparecer);

        // Desaparecer la pared
        if (paredADestruir != null)
            paredADestruir.SetActive(false);

        // Eliminar la dinamita
        GameObject[] dinamitas = GameObject.FindGameObjectsWithTag(tagDinamita);
        foreach (GameObject dinamita in dinamitas)
        {
            Destroy(dinamita);
        }

        // Generar escombros
        if (escombros != null)
            Instantiate(escombros, puntoCentralExplosion.position, Quaternion.identity);

        // Reiniciar el contador de hoyos perforados
        PerforadoraHoyoConDelay.hoyosColocados = 0;  // Reiniciamos el contador
    }

    private IEnumerator IniciarTemporizador()
    {
        contadorText.gameObject.SetActive(true);
        while (contadorTiempo > 0)
        {
            contadorText.text = Mathf.RoundToInt(contadorTiempo).ToString();
            contadorTiempo -= Time.deltaTime;
            yield return null;
        }
        Detonar();
    }
}
