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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {

        float distancia = Vector2.Distance(transform.position, player.position);

        if (distancia > attackRange)
            Chase();
        else
            Attack();
    }

    void Chase()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        attackTimer = 0f;
    }

    void Attack()
    {
        rb.linearVelocity = Vector2.zero;

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            attackTimer = attackCooldown;
        }
    }

}
