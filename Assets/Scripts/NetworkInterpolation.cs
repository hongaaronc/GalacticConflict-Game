using UnityEngine;
using System.Collections;

public class NetworkInterpolation : MonoBehaviour {
    public float mySendRate = 25f;
	// Use this for initialization
	void Start () {
        Network.sendRate = mySendRate;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
