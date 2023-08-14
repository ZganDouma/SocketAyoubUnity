using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Chat.Demo;
using Photon.Pun;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    private ChatClient chatClient;
    private ChatAppSettings chatAppSettings;
    private List<string> listmessages = new List<string>();
    [SerializeField] private int maxMsg = 8;
    [SerializeField] private TextMeshProUGUI messagesText;
    [SerializeField] private TMP_InputField inputMessage;
    public static ChatManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            ConnectToChatServer();
        }
        else
            Destroy(this.gameObject);

       
    }


    // Update is called once per frame
    private void Update()
    {
        if (this.chatClient != null)
            this.chatClient.Service();
    }

    #region EventChat

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log(message);
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log(state.ToString());
    }

    public void OnConnected()
    {
        Debug.Log("Connected to chat");
        chatClient.Subscribe(new string[] { "channelA", "channelB" });
    }

    public void OnDisconnected()
    {
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        GetMessages(senders, messages);
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
    }

    public void OnUnsubscribed(string[] channels)
    {
    }

    public void OnUserSubscribed(string channel, string user)
    {
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
    }

    #endregion EventChat
    #region photonChat
    private void ConnectToChatServer()
    {
        string username = PhotonNetwork.NickName;
        chatClient = new ChatClient(this);
        this.chatClient.AuthValues = new AuthenticationValues(username);
        chatAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
        chatClient.ConnectUsingSettings(chatAppSettings);
        Debug.Log("Connecting as: " + chatClient.AuthValues.UserId);
    }
    private void GetMessages(string[] senders, object[] messages)
    {
        //When we recive a message we added to list of msgs so we can show later
        string msgs = "";
        for (int i = 0; i < senders.Length; i++)
        {
            DateTime dateTime = DateTime.Now;
            string time = dateTime.Hour + ":" + dateTime.Minute + ":" + dateTime.Second;
            string header = "<color=black>{" + time + "} " + "[" + senders[i] + "]</color>";
            if (senders[i] == PhotonNetwork.NickName)
                header = "<color=green>{" + time + "} " + "[" + senders[i] + "]</color>";
            msgs = string.Format("{0}{1}={2} ", msgs, header, messages[i]);
            msgs += "[photon]";
        }
        //when we list of messages is too big we remove the first one
        if (listmessages.Count > maxMsg)
            listmessages.RemoveAt(0);

        listmessages.Add(msgs);
        ShowMsgUI();
    }

    private void ShowMsgUI()
    {
        //Show the msg in the ui
        messagesText.text = "";
        string msg = "";
        foreach (var item in listmessages)
        {
            msg += item + "\n";
        }
        messagesText.text = msg;
    }

    public void SendMessageToAll()
    {
        if (inputMessage.text != "")
        {
            chatClient.PublishMessage("channelA", inputMessage.text);

            inputMessage.text = "";
        }
    }
    #endregion
    #region SocketNetChat
  public void   SendMessageToAllSocket()
    {
        if (inputMessage.text != "")
        {
            ClientSendData.instance.SendMessageChat(inputMessage.text);
            inputMessage.text = "";
        }
    }
    public void GetMessagesSocket(string sender, string msg)
    {
        //When we recive a message we added to list of msgs so we can show later
        string msgs = "";
        
            DateTime dateTime = DateTime.Now;
            string time = dateTime.Hour + ":" + dateTime.Minute + ":" + dateTime.Second;
            string header = "<color=black>{" + time + "} " + "[" + sender + "]</color>";
            if (sender == PhotonNetwork.NickName)
                header = "<color=green>{" + time + "} " + "[" + sender + "]</color>";
            msgs = string.Format("{0}{1}={2} ", msgs, header, msg);
            msgs += "[Socket Net]";
        
        //when we list of messages is too big we remove the first one
        if (listmessages.Count > maxMsg)
            listmessages.RemoveAt(0);

        listmessages.Add(msgs);
        ShowMsgUI();
    }
    #endregion


}