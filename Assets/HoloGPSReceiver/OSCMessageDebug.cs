using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OSCMessageDebug : MonoBehaviour {

    public Text text;
    [SerializeField] string targetPath = "gps";

    public void UpdateText(Osc.Message msg) {
        if (msg.path.Contains(targetPath)) {
            text.text = string.Format("latitude: {0}, longitude: {1}", msg.data[0], msg.data[1]);
        }
    }
}
