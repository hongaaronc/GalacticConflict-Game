using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Rigidbody[] myTargets;
    public Transform GUIcursor;
    public Vector3 mousePosition;
    public float distance = 60f;
    public float easing = 2f;
    public float leadMultiplier = 5f;
    public float maxLead = 30f;
    Vector3 velocity;
    public float dampTime = 1.0f;
    public int lives = 3;

    // Use this for initialization
	void Start () {
        velocity = Vector3.zero;
        try
        {
            Vector3 leadValue = Vector3.ClampMagnitude(leadMultiplier * myTargets[0].velocity, maxLead);
            transform.position = new Vector3(myTargets[0].transform.position.x, distance, myTargets[0].transform.position.z) + leadValue;
        }
        catch { }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F2))
            lives++;
        if (myTargets[0] == null && Camera.main.GetComponent<NetworkManager>().gameStarted)
        {
            if (lives > 0)
            {
                Camera.main.GetComponent<NetworkManager>().spawnShip();
                FindObjectOfType<Chat>().sendMessageToSelf(lives + " lives remaining.");
                lives--;
            }
            else
            {
                FindObjectOfType<Chat>().sendMessageToSelf("Out of lifes. Game over.");
            }
        }
        try
        {
            Vector3 leadValue = Vector3.ClampMagnitude(leadMultiplier * myTargets[0].velocity, maxLead);
            //transform.position += ((new Vector3(myTargets[0].transform.position.x, distance, myTargets[0].transform.position.z) + leadValue) - transform.position) / easing;
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(myTargets[0].transform.position.x, distance, myTargets[0].transform.position.z) + leadValue, ref velocity, dampTime);
        }
        catch { }
        transform.eulerAngles = new Vector3(90f, 0f, 0f);
        mousePosition = Camera.main.ScreenToWorldPoint(GUIcursor.position);
        mousePosition.y = 0f;
	}
}
