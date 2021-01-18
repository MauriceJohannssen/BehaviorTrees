using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public GameObject bullet;

    public void Shoot()
    {
        transform.LookAt(GetComponent<AIBlackboard>().Player.transform.position);
        Instantiate(bullet, transform.position + transform.rotation * new Vector3(0.12f,0.4f,0.7f), Quaternion.identity).GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.Impulse);
    }
}
