using UnityEngine;
using System.Collections;

public class BumpDetection : MonoBehaviour {
    public AudioSource bumpSound;
    private HUDChromaticAbberation hudChromAb;
    public float minPitch;
    public float minVolume;
    public float volumeMultiplier;
    public float pitchMultiplier;
    public float forceMultiplier = 0.25f;
    private NetworkView myNetworkView;
    private NetworkManager myNetworkManager;
    public Health health;
    public float damageMultiplier;

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
        {
            bumpSound.pitch = minPitch + pitchMultiplier * collision.impulse.magnitude;
            bumpSound.volume = Mathf.Clamp01(minVolume + volumeMultiplier * collision.impulse.magnitude);
            bumpSound.Play();
            hudChromAb.distort(forceMultiplier * collision.impulse.magnitude);
            health.takeDamage(damageMultiplier * collision.impulse.magnitude);
        }
    }
}
