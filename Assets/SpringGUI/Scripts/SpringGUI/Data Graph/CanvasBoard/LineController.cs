using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using UnityEngine.EventSystems;

namespace SpringGUI {

    //[RequireComponent (typeof (LineSelectable))]
    public class LineController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, ISelectHandler, IDeselectHandler {

        public float moveSpeed = 15.0f;

        //private LineSelectable lineSelectable;
        private LineData lineData;
        private RectTransform rectTransform;
        private CanvasBoard canvasBoard;

        private Vector3[] v = new Vector3[4];

        //public bool draggingHandle { get; private set; }
        public bool isDragging = false;

        // Use this for initialization
        void Start () {
            rectTransform = GetComponent<RectTransform> ();
            //lineSelectable = GetComponent<LineSelectable> ();
            lineData = GetComponent<LineData> ();
            canvasBoard = this.transform.parent.parent.GetComponent<CanvasBoard> ();
            if(canvasBoard == null) {
                Debug.LogError ("Get component CanvasBoard failed!");
            }

            //draggingHandle = false;
        }

        // Update is called once per frame
        void Update () {
            //if (!lineSelectable.selected) {
            //    return;
            //}
            if (canvasBoard.canvasBoardBasis.drawingState != DrawingState.Edit)
                return;
            if (EventSystem.current.currentSelectedGameObject != this.gameObject)
                return;
            // Delete
            if (Input.GetKeyUp (KeyCode.Delete)) {
                canvasBoard.DeleteLine (lineData.line);

                Destroy (gameObject);
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

                    lineData.line.start.vec = canvasBoard.transform.InverseTransformPoint (start3);
                    lineData.line.end.vec = canvasBoard.transform.InverseTransformPoint (end3);

                    canvasBoard.CalculatePoints ();
                    canvasBoard.CalculateLines ();
                    canvasBoard.CalculateRooms ();
                }
            }
        }

        void FixedUpdate () {
            return;
            if (isDragging && canvasBoard.canvasBoardBasis.drawingState == DrawingState.Edit) {
                //if (lineData != null && lineData.line.lineType == SpringGUI.LineType.Indoor) {
                    var filter = new ContactFilter2D ();
                    var hitResults = new RaycastHit2D[20];

                filter.useLayerMask = true;
                filter.useTriggers = true;
                filter.layerMask = LayerMaskExtensions.Create ("UI");

                LineController targetLineCtrl = null;
                    RaycastHit2D hit;

                    Vector2 origin = transform.parent.TransformPoint(rectTransform.anchoredPosition + new Vector2(0, 11f));
                    Vector2 direction = Vector2.up;

                int hitCount = Physics2D.Raycast (origin, direction, filter, hitResults, Mathf.Infinity);
                Debug.Log ("hitCount: " + hitCount);
                Debug.DrawRay (origin, direction * 1000f, Color.cyan);

                //hit = Physics2D.Raycast (origin, direction);
                //if(hit.collider != null) {
                //    Debug.Log ("hit distance" + hit.distance);
                       //Debug.DrawRay (origin, direction * hit.distance, Color.red);
                //}
                //}
            }
        }
        
        //void OnFinishHandleMovement () {
        //    RebuildGizmoMesh (Vector3.one);
        //    RebuildGizmoMatrix ();

        //    if (OnHandleFinish != null)
        //        OnHandleFinish ();

        //    StartCoroutine (SetDraggingFalse ());
        //}
        public void OnDrag (PointerEventData eventData) {
            //Debug.Log ("OnDrag");
            if (isDragging && canvasBoard.canvasBoardBasis.drawingState == DrawingState.Edit) {
                // Move with mouse drag
                float translationV = eventData.delta.y;
                float translationH = eventData.delta.x;

                if (Mathf.Abs (translationH) > float.Epsilon || Mathf.Abs (translationV) > float.Epsilon) {
                    //Debug.Log ("Translate H: " + translationH + " V: " + translationV);
                    transform.Translate (translationH, translationV, 0, transform.parent);

                    var rt = GetComponent<RectTransform> ();
                    rt.GetWorldCorners (v);
                    var start3 = (v[0] + v[1]) / 2;
                    var end3 = (v[2] + v[3]) / 2;

                    if (lineData.line != null) {
                        if(lineData.line.start != null)
                            lineData.line.start.vec = canvasBoard.transform.InverseTransformPoint (start3);
                        if(lineData.line.end != null)
                            lineData.line.end.vec = canvasBoard.transform.InverseTransformPoint (end3);
                    }

                    canvasBoard.CalculatePoints ();
                    canvasBoard.CalculateLines ();
                    canvasBoard.CalculateRooms ();
                }
            }
        }

        public void OnBeginDrag (PointerEventData eventData) {
            isDragging = true;
        }

        public void OnEndDrag (PointerEventData eventData) {
            isDragging = false;
        }

        public void OnSelect (BaseEventData eventData) {
            if (canvasBoard.canvasBoardBasis.drawingState == DrawingState.Edit) {
                //if (lineData != null && lineData.line.lineType == SpringGUI.LineType.Indoor) {
                var filter = new ContactFilter2D ();
                var hitResults = new RaycastHit2D[20];

                filter.useLayerMask = true;
                filter.useTriggers = true;
                filter.layerMask = LayerMaskExtensions.Create ("UI");

                LineController targetLineCtrl = null;
                RaycastHit2D hit;

                Vector2 origin = transform.parent.TransformPoint (rectTransform.anchoredPosition + new Vector2 (0, 11f));
                Vector2 direction = Vector2.up;

                int hitCount = Physics2D.Raycast (origin, direction, filter, hitResults, Mathf.Infinity);
                Debug.Log ("hitCount: " + hitCount);
                Debug.DrawRay (origin, direction * 1000f, Color.cyan);

                //hit = Physics2D.Raycast (origin, direction);
                //if(hit.collider != null) {
                //    Debug.Log ("hit distance" + hit.distance);
                //Debug.DrawRay (origin, direction * hit.distance, Color.red);
                //}
                //}
            }
        }

        public void OnDeselect (BaseEventData eventData) {
            throw new System.NotImplementedException ();
        }
    }
}
