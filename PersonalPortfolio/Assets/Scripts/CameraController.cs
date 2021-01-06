using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject objectToFollow;
    public Vector3 offset = new Vector3(0,0,0);
    private Camera camera;
    private Quaternion _combinedRotation  = new Quaternion();

    void Start(){
        camera = GetComponent<Camera>();
    }

    void LateUpdate(){
        _combinedRotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x - Input.GetAxis("Mouse Y"), objectToFollow.transform.rotation.eulerAngles.y,0);
        camera.transform.position = objectToFollow.transform.position + offset;
        camera.transform.rotation = _combinedRotation;
    }
}
