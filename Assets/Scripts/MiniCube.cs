using UnityEngine;

public class MiniCube : MonoBehaviour
{
    [SerializeField] MiniCubeModel _model;
    [SerializeField] Vector2Int _index;

    public void Init(Vector2Int position)
    {
        _model = new MiniCubeModel(_index, position);
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


