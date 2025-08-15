using UnityEngine;

public class ActivarEscombros : MonoBehaviour
{
    public GameObject[] escombrosHijos; // Referencia a los escombros hijos (que deberían tener Rigidbody y Collider)
    private bool activado = false;

    public void Activar()
    {
        if (activado) return;

        // Activar escombros visualmente
        foreach (GameObject escombro in escombrosHijos)
        {
            // Activar el renderer para que se vean
            Renderer escombroRenderer = escombro.GetComponent<Renderer>();
            if (escombroRenderer != null)
                escombroRenderer.enabled = true;  // Hacerlos visibles

            // Asegurarse de que los Rigidbody no sean cinemáticos
            Rigidbody rb = escombro.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // Activa la física
                rb.AddExplosionForce(300f, transform.position, 5f); // Aplicar la fuerza de la explosión
            }
        }

        activado = true; // Evitar que se active más de una vez
    }
}
