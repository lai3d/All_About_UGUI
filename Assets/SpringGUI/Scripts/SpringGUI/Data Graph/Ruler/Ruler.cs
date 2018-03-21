﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI {
    public class Ruler : MaskableGraphic {
        [SerializeField]
        public RulerData rulerBasis = null;

        private IRuler RulerCreator {
            get {
                return new Ruler1 ();
            }
        }

        protected override void OnPopulateMesh (VertexHelper vh) {
            vh.Clear ();
            //if (rulerBasis.Lines.Count.Equals (0))
            //    return;
            var rect = base.GetPixelAdjustedRect ();
            RulerCreator.DrawRuler (vh, rect, rulerBasis);
        }

        public void Refresh () {
            OnEnable ();
        }
    }

    public interface IRuler {
        VertexHelper DrawRuler (VertexHelper vh, Rect rect, RulerData basis);
        VertexHelper DrawMesh (VertexHelper vh);
        VertexHelper DrawAxis (VertexHelper vh);
    }

    [Serializable]
    public class RulerData {
        [Header ("LineChart Axis Setting")]
        public bool IsDrawAxis = true;
        public float AxisWidth = 2.0f;
        public Color AxisColor = Color.white;
        public bool ShowArrow = false;
    }

    public class BaseRuler : IRuler {
        protected Rect rect;
        protected Vector2 size;
        protected Vector2 origin;
        protected RulerData basis;
        protected Dictionary<int, VertexStream> lines;

        public VertexHelper DrawRuler (VertexHelper vh, Rect rect, RulerData basis) {
            this.basis = basis;
            //lines = basis.Lines;
            this.rect = rect;
            size = rect.size;
            //origin = new Vector2 (-size.x / 2.0f, -size.y / 2.0f);
            origin = new Vector2 (-size.x / 2.0f, size.y / 2.0f);
            //vh = DrawMesh (vh);
            vh = DrawAxis (vh);
            return vh;
        }

        public VertexHelper DrawAxis (VertexHelper vh) {
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
            return vh;
        }

        public VertexHelper DrawMesh (VertexHelper vh) {
            throw new NotImplementedException ();
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

    public class Ruler1 : BaseRuler {

    }
}
