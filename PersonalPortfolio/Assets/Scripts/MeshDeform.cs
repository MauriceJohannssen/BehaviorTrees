using System.Collections.Generic;
using UnityEngine;

public class MeshDeform : MonoBehaviour
{
    //public
    public float radius = 0.5f;
    public float collisionRayLength = 5.0f;
    
    //Previous implementation
    //public bool drawEntryRay = true;
    public float punchForce = 1.0f;
    
    //private 
    bool _alreadyExecuted = false;
    private Vector3 _entryVec;
    private Vector3 _entryPoint;
    private List<GameObject> _hitGameObjects = new List<GameObject>();
    //private Rigidbody _hitObjectRigidbody = null;
    private Vector3[] _hitObjectVertices;

    public static int overlapCollision = 0;

    

    // void Update(){
    //     Debug.Log(overlapCollision);
    // }

    //===================================================Previous implementation============================================
    //Info: This wa s the previous impementation using a ray and not a bullet as impact point and direction
    // void Update()
    // {
    //     SetEntryRay();
    //     if(drawEntryRay)DrawEntryRay();
    //     MouseInputHandler();
    // }

    // private void SetEntryRay(){
    //     _entryRay.origin = transform.position;
    //     _entryRay.direction = transform.forward;
    // }

    // private void DrawEntryRay(){
    //     Debug.DrawRay(_entryRay.origin, _entryRay.direction * collisionRayLength, Color.magenta);
    // }

    // private void GetEntryPoint()
    // {
    //     if(Physics.Raycast(_entryRay, out RaycastHit raycastHit, collisionRayLength)){
    //         _hitGameObject = raycastHit.collider.gameObject;
    //         _entryPoint = raycastHit.point;
    //     }
    //     else{
    //         Debug.LogWarning("No game object hit!");
    //     }
    // }

    // private void MouseInputHandler(){
    //     if(Input.GetMouseButtonDown(0)){
    //         //GetEntryPoint();
    //         DeformMesh();
    //     }
    // }
    //======================================================================================================================

    private void DeformMesh(){

        foreach(GameObject hitGameObject in _hitGameObjects){
        
        MeshFilter objectMeshFilter = hitGameObject.GetComponent<MeshFilter>();
        Mesh objectMesh = objectMeshFilter.mesh;
        Destroy(hitGameObject.GetComponent<MeshCollider>());
    
        //Puts objects mesh vertices in separate array
        _hitObjectVertices = objectMesh.vertices;

        for(int i = 0; i < _hitObjectVertices.Length; i++){
            Vector3 vertexToDeform = hitGameObject.transform.TransformPoint(_hitObjectVertices[i]);
            float diffVecMagnitude = (vertexToDeform - _entryPoint).magnitude;

            if(diffVecMagnitude <= radius){
                //Adds the deformation vector to the vertex depending on its position relative to the POI
                Vector3 deformationVector = (_entryVec * punchForce) * (radius - diffVecMagnitude);
                _hitObjectVertices[i] = hitGameObject.transform.InverseTransformPoint(vertexToDeform + deformationVector);
            }
        }
        objectMesh.SetVertices(_hitObjectVertices);
        hitGameObject.AddComponent<MeshCollider>().convex = true;
        objectMesh.RecalculateNormals();
        }
    }
    void OnCollisionEnter(Collision hitGameObject){
        //Check if it's a valid object
        if(!hitGameObject.transform.tag.Equals("Deformable")) return;
        //Check if this script was already executed once
        if(_alreadyExecuted) return;
        _alreadyExecuted = true;

        //Get first (and only) collision point
        ContactPoint contactPoint = hitGameObject.GetContact(0);
        _entryPoint = transform.TransformPoint(contactPoint.point);
        
        //Approach 1
        //Get collision game object
        //_hitGameObjects.Add(hitGameObject.collider.gameObject);
        // AttachedBodies attachedBodies = hitGameObject.collider.transform.GetComponent<AttachedBodies>();
        // if(attachedBodies != null){
        //     foreach(GameObject attachedBody in attachedBodies.attachedGameObjects){
        //     _hitGameObjects.Add(attachedBody);
        //     }
        // }


        //Approach 2
        //if(overlapCollision < 2) {
            Collider[] hitColliders = Physics.OverlapSphere(_entryPoint, radius);
            
            foreach(Collider collider in hitColliders)
            {
                 if(!collider.transform.tag.Equals("Deformable")) {continue; }  
                 _hitGameObjects.Add(collider.gameObject);
                 overlapCollision++;     
            }
        //}
        // else{
        //     _hitGameObjects.Add(hitGameObject.gameObject);
        // }
        
        //Get entry vector/direction
        _entryVec = GetComponent<Rigidbody>().velocity.normalized;
        DeformMesh();
        overlapCollision--;
        Destroy(this);
    }
}

