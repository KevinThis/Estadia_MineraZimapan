using UnityEngine;
using System.Collections;

public class HelmetSnapTrigger : MonoBehaviour
{
    public Transform headSnapPoint;
    private bool isSnapped = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isSnapped) return;

        if (other.CompareTag("HeadZone")) // El objeto con collider debe tener este tag
        {
            StartCoroutine(SnapToHeadDelayed());
        }
    }

    IEnumerator SnapToHeadDelayed()
    {
        yield return null; // Espera 1 frame para asegurarse de que el SocketInteractor esté activado

        isSnapped = true;
        transform.position = headSnapPoint.position;
        transform.rotation = headSnapPoint.rotation;
        transform.SetParent(headSnapPoint);
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
    }
}
