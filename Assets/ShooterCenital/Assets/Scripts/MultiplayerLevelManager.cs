using Photon.Pun;
using UnityEngine;

public class MultiplayerLevelManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PhotonNetwork.Instantiate("PlayerMultiplayer", Vector3.zero, Quaternion.identity); //Carpeta Resources
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
