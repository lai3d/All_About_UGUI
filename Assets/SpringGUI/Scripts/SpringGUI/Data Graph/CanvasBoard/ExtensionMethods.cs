using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods {

    static public T FindInParents<T> (this GameObject go) where T : Component {
        if (go == null) return null;
        var comp = go.GetComponent<T> ();

        if (comp != null)
            return comp;

        var t = go.transform.parent;
        while (t != null && comp == null) {
            comp = t.gameObject.GetComponent<T> ();
            t = t.parent;
        }
        return comp;
    }

    // https://answers.unity.com/questions/812240/convert-hex-int-to-colorcolor32.html
    // Note that Color32 and Color implictly convert to each other. You may pass a Color object to this method without first casting it.
    public static string colorToHex (this Color32 color) {
        string hex = color.r.ToString ("X2") + color.g.ToString ("X2") + color.b.ToString ("X2");
        return hex;
    }

    public static Color hexToColor (this string hex) {
        hex = hex.Replace ("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace ("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse (hex.Substring (0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse (hex.Substring (2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse (hex.Substring (4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8) {
            a = byte.Parse (hex.Substring (6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32 (r, g, b, a);
    }
}
