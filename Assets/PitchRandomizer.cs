using UnityEngine;
using System.Collections;

public class PitchRandomizer : MonoBehaviour {
    public AudioSource source;
    public float randomOffset;
	// Use this for initialization
	void Start () {
        source.pitch += Random.Range(-randomOffset, randomOffset);
	}
}
