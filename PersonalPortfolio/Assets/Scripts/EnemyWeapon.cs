using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public GameObject bullet;

    public void Shoot()
    {
        Instantiate(bullet, transform.position + transform.rotation * new Vector3(0.12f,0.5f,0.55f), Quaternion.identity).GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.Impulse);
    }
}
