using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientHandler : MonoBehaviour
{
    private delegate void Packet_( byte[] data);

    private static Dictionary<int, Packet_> Packets;

    public static void InitializeNetworkPackages()
    {
        Debug.Log("Initialize Network Packages");
        Packets = new Dictionary<int, Packet_>
        {
                 { (int)ServerPackets.Scmd, HandleSCmd},
                  { (int)ServerPackets.SSendMessage, HandleChat},
              };
    }
    public static void HandlNetworkInformation( byte[] data)
    {
        try
        {
            int packetnum;
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            packetnum = buffer.ReadInteger();
            buffer.Dispose();

            Packet_ packet_;
            if (Packets.TryGetValue(packetnum, out packet_))

            {
                packet_.Invoke(data);
            }
        }
        catch
        {
        }
    }
    private static void HandleChat(byte[] data)
    {
        try
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            int packetnum = buffer.ReadInteger();
    
            string msg = buffer.ReadString();
            string sender = buffer.ReadString();
            buffer.Dispose();
            Dispatcher.RunOnMainThread(() =>
            {
                ChatManager.instance.GetMessagesSocket(sender, msg);
            });
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    private static void HandleSCmd( byte[] data)
    {
        try
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            int packetnum = buffer.ReadInteger();
            int cmd = buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();
            Dispatcher.RunOnMainThread(() =>
            {
                RpcCmd.instance.ShowCmd(msg, cmd, false);
            });
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    private void Awake()
    {
        InitializeNetworkPackages();
    }
}