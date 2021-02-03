using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject objectToFollow;
    Vector3 offsetToCamera = new Vector3(0.3f,-0.9f,1);
    public GameObject bullet;

    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update(){
        if(Input.GetMouseButtonDown(0)){
            Instantiate(bullet, transform.position + transform.rotation * new Vector3(0f,0.5f,0.5f), Quaternion.identity).GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.Impulse);
        }

        transform.position = Vector3.Lerp(transform.position, objectToFollow.transform.position + objectToFollow.transform.rotation * offsetToCamera, 0.9f);
        transform.rotation = objectToFollow.transform.rotation;
    }
}
