using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UniRx;

namespace GATARI.HoloLensGPS {

    [CreateAssetMenu]
    public class GPSObjectData : ScriptableObject {

        [SerializeField] int id;
        public int ID {
            get {
                return id;
            }
            private set {
                id = value;
            }
        }

        [SerializeField] double[] position;
        public double[] Position {
            get {
                return position;
            }
            private set {
                position = value;
            }
        }

        [SerializeField] string gpsObjectName;
        public string GPSObjectName {
            get {
                return gpsObjectName;
            }
            private set {
                gpsObjectName = value;
            }
        }

        [SerializeField] float areaInboundRadius;
        public float AreaInsideRadius {
            get {
                return areaInboundRadius;
            }
            private set {
                areaInboundRadius = value;
            }
        }

        [SerializeField] float areaOutboundRadius;
        public float AreaOutboundRadius {
            get {
                return areaOutboundRadius;
            }
            private set {
                areaOutboundRadius = value;
            }
        }
        public ReactiveProperty<bool> IsInside;
        [SerializeField] bool isInside;

        [SerializeField] double distance;
        public double Distance { get {
                return distance;
            } private set {
                distance = value;
            }
        }
        [SerializeField] double angle;
        public double Angle { get {
                return angle;
            } private set {
                angle = value;
            }
        }

        public void CheckArea(double[] playerPosition, double playerAngle) {
            UpdateInformation(playerPosition, playerAngle);
            if (IsInside.Value) {
                if (IsExiting(playerPosition)) {
                    IsInside.Value = false;
                }
            } else {
                if (IsEntering(playerPosition)) {
                    IsInside.Value = true;
                }
            }
        }

        private void OnEnable() {
            IsInside = new ReactiveProperty<bool>(false);
            Distance = double.MaxValue;
            Angle = 0;
        }

        public void UpdateInformation(double[] playerPosition, double playerAngle) {
            Distance = GPSUtility.HubenyDistance(playerPosition, Position);
            Angle = GPSUtility.Angle(playerPosition, Position, playerAngle);
        }

        public bool IsEntering(double[] playerPosition) {
            return AreaInsideRadius > Distance;
        }

        public bool IsExiting(double[] playerPosition) {
            var distance = GPSUtility.HubenyDistance(playerPosition, Position);
            return AreaOutboundRadius < distance;
        }
    }
}

