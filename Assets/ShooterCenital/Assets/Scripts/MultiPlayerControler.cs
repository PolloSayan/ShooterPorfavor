using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class MultiplayerControler : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("Player Utils")]
    private PlayerInput playerInput;
    private Animator animator;
    private Rigidbody rb;

    [SerializeField] 
    private float speed;
    [SerializeField]
    private float life;
    [SerializeField]
    private float maxLife = 100f;



    [Header("Shoot")]

    [SerializeField] 
    private GameObject bulletPrefab;
    [SerializeField] 
    private Transform bulletSpawnPoint;

    [Header("Other")]
    [SerializeField] 
    private float respawnTime;
    [SerializeField] 
    private Image fillHealthbar;
    [SerializeField]
    private TextMeshProUGUI nombrePlayerID;
    [SerializeField] 
    private Transform canvasTransform;
    [SerializeField] 
    private GameObject Maincamera;

    private MultiplayerLevelManager multiplayerLM;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(life);
        }
        else
        {
            life = (float)stream.ReceiveNext();
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        life = maxLife;
        multiplayerLM = GetComponent<MultiplayerLevelManager>();


        nombrePlayerID.text = photonView.Owner.NickName;

        if (photonView.IsMine)
        {
            //Camera.main.GetComponent<MultiplayerCameraController>().SetPlayer(transform);
        }
    }

    void Update()
    {
        canvasTransform.LookAt(Camera.main.transform);
        if (photonView.IsMine)
        {
            Vector2 leftStickInput = playerInput.actions["Move"].ReadValue<Vector2>();
            Vector3 arriba = Vector3.forward;
            Vector3 derecha = Vector3.right;
            Vector3 movement = ((arriba * leftStickInput.y) + derecha * leftStickInput.x) * speed;
            rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);

            if (leftStickInput.magnitude > 0.1f)
                animator.SetBool("Run", true);
            else
                animator.SetBool("Run", false);

            //float y = Camera.main.GetComponent<MultiplayerCameraController>().camOffset.y;
            Vector2 mousePos = playerInput.actions["LookCenital"].ReadValue<Vector2>();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, y));
            Vector3 rotActual = transform.eulerAngles;
            transform.LookAt(worldPos);
            transform.eulerAngles = new Vector3(rotActual.x, transform.eulerAngles.y, rotActual.z);

        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (context.performed)
        {
            GameObject bullet = PhotonNetwork.Instantiate("BulletPrefab", bulletSpawnpoint.position, bulletSpawnpoint.rotation);
            bullet.GetComponent<Rigidbody>().linearVelocity = bullet.transform.forward * 20f;
        }
    }

    [PunRPC]
    public void RPC_ApplyDamage(float damage)
    {
        if (photonView.IsMine == false)
        {
            return;
        }

        life -= damage;
        fillHealthbar.fillAmount -= life / maxLife;

        if (life <= 0)
        {
            Death();
            fillHealthbar.fillAmount = 0;
        }
    }

    private void Death()
    {

        if (photonView.IsMine)
        {
            StartCoroutine(RespawnCoroutine());
        }
    }

    private IEnumerator RespawnCoroutine()
    {


        yield return new WaitForSeconds(respawnTime);
        Vector3 respawnPosition = MLM.GetRandomSpawnPoint();
        transform.position = respawnPosition;
        life = maxLife;
        fillHealthbar.fillAmount += life / maxLife;
    }

}