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
        public GameObject floatingAraImagePrefab;

        Vector3 localPosition = new Vector3();
        private Vector3 velocity = new Vector3();
        GameObject floatingAreaImageInstance;
        double gpsAngle, cameraAngle;

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
            gpsAngle = GPSUtility.Deg2Rad(gpsObjectData.Angle);
            cameraAngle = GPSUtility.Deg2Rad(Camera.main.transform.eulerAngles.y);

            if (gpsObjectData.IsInside.Value) {
                if (AreaEnterDebug.Instance != null) {
                    AreaEnterDebug.Instance.UpdateDebugText(this.gpsObjectData);
                }
            }
        }

        private void Update() {
            if (!gpsObjectData.IsInside.Value) {
                localPosition.Set((float)(uiDistance * Math.Sin(gpsAngle+cameraAngle)), 0, (float)(uiDistance * Math.Cos(gpsAngle+cameraAngle)));
                transform.position = Vector3.SmoothDamp(transform.position, Camera.main.transform.position + localPosition, ref velocity, 0.1f);
                transform.LookAt(2 * transform.position - Camera.main.transform.position);
                uiDebugText.text = string.Format("Area: {0}\nAngle: {1}\nDistance: {2}", gpsObjectData.GPSObjectName, gpsObjectData.Angle.ToString("0.0"), gpsObjectData.Distance.ToString("0.0"));
            }
        }

        public void ShowConcreteObject() {
            transform.position = Camera.main.transform.position + new Vector3((float)(gpsObjectData.Distance * Math.Sin(gpsAngle + cameraAngle)), -0.5f, (float)(gpsObjectData.Distance * Math.Cos(gpsAngle + cameraAngle)));
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
            floatingAreaImageInstance = Instantiate(floatingAraImagePrefab);
            var image = floatingAreaImageInstance.GetComponentInChildren<Image>();
            if (gpsObjectData.locationImage != null) {
                Debug.Log("Image Found");
                image.sprite = gpsObjectData.locationImage;
            } else {
                image.sprite = Resources.Load("missing", typeof(Sprite)) as Sprite;
            }
            image.preserveAspect = true;
        }

        public void ShowUIObject() {
            concreteAnimator.SetBool("isVisible", false);
            uiAnimator.SetBool("isVisible", true);
            WorldAnchorManager.Instance.RemoveAnchor(gameObject);
            audioSource.PlayOneShot(deactivationSound);
            if (AreaEnterDebug.Instance != null) {
                AreaEnterDebug.Instance.ClearDebugText();
            }
            Destroy(floatingAreaImageInstance);
            floatingAreaImageInstance = null;
        }
    }
}