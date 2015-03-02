using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TestPanel : MonoBehaviour {
    public GameObject defaultSelected;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetAxisRaw("Submit") == 1.0f && EventSystem.current.currentSelectedGameObject == null) {
            EventSystem.current.SetSelectedGameObject(defaultSelected, null);
        }
        if (Input.GetAxisRaw("Cancel") == 1.0f)
        {
            EventSystem.current.SetSelectedGameObject(null, null);
        }
	}
}
