using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] Figure[] _figuresPrefab;
    [SerializeField] int _delay;
    [SerializeField] GameObject _cube;
    [SerializeField] GameObject _loseUI;

    [SerializeField] FieldModel _model;

    public void Start()
    {
        _model = new FieldModel(_delay);

        for (int i = -(_model.Size.x / 2); i < _model.Size.x / 2; i++)
            for (int j = 0; j < _model.Size.y; j++)
                _model.Cubes[new Vector2Int(i, j)] = j == 0 ? Instantiate(_cube, new Vector3(i, j, 0), Quaternion.identity, transform) : null;

        CreateFigure();
    }

    private void CheckForFull()
    {
        for (int j = 1; j < _model.Size.y; j++)
        {
            bool isFull = true;

            for (int i = -(_model.Size.x / 2); i < _model.Size.x / 2; i++)
                if (_model.Cubes[new Vector2Int(i, j)] == null)
                    isFull = false;

            if (isFull)
            {
                for (int i = -(_model.Size.x / 2); i < _model.Size.x / 2; i++)
                    Destroy(_model.Cubes[new Vector2Int(i, j)].gameObject);

                MoveAllDown(j);
            }
        }
    }

    private void FullEmptySpace(FigureState state, Figure figure)
    {
        if (state == FigureState.Idle)
            foreach(var mini in figure.MiniCubes)
                _model.Cubes[new Vector2Int(mini.GetPosition().x, mini.GetPosition().y)] = mini.gameObject;

        CheckForFull();
        CreateFigure();
    }

    private void CreateFigure()
    {
        var figure = Instantiate(_figuresPrefab[Random.Range(0, _figuresPrefab.Length)]);
        figure.SetPosition(new Vector2(0, _model.Size.y), this);
        figure.OnStateChanged += FullEmptySpace;
        _model.CurrentFigure = figure;
    }

    public void MoveFigureX(Direction dir)
    {
        _model.CurrentFigure.MoveX(dir);
    }

    private void MoveAllDown(int index)
    {
        for (int j = index + 1; j < _model.Size.y; j++)
        {
            for (int i = -(_model.Size.x / 2); i < _model.Size.x / 2; i++)
            {
                if (_model.Cubes[new Vector2Int(i, j)] != null)
                {
                    _model.Cubes[new Vector2Int(i, j)].GetComponent<MiniCube>().SetNewPosition(new Vector2Int(i, j - 1));
                    _model.Cubes[new Vector2Int(i, j - 1)] = _model.Cubes[new Vector2Int(i, j)];
                    _model.Cubes[new Vector2Int(i, j)] = null;
                }
            }
        }
    }

    public Dictionary<Vector2Int, GameObject> GetCubes()
    {
        return _model.Cubes;
    }

    public int GetDelay()
    {
        return _model.Delay;
    }

    public void SetState(GameState state)
    {
        if (state == GameState.GameOver)
            _loseUI.SetActive(true);

        _model.GameState = state;
    }

    public GameState GetState()
    {
        return _model.GameState;
    }

    public Vector2Int GetSize()
    {
        return _model.Size;
    }
}