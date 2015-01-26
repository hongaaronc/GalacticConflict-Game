using UnityEngine;
using System.Collections;

public class StarSpawner : MonoBehaviour {
	public int numStars = 100;
	public float range = 100f;
	public GameObject myStar;
	// Use this for initialization
	void Start () {
		for (int i=0; i<numStars; i++) {
            GameObject newStar = (GameObject)Instantiate(myStar, transform.position + Random.insideUnitSphere * range, Quaternion.identity);
            newStar.transform.parent = transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
