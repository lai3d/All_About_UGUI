using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// https://answers.unity.com/questions/785696/global-list-of-colour-names-and-colour-values.html
/// </summary>
public static class HueColor {

    public enum Names {
        Aqua,
        Blue,
        Green,
        Lime,
        Navy,
        Olive,
        Orange,
        Purple,
        Pink,
        Red,
        Yellow
    }

    public static int Count {
        get {
            return hueColorValues.Count;
        }
    }

    private static Hashtable hueColorValues = new Hashtable{
         { Names.Lime,     new Color32( 166 , 254 , 0, 255 ) },
         { Names.Green,     new Color32( 0 , 254 , 111, 255 ) },
         { Names.Aqua,     new Color32( 0 , 201 , 254, 255 ) },
         { Names.Blue,     new Color32( 0 , 122 , 254, 255 ) },
         { Names.Navy,     new Color32( 60 , 0 , 254, 255 ) },
         { Names.Purple, new Color32( 143 , 0 , 254, 255 ) },
         { Names.Pink,     new Color32( 232 , 0 , 254, 255 ) },
         { Names.Red,     new Color32( 254, 9, 0, 255 ) },
         { Names.Olive,  new Color32( 128, 128, 0, 255 ) },
         { Names.Orange, new Color32( 254, 161, 0, 255 ) },
         { Names.Yellow, new Color32( 254, 224, 0, 255 ) },
     };

    public static Color32 HueColorValue (Names color, byte alpha = 255) {
        Color32 result = (Color32)hueColorValues[color];
        result.a = alpha;
        return result;
    }

    /// <summary>
    /// Extension for getting color value easier
    /// </summary>
    /// <param name="color"></param>
    /// <param name="alpha"></param>
    /// <returns></returns>
    /// <example>Color color = HueColor.HueColorNames.Olive.Color();</example>
    public static Color32 Color(this Names color, byte alpha = 255) {
        Color32 result = (Color32)hueColorValues[color];
        result.a = alpha;
        return result;
    }

    public static Color32 Aqua { get { return HueColorValue (Names.Aqua); } }
    public static Color32 Olive { get { return HueColorValue (Names.Olive); } }
}