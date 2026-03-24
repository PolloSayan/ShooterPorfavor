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
    
    
    private Camera cam;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        anim.SetBool("walk", false);
        anim.SetBool("idle", true);
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 leftStickInput = playerInput.actions["Move"].ReadValue<Vector2>();

        // Ejes globales en vez de transform.forward / transform.right
        Vector3 movement = (Vector3.forward * leftStickInput.y + Vector3.right * leftStickInput.x) * speed;
        rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);

        if (leftStickInput == Vector2.zero)
            anim.SetBool("walk", false);
        else
            anim.SetBool("walk", true);

        RotarPlayer();
    }

    private void RotarPlayer()
    {

        //RayCast camara
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
}
