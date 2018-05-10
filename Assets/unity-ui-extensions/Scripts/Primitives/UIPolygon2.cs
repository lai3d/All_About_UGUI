/// Credit CiaccoDavide
/// Sourced from - http://ciaccodavi.de/unity/UIPolygon

using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions {
    [AddComponentMenu ("UI/Extensions/Primitives/UI Polygon2")]
    public class UIPolygon2 : UIPrimitiveBase {
        //public bool fill = true;
        public float thickness = 5;
        //[Range(3, 360)]
        //public int sides = 3;
        //[Range(0, 360)]
        //public float rotation = 0;
        //[Range(0, 1)]
        //public float[] VerticesDistances = new float[3];
        private float size = 0;

        public List<Vector2> points = new List<Vector2> ();
        //[HideInInspector]
        [SerializeField]
        private Vector2[] pointsReal = new Vector2[0];
        private bool isInitialState = true;
        [SerializeField]
        private float TooNearEpsilon = 0.0001f;

        public RaycastMode raycastMode = RaycastMode.ReceiveAll;

        public enum RaycastMode {
            ReceiveNone,        // You can't interact with us or any of our children
            ReceiveAll,         // You can interact with us and all of our children
            OnlyMyself,         // You can interact only with us
            OnlyChildsReceive   // You can't interact with us, but you can with any of our children
        }

        protected override void Start () {
            var rt = GetComponent<RectTransform> ();
            TooNearEpsilon = rt.rect.width * 0.0001f;
        }

        public void DrawPolygon (Vector2[] _points) {
            isInitialState = false;
            List<Vector2> tmpList = new List<Vector2> ();

            if (!isTooNear (_points[0], _points[_points.Length - 1])) {
                tmpList.Add (_points[0]);
            }
            for (int i = 1; i < _points.Length; ++i) {
                if (!isTooNear (_points[i - 1], _points[i])) {
                    tmpList.Add (_points[i]);
                }
            }
            pointsReal = tmpList.ToArray ();
            SetAllDirty ();
        }

        bool isTooNear (Vector2 p1, Vector2 p2) {
            if (Vector2.Distance (p1, p2) < TooNearEpsilon)
                return true;
            else
                return false;
        }

        void Update () {
            size = rectTransform.rect.width;
            if (rectTransform.rect.width > rectTransform.rect.height)
                size = rectTransform.rect.height;
            else
                size = rectTransform.rect.width;
            thickness = (float)Mathf.Clamp (thickness, 0, size / 2);
        }

        protected override void OnPopulateMesh (VertexHelper vh) {
            vh.Clear ();

            if (isInitialState) {
                Array.Resize (ref pointsReal, points.Count);

                for (int i = 0; i < pointsReal.Length; ++i) {
                    pointsReal[i] = new Vector2 (points[i].x * rectTransform.rect.width, points[i].y * rectTransform.rect.height);
                }
            }
            var triangulator = new Triangulator (pointsReal);

            List<UIVertex> verts = new List<UIVertex> ();
            List<int> indices = new List<int> ();

            foreach (var point in pointsReal) {
                var vert = UIVertex.simpleVert;
                vert.color = color;
                vert.position = point;

                verts.Add (vert);
            }

            indices.AddRange (triangulator.Triangulate ());

            vh.AddUIVertexStream (verts, indices);
        }

        #region ICanvasRaycastFilter Interface
        public override bool IsRaycastLocationValid (Vector2 screenPoint, Camera eventCamera) {
            switch (raycastMode) {
            case RaycastMode.ReceiveNone:
                return false;
            case RaycastMode.ReceiveAll: {
                    Vector2 local;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle (rectTransform, screenPoint, eventCamera, out local);

                    if (ContainsPoint (pointsReal, local))
                        return true;

                    for (int i = 0; i < transform.childCount; i++) {
                        RectTransform childRect = transform.GetChild (i) as RectTransform;
                        Vector2 childPos = childRect.worldToLocalMatrix.MultiplyPoint (screenPoint);
                        if (childRect.rect.Contains (childPos)) {
                            return true;
                        }
                    }

                    return false;
                }
            case RaycastMode.OnlyMyself: {
                    Vector2 local;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle (rectTransform, screenPoint, eventCamera, out local);

                    if (ContainsPoint (pointsReal, local))
                        return true;

                    return false;
                }
            case RaycastMode.OnlyChildsReceive:
                for (int i = 0; i < transform.childCount; i++) {
                    RectTransform childRect = transform.GetChild (i) as RectTransform;
                    Vector2 childPos = childRect.worldToLocalMatrix.MultiplyPoint (screenPoint);
                    if (childRect.rect.Contains (childPos)) {
                        return true;
                    }
                }
                return false;
            default:
                throw new System.NotImplementedException ("Mode not implemented");
            }
        }

        /// <summary>
        /// http://wiki.unity3d.com/index.php/PolyContainsPoint
        /// </summary>
        /// <param name="polyPoints"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool ContainsPoint (Vector2[] polyPoints, Vector2 p) {
            var j = polyPoints.Length - 1;
            var inside = false;
            for (int i = 0; i < polyPoints.Length; j = i++) {
                if (((polyPoints[i].y <= p.y && p.y < polyPoints[j].y) || (polyPoints[j].y <= p.y && p.y < polyPoints[i].y)) &&
                   (p.x < (polyPoints[j].x - polyPoints[i].x) * (p.y - polyPoints[i].y) / (polyPoints[j].y - polyPoints[i].y) + polyPoints[i].x))
                    inside = !inside;
            }
            return inside;
        }
        #endregion
    }
}