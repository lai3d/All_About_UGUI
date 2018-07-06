using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class ColorListExample : MonoBehaviour {

    public UICornerCut template;

	// Use this for initialization
	void Start () {
        for(int i = 0; i < HueColor.Count; ++i) {
            UICornerCut ucc = Instantiate (template, template.transform.parent);
            HueColor.Names enumColor = (HueColor.Names)i;

            //ucc.color = HueColor.HueColorValue (enumColor);
            ucc.color = enumColor.Color ();
            ucc.GetComponentInChildren<Text> ().text = enumColor.ToString ();

            ucc.gameObject.SetActive (true);
        }

        for (int i = 0; i < HueColor.Count; ++i) {
            UICornerCut ucc = Instantiate (template, template.transform.parent);
            HueColor.Names enumColor = (HueColor.Names)i;

            ucc.color = HueColor.HueColorValue (enumColor, 128);
            ucc.GetComponentInChildren<Text> ().text = enumColor.ToString () + " a=128";

            ucc.gameObject.SetActive (true);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
