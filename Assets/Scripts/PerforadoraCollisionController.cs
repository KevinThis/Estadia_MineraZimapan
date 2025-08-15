using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PerforadoraCollisionController : XRGrabInteractable
{
    private Rigidbody rb;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    // Cuando el usuario agarra la perforadora…
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        // Aseguramos que se respeten las colisiones activando la detección continua,
        // y desactivamos el modo kinematic para que la física actúe.
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.isKinematic = false;
    }

    // Al soltar, podrías querer restaurar el estado original (opcional)
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        // Aquí podrías, por ejemplo, volver a activar isKinematic si lo deseas.
        // rb.isKinematic = true;
    }
}
