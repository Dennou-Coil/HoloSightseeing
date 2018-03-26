using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GATARI.HoloLensGPS {

    [DefaultExecutionOrder(-100)]
    public class AreaEnterDebug : MonoBehaviour {
        public static AreaEnterDebug Instance { get; private set; }

        Text text;
        private void Start() {
            Instance = this;
            text = GetComponent<Text>();
        }

        public void UpdateDebugText(GPSObjectData data) {
            text.text = string.Format("Area: {0}", data.GPSObjectName);
        }
    }
}

