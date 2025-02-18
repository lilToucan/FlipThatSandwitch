using System.Collections.Generic;
using UnityEngine;

public class LevelManager : HomoBehaviour
{
    [SerializeField] private Ingredient breadPrefab;

    private Grid grid;

    [SerializeField] private Vector2Int ingredientAmountRange;

    [SerializeField] private float ingredientSize = 1.5f;

    [SerializeField] private Ingredient ingridientPrefab;

    //Vector2[] offsets = new Vector2[4] {new(0,1),new()}

    private List<Node> spawnedBreadNodes = new();

    private List<Node> spawnedIngredientNodes = new();
    Node lastNode;
    Vector2Int[] directions = new Vector2Int[4] { new(0, 1), new(0, -1), new(1, 0), new(-1, 0) };

    public Grid Grid { get => grid; private set => grid = value; }

    private void Awake()
    {
        GenerateLevel();
    }


    public void GenerateLevel()
    {
        grid = new(transform.position, ingredientSize);
        GenerateBread();
        GenerateIngridients();
    }

    private void GenerateBread()
    {
        spawnedBreadNodes.Clear();

        int x = Random.Range(0, 4);
        int z = Random.Range(0, 4);

        // spawn first bread
        lastNode = grid.NodeGrid[x, z];
        Ingredient lastIngridient = Instantiate(breadPrefab, lastNode.Position, Quaternion.identity);

        lastNode.isBread = true;
        spawnedBreadNodes.Add(lastNode);
        lastNode.IngridientStack.Push(lastIngridient);
        lastNode.IngridientStack.Peek().name = "Bread_1";

        // spawn second bread
        List<Node> adjacentNodes = CheckValidAdjacentNodes(lastNode);

        lastNode.isBread = true;
        lastNode = adjacentNodes[Random.Range(0, adjacentNodes.Count)];
        lastIngridient = Instantiate(breadPrefab, lastNode.Position, Quaternion.identity);

        spawnedBreadNodes.Add(lastNode);
        lastNode.IngridientStack.Push(lastIngridient);
        lastNode.IngridientStack.Peek().name = "Bread_2";
    }

    void GenerateIngridients()
    {
        List<Node> adjacentNodes = new();
        spawnedIngredientNodes.Clear();
        int amount = Random.Range(ingredientAmountRange.x, ingredientAmountRange.y);
        Ingredient lastIngridient;

        for (int i = 0; i < amount; i++)
        {
            if (Random.Range(0, 1f) > 0.6f && spawnedBreadNodes.Count != 0)
            {
                lastNode = spawnedBreadNodes[Random.Range(0, spawnedBreadNodes.Count)];
            }
            else if (spawnedIngredientNodes.Count != 0)
            {

                lastNode = spawnedIngredientNodes[Random.Range(0, spawnedIngredientNodes.Count)];
            }

            adjacentNodes = CheckValidAdjacentNodes(lastNode);

            if(adjacentNodes.Count <= 0)
            {
                i--;
                continue;
            }

            lastNode = adjacentNodes[Random.Range(0, adjacentNodes.Count)];
            spawnedIngredientNodes.Add(lastNode);

            lastIngridient = Instantiate(ingridientPrefab, lastNode.Position, Quaternion.identity);
            lastNode.IngridientStack.Push(lastIngridient);
            lastIngridient.name = $"ingridient_{i + 1}";
        }
    }

    private List<Node> CheckValidAdjacentNodes(Node _node)
    {
        int nodeX = _node.Coordinates.x;
        int nodeZ = _node.Coordinates.y;
        Node adjacentNode;
        List<Node> validAdjacentNodes = new();

        foreach (var dir in directions)
        {
            int offsetX = dir.x + nodeX;
            int offsetZ = dir.y + nodeZ;
            if (offsetX < 0 || offsetX >= 4 || offsetZ < 0 || offsetZ >= 4)
                continue;

            adjacentNode = grid.NodeGrid[offsetX, offsetZ];

            if (adjacentNode == _node || adjacentNode.IngridientStack.Count > 0)
                continue;

            validAdjacentNodes.Add(adjacentNode);
        }

        if (validAdjacentNodes.Count == 0)
        {
            if (lastNode.isBread)
                spawnedBreadNodes.Remove(lastNode);
            else
                spawnedIngredientNodes.Remove(lastNode);

            if (spawnedIngredientNodes.Count != 0)
                lastNode = spawnedIngredientNodes[Random.Range(0, spawnedIngredientNodes.Count)];

            else
                lastNode = spawnedBreadNodes[Random.Range(0, spawnedBreadNodes.Count)];

        }

        return validAdjacentNodes;
    }

    private void OnDrawGizmos()
    {
        if (grid == null)
            grid = new(transform.position, ingredientSize);
        foreach (Node node in Grid.NodeGrid)
        {
            Gizmos.DrawWireCube(node.Position, Vector3.one);
        }
    }
}