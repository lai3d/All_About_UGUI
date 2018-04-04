
/*=========================================
* Author: springDong
* Description: SpringGUI.LineChartGraph example.
==========================================*/

using System.Collections.Generic;
using SpringGUI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class CanvasBoardExample : MonoBehaviour
{
    public CanvasBoard canvasBoard = null;

    public Button eraseAll;
	public Button editLines;
    public Button deleteLines;

	//public RectTransform selectionMask;

	//private SelectionBox selectionBox;

    public void Awake()
    {
		//selectionBox = GetComponent<SelectionBox> ();
    }

    private void Start () {
        eraseAll.onClick.AddListener (delegate {
            canvasBoard.ClearBoard ();
        });

		editLines.onClick.AddListener (delegate {
			canvasBoard.EditLines();
			//selectionBox.enabled = !selectionBox.enabled;
			//selectionMask.gameObject.SetActive(!selectionMask.gameObject.activeInHierarchy);
		});

        deleteLines.onClick.AddListener (delegate {

        });
    }
}