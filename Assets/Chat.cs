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
        if (Input.GetKeyDown(KeyCode.Return))
        {
            EventSystem.current.SetSelectedGameObject(myInputBox.gameObject, null);
            myInputBox.OnPointerClick(new PointerEventData(EventSystem.current));
        }
    }

    public void sendChatMessage()
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

    [RPC]
    private void receiveChatMessage(string message)
    {
        myChatBox.text += "Player: " + message + '\n';
    }
}
