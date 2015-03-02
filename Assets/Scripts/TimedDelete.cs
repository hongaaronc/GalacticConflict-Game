using UnityEngine;
using System.Collections;

public class TimedDelete : MonoBehaviour {
    public float deleteTime;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        deleteTime -= Time.deltaTime;
        if (deleteTime <= 0f)
        {
            Destroy(gameObject);
        }
	}
}
