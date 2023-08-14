using Photon.Pun;
using TMPro;
using UnityEngine;

//Script to send cmd for other players
public class RpcCmd : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI cmdText;

    public static RpcCmd instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    //Send cmd with net Core
    public void SendCmdWithNetCore(int cmd)
    {
        ClientSendData.instance.SendCmd(cmd);
    }
    public void SendCmd(int cmd)
    {
        this.photonView.RPC("OnSendCmd", RpcTarget.All, (byte)cmd);
    }

    [PunRPC]
    public void OnSendCmd(byte cmd, PhotonMessageInfo info)
    {
        ShowCmd(info.Sender.NickName, cmd,true);
    }
    public void ShowCmd(string sender, int cmd,bool fromPhoton=false)
    {
        string from = fromPhoton ? "from photon " : "from net core socket";
        cmdText.text = sender + " send Cmd " + cmd  +" "+ from;

    }
}