using UnityEngine;
using System.Collections;

public class StarSpawner : MonoBehaviour {
	public int numStars = 100;
	public GameObject myStar;
	// Use this for initialization
	void Start () {
		for (int i=0; i<numStars; i++) {
            Vector3 newPosition = new Vector3(Camera.main.transform.position.x + Random.Range(-Camera.main.orthographicSize * Screen.width / Screen.height, Camera.main.orthographicSize * Screen.width / Screen.height), Camera.main.transform.position.y-50f, Camera.main.transform.position.z + Random.Range(-Camera.main.orthographicSize, Camera.main.orthographicSize));
            GameObject newStar = (GameObject)Instantiate(myStar, newPosition, Quaternion.identity);
            newStar.transform.parent = transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
