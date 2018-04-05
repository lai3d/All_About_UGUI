using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace SpringGUI {

    public class CanvasBoard1 : BaseCanvasBoard {

        private List<Line> listLines;

        public override VertexHelper DrawCanvasBoard (VertexHelper vh, Rect vRect, CanvasBoardData VBasis) {
            vh = base.DrawCanvasBoard (vh, vRect, VBasis);
            listLines = VBasis.listLines;
            vh = DrawPointLines (vh, vRect, VBasis);
            return vh;
        }

        public VertexHelper DrawPointLines (VertexHelper vh, Rect vRect, CanvasBoardData VBasis) {
            Debug.Log ("DrawPointLines");

            //listLines.Clear ();
            //foreach (var list in VBasis.listPoint) {
            //    foreach(var point in list) {
            //        point.lines.Clear ();
            //    }
            //}

            //foreach (var list in VBasis.listPoint) {
            //    for (int i = 1; i < list.Count; ++i) {
            //        listLines.Add (new Line (list[i - 1], list[i]));
            //    }
            //}

            //Debug.Log ("ListLines.Count " + listLines.Count);

            //foreach (var line in listLines) {
            //    vh.AddUIVertexQuad (GetQuad (line.start.vec, line.end.vec, VBasis.LineColor, VBasis.LineWidth));
            //}

            if (VBasis.isTracingMouse) {
                //var lastList = VBasis.listPoint[VBasis.listPoint.Count - 1];
                vh.AddUIVertexQuad (GetQuad (VBasis.mouseTracingStartPoint.vec, new Point { vec = VBasis.mouseLocalPoint }.vec, VBasis.LineColor, VBasis.LineWidth));
            }

            return vh;
        }

        public VertexHelper DrawLines (VertexHelper vh) {
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