using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GemsProfile
{
    public static Dictionary<GemType, Color> gemsDataDic = new Dictionary<GemType, Color>
    {
        {GemType.red, new Color(221 / 255f, 50 / 255f, 24 / 255f, 1)},
        {GemType.yellow, new Color(236 / 255f, 199 / 255f, 39 / 255f, 1)  },
        {GemType.green, new Color(39 / 255f, 236 / 255f, 107 / 255f, 1)  },
        {GemType.blue, new Color(40 / 255f, 143 / 255f, 215 / 255f, 1) },
        {GemType.white, new Color(213 / 255f, 213 / 255f, 213 / 255f, 1)  },
    };
    public static List<GemType> Gems => gemsDataDic.Keys.ToList();
}

public enum GemType
{
    red = 0,
    yellow,
    green,
    blue,
    white
}