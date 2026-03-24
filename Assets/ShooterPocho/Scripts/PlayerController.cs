using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private PlayerInput playerInput;
    private Rigidbody rb;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float lookSpeed;
    [SerializeField]
    private Transform followTarget;
    private LevelManager lm;
    [SerializeField]
    private float timeToStartHealing;
    [SerializeField]
    private float healing;
    private IEnumerator enumerator;
    [SerializeField]
    private Transform rightHand, leftHand;
    [SerializeField]
    private GameObject grenadePrefab;
    [SerializeField]
    private Transform grenadeSpawn;
    private LineRenderer lr;
    [SerializeField]
    private float throwForce;
    [SerializeField]
    private Transform elHueso;
    [SerializeField]
    private float offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        playerInput.actions["Reload"].Disable();
        lr = grenadeSpawn.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 leftStickInput = playerInput.actions["Move"].ReadValue<Vector2>();
        anim.SetFloat("Horizontal", leftStickInput.x);
        anim.SetFloat("Vertical", leftStickInput.y);
        Vector3 movement = ((transform.forward * leftStickInput.y) + (transform.right * leftStickInput.x)) * speed;
        rb.linearVelocity = new Vector3 (movement.x, rb.linearVelocity.y, movement.z);

        //lineRenderer
        if (lr.enabled == true)
        { 
            Vector3 speed = (Camera.main.transform.forward + Vector3.up) * throwForce;
            lr.positionCount = 100;
            for (int i = 0; i < lr.positionCount; i++)
            {
                float t /*t de tiempo*/ = i * 0.1f;
                Vector3 position = grenadeSpawn.position + speed * t + 0.5f * Physics.gravity * t * t;
                lr.SetPosition(i, position);
            }
        }
    
    }

    private void LateUpdate()
    {
        Vector2 lookInput = playerInput.actions["Look"].ReadValue<Vector2>();
        followTarget.localEulerAngles += new Vector3(lookInput.y * lookSpeed * Time.deltaTime, 0 ,0);
        transform.eulerAngles += new Vector3(0, lookInput.x * lookSpeed * Time.deltaTime, 0);
        elHueso.localEulerAngles = new Vector3(followTarget.localEulerAngles.x+offset, elHueso.localEulerAngles.y, elHueso.localEulerAngles.z);
    }


    public void Shoot(InputAction.CallbackContext callback)
    {

        if (callback.phase == InputActionPhase.Started)
        {
            anim.SetBool("Shooting", true);
            GameManager.instance.GetGameData.Weapons[GameManager.instance.GetGameData.WeaponIndex].TriggerDown();
        }
        else if (callback.phase == InputActionPhase.Canceled)
        {
            anim.SetBool("Shooting", false);
            GameManager.instance.GetGameData.Weapons[GameManager.instance.GetGameData.WeaponIndex].TriggerUp();
            playerInput.actions["Reload"].Disable();
        }
    
    }

    public void Reload(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Performed)
        {
            //animacion recarga
            anim.SetTrigger("Reload");
            GameManager.instance.GetGameData.Weapons[GameManager.instance.GetGameData.WeaponIndex].Reload();
            lm.UpdateAmmo();
            playerInput.actions["Shoot"].Disable();
            
        }

    }

    public void CanShoot()
    {
        playerInput.actions["Shoot"].Enable();
    }

    public void TakeDamage(float _damage)
    { 
        if (enumerator != null)
        {
            StopCoroutine(enumerator);
        }

        GameManager.instance.GetGameData.CurrentHP -= _damage;
        if (GameManager.instance.GetGameData.CurrentHP <= 0)
        { 
            GameObject MuñecoRagdoll = (GameObject) Resources.Load("MuñecoRagdoll");
            Instantiate(MuñecoRagdoll, transform.position, transform.rotation);
            gameObject.SetActive(false);
        }
        else
        {
            enumerator = Healing();
            StartCoroutine(enumerator);
                
        }
        lm.UpdateLife();
    }
    IEnumerator Healing()
    {
        yield return new WaitForSeconds(timeToStartHealing);
        while (GameManager.instance.GetGameData.CurrentHP < GameManager.instance.GetGameData.MaxHP)
        {
            GameManager.instance.GetGameData.CurrentHP = Mathf.Clamp(GameManager.instance.GetGameData.CurrentHP + (healing * Time.deltaTime), 0, GameManager.instance.GetGameData.MaxHP);
            yield return null;
            
            lm.UpdateLife();
            
        }
    }

    public void Grenade(InputAction.CallbackContext context)
    {
        if (context.started)
        { 
            anim.SetBool("Grenade", true);
            GameManager.instance.GetGameData.Weapons[GameManager.instance.GetGameData.WeaponIndex].transform.parent = leftHand;

            Instantiate(grenadePrefab, grenadeSpawn.position, grenadeSpawn.rotation, grenadeSpawn);

            lr.enabled = true;
        }

        if (context.canceled)
        {
            anim.SetBool("Grenade", false);
            GameManager.instance.GetGameData.Weapons[GameManager.instance.GetGameData.WeaponIndex].transform.parent = rightHand;
            lr.enabled = false;
        }
    }

    public void ThrowGrenade()
    {
        Transform grenade = grenadeSpawn.GetChild(0);
        grenade.parent = null;
        grenade.GetComponent<Rigidbody>().isKinematic = false;
        grenade.GetComponent<Rigidbody>().linearVelocity = (Camera.main.transform.forward + Vector3.up) * throwForce;
        grenade.GetComponent<Collider>().enabled = true;
        grenade.GetComponent<Grenade>().countDownActive = true;
    }
}
