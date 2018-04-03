using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace SpringGUI {

	[RequireComponent(typeof(LineSelectable))]
	public class LineController : MonoBehaviour {

		public float moveSpeed = 15.0f;

		private LineSelectable lineSelectable;
		private LineData lineData;
		private CanvasBoard canvasBoard;

		private Vector3[] v = new Vector3[4];

		// Use this for initialization
		void Start () {
			lineSelectable = GetComponent<LineSelectable> ();
			lineData = GetComponent<LineData> ();
			canvasBoard = this.transform.parent.GetComponent<CanvasBoard> ();
		}
		
		// Update is called once per frame
		void Update () {
			if (lineSelectable.selected) {
                // Delete
                if (Input.GetKeyUp (KeyCode.Delete)) {
                    canvasBoard.DeleteLine (lineData.line);

                    Destroy (gameObject);
                }
                else {
                    // Move
                    float translationV = Input.GetAxis ("Vertical") * moveSpeed;
                    float translationH = Input.GetAxis ("Horizontal") * moveSpeed;
                    translationV *= Time.deltaTime;
                    translationH *= Time.deltaTime;

                    if (Mathf.Abs (translationH) > float.Epsilon || Mathf.Abs (translationV) > float.Epsilon) {
                        //Debug.Log ("Translate H: " + translationH + " V: " + translationV);
                        transform.Translate (translationH, translationV, 0, transform.parent);

                        var rt = GetComponent<RectTransform> ();
                        rt.GetWorldCorners (v);
                        var start3 = (v[0] + v[1]) / 2;
                        var end3 = (v[2] + v[3]) / 2;

                        lineData.line.start.vec = canvasBoard.transform.InverseTransformPoint (start3);
                        lineData.line.end.vec = canvasBoard.transform.InverseTransformPoint (end3);

                        canvasBoard.CalculateLines ();
                    }
                }
			}
		}
	}

}