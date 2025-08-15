using UnityEngine;

public class WindSwayGroup : MonoBehaviour
{
    [Header("Configuración de movimiento")]
    public float swayAmount = 10f;
    public float swaySpeed = 1f;
    public Vector3 swayAxis = new Vector3(0, 0, 1);
    public bool variarMovimiento = true;

    private Transform[] flores;
    private Quaternion[] rotacionesIniciales;
    private float[] offsets;

    void Start()
    {
        // Obtenemos todas las flores hijas
        int count = transform.childCount;
        flores = new Transform[count];
        rotacionesIniciales = new Quaternion[count];
        offsets = new float[count];

        for (int i = 0; i < count; i++)
        {
            flores[i] = transform.GetChild(i);
            rotacionesIniciales[i] = flores[i].localRotation;
            offsets[i] = variarMovimiento ? Random.Range(0f, Mathf.PI * 2f) : 0f;
        }
    }

    void Update()
    {
        for (int i = 0; i < flores.Length; i++)
        {
            float angle = Mathf.Sin(Time.time * swaySpeed + offsets[i]) * swayAmount;
            Quaternion swayRotation = Quaternion.AngleAxis(angle, swayAxis.normalized);
            flores[i].localRotation = rotacionesIniciales[i] * swayRotation;
        }
    }
}
