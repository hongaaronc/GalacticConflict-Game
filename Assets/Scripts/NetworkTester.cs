using UnityEngine;
using System.Collections;

public class NetworkTester : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Network.isClient || Network.isServer)
		{
			if (Input.GetKeyDown (KeyCode.A)) {
				Network.Instantiate(Resources.Load("Cube"), new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 10f), Quaternion.identity, 0);
			}
		}
	}

	void OnServerInitialized()
	{
		SpawnPlayer();
	}
	
	void OnConnectedToServer()
	{
		SpawnPlayer();
	}

	private void SpawnPlayer()
	{
		Network.Instantiate(Resources.Load("Cube"), new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
	}
}
