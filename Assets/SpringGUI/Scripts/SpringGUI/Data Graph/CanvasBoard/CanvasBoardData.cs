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
        public bool isTracingMouse = false;
        public bool isEditingLines = false;

        public RectTransform lineTemplate;
        public RectTransform pointTemplate;

        public RectTransform linesRoot;
        public RectTransform pointsRoot;

        public void AddPoint (Vector2 point, bool bNewList) {
            if (bNewList) {
                listPoint.Add (new List<Point> () { new Point { vec = point } });
            }
            else {
                listPoint[listPoint.Count - 1].Add (new Point { vec = point });
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