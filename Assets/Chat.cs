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

    public void sendChatMessage()
    {
        if (Input.GetAxisRaw("Chat") == 1.0f && myInputBox.text != "")
        {
            if (myNetworkManager.multiplayerEnabled && myNetworkView.isMine)
            {
                myNetworkView.RPC("receiveChatMessage", RPCMode.AllBuffered, myInputBox.text);
            }
            else if (!myNetworkManager.multiplayerEnabled)
            {
                receiveChatMessage(myInputBox.text);
            }
        }
        myInputBox.text = "";
    }

    [RPC]
    private void receiveChatMessage(string message)
    {
        myChatBox.text = myChatBox.text.Substring(myChatBox.text.IndexOf('\n')+1) + "Player: " + message + '\n';
    }
}
