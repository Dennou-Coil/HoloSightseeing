using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Osc;

namespace GATARI.HoloLensGPS {

    [DefaultExecutionOrder(-100)]
    public class GPSMapService : MonoBehaviour {
        public static GPSMapService Instance { get; private set; }
        public float compassOffset;
        public PlayerPosture Posture { get; private set; }
        bool isPositionUpdated, isAngleUpdated;
        
        // Use this for initialization
        private void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
            }
        }
        
        public class PostureUpdateEvent : UnityEvent<PlayerPosture> { }
        public PostureUpdateEvent onPostureUpdate;

        void Start() {
            Instance = this;
            Posture = new PlayerPosture() {
                playerPosition = new double[2]
            };
            onPostureUpdate = new PostureUpdateEvent();
        }

        public void UpdatePositionAndAngle(Message msg) {
            if (msg.path.Contains("gps")) {
                Posture.playerPosition[0] = double.Parse(msg.data[0].ToString());
                Posture.playerPosition[1] = double.Parse(msg.data[1].ToString());
                isPositionUpdated = true;
            } else if (msg.path.Contains("compass")) {
                Posture.playerAngle = double.Parse(msg.data[0].ToString());
                isAngleUpdated = true;
            }
            if (isAngleUpdated && isPositionUpdated) {
                onPostureUpdate.Invoke(Posture);
                isPositionUpdated = false;
                isAngleUpdated = false;
            }
        }
    }

    [System.Serializable]
    public class PlayerPosture {
        [SerializeField] public double playerAngle;
        [SerializeField] public double[] playerPosition;
    }
}
