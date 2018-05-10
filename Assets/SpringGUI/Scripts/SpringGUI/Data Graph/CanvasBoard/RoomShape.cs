using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using UnityEngine.EventSystems;

namespace SpringGUI {

    public class RoomShape : MonoBehaviour {
        public List<PointData> Points;
        public UIPolygon2 polygon;

        // Use this for initialization
        void Start () {

        }

        // Update is called once per frame
        void Update () {

        }


        public void CalculateRectTransform () {
            var rt = GetComponent<RectTransform> ();
            float minX = Points[0].point.vec.x;
            float maxX = Points[0].point.vec.x;
            float minY = Points[0].point.vec.y;
            float maxY = Points[0].point.vec.y;

            foreach (var point in Points) {
                if (point.point.vec.x < minX) {
                    minX = point.point.vec.x;
                }
                if (point.point.vec.x > maxX) {
                    maxX = point.point.vec.x;
                }
                if (point.point.vec.y < minY) {
                    minY = point.point.vec.y;
                }
                if (point.point.vec.y > maxY) {
                    maxY = point.point.vec.y;
                }
            }

            rt.anchoredPosition = new Vector2 ((minX + maxX) / 2, (minY + maxY) / 2);
            rt.sizeDelta = new Vector2 (maxX - minX, maxY - minY);

            CalculateVisualPosition ();

            Vector2[] _points = new Vector2[Points.Count];
            for (int i = 0; i < Points.Count; ++i) {
                _points[i] = Points[i].point.vec - rt.anchoredPosition;
            }
            // Update UIPolygon2
            polygon.DrawPolygon (_points);
        }

        public void CalculateVisualPosition () {
            Vector2 offset = Vector2.zero;
            offset = GetComponent<RectTransform> ().anchoredPosition;
            offset = -offset;

            // points
            for (int i = 0; i < Points.Count; ++i) {
                var point = Points[i].point;
                var rt = Points[i].GetComponent<RectTransform> ();
                rt.anchoredPosition = point.vec + offset;
            }
        }

        public void CalculateRealPointPositionFromVisual () {
            var offset = GetComponent<RectTransform> ().anchoredPosition;
            foreach (var point in Points) {
                point.point.vec = point.GetComponent<RectTransform> ().anchoredPosition + offset;
            }
        }
    }


}
