using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    [Header("Panels")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject roomPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Room UI")]
    [SerializeField] private GameObject startGameButton;
    [SerializeField] private TextMeshProUGUI playersInRoom;
    [SerializeField] public TMP_InputField nicknameInput;

    [Header("Settings")]
    [SerializeField] private int maxPlayers;
    [SerializeField] private string gameSceneName = "Shooter2";
    void Start()
    {

        PhotonNetwork.AutomaticallySyncScene = true;

        if (startGameButton != null)
            startGameButton.SetActive(false);
    }

    #region Buttons Logic

    public void MultiplayerButton()
    {
        loadingPanel.SetActive(true);
        PhotonNetwork.NickName = string.IsNullOrEmpty(nicknameInput.text) ? "Player" + Random.Range(0, 100) : nicknameInput.text;
        PhotonNetwork.ConnectUsingSettings();
    }


    public void BackButton()
    {
        settingsPanel.SetActive(false);
    }



    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {

            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(gameSceneName);
        }
    }
    public void LoadMultiplayerMenu()
    {
        SceneManager.LoadScene(3);
    }

    public void SettingsButton()
    {
        settingsPanel.SetActive(true);

    }

    public void ExitButton()
    {
        Application.Quit();

    }
    public void Shooter1button()
    {
        SceneManager.LoadScene(1);
    }

    public void ReadyButton()
    {

        bool isReady = false;
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Ready"))
        {
            isReady = (bool)PhotonNetwork.LocalPlayer.CustomProperties["Ready"];
        }

        Hashtable props = new Hashtable { { "Ready", !isReady } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    #endregion

    #region Photon Callbacks

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = (byte)maxPlayers });
    }

    public override void OnJoinedRoom()
    {
        loadingPanel.SetActive(false);
        roomPanel.SetActive(true);
        UpdatePlayerList();
        CheckAllPlayersReady();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
        CheckAllPlayersReady();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
        CheckAllPlayersReady();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Ready"))
        {
            UpdatePlayerList();
            CheckAllPlayersReady();
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        CheckAllPlayersReady();
    }

    #endregion

    #region Private Methods

    private void UpdatePlayerList()
    {
        playersInRoom.text = "PLAYERS:\n";
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            bool isReady = false;
            if (player.Value.CustomProperties.ContainsKey("Ready"))
            {
                isReady = (bool)player.Value.CustomProperties["Ready"];
            }

            string color = isReady ? "<color=green>" : "<color=red>";
            string status = isReady ? " [READY]" : " [NOT READY]";


            playersInRoom.text += $"{color}{player.Value.NickName}{status}</color>\n";

        }
    }

    private void CheckAllPlayersReady()
    {

        if (PhotonNetwork.IsMasterClient == false)
        {
            if (startGameButton != null)
            {
                startGameButton.SetActive(false);
                return;
            }
        }

        bool allReady = true;

        foreach (Player _players in PhotonNetwork.CurrentRoom.Players.Values)
        {

            if (!_players.CustomProperties.ContainsKey("Ready") || !(bool)_players.CustomProperties["Ready"])
            {
                allReady = false;
                break;
            }
        }

        if (startGameButton != null)
        {
            startGameButton.SetActive(allReady);
        }
    }

    #endregion
}
