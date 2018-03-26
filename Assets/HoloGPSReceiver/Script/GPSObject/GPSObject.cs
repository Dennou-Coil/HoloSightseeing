using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity;
using System;
using UniRx;

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
            transform.position = new Vector3((float)(data.Distance * Math.Sin(data.Angle)), 0, (float)(data.Distance * Math.Cos(data.Angle)));
            textMesh.text = data.GPSObjectName;
            anim.SetBool("isVisible", true);
        }

        public void Deactivate() {
            anim.SetBool("isVisible", false);
        }
    }
}