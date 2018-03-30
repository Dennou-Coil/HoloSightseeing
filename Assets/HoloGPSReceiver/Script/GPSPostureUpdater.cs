using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GATARI.HoloLensGPS {
    public class GPSPostureUpdater : MonoBehaviour {
        OnlineMapsLocationService locationService;
        OnlineMaps maps;

        Vector3 rotationCache = new Vector3();

        int zoomMaxValue = 19;
        [SerializeField] int zoom = 18;
        public int Zoom {
            get {
                return zoom;
            }
            set {
                if (value > zoomMaxValue) {
                    zoom = zoomMaxValue;
                } else if (value < 0) {
                    zoom = 0;
                } else {
                    zoom = value;
                } 
            }
        }

        private void Start() {
            //locationService = GetComponentInChildren<OnlineMapsLocationService>();
            //locationService.useGPSEmulator = true;
            //locationService.disableEmulatorInPublish = false;
            //GPSMapService.Instance.onPostureUpdate.AddListener((p) => {
            //    locationService.emulatorPosition.Set((float)p.playerPosition[1], (float)p.playerPosition[0]);
            //    locationService.emulatorCompass = (float)p.playerAngle;
            //});
            maps = GetComponentInChildren<OnlineMaps>();
            GPSMapService.Instance.onPostureUpdate.AddListener((p) => {
                //locationService.emulatorPosition.Set((float)p.playerPosition[1], (float)p.playerPosition[0]);
                //locationService.emulatorCompass = (float)p.playerAngle;
                maps.SetPosition(p.playerPosition[1], p.playerPosition[0]);
                //rotationCache.Set(0, 0, (float)p.playerAngle);
                transform.localRotation = Quaternion.Euler(0, 0, (float)p.playerAngle);
            });
            maps.zoom = Zoom;
        }

        public void ZoomIn() {
            Zoom += 1;
            maps.zoom = Zoom;
        }


        public void ZoomOut() {
            Zoom -= 1;
            maps.zoom = Zoom;
        }
    }
}
