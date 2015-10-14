using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TestPanel : MonoBehaviour {
    public GameObject defaultSelected;
    public UnityEngine.UI.Text connectionText;

    public Chat myChat;

    private NetworkView myNetworkView;
    private NetworkManager myNetworkManager;
	// Use this for initialization
	void Start () {
        myNetworkView = GetComponent<NetworkView>();
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
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F))
        {
            try
            {
                Screen.fullScreen = !Screen.fullScreen;
            }
            catch { }
        }
	}

    public void startOffline()
    {
        if (!Network.isClient && !Network.isServer && myNetworkManager.multiplayerEnabled)
        {
            myNetworkManager.StartOffline();
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
    private bool refreshing = false;

    public void refreshServers()
    {
        MasterServer.RequestHostList(myNetworkManager.typeName);
        refreshing = true;
    }

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
            hostList = MasterServer.PollHostList();
        print("Host list received");
        if (refreshing)
        {
            float i=0f;
            foreach (HostData host in hostList)
            {
                GameObject newButton = (GameObject) Instantiate(Resources.Load("GUI/GUIButton"), Vector3.zero, Quaternion.identity);
                newButton.transform.parent = transform;
                newButton.transform.localPosition = new Vector3(0f, -35f * i, 0f);
                newButton.GetComponentInChildren<UnityEngine.UI.Text>().text = host.gameName;
                newButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { myNetworkManager.JoinServer(host); });
                i++;
            }
            refreshing = false;
        }
    }

    void OnServerInitialized()
    {
        myChat.sendChatMessage("Server started");
        myNetworkManager.spawnShip();
        gameObject.SetActive(false);
    }

    void OnConnectedToServer()
    {
        myChat.sendChatMessage("Player has joined the game");
        myNetworkManager.spawnShip();
        gameObject.SetActive(false);
    }

    public void RespawnShip()
    {
        myChat.sendChatMessage("Player respawned");
        myNetworkManager.spawnShip();
    }
}
