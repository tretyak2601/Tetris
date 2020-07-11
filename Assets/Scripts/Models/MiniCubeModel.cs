using UnityEngine;
using System;

[Serializable]
public class MiniCubeModel
{
    public Vector2Int Index;
    public Vector2Int CurrentPosition;
    public FigureState State;

    public MiniCubeModel(Vector2Int index, Vector2Int currentPosition)
    {
        Index = index;
        CurrentPosition = currentPosition;
        State = FigureState.Mooving;
    }
}
