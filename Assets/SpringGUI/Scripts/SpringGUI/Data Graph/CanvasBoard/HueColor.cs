using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// https://answers.unity.com/questions/785696/global-list-of-colour-names-and-colour-values.html
/// </summary>
public class HueColor {

    public enum HueColorNames {
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
         { HueColorNames.Lime,     new Color32( 166 , 254 , 0, 255 ) },
         { HueColorNames.Green,     new Color32( 0 , 254 , 111, 255 ) },
         { HueColorNames.Aqua,     new Color32( 0 , 201 , 254, 255 ) },
         { HueColorNames.Blue,     new Color32( 0 , 122 , 254, 255 ) },
         { HueColorNames.Navy,     new Color32( 60 , 0 , 254, 255 ) },
         { HueColorNames.Purple, new Color32( 143 , 0 , 254, 255 ) },
         { HueColorNames.Pink,     new Color32( 232 , 0 , 254, 255 ) },
         { HueColorNames.Red,     new Color32( 254, 9, 0, 255 ) },
         { HueColorNames.Olive,  new Color32( 128, 128, 0, 255 ) },
         { HueColorNames.Orange, new Color32( 254, 161, 0, 255 ) },
         { HueColorNames.Yellow, new Color32( 254, 224, 0, 255 ) },
     };

    public static Color32 HueColorValue (HueColorNames color, byte alpha = 255) {
        Color32 result = (Color32)hueColorValues[color];
        result.a = alpha;
        return result;
    }
}