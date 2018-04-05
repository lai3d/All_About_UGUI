using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using UnityEngine.EventSystems;

namespace SpringGUI {

    //[RequireComponent (typeof (PointSelectable))]
    [RequireComponent(typeof(Selectable))]
    public class PointController : MonoBehaviour, IDragHandler, IPointerClickHandler {

        public float moveSpeed = 15.0f;

        //private PointSelectable pointSelectable;
        private Selectable selectable;
        private PointData pointData;
        private CanvasBoard canvasBoard;

        private Vector3[] v = new Vector3[4];

        public bool draggingHandle { get; private set; }

        // Use this for initialization
        void Start () {
            //pointSelectable = GetComponent<PointSelectable> ();
            selectable = GetComponent<Selectable> ();
            pointData = GetComponent<PointData> ();
            canvasBoard = this.transform.parent.parent.GetComponent<CanvasBoard> ();
            if(canvasBoard == null) {
                Debug.LogError ("Get component CanvasBoard failed!");
            }

            draggingHandle = false;
        }

        // Update is called once per frame
        void Update () {
            //if (!pointSelectable.selected) {
            //    return;
            //}
            if (canvasBoard.canvasBoardBasis.drawingState != DrawingState.Edit)
                return;
            if (EventSystem.current.currentSelectedGameObject != this.gameObject)
                return;
            // Delete
            if (Input.GetKeyUp (KeyCode.Delete)) {
                canvasBoard.DeletePoint (pointData.point);

                Destroy (gameObject);
            }

            if(Input.GetMouseButtonDown(0)) {
                OnBeginDrag ();
            }

            if(Input.GetMouseButton(0) && draggingHandle) {
                
            }

            {
                // Move with direction keys
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

                    pointData.point.vec = canvasBoard.transform.InverseTransformPoint ((start3 + end3) / 2);

                    canvasBoard.CalculatePoints ();
                    canvasBoard.CalculateLines ();
                    canvasBoard.CalculateRooms ();
                }
            }

            if (Input.GetMouseButtonUp (0)) {
                OnFinishDrag ();
            }
        }

        void OnBeginDrag() {
            draggingHandle = true;
        }

        void OnFinishDrag() {
            StartCoroutine (SetDraggingFalse ());
        }

        //void OnFinishHandleMovement () {
        //    RebuildGizmoMesh (Vector3.one);
        //    RebuildGizmoMatrix ();

        //    if (OnHandleFinish != null)
        //        OnHandleFinish ();

        //    StartCoroutine (SetDraggingFalse ());
        //}

        IEnumerator SetDraggingFalse () {
            yield return new WaitForEndOfFrame ();
            draggingHandle = false;
        }

        public void OnDrag (PointerEventData eventData) {
            //Debug.Log ("OnDrag");
            if (canvasBoard.canvasBoardBasis.drawingState == DrawingState.Edit) { 
            // Move with mouse drag
            float translationV = eventData.delta.y;
                float translationH = eventData.delta.x;

                if (Mathf.Abs (translationH) > float.Epsilon || Mathf.Abs (translationV) > float.Epsilon) {
                    Debug.Log ("Translate H: " + translationH + " V: " + translationV);
                    transform.Translate (translationH, translationV, 0, transform.parent);

                    var rt = GetComponent<RectTransform> ();
                    //rt.GetWorldCorners (v);
                    //var start3 = (v[0] + v[1]) / 2;
                    //var end3 = (v[2] + v[3]) / 2;

                    //pointData.point.vec = canvasBoard.transform.InverseTransformPoint ((start3 + end3) / 2);
                    pointData.point.vec = rt.anchoredPosition;

                    canvasBoard.CalculatePoints ();
                    canvasBoard.CalculateLines ();
                    canvasBoard.CalculateRooms ();
                }
            }
        }

        public void OnPointerClick (PointerEventData eventData) {
            if(canvasBoard.canvasBoardBasis.drawingState == DrawingState.Walls)
                canvasBoard.InjectExist (pointData.point);
            //canvasBoard.canvasBoardBasis.isTracingMouse = true;
        }
    }
}
