using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArrowType
{
    Default,
    Fire,
    Multiple
}
[CreateAssetMenu(fileName = "ArrowData", menuName = "Data/ArrowData")]
public class ArrowData : ScriptableObject
{
    public ArrowType arrowType = ArrowType.Default;
    public Sprite icon;
}
