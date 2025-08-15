using UnityEngine;

public class ElevatorPlayerAttach : MonoBehaviour
{
    private bool playerOnElevator = false;
    private Transform playerTransform;
    private Vector3 lastElevatorPosition;

    void Start()
    {
        lastElevatorPosition = transform.position;
    }

    void LateUpdate()
    {
        if (playerOnElevator && playerTransform != null)
        {
            Vector3 delta = transform.position - lastElevatorPosition;
            playerTransform.position += delta;
        }

        lastElevatorPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            playerOnElevator = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnElevator = false;
            playerTransform = null;
        }
    }
}
