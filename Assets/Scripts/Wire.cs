/**using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire1 : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshFilter meshFilter;

    [SerializeField] Transform startTransform, endTransform;
    [SerializeField] int segmentCount = 10;
    [SerializeField] float totalLength = 10;

    [SerializeField] float radius = 0.5f;

    [SerializeField] int sides = 4;

    [SerializeField] float totalWeight = 10;
    [SerializeField] float drag = 1;
    [SerializeField] float angularDrag = 1;

    [SerializeField] bool usePhysics = false;

    Transform[] segments = new Transform[0];
    [SerializeField] Transform segmentParent;

    private int prevSegmentCount;
    private float prevTotalLength;
    private float prevDrag;
    private float prevTotalWeight;
    private float prevAngularDrag;
    private float prevRadius;
    private int prevSides;

    private MeshDataRope meshData;
    private Vector3[] vertices;
    private int[,] vertexIndicesMap;
    private bool createTriangles = false;

    private void Start()
    {
        vertices = new Vector3[segmentCount * sides * 3];
        GenerateMesh();
    }

    private void Update()
    {
        if (prevSegmentCount != segmentCount)
        {
            RemoveSegments();
            segments = new Transform[segmentCount];
            GenerateSegments();
            GenerateMesh();
        }

        if (sides != prevSides)
        {
            vertices = new Vector3[segmentCount * sides * 3];
            GenerateMesh();
        }

        if (totalLength != prevTotalLength || drag != prevDrag || totalWeight != prevTotalWeight || angularDrag != prevAngularDrag)
        {
            UpdateWire();
        }

        if (prevRadius != radius && usePhysics)
        {
            UpdateRadius();
            GenerateMesh();
        }

        prevSegmentCount = segmentCount;
        prevSides = sides;
        prevTotalLength = totalLength;
        prevDrag = drag;
        prevTotalWeight = totalWeight;
        prevAngularDrag = angularDrag;
        prevRadius = radius;

        UpdateMesh();
    }

    private void UpdateRadius()
    {
        for (int i = 0; i < segments.Length; i++)
        {
            SetRadiusOnSegment(segments[i], radius);
        }
    }

    void UpdateMesh()
    {
        GenerateVertices();
        meshFilter.mesh.vertices = vertices;
    }

    void GenerateMesh()
    {
        createTriangles = true;

        if (meshData == null)
        {
            meshData = new MeshDataRope(sides, segmentCount + 1, false);
        }
        else
        {
            meshData.ResetMesh(sides, segmentCount + 1, false);
        }

        UpdateIndicesMap();
        GenerateVertices();
        meshData.ProcessMesh();

        Mesh mesh = meshData.CreateMesh();
        meshFilter.sharedMesh = mesh;

        createTriangles = false;
    }

    private void UpdateIndicesMap()
    {
        vertexIndicesMap = new int[segmentCount + 1, sides];
        int meshVertexIndex = 0;
        for (int segmentIndex = 0; segmentIndex < segmentCount + 1; segmentIndex++)
        {
            for (int sideIndex = 0; sideIndex < sides; sideIndex++)
            {
                vertexIndicesMap[segmentIndex, sideIndex] = meshVertexIndex;
                meshVertexIndex++;
            }
        }
    }

    private void GenerateVertices()
    {
        for (int i = 0; i < segments.Length; i++)
        {
            GenerateCircleVerticesAndTriangles(segments[i], i);
        }
    }

    private void GenerateCircleVerticesAndTriangles(Transform segmentTransform, int segmentIndex)
    {
        float angleDiff = 360f / sides;
        Quaternion diffRotation = Quaternion.FromToRotation(Vector3.forward, segmentTransform.forward);

        for (int sideIndex = 0; sideIndex < sides; sideIndex++)
        {
            float angleInRad = angleDiff * sideIndex * Mathf.Deg2Rad;
            float x = -radius * Mathf.Cos(angleInRad);  
            float y = radius * Mathf.Sin(angleInRad);

            Vector3 pointOffset = new Vector3(x, y, 0);
            Vector3 pointRotated = diffRotation * pointOffset;
            Vector3 pointRotatedAtCenter = segmentTransform.position + pointRotated;

            int vertexIndex = segmentIndex * sides + sideIndex;
            vertices[vertexIndex] = pointRotatedAtCenter;

            if (createTriangles)
            {
                meshData.AddVertex(pointRotatedAtCenter, new Vector2(0, 0), vertexIndex);

                if (segmentIndex < segmentCount - 1)
                {
                    int a = vertexIndicesMap[segmentIndex, sideIndex];
                    int b = vertexIndicesMap[segmentIndex + 1, sideIndex];
                    int c = vertexIndicesMap[segmentIndex, (sideIndex + 1) % sides];
                    int d = vertexIndicesMap[segmentIndex + 1, (sideIndex + 1) % sides];

                    meshData.AddTriangle(a, b, c);
                    meshData.AddTriangle(d, a, b);
                }
            }
        }
    }

    private void SetRadiusOnSegment(Transform transform, float radius)
    {
        SphereCollider sphereCollider = transform.GetComponent<SphereCollider>();
        if (sphereCollider != null)
        {
            sphereCollider.radius = radius;
        }
    }

    private void UpdateWire()
    {
        for (int i = 0; i < segments.Length; i++)
        {
            UpdateLengthOnSegment(segments[i], totalLength / segmentCount);
            UpdateWeightOnSegment(segments[i], totalWeight, drag, angularDrag);
        }
    }

    private void UpdateWeightOnSegment(Transform transform, float totalWeight, float drag, float angularDrag)
    {
        Rigidbody rigidbody = transform.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.mass = totalWeight / segmentCount;
            rigidbody.drag = drag;
            rigidbody.angularDrag = angularDrag;
        }
    }

    private void UpdateLengthOnSegment(Transform transform, float lengthPerSegment)
    {
        ConfigurableJoint joint = transform.GetComponent<ConfigurableJoint>();
        if (joint != null)
        {
            joint.connectedAnchor = Vector3.forward * totalLength / segmentCount;
            /**joint.connectedAnchor = Vector3.forward * lengthPerSegment;**
        }
    }

    private void RemoveSegments()
    {
        for (int i = 0; i < segments.Length; i++)
        {
            if (segments[i] != null)
            {
                Destroy(segments[i].gameObject);
            }
        }
    }

    private void GenerateSegments()
    {
        JoinSegment(startTransform, null, true);
        Transform prevTransform = startTransform;

        Vector3 direction = (endTransform.position - startTransform.position);

        for (int i = 0; i < segmentCount; i++)
        {
            GameObject segment = new GameObject($"Segment_{i}");
            segment.transform.SetParent(segmentParent);

            Vector3 pos = prevTransform.position + (direction / segmentCount);
            segment.transform.position = pos;

            JoinSegment(segment.transform, prevTransform);

            segments[i] = segment.transform;
            prevTransform = segment.transform;
        }

        JoinSegment(endTransform, prevTransform, true, true);
    }

    private void JoinSegment(Transform current, Transform connectedTransform, bool isKinematic = false, bool isCloseConnected = true)
    {
        if (current.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rigidbody = current.gameObject.AddComponent<Rigidbody>();
            rigidbody.isKinematic = isKinematic;
            rigidbody.mass = totalWeight / segmentCount;
            rigidbody.drag = drag;
            rigidbody.angularDrag = angularDrag;
        }

        if (usePhysics)
        {
            if (current.GetComponent<SphereCollider>() == null)
            {
                SphereCollider sphereCollider = current.gameObject.AddComponent<SphereCollider>();
                sphereCollider.radius = radius;
            }
        }

        if (connectedTransform != null)
        {
            ConfigurableJoint joint = current.GetComponent<ConfigurableJoint>();
            if (joint == null)
            {
                joint = current.gameObject.AddComponent<ConfigurableJoint>();
            }

            Rigidbody connectedRb = connectedTransform.GetComponent<Rigidbody>();
            joint.connectedBody = connectedRb;

            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = Vector3.forward * (isCloseConnected ? 0.1f : totalLength / segmentCount);

            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;

            joint.angularXMotion = ConfigurableJointMotion.Free;
            joint.angularYMotion = ConfigurableJointMotion.Free;
            joint.angularZMotion = ConfigurableJointMotion.Limited;

            SoftJointLimit softJointLimit = new SoftJointLimit();
            softJointLimit.limit = 0;
            joint.angularZLimit = softJointLimit;

            JointDrive jointDrive = new JointDrive();
            jointDrive.positionDamper = 0;
            jointDrive.positionSpring = 0;
            joint.angularXDrive = jointDrive;
            joint.angularYZDrive = jointDrive;
        }
    }

    void OnDrawGizmos()
    {
        if (segments == null) return;

        for (int i = 0; i < segments.Length; i++)
        {
            if (segments[i] != null)
            {
                Gizmos.DrawWireSphere(segments[i].position, radius);
            }
        }
    }
}**/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshFilter meshFilter;

    [SerializeField] Transform startTransform, endTransform;
    [SerializeField] int segmentCount = 10;
    [SerializeField] float totalLength = 10;

    [SerializeField] float radius = 0.0156f;

    [SerializeField] int sides = 4;

    [SerializeField] float totalWeight = 10;
    [SerializeField] float drag = 1;
    [SerializeField] float angularDrag = 1;

    [SerializeField] bool usePhysics = false;

    Transform[] segments = new Transform[0];
    [SerializeField] Transform segmentParent;

    private int prevSegmentCount;
    private float prevTotalLength;
    private float prevDrag;
    private float prevTotalWeight;
    private float prevAngularDrag;
    private float prevRadius;
    private int prevSides;

    private MeshDataRope meshData;
    private Vector3[] vertices;
    private int[,] vertexIndicesMap;
    private bool createTriangles = false;

    private void Start()
    {
        // Inicializar valores previos
        prevSegmentCount = segmentCount;
        prevSides = sides;
        prevTotalLength = totalLength;
        prevDrag = drag;
        prevTotalWeight = totalWeight;
        prevAngularDrag = angularDrag;
        prevRadius = radius;

        // Crear y posicionar segmentos
        segments = new Transform[segmentCount];
        GenerateSegments();

        // Ahora hay (segmentCount + 2) anillos: start, segmentos intermedios y end
        vertices = new Vector3[(segmentCount + 2) * sides];

        // Generar la malla
        GenerateMesh();
    }


    private void Update()
    {
        if (prevSegmentCount != segmentCount)
        {
            RemoveSegments();
            segments = new Transform[segmentCount];
            GenerateSegments();
            GenerateMesh();
        }

        if (sides != prevSides)
        {
            vertices = new Vector3[(segmentCount + 2) * sides];
            GenerateMesh();
        }


        if (totalLength != prevTotalLength || drag != prevDrag || totalWeight != prevTotalWeight || angularDrag != prevAngularDrag)
        {
            UpdateWire();
        }

        if (prevRadius != radius && usePhysics)
        {
            UpdateRadius();
            GenerateMesh();
        }

        prevSegmentCount = segmentCount;
        prevSides = sides;
        prevTotalLength = totalLength;
        prevDrag = drag;
        prevTotalWeight = totalWeight;
        prevAngularDrag = angularDrag;
        prevRadius = radius;

        UpdateMesh();
    }

    private void UpdateRadius()
    {
        for (int i = 0; i < segments.Length; i++)
        {
            SetRadiusOnSegment(segments[i], radius);
        }
    }

    void UpdateMesh()
    {
        GenerateVertices();
        meshFilter.mesh.vertices = vertices;
    }

    void GenerateMesh()
    {
        createTriangles = true;

        // si meshData es null lo creamos por primera vez,
        // si no, lo reseteamos siempre con segmentCount+2
        if (meshData == null)
            meshData = new MeshDataRope(sides, segmentCount + 2, false);
        else
            meshData.ResetMesh(sides, segmentCount + 2, false);

        UpdateIndicesMap();
        GenerateVertices();
        meshData.ProcessMesh();

        Mesh mesh = meshData.CreateMesh();
        meshFilter.sharedMesh = mesh;

        createTriangles = false;
    }


    private void UpdateIndicesMap()
    {
        // +2 para start y end
        vertexIndicesMap = new int[segmentCount + 2, sides];
        int meshVertexIndex = 0;
        for (int ring = 0; ring < segmentCount + 2; ring++)
            for (int s = 0; s < sides; s++)
                vertexIndicesMap[ring, s] = meshVertexIndex++;
    }


    void GenerateVertices()
    {
        // i = 0 representa startTransform, luego segmentos intermedios, y al final endTransform
        GenerateCircleVerticesAndTriangles(startTransform, 0);
        for (int i = 0; i < segments.Length; i++)
            GenerateCircleVerticesAndTriangles(segments[i], i + 1);
        GenerateCircleVerticesAndTriangles(endTransform, segments.Length + 1);
    }


    private void GenerateCircleVerticesAndTriangles(Transform segmentTransform, int segmentIndex)
    {
        float angleDiff = 360f / sides;

        // --- 1) Calcula el anillo en espacio MUNDIAL como lo haces ahora ---
        //    puntoOffset está en "plano XY", luego lo alineas con la tangente:
        Quaternion diffRotWorld = Quaternion.FromToRotation(
            Vector3.forward,
            segmentTransform.forward
        );
        for (int sideIndex = 0; sideIndex < sides; sideIndex++)
        {
            float angleRad = angleDiff * sideIndex * Mathf.Deg2Rad;
            Vector3 pointOffset = new Vector3(
                -radius * Mathf.Cos(angleRad),
                 radius * Mathf.Sin(angleRad),
                 0
            );
            // punto en espacio MUNDIAL:
            Vector3 worldPt = segmentTransform.position + diffRotWorld * pointOffset;

            // --- 2) ¡Transforma a espacio LOCAL de la malla! ---
            Vector3 localPt = meshFilter.transform.InverseTransformPoint(worldPt);

            // --- 3) Asigna y crea triángulos usando localPt ---
            int vi = segmentIndex * sides + sideIndex;
            vertices[vi] = localPt;
            if (createTriangles)
            {
                meshData.AddVertex(localPt, Vector2.zero, vi);
                // idem para AddTriangle(), usando vertexIndicesMap…
                if (segmentIndex < segmentCount + 1) // ajusta según tu recuento de anillos
                {
                    int a = vertexIndicesMap[segmentIndex, sideIndex];
                    int b = vertexIndicesMap[segmentIndex + 1, sideIndex];
                    int c = vertexIndicesMap[segmentIndex, (sideIndex + 1) % sides];
                    int d = vertexIndicesMap[segmentIndex + 1, (sideIndex + 1) % sides];
                    meshData.AddTriangle(a, b, c);
                    meshData.AddTriangle(c, b, d);
                }
            }
        }
    }


    private void SetRadiusOnSegment(Transform transform, float radius)
    {
        SphereCollider sphereCollider = transform.GetComponent<SphereCollider>();
        if (sphereCollider != null)
        {
            sphereCollider.radius = radius;
        }
    }

    private void UpdateWire()
    {
        for (int i = 0; i < segments.Length; i++)
        {
            UpdateLengthOnSegment(segments[i], totalLength / segmentCount);
            UpdateWeightOnSegment(segments[i], totalWeight, drag, angularDrag);
        }
    }

    private void UpdateWeightOnSegment(Transform transform, float totalWeight, float drag, float angularDrag)
    {
        Rigidbody rigidbody = transform.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.mass = totalWeight / segmentCount;
            rigidbody.drag = drag;
            rigidbody.angularDrag = angularDrag;
        }
    }

    private void UpdateLengthOnSegment(Transform transform, float lengthPerSegment)
    {
        ConfigurableJoint joint = transform.GetComponent<ConfigurableJoint>();
        if (joint != null)
        {
            joint.connectedAnchor = Vector3.forward * totalLength / segmentCount;
            /**joint.connectedAnchor = Vector3.forward * lengthPerSegment;**/
        }
    }

    private void RemoveSegments()
    {
        for (int i = 0; i < segments.Length; i++)
        {
            if (segments[i] != null)
            {
                Destroy(segments[i].gameObject);
            }
        }
    }

    private void GenerateSegments()
    {
        JoinSegment(startTransform, null, true);
        Transform prevTransform = startTransform;

        Vector3 direction = (endTransform.position - startTransform.position);

        for (int i = 0; i < segmentCount; i++)
        {
            GameObject segment = new GameObject($"Segment_{i}");
            segment.transform.SetParent(segmentParent);

            Vector3 pos = prevTransform.position + (direction / segmentCount);
            segment.transform.position = pos;

            // Ajustar el tamaño del collider
            AdjustSphereCollider(segment.transform, i);

            JoinSegment(segment.transform, prevTransform, false);

            segments[i] = segment.transform;
            prevTransform = segment.transform;
        }

        JoinSegment(endTransform, prevTransform, true, true);
    }

    private void AdjustSphereCollider(Transform segmentTransform, int segmentIndex)
    {
        SphereCollider sphereCollider = segmentTransform.GetComponent<SphereCollider>();
        if (sphereCollider != null)
        {
            // Puedes hacer que el tamaño sea proporcional al índice del segmento
            // o usar un valor fijo que sea más pequeño que el radio de la esfera blanca.
            // Aquí, se hace que el radio sea un 50% más pequeño por cada segmento:
            sphereCollider.radius = radius * (1 - (segmentIndex * 0.05f));  // Ajuste del radio
        }
    }


    private void JoinSegment(Transform current, Transform connectedTransform, bool isKinematic = false, bool isCloseConnected = true)
    {
        if (current.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rigidbody = current.gameObject.AddComponent<Rigidbody>();
            rigidbody.isKinematic = isKinematic;
            rigidbody.mass = totalWeight / segmentCount;
            rigidbody.drag = drag;
            rigidbody.angularDrag = angularDrag;
            rigidbody.useGravity = true;
        }

        if (usePhysics)
        {
            if (current.GetComponent<SphereCollider>() == null)
            {
                SphereCollider sphereCollider = current.gameObject.AddComponent<SphereCollider>();
                sphereCollider.radius = radius;
            }
        }

        if (connectedTransform != null)
        {
            ConfigurableJoint joint = current.GetComponent<ConfigurableJoint>();
            if (joint == null)
            {
                joint = current.gameObject.AddComponent<ConfigurableJoint>();
            }

            Rigidbody connectedRb = connectedTransform.GetComponent<Rigidbody>();
            joint.connectedBody = connectedRb;

            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = Vector3.forward * (isCloseConnected ? 0.1f : totalLength / segmentCount);

            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;

            joint.angularXMotion = ConfigurableJointMotion.Free;
            joint.angularYMotion = ConfigurableJointMotion.Free;
            joint.angularZMotion = ConfigurableJointMotion.Limited;

            SoftJointLimit softJointLimit = new SoftJointLimit();
            softJointLimit.limit = 0;
            joint.angularZLimit = softJointLimit;

            SoftJointLimit zLimit = new SoftJointLimit();
            zLimit.limit = 45f;
            joint.angularZLimit = zLimit;

            JointDrive jointDrive = new JointDrive();
            jointDrive.positionDamper = 0;
            jointDrive.positionSpring = 0;
            joint.angularXDrive = jointDrive;
            joint.angularYZDrive = jointDrive;
        }
    }

    void OnDrawGizmos()
    {
        if (segments == null) return;

        for (int i = 0; i < segments.Length; i++)
        {
            if (segments[i] != null)
            {
                Gizmos.DrawWireSphere(segments[i].position, radius);
            }
        }
    }
}