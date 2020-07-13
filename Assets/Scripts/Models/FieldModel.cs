using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FieldModel
{
    [ReadOnly] public int Delay;
    [ReadOnly] public Dictionary<Vector2Int, GameObject> Cubes;
    [ReadOnly] public Vector2Int Size = new Vector2Int(20, 20);
    [ReadOnly] public GameState GameState;
    [ReadOnly] public Figure CurrentFigure;

    public FieldModel(int delay)
    {
        Delay = delay;
        Cubes = new Dictionary<Vector2Int, GameObject>(Size.x);
        GameState = GameState.Play;
    }
}
