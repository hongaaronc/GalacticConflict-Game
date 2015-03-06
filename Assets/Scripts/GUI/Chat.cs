using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Chat : MonoBehaviour
{
    public UnityEngine.UI.InputField myInputBox;
    public UnityEngine.UI.Text myChatBox;
    private NetworkView myNetworkView;
    private NetworkManager myNetworkManager;

    // Use this for initialization
    void Start()
    {
        //myInputBox = GetComponentInChildren<UnityEngine.UI.InputField>();
        //myChatBox = GetComponentInChildren<UnityEngine.UI.Text>();
        myNetworkView = GetComponent<NetworkView>();
        myNetworkManager = Camera.main.GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Chat") == 1.0f)
        {
            myInputBox.Select();
        }
    }

    public void inputChatMessage()
    {
        if (Input.GetKey(KeyCode.Return) && myInputBox.text != "")
        {
            sendChatMessage(myInputBox.text);
        }
        myInputBox.text = "";
        EventSystem.current.SetSelectedGameObject(null, null);
    }

    public void sendChatMessage(string message)
    {
        if (myNetworkManager.multiplayerEnabled)
        {
            if (myNetworkView.isMine)
            {
                myNetworkView.RPC("checkHostCommands", RPCMode.AllBuffered);
            }
            myNetworkView.RPC("receiveChatMessage", RPCMode.AllBuffered, message);
        }
        else if (!myNetworkManager.multiplayerEnabled)
        {
            receiveChatMessage(message);
        }
    }

    [RPC]
    public void checkHostCommands()
    {
        
    }

    [RPC]
    public void receiveChatMessage(string message)
    {
        myChatBox.text = myChatBox.text.Substring(myChatBox.text.IndexOf('\n')+1) + "Player: " + message + '\n';
    }
}
