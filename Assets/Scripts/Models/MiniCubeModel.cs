using UnityEngine;
using System;

[Serializable]
public class MiniCubeModel
{
    [ReadOnly] public Vector2Int Index;
    [ReadOnly] public Vector2Int CurrentPosition;

    public MiniCubeModel(Vector2Int index, Vector2Int currentPosition)
    {
        Index = index;
        CurrentPosition = currentPosition;
    }
}
