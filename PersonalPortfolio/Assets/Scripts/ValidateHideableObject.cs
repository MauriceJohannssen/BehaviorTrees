using UnityEngine;

public class ValidateHideableObject : MonoBehaviour
{
    private Mesh _mesh;
    public Vector3 HighestPoint { get; private set; }
    private Rigidbody _rigidbody;
    [SerializeField] private float angularMovementTreshold = 0.05f;
    private bool _recalculateHighestPoint = false;

    // Start is called before the first frame update
    void Start()
    {
        _mesh = GetComponent<MeshFilter>().mesh;
        HighestPoint = CalculateHighestPoint();
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if (_rigidbody.angularVelocity.magnitude > angularMovementTreshold) _recalculateHighestPoint = true;
        else if (_recalculateHighestPoint && _rigidbody.angularVelocity.magnitude < 0.01f)
        {
            HighestPoint = CalculateHighestPoint();
            _recalculateHighestPoint = false;
        }
    }


    private Vector3 CalculateHighestPoint()
    {
        Vector3 currentHighestVertex = Vector3.zero;
        
        foreach (var vertex in _mesh.vertices)
        {
            if(vertex.y > currentHighestVertex.y) currentHighestVertex = vertex;
        }
        
        //Debug.Log(transform.TransformPoint(currentHighestVertex).y);
        return transform.TransformPoint(currentHighestVertex);
    }

    public float GetRadius()
    {
        float longestRadius = transform.lossyScale.x > transform.lossyScale.z
            ? transform.lossyScale.x
            : transform.lossyScale.z;
        return longestRadius;
    }
}
