using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {
    public Vector3 targetVector;
    public float turnRate = 2f;
    public float turnRange = 360f;

    private float parentedAngleOffset = 0f;

	private NetworkView myNetworkView;
    private NetworkManager myNetworkManager;
	// Use this for initialization
	void Start () {
        targetVector = Camera.main.GetComponent<CameraFollow>().mousePosition;

        parentedAngleOffset += GetComponentInParent<Transform>().localEulerAngles.y;

        myNetworkView = GetComponent<NetworkView>();
        myNetworkManager = Camera.main.GetComponent<NetworkManager>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!myNetworkManager.multiplayerEnabled || myNetworkView.isMine)
        {
            targetVector = Camera.main.GetComponent<CameraFollow>().mousePosition;
            float targetAngle = Mathf.Atan2(transform.position.z - targetVector.z, targetVector.x - transform.position.x) * Mathf.Rad2Deg + parentedAngleOffset;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnRate), transform.eulerAngles.z);
            float newAngle = transform.localEulerAngles.y;
            if (newAngle > 180f)
                newAngle -= 360f;
            if (newAngle < -180f)
                newAngle += 360f;
            newAngle = Mathf.Clamp(newAngle, -turnRange, turnRange);
            transform.localEulerAngles = new Vector3(0f, newAngle, 0f);
        }
	}
}
