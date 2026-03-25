using Photon.Pun;
using UnityEngine;

public class MultiplayerLevelManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform[] spawnPoints;


    public override void OnJoinedRoom()
    {
        SpawnPlayer();
    }

    void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        int playerIndex = 0;

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                playerIndex = i;
                break;
            }
        }

        int spawnIndex = playerIndex % spawnPoints.Length;
        Transform selectedSpawn = spawnPoints[spawnIndex];

        PhotonNetwork.Instantiate("MultiplayerPlayer", selectedSpawn.position, selectedSpawn.rotation);
    }
    public Vector3 GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)].position;
    }
}