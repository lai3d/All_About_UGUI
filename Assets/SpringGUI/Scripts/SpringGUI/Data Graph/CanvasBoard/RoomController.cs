using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using UnityEngine.EventSystems;

namespace SpringGUI {

    //[RequireComponent (typeof (LineSelectable))]
    public class RoomController : MonoBehaviour {

        //public float moveSpeed = 15.0f;

        //private LineSelectable lineSelectable;
        private RoomData roomData;
        private CanvasBoard canvasBoard;
        
        // Use this for initialization
        void Start () {
            //lineSelectable = GetComponent<LineSelectable> ();
            roomData = GetComponent<RoomData> ();
            canvasBoard = this.transform.parent.parent.GetComponent<CanvasBoard> ();
            if(canvasBoard == null) {
                Debug.LogError ("Get component CanvasBoard failed!");
            }
        }

        // Update is called once per frame
        void Update () {
            if (canvasBoard.canvasBoardBasis.drawingState != DrawingState.Edit)
                return;
            if (EventSystem.current.currentSelectedGameObject != this.gameObject)
                return;
            // Delete
            if (Input.GetKeyUp (KeyCode.Delete)) {
                canvasBoard.DeleteRoom (roomData.room);

                Destroy (gameObject);
            }
        }
        
    }
}
