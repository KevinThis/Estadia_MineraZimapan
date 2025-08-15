using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class HelmetSnapToHead : MonoBehaviour
{
    public Transform headSnapPoint;
    private XRGrabInteractable grab;
    private bool isSnapped = false;

    public bool IsSnapped => isSnapped;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectExited.AddListener(CheckSnap);
    }

    void CheckSnap(SelectExitEventArgs args)
    {
        if (isSnapped) return;

        float distance = Vector3.Distance(transform.position, headSnapPoint.position);
        if (distance < 0.3f)
        {
            SnapToHead();
        }
    }

    void SnapToHead()
    {
        isSnapped = true;
        transform.position = headSnapPoint.position;
        transform.rotation = headSnapPoint.rotation;
        transform.SetParent(headSnapPoint);

        grab.enabled = false;
    }
}
