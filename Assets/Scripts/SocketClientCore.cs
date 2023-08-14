using System;
using System.Collections;
using System.Net.Sockets;
using UnityEngine;

public class SocketClientCore : MonoBehaviour
{
    public static Socket _clientSocket;
    public string Address;
    public int Port = 5555;
    private byte[] _asyncbuffer;
    public static bool canReceive = true;
    public static bool Connected = true;
    public static SocketClientCore instance;
    private void Awake()
    {
        if(instance == null)
            instance = this;
        _asyncbuffer = new byte[4096];
        _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        StartCoroutine(ConnectToServer(1f));
    }
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public IEnumerator ConnectToServer(float time)
    {
        yield return new WaitForSeconds(time);

        Address = "127.0.0.1";

      
        _clientSocket.BeginConnect(Address, Port, new AsyncCallback(ConnectCallback), _clientSocket);

        Debug.Log("Connection to server");
    }
    private void ConnectCallback(IAsyncResult ar)
    {
        if (_clientSocket == null || !_clientSocket.Connected)
        {

          
                Debug.Log("Server Down");
            StartCoroutine(ConnectToServer(1f));

        }
        _clientSocket.EndConnect(ar);

        while (true && canReceive)
        {
            OnReceive();

        }
    }
    private void OnReceive()
    {
        byte[] _sizeinfo = new byte[4];
        byte[] receivedbuffer = new byte[16384];
        int totalread = 0, currentread = 0;
        try
        {
            currentread = totalread = _clientSocket.Receive(_sizeinfo);
            if ((totalread <= 0) || !Connected)
            {
                Debug.Log("You are not connected to the server");
                canReceive = false;
          

            }
            else
            {
                while (totalread < _sizeinfo.Length && currentread > 0)
                {
                    currentread = _clientSocket.Receive(_sizeinfo, totalread, _sizeinfo.Length - totalread, SocketFlags.None);
                    totalread += currentread;
                }
                int messagesize = 0;
                messagesize |= _sizeinfo[0];
                messagesize |= (_sizeinfo[1] << 8);
                messagesize |= (_sizeinfo[2] << 16);
                messagesize |= (_sizeinfo[3] << 24);

                byte[] data = new byte[messagesize];
                totalread = 0;
                currentread = totalread = _clientSocket.Receive(data, totalread, data.Length - totalread, SocketFlags.None);
                while (totalread < messagesize && currentread > 0)
                {
                    currentread = _clientSocket.Receive(data, totalread, data.Length - totalread, SocketFlags.None);
                    totalread += currentread;
                }
                //HandleNetworkingInformation

              ClientHandler.HandlNetworkInformation(data);

            }
        }
        catch (Exception ex)
        {
            canReceive = false;
            Debug.Log("You are not connected to the server" + ex.Message);
          


        }
    }
}