using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class PerforadoraHoyoConDelay : MonoBehaviour
{
    [Header("Referencia al objeto que se destruirá")]
    public Transform objetoPiedras;

    [Header("Elementos visuales y de sonido")]
    public GameObject hoyoPrefab;
    public GameObject chispasPrefab;
    public AudioSource taladroAudio;
    public Image circuloCarga; // Reemplaza TextMeshProUGUI por Image

    [Header("Configuración")]
    public float tiempoParaPerforar = 4f;
    public float velocidadRotacion = 200f;
    public Transform broca;
    public InputActionReference inputTrigger;

    [Header("Haptics")]
    public XRBaseController controladorDerecho;
    public float intensidadVibracion = 0.5f;
    public float duracionVibracion = 0.2f;

    // Establece un contador estático para la cantidad de hoyos perforados
    public static int hoyosColocados = 0;

    private bool presionandoGatillo = false;
    private bool girando = false;
    private Coroutine perforacionCoroutine;
    private XRGrabInteractable grabInteractable;
    private bool estaAgarrada = false;

    private void Start()
    {
        grabInteractable = GetComponentInParent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener((args) => estaAgarrada = true);
            grabInteractable.selectExited.AddListener((args) => estaAgarrada = false);
        }

        if (inputTrigger != null && inputTrigger.action != null)
            inputTrigger.action.Enable();
    }

    void Update()
    {
        if (inputTrigger != null && inputTrigger.action != null)
            presionandoGatillo = inputTrigger.action.IsPressed();

        if (girando && broca != null)
            broca.Rotate(Vector3.forward, velocidadRotacion * Time.deltaTime);

        if (presionandoGatillo && perforacionCoroutine == null && estaAgarrada)
            perforacionCoroutine = StartCoroutine(EsperarPerforar());

        // Mover el origen del raycast aún más atrás de la broca
        Debug.DrawRay(broca.position - broca.forward * 0.15f, -broca.forward * 0.6f, Color.blue);  // Desplazado más atrás
    }

    void CancelarPerforacion()
    {
        // Detener la corrutina si está en ejecución
        if (perforacionCoroutine != null)
            StopCoroutine(perforacionCoroutine);

        perforacionCoroutine = null;
        girando = false;
        taladroAudio?.Stop();

        if (circuloCarga != null)
        {
            circuloCarga.fillAmount = 0f;
            circuloCarga.enabled = false;
        }

        Debug.Log("Perforación cancelada.");
    }

    IEnumerator EsperarPerforar()
    {
        // Imprimir mensaje de debug cuando comience la perforación
        Debug.Log("🔨 Comenzando perforación...");

        // Realiza el raycast para verificar si la broca está frente a una pared
        RaycastHit hit;
        bool raycastDetectaPared = false;

        Debug.Log("Dirección del raycast: " + broca.forward);

        // Desplazado más hacia atrás y con un alcance mayor
        if (Physics.Raycast(broca.position - broca.forward * 0.15f, -broca.forward, out hit, 0.6f))
        {
            Debug.Log("Impacto contra: " + hit.collider.name + " en layer: " + LayerMask.LayerToName(hit.collider.gameObject.layer));

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Pared"))
            {
                raycastDetectaPared = true;
            }
        }

        // Si no detecta la pared, no hace nada (ni círculo de carga, ni vibración, ni perforación)
        if (!raycastDetectaPared)
        {
            Debug.Log("⚠ No se detectó superficie al frente de la broca. No se realiza perforación.");
            yield break;  // Salir del método sin hacer nada
        }

        // Si detecta la pared, procede con la perforación
        girando = true;
        taladroAudio?.Play();
        controladorDerecho?.SendHapticImpulse(intensidadVibracion, duracionVibracion);

        if (circuloCarga != null)
        {
            circuloCarga.fillAmount = 0f;
            circuloCarga.enabled = true;

            // Posiciona el círculo de carga frente al jugador
            circuloCarga.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1f;
            circuloCarga.transform.LookAt(Camera.main.transform);
        }

        float tiempo = 0f;
        while (tiempo < tiempoParaPerforar && presionandoGatillo)
        {
            tiempo += Time.deltaTime;
            if (circuloCarga != null)
                circuloCarga.fillAmount = tiempo / tiempoParaPerforar;

            yield return null;
        }

        // Mostrar en debug cuando termine el reticleTimer
        Debug.Log("⏱️ ReticleTimer finalizado.");

        CancelarPerforacion();

        if (tiempo >= tiempoParaPerforar)
        {
            // Mostrar en debug cuando se haya perforado exitosamente
            Debug.Log("✅ Perforación exitosa.");

            // Como ya validamos que impactó con una pared, usamos el hit para crear el hoyo hijo de la pared golpeada
            Vector3 posicion = hit.point - hit.normal * 0.05f;
            Quaternion rotacion = Quaternion.LookRotation(-hit.normal);

            Transform paredImpactada = hit.collider.transform;

            // Crear el hoyo
            GameObject hoyoInstanciado = Instantiate(hoyoPrefab, posicion, rotacion, paredImpactada);
            hoyoInstanciado.transform.localScale = new Vector3(1.5f, 1.5f, 2f);

            // Aumentamos el contador de hoyos perforados
            hoyosColocados++;

            Debug.Log($"🎯 Hoyos perforados: {hoyosColocados} / 3");
        }
        else
        {
            Debug.Log("❌ La perforación no se completó a tiempo.");
        }
    }

    public static bool HaySuficientesHoyos()
    {
        return hoyosColocados >= 3;
    }

}
