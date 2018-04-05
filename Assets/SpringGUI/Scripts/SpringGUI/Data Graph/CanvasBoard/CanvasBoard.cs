using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace SpringGUI {
    public class CanvasBoard : MaskableGraphic, IPointerClickHandler {
        [SerializeField]
        public CanvasBoardData canvasBoardBasis = null;
        public Camera eventCamera;

        public ICanvasBoard _CanvasBoardCreator = null;

        //[SerializeField]
        //private List<Vector2> listPoints = new List<Vector2>();

        private ICanvasBoard CanvasBoardCreator {
            get {
                //return new CanvasBoard1 ();

                if (_CanvasBoardCreator == null) {
                    _CanvasBoardCreator = new CanvasBoard1 ();
                }
                return _CanvasBoardCreator;
            }
        }

        protected override void OnPopulateMesh (VertexHelper vh) {
            vh.Clear ();
            //if (canvasBoardBasis.Lines.Count.Equals (0))
            //    return;
            var rect = base.GetPixelAdjustedRect ();
            CanvasBoardCreator.DrawCanvasBoard (vh, rect, canvasBoardBasis);
        }

        public void Refresh () {
            OnEnable ();
        }

        //public void Inject (IList<Vector2> vertexs) {
        //    canvasBoardBasis.AddLine (vertexs);
        //}

        public void Inject (Vector2 point, bool bMousePosition = false) {
            if (!bMousePosition) {
                canvasBoardBasis.AddPoint (point, !canvasBoardBasis.isTracingMouse);
            }
            else {
                canvasBoardBasis.mouseLocalPoint = point;
            }
        }

        public void InjectExist (Point point) {
            canvasBoardBasis.AddExistPoint (point);
        }

        public void ClearBoard () {
            canvasBoardBasis.listPoint.Clear ();
            canvasBoardBasis.isTracingMouse = false;
            SetAllDirty ();
        }

        public void EditLines () {
            canvasBoardBasis.isTracingMouse = false;
            canvasBoardBasis.isEditingLines = !canvasBoardBasis.isEditingLines;

            GenerateSelectableLines ();
        }

        private void GenerateSelectableLines () {
            // lines
            for (int i = 1; i < canvasBoardBasis.linesRoot.transform.childCount; ++i) {
                var child = canvasBoardBasis.linesRoot.transform.GetChild (i);
                Destroy (child.gameObject);
            }

            foreach (var line in canvasBoardBasis.listLines) {
                GameObject go = Instantiate (canvasBoardBasis.lineTemplate.gameObject, canvasBoardBasis.linesRoot.transform);
                go.GetComponent<LineData> ().line = line;
                var rt = go.GetComponent<RectTransform> ();
                rt.anchoredPosition = (line.start.vec + line.end.vec) / 2;
                float width = Mathf.Abs ((line.end.vec - line.start.vec).magnitude);
                rt.sizeDelta = new Vector2 (width, 20);
                Vector2 vec = (line.end.vec - line.start.vec).normalized;

                // calculate rotation
                float targetRotation = Mathf.Atan2 (vec.y, vec.x) * Mathf.Rad2Deg;
                rt.localRotation = Quaternion.Euler (0, 0, targetRotation);

                go.SetActive (true);
            }

            // points
            for (int i = 1; i < canvasBoardBasis.pointsRoot.transform.childCount; ++i) {
                var child = canvasBoardBasis.pointsRoot.transform.GetChild (i);
                Destroy (child.gameObject);
            }

            foreach (var list in canvasBoardBasis.listPoint) {
                foreach (var point in list) {
                    GameObject go = Instantiate (canvasBoardBasis.pointTemplate.gameObject, canvasBoardBasis.pointsRoot.transform);
                    go.GetComponent<PointData> ().point = point;
                    var rt = go.GetComponent<RectTransform> ();
                    rt.anchoredPosition = point.vec;

                    go.SetActive (true);
                }
            }

            //canvasBoardBasis.listPoint.Clear ();
            //SetAllDirty ();
        }

        public void CalculateLines () {
            for (int i = 0; i < canvasBoardBasis.linesRoot.transform.childCount; ++i) {
                var child = canvasBoardBasis.linesRoot.transform.GetChild (i);
                if (child.gameObject.activeInHierarchy) {
                    var line = child.GetComponent<LineData> ().line;
                    var rt = child.GetComponent<RectTransform> ();
                    rt.anchoredPosition = (line.start.vec + line.end.vec) / 2;
                    float width = Mathf.Abs ((line.end.vec - line.start.vec).magnitude);
                    rt.sizeDelta = new Vector2 (width, 20);

                    Vector2 vec = (line.end.vec - line.start.vec).normalized;

                    // calculate rotation
                    float targetRotation = Mathf.Atan2 (vec.y, vec.x) * Mathf.Rad2Deg;
                    rt.localRotation = Quaternion.Euler (0, 0, targetRotation);
                }
            }
        }

        public void CalculatePoints () {
            for (int i = 0; i < canvasBoardBasis.pointsRoot.transform.childCount; ++i) {
                var child = canvasBoardBasis.pointsRoot.transform.GetChild (i);
                if (child.gameObject.activeInHierarchy) {
                    var point = child.GetComponent<PointData> ().point;
                    var rt = child.GetComponent<RectTransform> ();
                    rt.anchoredPosition = point.vec;
                }
            }
        }

        public void CalculateRooms() {

        }

        public void DeleteRoom (Room room) {

        }

        public void DeleteLine (Line line) {
            bool dirty = false;

            if (!line.start.lines.Remove (line)) {
                Debug.LogError ("Remove line from startPoint failed!");
            }

            if (!line.end.lines.Remove (line)) {
                Debug.LogError ("Remove line from endPoint failed!");
            }

            
            //foreach (var list in canvasBoardBasis.listPoint) {
            //    int idx = list.FindIndex (x => x == line.start);
            //    if (idx != -1) {
            //        // found
            //        if (idx != 0 && idx != list.Count - 1) {
            //            // neither head nor tail
            //            // divide list into two
            //            List<Point> laterHalfList = list.GetRange (idx, list.Count - idx);
            //            canvasBoardBasis.listPoint.Add (laterHalfList);
            //            list.RemoveRange (idx, list.Count - idx);

            //            dirty = true;
            //            break;
            //        } else {
            //            // head or tail, when no lines left, remove point directly
            //            if (line.start.lines.Count == 0) {
            //                list.RemoveAt (idx);

            //                dirty = true;
            //                break;
            //            }
            //        }

            //    }
            //}

            
            foreach (var list in canvasBoardBasis.listPoint) {
                int idx = list.FindIndex (x => x == line.end);
                if (idx != -1) {
                    if (idx != 0 && idx != list.Count - 1) {
                        // found and neither head nor tail
                        // divide list into two
                        List<Point> laterHalfList = list.GetRange (idx, list.Count - idx);
                        canvasBoardBasis.listPoint.Add (laterHalfList);
                        list.RemoveRange (idx, list.Count - idx);

                        dirty = true;
                        break;
                    } else {
                        // head or tail, when no lines left, remove point directly
                        if (line.end.lines.Count == 0) {
                            list.RemoveAt (idx);

                            if(line.start.lines.Count == 0) {
                                list.RemoveAt (idx - 1);
                            }

                            dirty = true;
                            break;
                        }
                    }
                }
            }
            
            if (dirty)
                SetAllDirty ();
        }

        public void DeletePoint(Point point) {

        }

        private void Update () {
            if (canvasBoardBasis.isTracingMouse) {
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle (rectTransform, Input.mousePosition, eventCamera, out localPoint);
                Inject (localPoint, true);
                SetAllDirty ();
            }
            if (canvasBoardBasis.isEditingLines) {

            }
        }

        public void OnPointerClick (PointerEventData eventData) {
            if (canvasBoardBasis.isEditingLines)
                return;

            switch (eventData.button) {
            case PointerEventData.InputButton.Left: {
                    switch (canvasBoardBasis.drawingState) {
                    case DrawingState.Edit:
                        break;
                    case DrawingState.Walls: {
                            Vector2 pos = eventData.position;
                            //Debug.Log (pos.ToString ());
                            Vector2 localPoint;
                            RectTransformUtility.ScreenPointToLocalPointInRectangle (rectTransform, pos, eventData.pressEventCamera, out localPoint);
                            //Debug.Log ("localPoint: " + localPoint.ToString ());

                            Inject (localPoint, false);

                            canvasBoardBasis.isTracingMouse = true;
                            SetAllDirty ();
                        }
                        break;
                    case DrawingState.Rooms:
                        break;
                    }
                }
                break;
            case PointerEventData.InputButton.Right: {
                    switch(canvasBoardBasis.drawingState) {
                    case DrawingState.Edit:
                        break;
                    case DrawingState.Walls: {
                            if (canvasBoardBasis.isTracingMouse) {
                                canvasBoardBasis.isTracingMouse = false;

                                SetAllDirty ();
                            } else {
                                canvasBoardBasis.drawingState = DrawingState.Edit;
                            }
                        }
                        break;
                    case DrawingState.Rooms:
                        break;
                    }
                }
                break;
            }
        }
    }

    public enum DrawingState {
        Edit,
        Walls,
        Rooms
    }

    public interface ICanvasBoard {
        VertexHelper DrawCanvasBoard (VertexHelper vh, Rect rect, CanvasBoardData basis);
        VertexHelper DrawMesh (VertexHelper vh);
        VertexHelper DrawAxis (VertexHelper vh);
        VertexHelper DrawRectangle (VertexHelper vh);
    }


}
