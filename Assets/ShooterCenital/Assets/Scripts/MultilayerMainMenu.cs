using Photon.Pun;
using UnityEngine;

public class MultilayerMainMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int maxPlayers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BotonMultiPlayer()
    { 
        PhotonNetwork.ConnectUsingSettings();

    }
    
    void OnConnectedToMaster()
    {
        Debug.Log("Conectado al servidor Master");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinRandomRoom();
    }

    void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Fallo al unirce a una sala aleatoria, creando una nueva sala");
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions {MaxPlayers = maxPlayers});
    }
    
    void OnJoinedRoom()
    {
        Debug.Log("Unido a la sala correctamente");
    }

    void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("Nuevo jugador ha entrado en la sala: " + newPlayer.NickName);
    }

    void OnPlayerDisconnected(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log("Jugador desconectado: " + otherPlayer.NickName);
    }
}
