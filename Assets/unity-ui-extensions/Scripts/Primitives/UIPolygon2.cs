/// Credit CiaccoDavide
/// Sourced from - http://ciaccodavi.de/unity/UIPolygon

using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
    [AddComponentMenu("UI/Extensions/Primitives/UI Polygon2")]
    public class UIPolygon2 : UIPrimitiveBase
    {
        public bool fill = true;
        public float thickness = 5;
        [Range(3, 360)]
        public int sides = 3;
        [Range(0, 360)]
        public float rotation = 0;
        [Range(0, 1)]
        public float[] VerticesDistances = new float[3];
        private float size = 0;

		public List<Vector2> points = new List<Vector2>();
        private List<Vector2> pointsReal = new List<Vector2> ();

        public void DrawPolygon(int _sides)
        {
            sides = _sides;
            VerticesDistances = new float[_sides + 1];
            for (int i = 0; i < _sides; i++) VerticesDistances[i] = 1; ;
            rotation = 0;
            SetAllDirty();
        }
        public void DrawPolygon(int _sides, float[] _VerticesDistances)
        {
            sides = _sides;
            VerticesDistances = _VerticesDistances;
            rotation = 0;
            SetAllDirty();
        }
        public void DrawPolygon(int _sides, float[] _VerticesDistances, float _rotation)
        {
            sides = _sides;
            VerticesDistances = _VerticesDistances;
            rotation = _rotation;
            SetAllDirty();
        }
        void Update()
        {
            size = rectTransform.rect.width;
            if (rectTransform.rect.width > rectTransform.rect.height)
                size = rectTransform.rect.height;
            else
                size = rectTransform.rect.width;
            thickness = (float)Mathf.Clamp(thickness, 0, size / 2);
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            
            pointsReal.Clear ();

            foreach(var point in points) {
                pointsReal.Add (new Vector2 (point.x * rectTransform.rect.width, point.y * rectTransform.rect.height));    
            }
            var triangulator = new Triangulator (pointsReal.ToArray ());

            List<UIVertex> verts = new List<UIVertex> ();
            List<int> indices = new List<int> ();

            foreach(var point in pointsReal) {
                var vert = UIVertex.simpleVert;
                vert.color = color;
                vert.position = point;

                verts.Add (vert);
            }

            indices.AddRange(triangulator.Triangulate());
            
            vh.AddUIVertexStream (verts, indices);
        }

        #region ICanvasRaycastFilter Interface
        public override bool IsRaycastLocationValid (Vector2 screenPoint, Camera eventCamera) {
            Vector2 local;
            RectTransformUtility.ScreenPointToLocalPointInRectangle (rectTransform, screenPoint, eventCamera, out local);

            if (ContainsPoint (pointsReal.ToArray (), local))
                return true;
            return false;
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
              if (((polyPoints[i].y <= p.y && p.y<polyPoints[j].y) || (polyPoints[j].y <= p.y && p.y<polyPoints[i].y)) && 
                 (p.x<(polyPoints[j].x - polyPoints[i].x) * (p.y - polyPoints[i].y) / (polyPoints[j].y - polyPoints[i].y) + polyPoints[i].x)) 
                 inside = !inside; 
           } 
           return inside; 
        }
        #endregion
    }
}