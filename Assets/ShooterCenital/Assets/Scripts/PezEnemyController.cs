using UnityEngine;

public class PezEnemyController : MonoBehaviour
{


    public Transform player;

    [SerializeField]
    public float moveSpeed = 3f;
    [SerializeField]
    public float attackRange = 1.2f;
    [SerializeField]
    public float attackCooldown = 1.5f;
    [SerializeField]
    public int attackDamage = 10;
    [SerializeField]
    private float attackTimer = 0f;

    private Rigidbody rb;
    private Animator anim;

    private bool isAttacking;
    private bool isChasing;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        isChasing = true;
    }

    void Update()
    {
        if (isChasing == true)
        {
            Chase();
        }
        if (isAttacking == true)
        {
            Attack();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isAttacking = true;
            isChasing = false;

        }
        else
        { 
            isAttacking = false;
            isChasing = true;
        }
    }

    void Chase()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
        anim.SetBool("Running", true);
        attackTimer = 0f;
    }

    void Attack()
    {
        rb.linearVelocity = Vector3.zero;
        anim.SetBool("Running", false);
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            attackTimer = attackCooldown;
        }
    }

}
