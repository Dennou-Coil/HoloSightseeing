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
            var radianAngle = data.Angle / 180 * Math.PI;
            transform.position = new Vector3((float)(data.Distance * Math.Sin(radianAngle)), 0, (float)(data.Distance * Math.Cos(radianAngle)));
            textMesh.text = data.GPSObjectName;
            anim.SetBool("isVisible", true);
            AreaEnterDebug.Instance.UpdateDebugText(data);
        }

        public void Deactivate() {
            anim.SetBool("isVisible", false);
        }
    }
}