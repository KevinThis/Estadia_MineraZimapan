using UnityEngine;

public class BrocaController : MonoBehaviour
{
    [Header("Configuración de Rotación")]
    // El transform de la broca; si este script está en la broca, puede ser este mismo objeto (usar "transform")
    public Transform brocaTransform;
    public float rotationSpeed = 300f;
    private bool isRotating = false;

    [Header("Sistema de Partículas")]
    // Sistema de partículas para activar al colisionar con la pared
    public ParticleSystem collisionParticles;

    // Método para iniciar la rotación (llamado desde, por ejemplo, la palanca)
    public void StartRotation()
    {
        isRotating = true;
    }

    // Método para detener la rotación
    public void StopRotation()
    {
        isRotating = false;

        // Si las partículas están activas, detenerlas
        if(collisionParticles != null && collisionParticles.isPlaying)
        {
            collisionParticles.Stop();
        }
    }

    void Update()
    {
        // Si la broca está en modo rotación, la giramos en el eje que deseemos (por ejemplo, eje Z o Forward)
        if(isRotating && brocaTransform != null)
        {
            brocaTransform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }

    // Método para detectar el inicio de una colisión
    private void OnCollisionEnter(Collision collision)
    {
        // Sólo activamos las partículas si la broca está girando y colisiona contra la pared
        if(isRotating && collision.gameObject.CompareTag("Wall"))
        {
            if(collisionParticles != null && !collisionParticles.isPlaying)
            {
                collisionParticles.Play();
            }
        }
    }

    // Si la colisión termina, detenemos las partículas
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
            if(collisionParticles != null && collisionParticles.isPlaying)
            {
                collisionParticles.Stop();
            }
        }
    }
}
