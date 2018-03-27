using System;
using UniRx;
using UnityEngine;
using HoloToolkit.Unity;
using UnityEngine.UI;

namespace GATARI.HoloLensGPS {
    public class GPSObject : MonoBehaviour {
        public GPSObjectData gpsObjectData;
        public TextMesh[] textMeshes;
        public AudioSource audioSource;
        public AudioClip activationSound;
        public AudioClip deactivationSound;
        public Animator concreteAnimator;
        public Animator uiAnimator;
        public float uiDistance = 2.0f;
        public Text uiDebugText;

        Vector4 localPosition = new Vector4();

        private void Start() {
            audioSource = GetComponent<AudioSource>();
            SpatialSoundSettings.SetRoomSize(audioSource, SpatialSoundRoomSizes.Medium);
            GPSMapService.Instance.onPostureUpdate.AddListener(CheckArea);
            gpsObjectData.IsInside.Subscribe(b => {
                if (b) {
                    ShowConcreteObject();
                } else {
                    ShowUIObject();
                }
            });
        }

        void CheckArea(PlayerPosture posture) {
            gpsObjectData.CheckArea(posture.playerPosition, posture.playerAngle);
            if (gpsObjectData.IsInside.Value) {
                AreaEnterDebug.Instance.UpdateDebugText(this.gpsObjectData);
            }

            if (!gpsObjectData.IsInside.Value) {
                var radianAngle = gpsObjectData.Angle / 180 * Math.PI;
                localPosition.Set((float)(uiDistance * Math.Sin(radianAngle)), 0, (float)(uiDistance * Math.Cos(radianAngle)), 1);
                transform.position = Camera.main.transform.localToWorldMatrix * localPosition;
                transform.LookAt(2 * transform.position - Camera.main.transform.position);
                uiDebugText.text = string.Format("Area: {0}\nAngle: {1}\nDistance: {2}", gpsObjectData.GPSObjectName, gpsObjectData.Angle.ToString("0.0"), gpsObjectData.Distance.ToString("0.0"));
            }
        }

        public void ShowConcreteObject() {
            var radianAngle = gpsObjectData.Angle / 180 * Math.PI;
            var localPositionToCamera = new Vector4((float)(gpsObjectData.Distance * Math.Sin(radianAngle)), -1f, (float)(gpsObjectData.Distance * Math.Cos(radianAngle)), 1);
            transform.position = Camera.main.transform.localToWorldMatrix * localPositionToCamera;
            foreach (var textMesh in textMeshes) {
                textMesh.text = gpsObjectData.GPSObjectName;
            }
            concreteAnimator.SetBool("isVisible", true);
            uiAnimator.SetBool("isVisible", false);
            if (AreaEnterDebug.Instance != null) {
                AreaEnterDebug.Instance.UpdateDebugText(gpsObjectData);
            }
            WorldAnchorManager.Instance.AttachAnchor(gameObject);
            audioSource.PlayOneShot(activationSound);
        }

        public void ShowUIObject() {
            concreteAnimator.SetBool("isVisible", false);
            uiAnimator.SetBool("isVisible", true);
            WorldAnchorManager.Instance.RemoveAnchor(gameObject);
            audioSource.PlayOneShot(deactivationSound);
            if (AreaEnterDebug.Instance != null) {
                AreaEnterDebug.Instance.ClearDebugText();
            }
        }
    }
}