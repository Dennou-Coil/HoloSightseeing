using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Osc;

namespace GATARI.HoloLensGPS {
    public class GPSMapService : MonoBehaviour {
        public static GPSMapService Instance { get; private set; }
        public float compassOffset;
        public GPSObjectData[] datas;

        [SerializeField] float playerAngle;
        [SerializeField] double[] playerPosition;
        // Use this for initialization
        private void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
            }
        }

        void Start() {
            Instance = this;
            playerPosition = new double[2];
        }

        public void UpdatePositionAndAngle(Message msg) {
            if (msg.path.Contains("gps")) {
                playerPosition[0] = double.Parse(msg.data[0].ToString());
                playerPosition[1] = double.Parse(msg.data[1].ToString());
            } else if (msg.path.Contains("compass")) {
                playerAngle = float.Parse(msg.data[0].ToString());
            }
            CheckDataArea();
        }

        public void CheckDataArea() {
            foreach (var data in datas) {
                if (data.IsInside(playerPosition)) {
                    Debug.Log(data.GPSObjectName);
                }
            }
        }
    }
}
