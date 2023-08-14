using Newtonsoft.Json;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSendData :MonoBehaviour {
    public static ClientSendData instance;
   private void Awake()
    {
    instance = this;

   }
public void SendData(byte[] data)
{
    SocketClientCore._clientSocket.Send(data);
}
    public void SendMessageChat(string message)
    {
        PacketBuffer buffer = new PacketBuffer();
        buffer.WriteInteger((int)ClientPackets.CSendMessage);
     
        buffer.WriteString(PhotonNetwork.NickName);
        buffer.WriteString(message);
        SendData(buffer.ToArray());
        buffer.Dispose();
    }
    internal void SendCmd(int cmd)
    {

        PacketBuffer buffer = new PacketBuffer();
        buffer.WriteInteger((int)ClientPackets.CCmd);
        buffer.WriteInteger(cmd);
        buffer.WriteString(PhotonNetwork.NickName);
        Debug.Log("Send " + PhotonNetwork.NickName);
        SendData(buffer.ToArray());
        buffer.Dispose();
      

    }
}
