using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class OSCMessageDebug : MonoBehaviour {

    public Text text;
    public string targetPath = "";
    public string debugInfo = "";
    StringBuilder stringBuilder;

    private void Start() {
        stringBuilder = new StringBuilder();
    }

    public void UpdateText(Osc.Message msg) {
        stringBuilder.Length = 0;
        stringBuilder.AppendLine(debugInfo);
        if (msg.path.Contains(targetPath)) {
            for (int i = 0; i < msg.data.Length; i++) {
                stringBuilder.AppendLine(msg.data[i].ToString());
            }
            text.text = stringBuilder.ToString();
        }
    }
}
