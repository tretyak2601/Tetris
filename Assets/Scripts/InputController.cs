using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] Field _field;

    public void MoveLeft()
    {
        _field.MoveFigureX(Direction.Left);
    }

    public void MoveRight()
    {
        _field.MoveFigureX(Direction.Right);
    }
}
