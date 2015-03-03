using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    public float fireForce;
    public float lifetime = 1f;

    private NetworkView myNetworkView;
    private NetworkManager myNetworkManager;
	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().AddRelativeForce(fireForce * Vector3.forward);

        myNetworkView = GetComponent<NetworkView>();
        myNetworkManager = Camera.main.GetComponent<NetworkManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!myNetworkManager.multiplayerEnabled || myNetworkView.isMine)
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0f)
            {
                if (myNetworkManager.multiplayerEnabled)
                    Network.Destroy(gameObject);
                else
                    Destroy(gameObject);
            }
        }
	}
}
