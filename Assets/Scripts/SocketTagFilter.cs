/**using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketTagFilter : MonoBehaviour
{
    public string requiredTag = "Dinamita";
    private XRSocketInteractor socket;

    private void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
        if (socket != null)
        {
            socket.selectEntered.AddListener(OnSelectEntered);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.tag != requiredTag)
        {
            // Expulsa el objeto si no tiene el tag correcto
            socket.interactionManager.SelectExit(socket, args.interactableObject);
        }
    }
}**/

/**using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class SocketTagFilter : MonoBehaviour
{
    public string requiredTag = "Dinamita";
    private XRSocketInteractor socket;

    private void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
        if (socket != null)
        {
            socket.selectEntered.AddListener(OnSelectEntered);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        Transform objTransform = args.interactableObject.transform;

        if (objTransform.tag != requiredTag)
        {
            // ❌ No es dinamita: la expulsa
            socket.interactionManager.SelectExit(socket, args.interactableObject);
        }
        else
        {
            // ✅ Es dinamita: iniciar proceso para bloquearla
            StartCoroutine(WaitAndLock(objTransform));
        }
    }

    private IEnumerator WaitAndLock(Transform objTransform)
    {
        // Espera 0.2 segundos para que el socket tenga tiempo de "fijarla"
        yield return new WaitForSeconds(0.2f);

        // Ahora sí, desactiva el agarre y congela el Rigidbody
        XRGrabInteractable grab = objTransform.GetComponent<XRGrabInteractable>();
        Rigidbody rb = objTransform.GetComponent<Rigidbody>();

        if (grab != null)
            grab.enabled = false;

        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true; // Lo deja totalmente fijo
        }
    }
}**/

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class SocketTagFilter : MonoBehaviour
{
    public string[] requiredTags = { "Dinamita", "Dinamita2" }; // Acepta dos tags
    private XRSocketInteractor socket;

    private void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
        if (socket != null)
        {
            socket.selectEntered.AddListener(OnSelectEntered);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        Transform objTransform = args.interactableObject.transform;

        // Verifica si el tag está en el array de tags permitidos
        bool isValidTag = false;
        foreach (string tag in requiredTags)
        {
            if (objTransform.tag == tag)
            {
                isValidTag = true;
                break;
            }
        }

        if (!isValidTag)
        {
            // ❌ No es una dinamita válida: la expulsa
            socket.interactionManager.SelectExit(socket, args.interactableObject);
        }
        else
        {
            // ✅ Es una dinamita válida: iniciar proceso para bloquearla
            StartCoroutine(WaitAndLock(objTransform));
        }
    }

    private IEnumerator WaitAndLock(Transform objTransform)
    {
        // Espera 0.2 segundos para que el socket tenga tiempo de "fijarla"
        yield return new WaitForSeconds(0.2f);

        // Ahora sí, desactiva el agarre y congela el Rigidbody
        XRGrabInteractable grab = objTransform.GetComponent<XRGrabInteractable>();
        Rigidbody rb = objTransform.GetComponent<Rigidbody>();

        if (grab != null)
            grab.enabled = false;

        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true; // Lo deja totalmente fijo
        }
    }
}

