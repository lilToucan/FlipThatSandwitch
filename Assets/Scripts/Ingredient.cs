using UnityEngine;

public class Ingredient : HomoBehaviour
{
    // TODO: add grid node variable
    public Vector2Int Coordinates { get; set; }
    public virtual void Flip(Node _oldNode,Node _newNode)
    {
        Coordinates = _newNode.Coordinates;


        
    }
}