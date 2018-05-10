using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using UnityEngine.EventSystems;

namespace SpringGUI {

    //[RequireComponent (typeof (LineSelectable))]
    public class RoomTestController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {

        public bool canDrag = true;

        public ClipperTest ct;
        private RoomShape rs;

        // Use this for initialization
        void Start () {
            ct = gameObject.FindInParents<ClipperTest> ();

            rs = GetComponent<RoomShape> ();
        }

        // Update is called once per frame
        void Update () {
        }
        
        public void OnDrag (PointerEventData eventData) {
            if (canDrag) {
                this.transform.position += (Vector3)eventData.delta;

                if (rs != null) {
                    rs.CalculateRealPointPositionFromVisual ();
                }
            }
        }

        public void OnBeginDrag (PointerEventData eventData) {
            if(canDrag) {

            }
        }

        public void OnEndDrag (PointerEventData eventData) {
            if (canDrag) {
                var rt = GetComponent<RectTransform> ();
                //Vector2 offset = rt.anchoredPosition;

                //foreach(var pointData in rs.Points) {
                //    pointData.point.vec -= offset;
                //}

                

                ct.Calculate ();
            }
        }
    }
}
