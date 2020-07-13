using UnityEngine;

public class MiniCube : MonoBehaviour
{
    [SerializeField] MiniCubeModel _model;
    [SerializeField] Vector2Int _index;

    public void Init(Vector3 position)
    {
        transform.position = new Vector3(position.x + _index.x, position.y + _index.y, 0);
        _model = new MiniCubeModel(_index, new Vector2Int((int)transform.position.x, (int)transform.position.y));
    }

    public void SetNewPosition(Vector2Int position)
    {
        transform.position = new Vector3(position.x, position.y, 0);
        _model.CurrentPosition = position;
    }

    public Vector2Int GetPosition()
    {
        return _model.CurrentPosition;
    }

    public Vector2Int GetIndex()
    {
        return _index;
    }
}


