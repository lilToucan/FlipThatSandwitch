using System.Collections.Generic;
using UnityEngine;


public class Grid
{
    int side = 4;
    Node[,] nodeGrid;
    public Node[,] NodeGrid { get => nodeGrid; private set => nodeGrid = value; }

    public Grid(Vector3 _startPos, float _cellSize)
    {
        nodeGrid = new Node[4, 4];
        Vector3 pos = Vector3.zero;
        for (int x = 0; x < side; x++)
        {
            pos.x = (_cellSize * x);
            for (int z = 0; z < side; z++)
            {
                pos.z = (_cellSize * z);
                nodeGrid[x, z] = new Node(new(x, z), pos);
                
               
            }
        }
    }
}
