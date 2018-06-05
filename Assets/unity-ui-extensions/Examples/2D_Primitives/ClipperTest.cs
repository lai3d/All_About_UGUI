using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

//using SpringGUI;
using ClipperLib;
using PolygonPath = System.Collections.Generic.List<ClipperLib.IntPoint>;

namespace SpringGUI {
    using listPolygonPath = List<PolygonPath>;

    public class ClipperTest : MonoBehaviour {

        public UIPolygon2 polygonTemplate;
        public RectTransform pointTemplate;

        public ClipperLib.ClipType clipType = ClipType.ctIntersection;

        private listPolygonPath subj = new listPolygonPath ();
        private listPolygonPath clip = new listPolygonPath ();
        private listPolygonPath solution = new listPolygonPath ();
        private Clipper c;

        private List<GameObject> listPolygon = new List<GameObject> ();
        private List<GameObject> listSolutionPolygon = new List<GameObject> ();

        // Use this for initialization
        void Start () {
            //listPolygonPath subj = new listPolygonPath (2);
            //listPolygonPath subj = new listPolygonPath (1);
            subj.Add (new PolygonPath (4));
            subj[0].Add (new IntPoint (-180, 200));
            subj[0].Add (new IntPoint (-260, 200));
            subj[0].Add (new IntPoint (-260, 150));
            subj[0].Add (new IntPoint (-220, 100));
            subj[0].Add (new IntPoint (-180, 150));

            //subj.Add (new Path (3));
            //subj[1].Add (new IntPoint (215, 160));
            //subj[1].Add (new IntPoint (230, 190));
            //subj[1].Add (new IntPoint (200, 190));

            //listPolygonPath clip = new listPolygonPath (1);
            clip.Add (new PolygonPath (4));
            clip[0].Add (new IntPoint (-190, 210));
            clip[0].Add (new IntPoint (-215, 190));
            clip[0].Add (new IntPoint (-240, 210));
            clip[0].Add (new IntPoint (-240, 130));
            clip[0].Add (new IntPoint (-190, 130));

            //DrawPolygons (subj, Color.FromArgb (0x16, 0, 0, 0xFF), Color.FromArgb (0x60, 0, 0, 0xFF));
            DrawPolygons (subj, "0x0000FF9A".hexToColor (), "0x6000009A".hexToColor ());
            //DrawPolygons (clip, Color.FromArgb (0x20, 0xFF, 0xFF, 0), Color.FromArgb (0x30, 0xFF, 0, 0));
            DrawPolygons (clip, "0x00FF009A".hexToColor (), "0x30FF009A".hexToColor ());

            //listPolygonPath solution = new listPolygonPath ();

            c = new Clipper ();
            c.AddPaths (subj, PolyType.ptSubject, true);
            c.AddPaths (clip, PolyType.ptClip, true);
            c.Execute (clipType, solution, PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd);

            Debug.Log ("solution count:" + solution.Count + " points count: " + (solution.Count > 0 ? solution[0].Count.ToString () : "0"));

            //DrawPolygons (solution, Color.FromArgb (0x30, 0, 0xFF, 0), Color.FromArgb (0xFF, 0, 0x66, 0));
            DrawPolygons (solution, "0xFF00009A".hexToColor (), "0xFF00669A".hexToColor (), true);
        }

        void DrawPolygons (listPolygonPath paths, Color oneColor, Color anotherColor, bool isSolution = false) {
            foreach (var path in paths) {
                GameObject go = Instantiate (polygonTemplate.gameObject, transform);
                var polygon = go.GetComponent<UIPolygon2> ();
                var roomShape = go.GetComponent<RoomShape> ();
                var selectable = go.GetComponent<Selectable> ();

                roomShape.polygon = polygon;

                roomShape.Points = new List<PointData> (path.Count);
                for (int i = 0; i < path.Count; ++i) {
                    GameObject goPoint = Instantiate (pointTemplate.gameObject, go.transform);
                    var pointData = goPoint.GetComponent<PointData> ();
                    pointData.point = new Point { vec = new Vector2 (path[i].X, path[i].Y) };
                    goPoint.SetActive (true);

                    roomShape.Points.Add (pointData);
                }
                roomShape.CalculateRectTransform ();

                ColorBlock colorBlock = selectable.colors;
                colorBlock.normalColor = oneColor;
                selectable.colors = colorBlock;

                go.SetActive (true);

                if (isSolution) {
                    listSolutionPolygon.Add (go);
                }
                else {
                    listPolygon.Add (go);
                }
            }
        }

        public void Calculate () {
            Debug.Log ("Calculate");

            c.Clear ();
            subj[0].Clear ();
            clip[0].Clear ();
            solution.Clear ();

            for (int i = listSolutionPolygon.Count - 1; i >= 0; --i) {
                Destroy (listSolutionPolygon[i]);
            }
            listSolutionPolygon.Clear ();

            var roomShapeSubject = listPolygon[0].GetComponent<RoomShape> ();
            foreach (var pointData in roomShapeSubject.Points) {
                subj[0].Add (new IntPoint (pointData.point.vec.x, pointData.point.vec.y));
            }

            var roomShapeClip = listPolygon[1].GetComponent<RoomShape> ();
            foreach (var pointData in roomShapeClip.Points) {
                clip[0].Add (new IntPoint (pointData.point.vec.x, pointData.point.vec.y));
            }

            c.AddPaths (subj, PolyType.ptSubject, true);
            c.AddPaths (clip, PolyType.ptClip, true);
            c.Execute (clipType, solution, PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd);

            Debug.Log ("solution count:" + solution.Count + " points count: " + (solution.Count > 0 ? solution[0].Count.ToString () : "0"));

            DrawPolygons (solution, "0xFF00009A".hexToColor (), "0xFF00669A".hexToColor (), true);
        }

        // Update is called once per frame
        void Update () {

        }
    }

}