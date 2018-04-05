using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpringGUI {

    public class Room /*: IEquatable<Room>*/ {
        public List<Point> points;
        public List<Line> lines;

        //public Room () {
        //}

        //public bool Equals (Room other) {
        //    if (other == null)
        //        return false;
        //    if (this.start == other.start && this.end == other.end)
        //        return true;
        //    else
        //        return false;
        //}
    }

    //[Serializable]
    public class Line : IEquatable<Line> {
        public Point start;
        public Point end;

        public Line (Point start, Point end) {
            this.start = start;
            this.end = end;

            this.start.lines.Add (this);
            this.end.lines.Add (this);
        }

        public bool Equals (Line other) {
            if (other == null)
                return false;
            if (this.start == other.start && this.end == other.end)
                return true;
            else
                return false;
        }
    }

    //[Serializable]
    public class Point : IEquatable<Point> {
        public Vector2 vec;
        //[HideInInspector]
        public List<Line> lines = new List<Line> ();

        public bool Equals (Point other) {
            if (other == null)
                return false;
            if (this.vec == other.vec)
                return true;
            else
                return false;
        }
    }
}