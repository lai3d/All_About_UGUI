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

#if Polygon_1
            Vector2 prevX = Vector2.zero;
            Vector2 prevY = Vector2.zero;
            Vector2 uv0 = new Vector2(0, 0);
            Vector2 uv1 = new Vector2(0, 1);
            Vector2 uv2 = new Vector2(1, 1);
            Vector2 uv3 = new Vector2(1, 0);
            Vector2 pos0;
            Vector2 pos1;
            Vector2 pos2;
            Vector2 pos3;
            float degrees = 360f / sides;
            int vertices = sides + 1;
            if (VerticesDistances.Length != vertices)
            {
                VerticesDistances = new float[vertices];
                for (int i = 0; i < vertices - 1; i++) VerticesDistances[i] = 1;
            }
            // last vertex is also the first!
            VerticesDistances[vertices - 1] = VerticesDistances[0];
            for (int i = 0; i < vertices; i++)
            {
                float outer = -rectTransform.pivot.x * size * VerticesDistances[i];
                float inner = -rectTransform.pivot.x * size * VerticesDistances[i] + thickness;
                float rad = Mathf.Deg2Rad * (i * degrees + rotation);
                float c = Mathf.Cos(rad);
                float s = Mathf.Sin(rad);
                uv0 = new Vector2(0, 1);
                uv1 = new Vector2(1, 1);
                uv2 = new Vector2(1, 0);
                uv3 = new Vector2(0, 0);
                pos0 = prevX;
                pos1 = new Vector2(outer * c, outer * s);
                if (fill)
                {
                    pos2 = Vector2.zero;
                    pos3 = Vector2.zero;
                }
                else
                {
                    pos2 = new Vector2(inner * c, inner * s);
                    pos3 = prevY;
                }
                prevX = pos1;
                prevY = pos2;
                vh.AddUIVertexQuad(SetVbo(new[] { pos0, pos1, pos2, pos3 }, new[] { uv0, uv1, uv2, uv3 }));
            }
#else
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
#endif
        }
    }
}