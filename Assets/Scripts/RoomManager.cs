using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Script to manage server connection and join room
public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject connectingBackground;
    [SerializeField] private GameObject startGameBackground;
    [SerializeField] private GameObject connectionRoomBackground;
    [SerializeField] private TMP_InputField namePlayer;

    [SerializeField] private TextMeshProUGUI numberPlayer;

    [SerializeField] private Button StartGameButton;

    private void Awake()
    {
        ConnectToPhoton();
    }

    //Join Room (button)
    public void JoindRoom()
    {
        if (!PhotonNetwork.IsConnected)        
            PhotonNetwork.ConnectUsingSettings();        
        else
        {
            //Join a random room if we can't find a room we create a new one
            PhotonNetwork.JoinRandomOrCreateRoom();
            //if the name of the player is empty we put a random one
            if (namePlayer.text != "")
                PhotonNetwork.LocalPlayer.NickName = namePlayer.text;
            else
            {
                PhotonNetwork.LocalPlayer.NickName = "Player " + Random.Range(0, 100000);
            }

            connectingBackground.SetActive(true);
        }
    }

    private void ConnectToPhoton()
    {
        //To synchronize scenes between all players
        PhotonNetwork.AutomaticallySyncScene = true;
        //Connect To photon service
        PhotonNetwork.ConnectUsingSettings();
    }
    private void UpdateNumberPlayers()
    {
        //Check if the number of the players is more than two and check if the player is master client to start the game
        //if true we put the button in mode interactable 
        if (PhotonNetwork.CurrentRoom == null)
            return;

        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2 && PhotonNetwork.IsMasterClient)
        {
            numberPlayer.text = "Number of players = " + PhotonNetwork.CurrentRoom.PlayerCount + " You can Start the game now";
            StartGameButton.interactable = true;
        }
        else
        {
            string msg = "Number of players = " + PhotonNetwork.CurrentRoom.PlayerCount;
            msg += PhotonNetwork.IsMasterClient ? " You need a another player to start the game" : " Wait for the master to start the game";
            numberPlayer.text = msg;
        }
    }
    //Start Game
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel("GameRoom");
    }
    //Switch UI when we can start the game 
    private void UpdateUI()
    {
        connectionRoomBackground.SetActive(false);
        connectingBackground.SetActive(false);
        startGameBackground.SetActive(true);
    }
    #region punRegion
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Joined OnConnectedToMaster");
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined OnJoinedLobby");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        UpdateNumberPlayers();
    }

 
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        UpdateUI();
        UpdateNumberPlayers();
        Debug.Log("Joined Room");
    }

    #endregion

}