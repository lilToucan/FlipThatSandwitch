using System.Collections.Generic;
using UnityEngine;

public class Node
{
    Vector2Int coordinates;
    Vector3 position;

    public Vector2Int Coordinates { get => coordinates; private set => coordinates = value; }
    public Vector3 Position { get => position; private set => position = value; }
    public Stack<Ingredient> IngridientStack { get; private set; }
    public bool isBread { get; set; }

    public Node(Vector2Int _coordinates, Vector3 _position)
    {
        this.coordinates = _coordinates;
        this.position = _position;
        IngridientStack = new();
    }
}
