using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Figure : MonoBehaviour
{
    [SerializeField] MiniCube[] _miniCubes;

    public MiniCube[] Minis { get { return _miniCubes; } }
    public FigureState State
    {
        private get
        {
            return _state;
        }
        set
        {
            switch (value)
            {
                case FigureState.Mooving:
                    break;
                case FigureState.Idle:
                    break;
            }

            OnStateChanged(value, this);
            _state = value;
        }
    }

    public event Action OnMoved;
    public event Action<FigureState, Figure> OnStateChanged;

    private Field _field;
    private FigureState _state = FigureState.Mooving;

    public void SetPosition(Vector2 position, Field field)
    {
        _field = field;

        foreach (var mini in _miniCubes)
        {
            mini.transform.position = new Vector3(position.x + mini.GetIndex().x, position.y + mini.GetIndex().y, 0);
            mini.Init(new Vector2Int((int)mini.transform.position.x, (int)mini.transform.position.y));

            try
            {
                if (_field.Cubes[mini.GetPosition()] != null)
                {
                    _field.GameState = GameState.GameOver;
                    break;
                }
            }
            catch (KeyNotFoundException k)
            {

            }
        }

        if (_field.GameState == GameState.Play)
            MoveY();
    }

    public void MoveX(Direction direction)
    {
        if (_field.GameState == GameState.GameOver)
            return;

            Vector2Int dir = direction == Direction.Left ? Vector2Int.left : Vector2Int.right;

        bool allowMove = true;

        foreach(var mini in _miniCubes)
        {
            if ((mini.GetPosition() + dir).x < -_field.Size.x / 2 || (mini.GetPosition() + dir).x > _field.Size.x / 2 - 1)
                allowMove = false;

            try
            {
                if (_field.Cubes[mini.GetPosition() + dir] != null)
                    allowMove = false;
            }
            catch (Exception)
            {
                allowMove = false;
            }
        }

        if (allowMove)
        {
            foreach (var mini in _miniCubes)
            {
                mini.transform.position += new Vector3(dir.x, dir.y, 0);
                mini.Init(new Vector2Int((int)mini.transform.position.x, (int)mini.transform.position.y));
            }
        }
    }

    private async void MoveY()
    {
        while (_state == FigureState.Mooving)
        {
            await Task.Delay(_field.Delay * 100);

            try
            {
                foreach (var mini in _miniCubes)
                {
                    if (_field.Cubes[new Vector2Int(mini.GetPosition().x, mini.GetPosition().y - 1)] != null)
                    {
                        State = FigureState.Idle;
                        break;
                    }
                }
            }
            catch (KeyNotFoundException k)
            {

            }

            if (_state == FigureState.Idle)
                break;

            foreach (var mini in _miniCubes)
            {
                mini.transform.position += Vector3.down;
                mini.Init(new Vector2Int((int)mini.transform.position.x, (int)mini.transform.position.y));
            }
        }

    }
}


public enum FigureState
{
    Mooving,
    Idle
}
