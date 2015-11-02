using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIUpdateHealth : MonoBehaviour
{
    public Text text;
    public bool updating = true;
    public float refreshRate = 0.1f;
    public Slider hullSlider;
    public Slider shieldSlider;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(updateText());
        shieldSlider.maxValue = 100;
        hullSlider.maxValue = 100;
        shieldSlider.minValue = 0;
        hullSlider.minValue = 0;
    }


    IEnumerator updateText()
    {
        if (Camera.main.GetComponent<CameraFollow>().myTargets[0] != null)
            if (Camera.main.GetComponent<CameraFollow>().myTargets[0].GetComponent<Health>() != null)
            {
                text.text = "HULL:" + ((int)Camera.main.GetComponent<CameraFollow>().myTargets[0].GetComponent<Health>().myHealth) + " | SHIELD:" + ((int)Camera.main.GetComponent<CameraFollow>().myTargets[0].GetComponent<Health>().myShield);
                hullSlider.value = (int)Camera.main.GetComponent<CameraFollow>().myTargets[0].GetComponent<Health>().myHealth;
                shieldSlider.value = (int)Camera.main.GetComponent<CameraFollow>().myTargets[0].GetComponent<Health>().myShield;
            }
        yield return new WaitForSeconds(refreshRate);
        if (updating)
            StartCoroutine(updateText());
    }
}
