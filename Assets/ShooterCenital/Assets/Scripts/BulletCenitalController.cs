using UnityEngine;

public class BulletCenitalController : MonoBehaviour
{
private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }

    }
}
