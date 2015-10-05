using UnityEngine;
using System.Collections;

public class GUIHudArrows : MonoBehaviour {
    public GameObject arrowPrefab;

    public float arrowRadius;
    public float arrowVisibleRange = 10f;

    private GameObject[] ships;
    private ArrayList arrows;
    private Vector3 cameraCenter;
	// Use this for initialization
	void Start () {
        arrows = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () {
        cameraCenter = new Vector3(Camera.main.transform.position.x, 0f, Camera.main.transform.position.z);
        ships = GameObject.FindGameObjectsWithTag("Ship");
        foreach (GameObject ship in ships)
        {
            if ((ship.transform.position - cameraCenter).magnitude < arrowVisibleRange)
                continue;
            bool arrowFound = false;
            foreach (GUIHudArrow arrow in arrows)
            {
                if (arrow.targetShip == ship)
                {
                    Vector3 position = arrowRadius * (ship.transform.position - cameraCenter).normalized;
                    arrow.transform.localPosition = new Vector3(position.x, position.z, position.y);
                    arrowFound = true;
                }
            }
            if (!arrowFound)
            {
                GameObject newArrow = (GameObject)Instantiate(arrowPrefab, transform.position, Quaternion.identity);
                newArrow.transform.parent = transform;
                newArrow.GetComponent<GUIHudArrow>().targetShip = ship;
                Vector3 position = arrowRadius * (ship.transform.position - cameraCenter).normalized;
                newArrow.transform.localPosition = new Vector3(position.x, position.z, position.y);
                arrows.Add(newArrow.GetComponent<GUIHudArrow>());
            }
        }
        ArrayList arrowsCopy = (ArrayList)arrows.Clone();
        foreach (GUIHudArrow arrow in arrowsCopy)
        {
            bool shipFound = false;
            foreach (GameObject ship in ships)
            {
                if (ship == arrow.targetShip && (ship.transform.position - cameraCenter).magnitude >= arrowVisibleRange)
                    shipFound = true;
            }
            if (!shipFound)
            {
                Destroy(arrow.gameObject);
                arrows.Remove(arrow);
            }
        }
	}
}
