using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	public string typeName = "AaronsSpaceGame";
    public string gameName = "Room";
	public bool multiplayerEnabled = false;
    public float mySendRate = 1000f;
    public Object shipPrefab;
    public bool gameStarted = false;

	void Start() {
	}

    public void StartOffline()
    {
        gameStarted = true;
        multiplayerEnabled = false;
        spawnShip();
    }

	public void StartServer()
	{
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost (typeName, gameName);
        gameStarted = true;
	}

	void OnServerInitialized()
	{
        Network.sendRate = mySendRate;
		//Debug.Log("Server Initializied");
        //spawnShip();
	}

    void OnConnectedToServer()
    {
        Network.sendRate = mySendRate;
        //spawnShip();
        //Debug.Log("Server Joined");
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
        gameStarted = true;
		Network.Connect(hostData);
	}

	public void spawnShip() {
        GameObject enemyShip = GameObject.FindGameObjectWithTag("Ship");
        Vector3 spawnLocation = Vector3.zero;
        if (enemyShip != null)
        {
            spawnLocation = enemyShip.transform.position;
        }
        StartCoroutine(spawnShipRoutine(spawnLocation, 1f));
	}

    public IEnumerator spawnShipRoutine(Vector3 location, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject newShip;
        if (multiplayerEnabled)
            newShip = (GameObject)Network.Instantiate(shipPrefab, Vector3.zero, Quaternion.identity, 0);
        else
            newShip = (GameObject)Instantiate(shipPrefab, Vector3.zero, Quaternion.identity);
        GetComponent<CameraFollow>().myTargets[0] = newShip.GetComponent<Rigidbody>();
        newShip.transform.position = location;
        newShip.layer = 8;
    }
}
