using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SpringGUI {
    public class CanvasBoard : MaskableGraphic, IPointerClickHandler {
        [SerializeField]
        public CanvasBoardData canvasBoardBasis = null;

        [SerializeField]
        private List<Vector2> listPoints = new List<Vector2>();

        private ICanvasBoard CanvasBoardCreator {
            get {
                return new CanvasBoard1 ();
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

        public void Inject (IList<Vector2> vertexs) {
            canvasBoardBasis.AddLine (vertexs);
        }

        private void Update () {
            
        }

        public void OnPointerClick (PointerEventData eventData) {
            switch(eventData.button) {
            case PointerEventData.InputButton.Left: {
                    Vector2 pos = eventData.position;
                    Debug.Log (pos.ToString ());
                    Vector2 localPoint;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle (rectTransform, pos, eventData.pressEventCamera, out localPoint);
                    Debug.Log ("localPoint: " + localPoint.ToString ());
                    listPoints.Add (localPoint);
                    if(listPoints.Count > 0 && listPoints.Count % 2 == 0) {
                        Inject (listPoints);
                        UpdateGeometry ();
                        listPoints = new List<Vector2> ();
                    }
                }
                break;
            case PointerEventData.InputButton.Right: {

                }
                break;
            }
        }
    }

    public interface ICanvasBoard {
        VertexHelper DrawCanvasBoard (VertexHelper vh, Rect rect, CanvasBoardData basis);
        VertexHelper DrawMesh (VertexHelper vh);
        VertexHelper DrawAxis (VertexHelper vh);
    }

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
        [HideInInspector]
        public Dictionary<int, VertexStream> Lines = new Dictionary<int, VertexStream> ();

        public void AddLine (IList<Vector2> vertexs) {
            Lines.Add (Lines.Count, new VertexStream (vertexs, LineColor));
        }

        public IList<Vector2> GetLine (int id) {
            return Lines[id].vertexs;
        }

        public void ReplaceLines (int[] ids, IList<Vector2>[] vertexs) {
            for (int i = 0; i < ids.Length; i++)
                Lines[ids[i]] = new VertexStream (vertexs[i], LineColor);
        }

        public void RemoveLine (int[] ids) {
            foreach (int id in ids)
                Lines.Remove (id);
        }

        public void ClearLines () {
            Lines.Clear ();
        }
    }

    public class BaseCanvasBoard : ICanvasBoard {
        protected Rect rect;
        protected Vector2 size;
        protected Vector2 origin;
        protected CanvasBoardData basis;
        protected Dictionary<int, VertexStream> lines;

        public virtual VertexHelper DrawCanvasBoard (VertexHelper vh, Rect rect, CanvasBoardData basis) {
            this.basis = basis;
            lines = basis.Lines;
            this.rect = rect;
            size = rect.size;
            //origin = new Vector2 (-size.x / 2.0f, -size.y / 2.0f); // bottom left
            origin = new Vector2 (-size.x / 2.0f, size.y / 2.0f); // top left
            vh = DrawMesh (vh);
            vh = DrawAxis (vh);
            return vh;
        }

        public virtual VertexHelper DrawAxis (VertexHelper vh) {
            if (!basis.IsDrawAxis)
                return vh;
            Vector2 startPosX = origin + new Vector2 (-basis.AxisWidth / 2.0f, 0);
            Vector2 endPosX = startPosX + new Vector2 (size.x + basis.AxisWidth / 2.0f, 0);
            Vector2 startPosY = origin + new Vector2 (0, -basis.AxisWidth / 2.0f);
            Vector2 endPosY = startPosY - new Vector2 (0, size.y + basis.AxisWidth / 2.0f);
            vh.AddUIVertexQuad (GetQuad (startPosX, endPosX, basis.AxisColor, basis.AxisWidth));
            vh.AddUIVertexQuad (GetQuad (startPosY, endPosY, basis.AxisColor, basis.AxisWidth));
            if (basis.ShowArrow) {
                var xFirst = endPosX + new Vector2 (0, basis.AxisWidth);
                var xSecond = endPosX + new Vector2 (1.73f * basis.AxisWidth, 0);
                var xThird = endPosX + new Vector2 (0, -basis.AxisWidth);
                vh.AddUIVertexQuad (new UIVertex[]
                {
                    GetUIVertex(xFirst,basis.AxisColor),
                    GetUIVertex(xSecond,basis.AxisColor),
                    GetUIVertex(xThird,basis.AxisColor),
                    GetUIVertex(endPosX,basis.AxisColor),
                });

                var yFirst = endPosY + new Vector2 (-basis.AxisWidth, 0);
                var ySecond = endPosY + new Vector2 (0, 1.73f * basis.AxisWidth);
                var yThird = endPosY + new Vector2 (basis.AxisWidth, 0);
                vh.AddUIVertexQuad (new UIVertex[]
                {
                    GetUIVertex(yFirst,basis.AxisColor),
                    GetUIVertex(ySecond,basis.AxisColor),
                    GetUIVertex(yThird,basis.AxisColor),
                    GetUIVertex(endPosY,basis.AxisColor),
                });
            }
            // draw scale
            for (float x = basis.MeshCellSize.x; x <= size.x; x += basis.MeshCellSize.x) {
                Vector2 startPoint = origin + new Vector2 (x, 0);
                Vector2 endPoint = startPoint + new Vector2 (0, basis.AxisScaleHeight);
                vh.AddUIVertexQuad (GetQuad (startPoint, endPoint, basis.AxisScaleColor, basis.AxisScaleWidth));
            }

            for (float y = -basis.MeshCellSize.y; y >= -size.y; y -= basis.MeshCellSize.y) {
                Vector2 startPoint = origin + new Vector2 (0, y);
                Vector2 endPoint = startPoint - new Vector2 (basis.AxisScaleHeight, 0);
                vh.AddUIVertexQuad (GetQuad (startPoint, endPoint, basis.AxisScaleColor, basis.AxisScaleWidth));
            }
            return vh;
        }

        public virtual VertexHelper DrawMesh (VertexHelper vh) {
            if (!basis.IsDrawMeshX && !basis.IsDrawMeshY)
                return vh;
            if (basis.IsDrawMeshX) {
                if (!basis.IsImaginaryLine) {
                    for (float y = 0; y >= -size.y; y -= basis.MeshCellSize.y) {
                        Vector2 startPoint = origin + new Vector2 (0, y);
                        Vector2 endPoint = startPoint + new Vector2 (size.x, 0);
                        vh.AddUIVertexQuad (GetQuad (startPoint, endPoint, basis.MeshColor, basis.MeshWidth));
                    }
                }
                else {
                    for (float y = 0; y >= -size.y; y -= basis.MeshCellSize.y) {
                        Vector2 startPoint = origin + new Vector2 (0, y);
                        Vector2 endPoint = startPoint + new Vector2 (8, 0);
                        for (float x = 0; x < size.x; x += (8 + 2)) {
                            vh.AddUIVertexQuad (GetQuad (startPoint, endPoint, basis.MeshColor, basis.MeshWidth));
                            startPoint = startPoint + new Vector2 (10, 0);
                            endPoint = startPoint + new Vector2 (8, 0);
                            if (endPoint.x > size.x / 2.0f)
                                endPoint = new Vector2 (size.x / 2.0f, endPoint.y);
                        }
                    }
                }
            }
            if (basis.IsDrawMeshY) {
                if (!basis.IsImaginaryLine) {
                    for (float x = 0; x <= size.x; x += basis.MeshCellSize.x) {
                        Vector2 startPoint = origin + new Vector2 (x, 0);
                        Vector2 endPoint = startPoint - new Vector2 (0, size.y);
                        vh.AddUIVertexQuad (GetQuad (startPoint, endPoint, basis.MeshColor, basis.MeshWidth));
                    }
                }
                else {
                    for (float x = 0; x <= size.x; x += basis.MeshCellSize.x) {
                        Vector2 startPoint = origin + new Vector2 (x, 0);
                        Vector2 endPoint = startPoint - new Vector2 (0, 8);
                        for (float y = 0; y > -size.y; y -= (8 + 2)) {
                            vh.AddUIVertexQuad (GetQuad (startPoint, endPoint, basis.MeshColor, basis.MeshWidth));
                            startPoint = startPoint - new Vector2 (0, 10);
                            endPoint = startPoint - new Vector2 (0, 8);
                            if (endPoint.y < -size.y / 2.0f)
                                endPoint = new Vector2 (endPoint.x, -size.y / 2.0f);
                        }
                    }
                }
            }
            return vh;
        }

        /// <summary>
        /// draw line by two points
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <param name="color0"></param>
        /// <param name="LineWidth"></param>
        /// <returns></returns>
        protected UIVertex[] GetQuad (Vector2 startPos, Vector2 endPos, Color color0, float LineWidth = 2.0f) {
            float dis = Vector2.Distance (startPos, endPos);
            float y = LineWidth * 0.5f * (endPos.x - startPos.x) / dis;
            float x = LineWidth * 0.5f * (endPos.y - startPos.y) / dis;
            if (y <= 0)
                y = -y;
            else
                x = -x;
            UIVertex[] vertex = new UIVertex[4];
            vertex[0].position = new Vector3 (startPos.x + x, startPos.y + y);
            vertex[1].position = new Vector3 (endPos.x + x, endPos.y + y);
            vertex[2].position = new Vector3 (endPos.x - x, endPos.y - y);
            vertex[3].position = new Vector3 (startPos.x - x, startPos.y - y);
            for (int i = 0; i < vertex.Length; i++)
                vertex[i].color = color0;
            return vertex;
        }

        /// <summary>
        /// get uivertex
        /// </summary>
        /// <param name="point"></param>
        /// <param name="color0"></param>
        /// <returns></returns>
        protected UIVertex GetUIVertex (Vector2 point, Color color0) {
            UIVertex vertex = new UIVertex {
                position = point,
                color = color0,
            };
            return vertex;
        }

        protected Vector2 GetPos (Vector2 pos) {
            pos += new Vector2 (-0.5f, -0.5f);
            return new Vector2 (pos.x * size.x, pos.y * size.y);
        }
    }

    public class CanvasBoard1 : BaseCanvasBoard {
        public override VertexHelper DrawCanvasBoard (VertexHelper vh, Rect vRect, CanvasBoardData VBasis) {
            {
                vh = base.DrawCanvasBoard (vh, vRect, VBasis);
                foreach (KeyValuePair<int, VertexStream> line in lines) {
                    if (line.Value.vertexs.Count <= 1)
                        continue;
                    //var startPos = GetPos (line.Value.vertexs[0]);
                    var startPos = line.Value.vertexs[0];
                    UIVertex[] oldVertexs = new UIVertex[] { };
                    for (int i = 1; i < line.Value.vertexs.Count; i++) {
                        //var endPos = GetPos (line.Value.vertexs[i]);
                        var endPos = line.Value.vertexs[i];
                        var newVertexs = GetQuad (startPos, endPos, line.Value.color);
                        if (oldVertexs.Length.Equals (0)) {
                            oldVertexs = newVertexs;
                        }
                        else {
                            vh.AddUIVertexQuad (new UIVertex[]
                            {
                            oldVertexs[1],
                            newVertexs[1],
                            oldVertexs[2],
                            newVertexs[0]
                            });
                            vh.AddUIVertexQuad (new UIVertex[]
                            {
                            newVertexs[0],
                            oldVertexs[1],
                            newVertexs[3],
                            oldVertexs[2]
                            });
                            oldVertexs = newVertexs;
                        }
                        vh.AddUIVertexQuad (newVertexs);
                        startPos = endPos;
                    }
                }
                return vh;
            }
        }
    }
}
