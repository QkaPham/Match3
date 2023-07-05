using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public GemType type;
}
public enum GemType
{
    red,
    yellow,
    green,
    blue,
    white
}