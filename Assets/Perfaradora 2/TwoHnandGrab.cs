using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TwoHandGrabInteractable : XRGrabInteractable
{
    public List<XRSimpleInteractable> secondHandGrabPoints = new List<XRSimpleInteractable>(); // Puntos de agarre, incluyendo el cubo

    private IXRSelectInteractor secondInteractor;
    private BoxCollider secondHandCollider; // El cubo que estamos usando como segundo punto de agarre

    private Vector3 initialPosition; // Para guardar la posici�n inicial y restringir el movimiento

    void Start()
    {
        foreach (var item in secondHandGrabPoints)
        {
            item.selectEntered.AddListener(OnSecondHandGrab);
            item.selectExited.AddListener(OnSecondHandRelease);
        }

        // Se espera que el segundo punto de agarre sea el cubo, por eso se toma el primero
        secondHandCollider = secondHandGrabPoints[0].GetComponent<BoxCollider>();

        initialPosition = transform.position; // Guarda la posici�n inicial
    }

    public void OnSecondHandGrab(SelectEnterEventArgs args)
    {
        Debug.Log("SECOND HAND GRAB");
        secondInteractor = args.interactorObject;

        // Si es el cubo, asegurarse de que est� en el lugar correcto
        if (secondInteractor.transform.GetComponent<BoxCollider>() != null)
        {
            // Se asegura que el cubo tambi�n puede mover la perforadora
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
        // Si hay segunda mano, actualiza posici�n pero no rotaci�n
        if (secondInteractor != null && firstInteractorSelecting != null)
        {
            // Mueve el objeto solo en el eje Z (o el que desees)
            Vector3 newPosition = new Vector3(
                initialPosition.x, // Mantener la posici�n inicial en X
                initialPosition.y, // Mantener la posici�n inicial en Y
                (firstInteractorSelecting.transform.position.z + secondInteractor.transform.position.z) / 2f // Solo mueve en el eje Z
            );

            // Posiciona la perforadora en el punto intermedio de ambas manos
            transform.position = newPosition;

            // Si el cubo se est� utilizando como punto de agarre, tambi�n mueve la perforadora
            if (secondHandCollider != null)
            {
                // Actualiza la posici�n de la perforadora con respecto al cubo
                transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y,
                    secondHandCollider.transform.position.z
                );
            }

            // Restringir la rotaci�n a la inicial, no se permite ninguna rotaci�n
            transform.rotation = Quaternion.Euler(0, 0, 0); // Restringe la rotaci�n en todos los ejes
        }

        base.ProcessInteractable(updatePhase);
    }
}
