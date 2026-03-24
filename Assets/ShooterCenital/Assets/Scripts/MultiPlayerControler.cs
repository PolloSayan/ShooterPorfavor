using Photon.Pun;
using UnityEngine;

public class MultiPlayerControler : MonoBehaviourPunCallbacks, IPunObservable
{

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
     
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (photonView.IsMine)
        {
            Camera.main.GetComponent<CameraMultiplayer>().targetPlayer = this.transform;
            photonView.RPC("Shoot", RpcTarget.Others);

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    void Shoot()
    { 
        
    }
}
