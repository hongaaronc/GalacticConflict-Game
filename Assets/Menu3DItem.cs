using UnityEngine;
using System.Collections;

public class Menu3DItem : MonoBehaviour {

    public bool selected = false;
    public Menu3DItem navUp;
    public Menu3DItem navDown;
    public Menu3DItem navLeft;
    public Menu3DItem navRight;
    public Menu3DItem navSelect;
    private bool ready = false;
    private Renderer renderer;
    private Color originalColor;
    public Color highlightColor;
    public Transform newCameraPosition;
    public string newScene;

	// Use this for initialization
	void Start () {
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
	}
	
	// Update is called once per frame
	void Update () {
        if (ready && selected)
        {
            if (Input.GetAxisRaw("Vertical") >= 0.5f)
            {
                if (navUp != null)
                {
                    selected = false;
                    ready = false;
                    navUp.selected = true;
                }
            }
            if (Input.GetAxisRaw("Vertical") <= -0.5f)
            {
                if (navDown != null)
                {
                    selected = false;
                    ready = false;
                    navDown.selected = true;
                }
            }
            if (Input.GetAxisRaw("Horizontal") >= 0.5f)
            {
                if (navRight != null)
                {
                    selected = false;
                    ready = false;
                    navRight.selected = true;
                }
            }
            if (Input.GetAxisRaw("Horizontal") <= -0.5f)
            {
                if (navLeft != null)
                {
                    selected = false;
                    ready = false;
                    navLeft.selected = true;
                }
            }
        }
        if (Mathf.Abs(Input.GetAxisRaw("Vertical")) < 0.5f && Mathf.Abs(Input.GetAxisRaw("Horizontal")) < 0.5f && Mathf.Abs(Input.GetAxisRaw("Submit")) < 0.5f && selected)
        {
            ready = true;
        }
        if (selected)
        {
            renderer.material.color = highlightColor;
            if (Input.GetAxisRaw("Submit") >= 1.0f && ready)
            {
                if (newCameraPosition != null)
                    Camera.main.GetComponent<CameraMenu>().target = newCameraPosition;
                if (navSelect != null)
                {
                    selected = false;
                    ready = false;
                    navSelect.selected = true;
                }
            }
        }
        else
        {
            renderer.material.color = originalColor;
        }
	}
}
