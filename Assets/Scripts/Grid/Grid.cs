using System.Collections.Generic;
using UnityEngine;


public class Grid
{
    int side = 4;
   
    public Node[,] NodeGrid { get; private set; }

    public Grid(Vector3 _startPos, float _cellSize)
    {
        NodeGrid = new Node[4, 4];
        Vector3 pos = Vector3.zero;
        for (int x = 0; x < side; x++)
        {
            pos.x = (_cellSize * x);
            for (int z = 0; z < side; z++)
            {
                pos.z = (_cellSize * z);
                NodeGrid[x, z] = new Node(new(x, z), pos);
                
               
            }
        }
    }

    public void Reset()
    {
        foreach (Node node in NodeGrid)
        {
            node.isBread = false;
            node.IngridientStack.Clear();
        }
    }
}
