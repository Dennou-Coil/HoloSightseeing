using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Toolkit;

namespace GATARI.HoloLensGPS {

    public class GPSObjectManager : MonoBehaviour {
        public static GPSObjectManager Instance { get; private set; }

        private void Awake() {
            if (Instance != null) {
                Destroy(this);
            }
        }
        // Use this for initialization
        void Start() {
            Instance = this;
        }

        public void PlaceObject(GPSObjectData data) {

        }
    }
}

