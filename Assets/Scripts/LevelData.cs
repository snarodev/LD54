using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Level0.asset",menuName = "Level Data",order =0)]
public class LevelData : ScriptableObject
{
    public LevelObjectData[] tiles;
}
