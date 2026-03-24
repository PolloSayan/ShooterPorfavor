using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEditor.Rendering.Analytics;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour
{
    [SerializeField]
    private float life;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Script enemigo//////////////////////////////////////////////////////////////////

    void TakeDamage(float damage, Player player)
    {
        life -= damage;
        if (life <= 0)
        {
            //muerte
            int deaths = 0;

            if (player.CustomProperties.ContainsKey("Muertes") == true)
            {
                object muertes;
                player.CustomProperties.TryGetValue("Muertes", out muertes);
                deaths = (int)muertes;
                deaths += 1;

            }
            else
            {
                deaths = 1;


            }
            Hashtable muerdeaths = new Hashtable { { "Muertes", deaths } };
            player.SetCustomProperties(muerdeaths);

        }
        else
        {
            //animacion hit
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////

    void VerMuertes()
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            //PhotonNetwork.CurrentRoom.Players[i].CustomProperties.TryGetValue("Muertes", out nombrevariable);


        }
            
    }
}
