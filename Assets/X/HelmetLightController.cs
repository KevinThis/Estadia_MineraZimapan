using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class HelmetLightController : MonoBehaviour
{
    public Light spotLight;
    private XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrabbed);
        grab.selectExited.AddListener(OnReleased);

        if (spotLight != null)
            spotLight.enabled = false;
    }

    void OnGrabbed(SelectEnterEventArgs args)
    {
        if (spotLight != null)
            spotLight.enabled = true;
    }

    void OnReleased(SelectExitEventArgs args)
    {
        if (spotLight != null)
            spotLight.enabled = false;
    }
}
