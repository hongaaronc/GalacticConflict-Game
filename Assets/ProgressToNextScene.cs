using UnityEngine;
using System.Collections;

public class ProgressToNextScene : MonoBehaviour {
    public float timer;
    public string nextScene;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Application.LoadLevel(nextScene);
        }
	}
}
