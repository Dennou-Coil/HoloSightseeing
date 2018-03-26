using System;
using UniRx;
using UnityEngine;

namespace GATARI.HoloLensGPS {
    public class GPSObject : MonoBehaviour {
        public GPSObjectData gpsObjectData;
        public TextMesh textMesh;
        string anchorId;
        Animator anim;

        private void Start() {
            anim = GetComponent<Animator>();
            GPSMapService.Instance.onPostureUpdate.AddListener(CheckArea);
            gpsObjectData.IsInside.Subscribe(b => {
                if (b) {
                    Activate(gpsObjectData);
                } else {
                    Deactivate();
                }
            });
        }

        void CheckArea(PlayerPosture posture) {
            gpsObjectData.CheckArea(posture.playerPosition, posture.playerAngle);
        }

        public void Activate(GPSObjectData data) {
            Debug.Log(data.Angle);
            var radianAngle = data.Angle / 180 * Math.PI;
            Debug.Log(radianAngle);
            transform.position = new Vector3((float)(data.Distance * Math.Sin(radianAngle)), 0, (float)(data.Distance * Math.Cos(radianAngle)));
            Debug.Log(transform.position);
            textMesh.text = data.GPSObjectName;
            anim.SetBool("isVisible", true);
        }

        public void Deactivate() {
            anim.SetBool("isVisible", false);
        }
    }
}