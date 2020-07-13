using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Figure : MonoBehaviour
{
    [SerializeField] MiniCube[] _miniCubes;

    public event Action<FigureState, Figure> OnStateChanged;
    public MiniCube[] MiniCubes { get { return _miniCubes; } }
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

    private Field _field;
    private FigureState _state = FigureState.Mooving;

    public void SetPosition(Vector2 position, Field field)
    {
        _field = field;

        foreach (var mini in _miniCubes)
        {
            mini.Init(position);

            try
            {
                if (_field.GetCubes()[mini.GetPosition()] != null)
                {
                    _field.SetState(GameState.GameOver);
                    break;
                }
            }
            catch (KeyNotFoundException k)
            {

            }
        }

        if (_field.GetState() == GameState.Play)
            MoveY();
    }

    public void MoveX(Direction direction)
    {
        if (_field.GetState() == GameState.GameOver)
            return;

        Vector2Int dir = direction == Direction.Left ? Vector2Int.left : Vector2Int.right;

        bool allowMove = true;

        foreach(var mini in _miniCubes)
        {
            if ((mini.GetPosition() + dir).x < -_field.GetSize().x / 2 || (mini.GetPosition() + dir).x > _field.GetSize().x / 2 - 1)
                allowMove = false;

            try
            {
                if (_field.GetCubes()[mini.GetPosition() + dir] != null)
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
                mini.SetNewPosition(new Vector2Int((int)mini.transform.position.x, (int)mini.transform.position.y));
            }
        }
    }

    private async void MoveY()
    {
        while (_state == FigureState.Mooving)
        {
            await Task.Delay(_field.GetDelay() * 100);

            try
            {
                foreach (var mini in _miniCubes)
                {
                    if (_field.GetCubes()[new Vector2Int(mini.GetPosition().x, mini.GetPosition().y - 1)] != null)
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
                mini.SetNewPosition(new Vector2Int((int)mini.transform.position.x, (int)mini.transform.position.y));
            }
        }

    }
}