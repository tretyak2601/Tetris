using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] Figure[] _figuresPrefab;
    [SerializeField] int _delay;
    [SerializeField] GameObject _cube;
    [SerializeField] GameObject _loseUI;

    public int Delay { get { return _delay; } }
    public Dictionary<Vector2Int, GameObject> Cubes { get; private set; } = new Dictionary<Vector2Int, GameObject>();
    public Vector2Int Size { get { return _size; } }

    private GameState _gameState = GameState.Play;
    public GameState GameState
    {
        get
        {
            return _gameState;
        }
        set
        {
            _gameState = value;

            if (value == GameState.GameOver)
                _loseUI.SetActive(true);
        }
    }

    private Vector2Int _size = new Vector2Int(20, 20);
    private Figure _currentFigure;

    public void Start()
    {
        Cubes = new Dictionary<Vector2Int, GameObject>(_size.x);

        for (int i = -(_size.x / 2); i < _size.x / 2; i++)
            for (int j = 0; j < _size.y; j++)
                Cubes[new Vector2Int(i, j)] = j == 0 ? Instantiate(_cube, new Vector3(i, j, 0), Quaternion.identity, transform) : null;

        CreateFigure();
    }

    private void CheckForFull()
    {
        for (int j = 1; j < _size.y; j++)
        {
            bool isFull = true;

            for (int i = -(_size.x / 2); i < _size.x / 2; i++)
                if (Cubes[new Vector2Int(i, j)] == null)
                    isFull = false;

            if (isFull)
            {
                for (int i = -(_size.x / 2); i < _size.x / 2; i++)
                    Destroy(Cubes[new Vector2Int(i, j)].gameObject);

                MoveAllDown(j);
            }
        }
    }

    private void FullEmptySpace(FigureState state, Figure figure)
    {
        if (state == FigureState.Idle)
            foreach(var mini in figure.Minis)
                Cubes[new Vector2Int(mini.GetPosition().x, mini.GetPosition().y)] = mini.gameObject;

        CheckForFull();
        CreateFigure();
    }

    private void CreateFigure()
    {
        var figure = Instantiate(_figuresPrefab[Random.Range(0, _figuresPrefab.Length)]);
        figure.SetPosition(new Vector2(0, _size.y), this);
        figure.OnStateChanged += FullEmptySpace;
        _currentFigure = figure;
    }

    public void MoveFigureX(Direction dir)
    {
        _currentFigure.MoveX(dir);
    }

    private void MoveAllDown(int index)
    {
        for (int j = index + 1; j < _size.y; j++)
        {
            for (int i = -(_size.x / 2); i < _size.x / 2; i++)
            {
                if (Cubes[new Vector2Int(i, j)] != null)
                {
                    Cubes[new Vector2Int(i, j)].GetComponent<MiniCube>().SetNewPosition(new Vector2Int(i, j - 1));
                    Cubes[new Vector2Int(i, j - 1)] = Cubes[new Vector2Int(i, j)];
                    Cubes[new Vector2Int(i, j)] = null;
                }
            }
        }
    }
}

public enum Direction
{
    Left,
    Right
}

public enum GameState
{
    Play,
    GameOver
}
