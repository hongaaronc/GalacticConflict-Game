using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TestPanel : MonoBehaviour {
    public GameObject defaultSelected;
    public UnityEngine.UI.Text connectionText;

    private NetworkManager myNetworkManager;
	// Use this for initialization
	void Start () {
        myNetworkManager = Camera.main.GetComponent<NetworkManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if ((Input.GetAxisRaw("Submit") == 1f || Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f) && EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(defaultSelected, null);
        }
        if (Input.GetAxisRaw("Cancel") == 1f)
        {
            EventSystem.current.SetSelectedGameObject(null, null);
        }
        if (myNetworkManager.multiplayerEnabled)
        {
            if (Network.isServer)
            {
                connectionText.text = "Hosting server";
                connectionText.color = new Color(0f, 1f, 0f, 0.5f);
                connectionText.fontSize = 14;
            }
            else if (Network.isClient)
            {
                connectionText.text = "Connected to server";
                connectionText.color = new Color(0f, 1f, 0f, 0.5f);
                connectionText.fontSize = 14;
            }
            else
            {
                connectionText.text = "Not connected to server";
                connectionText.color = new Color(1f, 0f, 0f, 0.5f);
                connectionText.fontSize = 14;
            }
        }
        else
        {
            connectionText.text = "Multiplayer not enabled";
            connectionText.color = new Color(1f, 1f, 1f, 0.5f);
            connectionText.fontSize = 14;
        }
	}

    public void startServer()
    {
        if (!Network.isClient && !Network.isServer && myNetworkManager.multiplayerEnabled)
        {
            myNetworkManager.StartServer();
        }
    }

    private HostData[] hostList;

    public void refreshServers()
    {
        MasterServer.RequestHostList(myNetworkManager.typeName);
    }

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
            hostList = MasterServer.PollHostList();
        print("Host list received");
        foreach (HostData host in hostList)
        {
            print(host.gameName);
        }
    }
}
