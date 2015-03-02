﻿using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	private const string typeName = "AaronsSpaceGame";
	private const string gameName = "SpaceGameRoom";
	public bool multiplayerEnabled = false;
    private bool hostable = false;
    public float mySendRate = 1000f;

	void Start() {
		if (!multiplayerEnabled) {
            spawnShip(Resources.Load("Ships/Fighter"));
		}
	}
	private void StartServer()
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
        if (Input.GetKeyDown(KeyCode.Pause))
            hostable = true;
    }

	void OnGUI()
	{
		if (multiplayerEnabled && !Network.isClient && !Network.isServer)
		{
            if (hostable)
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
		}
	}
	
	private HostData[] hostList;
	
	private void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
	}

	private void JoinServer(HostData hostData)
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
