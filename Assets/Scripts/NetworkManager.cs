using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	public string typeName = "AaronsSpaceGame";
    public string gameName = "Room";
	public bool multiplayerEnabled = false;
    public float mySendRate = 1000f;

	void Start() {
		if (!multiplayerEnabled) {
            spawnShip(Resources.Load("Ships/Fighter"));
		}
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
        spawnShip(Resources.Load("Ships/Fighter"));
	}

    void Update()
    {
    }

	void OnGUI()
	{
        /*
		if (multiplayerEnabled && !Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
				StartServer();
			
			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
				RefreshHostList();
			
			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if(hostList[i].gameType == typeName) {
						if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
							JoinServer(hostList[i]);
					}
				}
			}
		}*/
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
		spawnShip (Resources.Load("Ships/Fighter"));
		Debug.Log("Server Joined");
	}

	void spawnShip(Object shipPrefab) {
		GameObject newShip;
		if (multiplayerEnabled)
            newShip = (GameObject)Network.Instantiate(shipPrefab, Vector3.zero, Quaternion.identity, 0);
		else
            newShip = (GameObject)Instantiate(shipPrefab, Vector3.zero, Quaternion.identity);
		GetComponent<CameraFollow> ().myTargets [0] = newShip.GetComponent<Rigidbody>();
	}
}
