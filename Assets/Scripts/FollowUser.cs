using UnityEngine;

public class FollowUser : MonoBehaviour
{
    public Transform cameraTransform;  // Cámara del XR Rig o la cámara del jugador
    public float distance = 2.0f;      // Distancia frente a la cámara

    void Update()
    {
        // Aseguramos que el panel se mantenga siempre frente a la cámara
        Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * distance;
        transform.position = targetPosition;

        // Hacer que el panel siempre mire hacia la cámara
        transform.rotation = Quaternion.LookRotation(cameraTransform.position - transform.position);
    }
}
