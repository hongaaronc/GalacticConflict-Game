using UnityEngine;
using System.Collections;

public class SystemController : MonoBehaviour {
    public int abilityNum;
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
            if (Input.GetAxisRaw("Ability"+abilityNum) == 1.0f)
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
            if (Input.GetAxisRaw("Ability" + abilityNum) == 1.0f)
            {
                foreach (GenericSystem system in systems)
                {
                    system.Activate();
                }
            }
        }
    }
}
