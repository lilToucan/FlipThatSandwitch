using UnityEngine;

public delegate Node GetNode(Vector2Int _coordinates);
public delegate void ExecuteComand(ICommand _command);
public delegate bool ValidateMove(Node _oldNode, Node _newNode);
