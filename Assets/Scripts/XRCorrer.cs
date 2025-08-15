using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class XRCorrer : MonoBehaviour
{
    public ActionBasedContinuousMoveProvider moveProvider;
    public float velocidadNormal = 1.5f;
    public float velocidadCorrer = 4f;

    public InputActionReference inputClickJoystickIzquierdo; // L3

    private void OnEnable()
    {
        if (inputClickJoystickIzquierdo != null)
        {
            inputClickJoystickIzquierdo.action.started += OnCorrer;
            inputClickJoystickIzquierdo.action.canceled += OnDejarDeCorrer;

            // Verificar si la acción está habilitada
            Debug.Log("Input Action para Joystick Click está habilitada.");
        }
    }


    private void OnDisable()
    {
        if (inputClickJoystickIzquierdo != null)
        {
            inputClickJoystickIzquierdo.action.started -= OnCorrer;
            inputClickJoystickIzquierdo.action.canceled -= OnDejarDeCorrer;
        }
    }

    private void OnCorrer(InputAction.CallbackContext context)
    {
        Debug.Log("Joystick presionado: Aumentando velocidad para correr");  // Log para verificar que se presionó el joystick

        if (moveProvider != null)
        {
            moveProvider.moveSpeed = velocidadCorrer;  // Cambia la velocidad a correr
        }
    }

    private void OnDejarDeCorrer(InputAction.CallbackContext context)
    {
        Debug.Log("Joystick liberado: Volviendo a la velocidad normal");  // Log para verificar que se soltó el joystick

        if (moveProvider != null)
        {
            moveProvider.moveSpeed = velocidadNormal;  // Vuelve a la velocidad normal
        }
    }

    private void Start()
    {
        if (moveProvider != null)
        {
            moveProvider.moveSpeed = velocidadNormal;  // Inicia con la velocidad normal
        }
    }
}
