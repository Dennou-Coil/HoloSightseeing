using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

        public bool IsInside(double[] playerPosition) {
            var distance = GPSUtility.HubenyDistance(playerPosition, Position);
            Debug.Log(distance);
            return AreaInsideRadius > distance;
        }
    }
}

