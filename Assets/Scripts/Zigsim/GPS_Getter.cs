using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPS_Getter : MonoBehaviour {

    public GameObject zigobject;
    public GameObject Lattext;
    public GameObject Lontext;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float lat = zigobject.GetComponent<Zigsim>().data.sensordata.gps.latitude;
        float lon = zigobject.GetComponent<Zigsim>().data.sensordata.gps.longitude;
        Debug.Log(lat);
        Debug.Log(lon);
        Lattext.GetComponent<TextMesh>().text = lat.ToString();
        Lontext.GetComponent<TextMesh>().text = lon.ToString();

    }
}
