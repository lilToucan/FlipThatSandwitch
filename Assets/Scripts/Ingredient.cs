using UnityEngine;

public class Ingredient : HomoBehaviour
{
    // TODO: add grid node variable
    public Vector2Int Coordinates { get; private set; }
    public virtual void Flip(Vector2Int coordinates)
    {
        Coordinates = coordinates;
        
    }
}