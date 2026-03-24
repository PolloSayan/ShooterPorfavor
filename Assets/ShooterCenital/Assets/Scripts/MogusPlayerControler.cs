using UnityEngine;
using UnityEngine.InputSystem;

public class MogusPlayerControler : MonoBehaviour
{
    private Animator anim;
    private PlayerInput playerInput;
    private Rigidbody rb;

    [Header("Player Utils")]
    
    [SerializeField]
    private float speed;
    [SerializeField]
    private float rotationSpeed = 10f;
    [SerializeField]
    private GameObject key;
    private Camera cam;
    private PuertaControler puertaControler;

    [Header("Shoot")]
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform bulletSpawnPoint;
    [SerializeField]
    private float bulletForce;
    private bool isShooting;

    void Start()
    {
        anim = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        anim.SetBool("walk", false);
        anim.SetBool("idle", true);
        cam = Camera.main;
        puertaControler = FindObjectOfType<PuertaControler>();

    }

    void Update()
    {
        Vector2 leftStickInput = playerInput.actions["Move"].ReadValue<Vector2>();

        Vector3 movement = (Vector3.forward * leftStickInput.y + Vector3.right * leftStickInput.x) * speed;
        rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);

        if (leftStickInput == Vector2.zero)
            anim.SetBool("walk", false);
        else
            anim.SetBool("walk", true);

        RotarPlayer();
    }


    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isShooting = true;
            SpawnBullet();
        }
        else if (context.canceled)
        {
            isShooting = false;
        }
    }

    private void SpawnBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet.transform.localEulerAngles += new Vector3(0f, 90f, 0f);

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        if (bulletRb != null)
        {
            bulletRb.linearVelocity = transform.forward * bulletForce;
        }

        
    }

    private void RotarPlayer()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 worldPos = ray.GetPoint(distance);
            Vector3 direction = worldPos - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                rb.MoveRotation(Quaternion.Lerp(rb.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == key)
        {
            key.SetActive(false);
            puertaControler.hasKey = true;
        }
    }
}