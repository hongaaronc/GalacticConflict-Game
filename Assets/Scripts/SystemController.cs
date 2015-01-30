using UnityEngine;
using System.Collections;

public class SystemController : MonoBehaviour {
    public GenericSystem[] systems;

    private NetworkView myNetworkView;
    private NetworkManager myNetworkManager;
	// Use this for initialization
	void Start () {
        myNetworkView = GetComponent<NetworkView>();
        myNetworkManager = Camera.main.GetComponent<NetworkManager>();
	}

    // Update is called once per frame
    void Update()
    {
        if (!myNetworkManager.multiplayerEnabled || myNetworkView.isMine)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                foreach (GenericSystem system in systems)
                {
                    system.Activate();
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (!myNetworkManager.multiplayerEnabled || myNetworkView.isMine)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                foreach (GenericSystem system in systems)
                {
                    system.Activate();
                }
            }
        }
    }
}
