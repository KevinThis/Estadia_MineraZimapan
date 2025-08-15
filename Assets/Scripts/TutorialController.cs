using UnityEngine;
using UnityEngine.UI;  // Necesario para trabajar con UI
using UnityEngine.XR.Interaction.Toolkit;

public class TutorialController : MonoBehaviour
{
    public GameObject[] tutorialPanels;  // Array de paneles de tutorial
    public Button nextTutorialButton;  // Botón para avanzar al siguiente tutorial

    private int currentTutorialIndex = 0;  // Índice para controlar qué tutorial se muestra

    void Start()
    {
        // Asegurarse de que el primer tutorial esté activo al principio
        SetTutorialPanelActive(currentTutorialIndex);

        // Configurar el botón para avanzar al siguiente tutorial
        nextTutorialButton.onClick.AddListener(NextTutorial);

    }

    // Mostrar el siguiente tutorial
    private void NextTutorial()
    {
        if (currentTutorialIndex < tutorialPanels.Length - 1)
        {
            // Desactivar el panel actual
            tutorialPanels[currentTutorialIndex].SetActive(false);

            // Incrementar el índice y activar el siguiente panel
            currentTutorialIndex++;
            SetTutorialPanelActive(currentTutorialIndex);
        }
        else
        {
            // Finalizar tutorial o mostrar mensaje de cierre
            Debug.Log("Tutorial completado.");
        }
    }

    // Activar el panel de tutorial según el índice
    private void SetTutorialPanelActive(int index)
    {
        tutorialPanels[index].SetActive(true);  // Activar el panel correspondiente
    }

    private void OnDestroy()
    {
        nextTutorialButton.onClick.RemoveListener(NextTutorial);
    }
}
