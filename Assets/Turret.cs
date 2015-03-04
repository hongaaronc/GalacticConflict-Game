using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {
    public Vector3 targetVector;
    public float turnRate = 2;

	private NetworkView myNetworkView;
    private NetworkManager myNetworkManager;
	// Use this for initialization
	void Start () {
        targetVector = Camera.main.GetComponent<CameraFollow>().mousePosition;

        myNetworkView = GetComponent<NetworkView>();
        myNetworkManager = Camera.main.GetComponent<NetworkManager>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!myNetworkManager.multiplayerEnabled || myNetworkView.isMine)
        {
            targetVector = Camera.main.GetComponent<CameraFollow>().mousePosition;
            float targetAngle = Mathf.Atan2(transform.position.z - targetVector.z, targetVector.x - transform.position.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnRate), transform.eulerAngles.z);
            transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);
        }
	}
}
