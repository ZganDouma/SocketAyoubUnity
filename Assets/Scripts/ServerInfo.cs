using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ServerInfo : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TextInfo;

    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsConnected)
        {
            TextInfo.text = "Not Connected To Master";
            return;
        }
        //tring looby = PhotonNetwork.CurrentLobby == null ? "Connecting To the lobby" : PhotonNetwork.CurrentLobby.Name;
        string room = PhotonNetwork.CurrentRoom == null ? "Connecting To the room \"" : PhotonNetwork.CurrentRoom.Name;

        TextInfo.text = "Connected To Master " +  room;


    }
}
