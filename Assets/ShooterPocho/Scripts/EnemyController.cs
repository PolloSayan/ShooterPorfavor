using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyController : MonoBehaviour
{
    private Animator anim;
    [SerializeField]
    private float speed;
    
    private Transform player;
    private NavMeshAgent agent;
    private bool following;
    [SerializeField]
    private Transform[] patrolPoint;
    private int patrolIndex;
    [SerializeField]
    private float lifePoints;
    [SerializeField]
    private Weapon weapon;
    private bool reloading;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (following == true)
        {
            agent.speed = speed;
            agent.SetDestination(player.position);
            agent.stoppingDistance = 10;
            anim.SetFloat("Vertical", 1f);
            float distance = (player.position - transform.position).magnitude;
            if (distance <= 10)
            { 
                //shoot
                anim.SetFloat("Vertical", 0f);
                transform.LookAt(player);
                if(reloading == false)
                { 
                weapon.EnemyShoot(player);
                }
            }
        }
     
        else 
        {
            agent.speed = speed * 0.5f;
            anim.SetFloat("Vertical", 0.5f);
            agent.SetDestination(patrolPoint[patrolIndex].position);
            float distance = (patrolPoint[patrolIndex].position - transform.position).magnitude;
            if (distance < 1)
            { 
                patrolIndex += 1;
                if (patrolIndex >= patrolPoint.Length)
                { 
                    patrolIndex = 0;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Ray ray = new Ray(transform.position + new Vector3(0, 1.55f, 0), (player.position - transform.position).normalized);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                
                if (hit.transform.tag == "Player")
                {
                    following = true;
                }
            }
            
        }
    }

    public void TakeDamage(float _damage)
    {
        lifePoints -= _damage;
        following = true;
        if (lifePoints <= 0)
        { 
            GameObject ragdollPrefab = (GameObject) Resources.Load("EnemyRagdoll");
            Instantiate(ragdollPrefab, transform.position, transform.rotation);
            gameObject.SetActive(false);
        }
        else
        {
            anim.SetTrigger("Hit");
            //AudioManager.instance.PlaySFX(, transform.position);
        }
    }


    public void Reload()
    { 
        reloading = true;
        anim.SetTrigger("Reload");
        weapon.Reload();
    }

    public void FinishReload()
    { 
        reloading = false;
    }
}
