using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace SpringGUI {

    [Serializable]
    public class CanvasBoardData {
        [Header ("Ruler Axis Setting")]
        public bool IsDrawAxis = true;
        public float AxisWidth = 2.0f;
        public Color AxisColor = Color.white;
        public bool ShowArrow = false;

        [Header ("Ruler Axis Scale Setting")]
        public float AxisScaleWidth = 2.0f;
        public float AxisScaleHeight = 8.0f;
        public Color AxisScaleColor = Color.white;

        [Header ("Ruler Mesh Setting")]
        public bool IsDrawMeshX = true;
        public bool IsDrawMeshY = true;
        public float MeshWidth = 2.0f;
        public Color MeshColor = Color.gray;
        [Range (5, 1000)]
        public float MeshCellXSize = 25.0f;
        [Range (5, 1000)]
        public float MeshCellYSize = 25.0f;
        public bool IsImaginaryLine = false;

        [HideInInspector]
        public Vector2 MeshCellSize { get { return new Vector2 (MeshCellXSize, MeshCellYSize); } }

        [Header ("Ruler Unit Setting")]
        public Color LineColor = Color.blue;
        public float LineWidth = 5.0f;
        //[HideInInspector]
        //public Dictionary<int, VertexStream> Lines = new Dictionary<int, VertexStream> ();
        public List<Line> listLines = new List<Line> ();
        [HideInInspector]
        public List<List<Point>> listPoint = new List<List<Point>> ();
        public Vector2 mouseLocalPoint;

        public DrawingState drawingState = DrawingState.Walls;

        public bool isTracingMouse = false;
        public bool isEditingLines = false;
        public bool isDetectNear = false;

        public float squareMagnitudeValue = 1.0f;

        public RectTransform lineTemplate;
        public RectTransform pointTemplate;

        public RectTransform linesRoot;
        public RectTransform pointsRoot;

        private bool FindPointNear (Vector2 point) {
            foreach (var list in listPoint) {
                foreach (var p in list) {
                    if ((point - p.vec).sqrMagnitude <= squareMagnitudeValue) {
                        return true;
                    }
                }
            }
            return false;
        }

        public void AddPoint (Vector2 point, bool bNewList) {
            if (isDetectNear) {
                if (bNewList) {
                    listPoint.Add (new List<Point> () { new Point { vec = point } });
                }
                else {
                    if (!FindPointNear (point)) {
                        listPoint[listPoint.Count - 1].Add (new Point { vec = point });
                    }
                }
            }
            else {
                if (bNewList) {
                    listPoint.Add (new List<Point> () { new Point { vec = point } });
                }
                else {
                    var list = listPoint[listPoint.Count - 1];
                    Point start = list[list.Count - 1];
                    Point end = new Point { vec = point };
                    list.Add (end);

                    bool bFirstLine = list.Count == 2;

                    // Generate line and points
                    var line = new Line (start, end);
                    listLines.Add (line);
                    // line
                    {
                        GameObject go = GameObject.Instantiate (lineTemplate.gameObject, linesRoot.transform);
                        go.GetComponent<LineData> ().line = line;
                        var rt = go.GetComponent<RectTransform> ();
                        rt.anchoredPosition = (start.vec + end.vec) / 2;
                        float width = Mathf.Abs ((end.vec - start.vec).magnitude);
                        rt.sizeDelta = new Vector2 (width, 20);
                        Vector2 vec = (end.vec - start.vec).normalized;

                        // calculate rotation
                        float targetRotation = Mathf.Atan2 (vec.y, vec.x) * Mathf.Rad2Deg;
                        rt.localRotation = Quaternion.Euler (0, 0, targetRotation);

                        go.SetActive (true);
                    }

                    // points
                    {
                        if(bFirstLine) {
                            GameObject go1 = GameObject.Instantiate (pointTemplate.gameObject, pointsRoot.transform);
                            go1.GetComponent<PointData> ().point = start;
                            var rt1 = go1.GetComponent<RectTransform> ();
                            rt1.anchoredPosition = start.vec;

                            go1.SetActive (true);
                        }
                        GameObject go = GameObject.Instantiate (pointTemplate.gameObject, pointsRoot.transform);
                        go.GetComponent<PointData> ().point = end;
                        var rt = go.GetComponent<RectTransform> ();
                        rt.anchoredPosition = end.vec;

                        go.SetActive (true);
                    }
                }
            }
        }

        public void AddLine (IList<Vector2> vertexs) {
            //Lines.Add (Lines.Count, new VertexStream (vertexs, LineColor));
        }

        //public IList<Vector2> GetLine (int id) {
        //return Lines[id].vertexs;
        //}

        public void ReplaceLines (int[] ids, IList<Vector2>[] vertexs) {
            //for (int i = 0; i < ids.Length; i++)
            //Lines[ids[i]] = new VertexStream (vertexs[i], LineColor);
        }

        public void RemoveLine (int[] ids) {
            //foreach (int id in ids)
            //Lines.Remove (id);
        }

        public void ClearLines () {
            //Lines.Clear ();
        }
    }

}