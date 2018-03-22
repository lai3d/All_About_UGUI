
/*=========================================
* Author: springDong
* Description: SpringGUI.LineChartGraph example.
==========================================*/

using System.Collections.Generic;
using SpringGUI;
using UnityEngine;

public class RulerExample : MonoBehaviour
{
    public Ruler ruler = null;

    public void Awake()
    {
        ruler.ShowUnit ();
    }
}