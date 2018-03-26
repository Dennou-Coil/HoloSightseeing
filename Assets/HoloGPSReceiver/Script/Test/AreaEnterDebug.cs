using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GATARI.HoloLensGPS {

    public class AreaEnterDebug : MonoBehaviour {
        Text text;
        private void Start() {
            text = GetComponent<Text>();
        }

        public void UpdateDebugText(GPSObjectData data) {
            text.text = string.Format("Area: {0}", data.GPSObjectName);
        }
    }
}

