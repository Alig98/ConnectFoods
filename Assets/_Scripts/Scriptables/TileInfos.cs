using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TileInfos : ScriptableObject
{
    public List<Sprite> TileSprites;
    public float ShuffleTime;
    public float ShuffleRotationTime;
    public float ShuffleMoveTime;
    public float FallTime;
}
