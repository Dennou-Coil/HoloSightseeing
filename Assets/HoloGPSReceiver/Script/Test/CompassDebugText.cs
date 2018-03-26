using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassDebugText : MonoBehaviour {
    Text debugText;
    Compass compass;

    // Use this for initialization
    void Start() {
        debugText = GetComponent<Text>();
        compass = new Compass();
        compass.enabled = true;
    }

    // Update is called once per frame
    void Update() {
        debugText.text = compass.magneticHeading.ToString();
    }
}
