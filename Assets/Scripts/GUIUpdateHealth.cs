using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIUpdateHealth : MonoBehaviour
{
    public Text text;
    public HudBar3D hullSlider;
    public HudBar3D shieldSlider;
    public bool updating = true;
    public float refreshRate = 0.1f;

    // Use this for initialization
    void Start()
    {
        shieldSlider.maxValue = 100;
        hullSlider.maxValue = 100;
        shieldSlider.minValue = 0;
        hullSlider.minValue = 0;
    }

    void Update()
    {
        if (Camera.main.GetComponent<CameraFollow>().myTargets[0] != null)
        if (Camera.main.GetComponent<CameraFollow>().myTargets[0].GetComponent<Health>() != null)
        {
            text.text = "HULL:" + ((int)Camera.main.GetComponent<CameraFollow>().myTargets[0].GetComponent<Health>().myHealth) + " | SHIELD:" + ((int)Camera.main.GetComponent<CameraFollow>().myTargets[0].GetComponent<Health>().myShield);
            hullSlider.value = (int)Camera.main.GetComponent<CameraFollow>().myTargets[0].GetComponent<Health>().myHealth;
            shieldSlider.value = (int)Camera.main.GetComponent<CameraFollow>().myTargets[0].GetComponent<Health>().myShield;
        }
    }
}
