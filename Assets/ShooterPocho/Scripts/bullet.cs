using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Video;

public class bullet : MonoBehaviour
{
    [SerializeField]
    public float damage;
    [SerializeField]
    private GameObject bulletHolePrefab;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit");
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
            //vfx sangre
        }
        else if (collision.gameObject.tag == "Player")
        { 
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            //vfx sangre
        }
        else
        {
            Quaternion rotation = Quaternion.FromToRotation(Vector3.back, collision.GetContact(0).normal);
            GameObject bulletHoleClone = Instantiate(bulletHolePrefab, collision.GetContact(0).point, rotation, collision.transform);
            bulletHoleClone.transform.localPosition += new Vector3(0, 0, 0.02f);
            Destroy(bulletHoleClone, 5f);
        }
            Destroy(gameObject);

    }
    //VIDEO//////////////////////////////////////////////////

    /*
        VideoPlayer videoPlayer;

        private void OnTriggerEnter(Collider other)
        {
            videoPlayer.Play();
            videoPlayer.Stop();
            videoPlayer.Pause();
            videoPlayer.clip

        }*/

    PlayableDirector videoRT;
    
    /*private void OnTriggerEnter(Collider other)
        {
            videoRT.Play();
            videoRT.Stop();
            videoRT.Pause();
            videoRT.state == PlayState.Playing;
        }*/


}
