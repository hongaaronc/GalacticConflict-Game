using UnityEngine;
using System.Collections;

public class BumpDetection : MonoBehaviour {
    private HUDChromaticAbberation hudChromAb;
    public float forceMultiplier = 0.25f;
    private NetworkView myNetworkView;
    private NetworkManager myNetworkManager;

    void Awake()
    {
        hudChromAb = FindObjectOfType<HUDChromaticAbberation>();
    }

    void Start()
    {
        myNetworkView = GetComponent<NetworkView>();
        myNetworkManager = Camera.main.GetComponent<NetworkManager>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!myNetworkManager.multiplayerEnabled || myNetworkView.isMine)
            hudChromAb.distort(forceMultiplier * collision.impulse.magnitude);
    }
}
