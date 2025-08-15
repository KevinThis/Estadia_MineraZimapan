using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PalancaController : XRGrabInteractable
{
    [SerializeField] private BrocaController brocaController; // Referencia a tu script que hace girar la broca

    // O puedes usar un evento que lance la perforadora, depende tu arquitectura

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        // Al agarrar la palanca, inicia la rotación
        brocaController.StartRotation();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        // Al soltar la palanca, detiene la rotación
        brocaController.StopRotation();
    }
}
