using UnityEngine;

public class PerforadoraRotacion : MonoBehaviour
{
    // Variables privadas de estado
    private bool isGrabbing = false;
    private bool isRotating = false;

    // Velocidad de rotación
    [SerializeField] private float rotationSpeed = 300f;

    // Este transform podría ser la broca, si el script no está en la propia broca
    [SerializeField] private Transform brocaTransform;

    void Update()
    {
        // Si estamos agarrando y rotando, rota la broca
        if (isGrabbing && isRotating && brocaTransform != null)
        {
            brocaTransform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }

    // Métodos que podrían ser llamados por otro script (por ejemplo, un XRGrabInteractable o PalancaController):
    public void StartGrabbing()
    {
        isGrabbing = true;
    }

    public void StopGrabbing()
    {
        isGrabbing = false;
        // También podrías detener rotación si se suelta
        isRotating = false;
    }

    public void StartRotation()
    {
        isRotating = true;
    }

    public void StopRotation()
    {
        isRotating = false;
    }
}
