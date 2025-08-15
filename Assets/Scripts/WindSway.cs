using UnityEngine;

public class WindSway : MonoBehaviour
{
    [Header("Configuración de movimiento")]
    public float swayAmount = 10f;      // Grados máximos de inclinación
    public float swaySpeed = 2f;        // Velocidad del movimiento
    public Vector3 swayAxis = new Vector3(2, 2, 1); // Eje de rotación (Z por defecto)

    private Quaternion initialRotation;

    void Start()
    {
        // Guardamos la rotación original
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        // Calculamos el ángulo usando una onda seno
        float angle = Mathf.Sin(Time.time * swaySpeed) * swayAmount;

        // Aplicamos la rotación suavemente al objeto
        Quaternion swayRotation = Quaternion.AngleAxis(angle, swayAxis.normalized);
        transform.localRotation = initialRotation * swayRotation;
    }
}
