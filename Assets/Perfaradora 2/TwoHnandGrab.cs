using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TwoHandGrabInteractable : XRGrabInteractable
{
    public List<XRSimpleInteractable> secondHandGrabPoints = new List<XRSimpleInteractable>(); // Puntos de agarre, incluyendo el cubo

    private IXRSelectInteractor secondInteractor;
    private BoxCollider secondHandCollider; // El cubo que estamos usando como segundo punto de agarre

    private Vector3 initialPosition; // Para guardar la posición inicial y restringir el movimiento

    void Start()
    {
        foreach (var item in secondHandGrabPoints)
        {
            item.selectEntered.AddListener(OnSecondHandGrab);
            item.selectExited.AddListener(OnSecondHandRelease);
        }

        // Se espera que el segundo punto de agarre sea el cubo, por eso se toma el primero
        secondHandCollider = secondHandGrabPoints[0].GetComponent<BoxCollider>();

        initialPosition = transform.position; // Guarda la posición inicial
    }

    public void OnSecondHandGrab(SelectEnterEventArgs args)
    {
        Debug.Log("SECOND HAND GRAB");
        secondInteractor = args.interactorObject;

        // Si es el cubo, asegurarse de que esté en el lugar correcto
        if (secondInteractor.transform.GetComponent<BoxCollider>() != null)
        {
            // Se asegura que el cubo también puede mover la perforadora
            secondHandCollider = secondInteractor.transform.GetComponent<BoxCollider>();
        }
    }

    public void OnSecondHandRelease(SelectExitEventArgs args)
    {
        Debug.Log("SECOND HAND RELEASE");
        if (secondInteractor == args.interactorObject)
        {
            secondInteractor = null;
            secondHandCollider = null; // Liberar el cubo cuando se suelta
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log("First Grab Enter");
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        Debug.Log("First Grab Exit");
        base.OnSelectExited(args);
        secondInteractor = null; // por si suelta ambas manos
    }

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        bool isAlreadyGrabbed = isSelected && !interactor.Equals(firstInteractorSelecting);
        return base.IsSelectableBy(interactor) && !isAlreadyGrabbed;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        // Si hay segunda mano, actualiza posición pero no rotación
        if (secondInteractor != null && firstInteractorSelecting != null)
        {
            // Mueve el objeto solo en el eje Z (o el que desees)
            Vector3 newPosition = new Vector3(
                initialPosition.x, // Mantener la posición inicial en X
                initialPosition.y, // Mantener la posición inicial en Y
                (firstInteractorSelecting.transform.position.z + secondInteractor.transform.position.z) / 2f // Solo mueve en el eje Z
            );

            // Posiciona la perforadora en el punto intermedio de ambas manos
            transform.position = newPosition;

            // Si el cubo se está utilizando como punto de agarre, también mueve la perforadora
            if (secondHandCollider != null)
            {
                // Actualiza la posición de la perforadora con respecto al cubo
                transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y,
                    secondHandCollider.transform.position.z
                );
            }

            // Restringir la rotación a la inicial, no se permite ninguna rotación
            transform.rotation = Quaternion.Euler(0, 0, 0); // Restringe la rotación en todos los ejes
        }

        base.ProcessInteractable(updatePhase);
    }
}
