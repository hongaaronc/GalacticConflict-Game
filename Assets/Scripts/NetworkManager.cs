using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	public string typeName = "AaronsSpaceGame";
    public string gameName = "Room";
	public bool multiplayerEnabled = false;
    public float mySendRate = 1000f;
    public Object shipPrefab;

	void Start() {
	}

    public void StartOffline()
    {
        multiplayerEnabled = false;
        spawnShip();
    }

	public void StartServer()
	{
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost (typeName, gameName);
	}

	void OnServerInitialized()
	{
        Network.sendRate = mySendRate;
		Debug.Log("Server Initializied");
        spawnShip();
	}

    void Update()
    {
    }
	
	public HostData[] hostList;
	
	public void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
	}

	public void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}
	
	void OnConnectedToServer()
	{
        Network.sendRate = mySendRate;
		spawnShip ();
		Debug.Log("Server Joined");
	}

	void spawnShip() {
		GameObject newShip;
		if (multiplayerEnabled)
            newShip = (GameObject)Network.Instantiate(shipPrefab, Vector3.zero, Quaternion.identity, 0);
		else
            newShip = (GameObject)Instantiate(shipPrefab, Vector3.zero, Quaternion.identity);
		GetComponent<CameraFollow> ().myTargets [0] = newShip.GetComponent<Rigidbody>();
	}
}
