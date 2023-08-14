using Photon.Pun;
using TMPro;
using UnityEngine;

//Script to send cmd for other players
public class RpcCmd : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI cmdText;

    public void SendCmd(int cmd)
    {
        this.photonView.RPC("OnSendCmd", RpcTarget.All, (byte)cmd);
    }

    [PunRPC]
    public void OnSendCmd(byte cmd, PhotonMessageInfo info)
    {
        cmdText.text = info.Sender.NickName + " send Cmd " + cmd;
    }
}