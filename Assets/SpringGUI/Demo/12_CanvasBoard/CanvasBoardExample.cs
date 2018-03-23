
/*=========================================
* Author: springDong
* Description: SpringGUI.LineChartGraph example.
==========================================*/

using System.Collections.Generic;
using SpringGUI;
using UnityEngine;
using UnityEngine.UI;

public class CanvasBoardExample : MonoBehaviour
{
    public CanvasBoard canvasBoard = null;
    public Button eraseAll;

    public void Awake()
    {
        
    }

    private void Start () {
        eraseAll.onClick.AddListener (delegate {
            canvasBoard.ClearBoard ();
        });
    }
}