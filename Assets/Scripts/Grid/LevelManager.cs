using System.Collections.Generic;
using UnityEngine;

public class LevelManager : IManager
{
    private Ingredient breadPrefab;
    private Ingredient ingridientPrefab;

    private Vector2Int ingredientAmountRange;

    private float ingredientSize = 1.5f;

    private Vector2Int[] directions = new Vector2Int[4] { new(0, 1), new(0, -1), new(1, 0), new(-1, 0) };

    private Node lastNode;

    private List<Node> spawnedBreadNodes = new();
    private List<Node> spawnedIngredientNodes = new();

    private Grid grid;
    private Transform transform;


    public LevelManager(Grid _grid, Ingredient _breadPrefab, Ingredient _ingridientPrefab, Vector2Int _ingredientAmountRange, float _ingredientSize)
    {
        grid = _grid;
        breadPrefab = _breadPrefab;
        ingridientPrefab = _ingridientPrefab;
        ingredientAmountRange = _ingredientAmountRange;
        ingredientSize = _ingredientSize;
    }
    public void GenerateLevel()
    {
        GenerateBread();
        GenerateIngridients();
    }

    public void AwakeFunction()
    {
        GenerateLevel();
    }

    public void OnEnableFunction() { }

    public void OnDisableFunction() { }

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

    private void GenerateBread()
    {
        spawnedBreadNodes.Clear();

        int x = Random.Range(0, 4);
        int z = Random.Range(0, 4);

        // spawn first bread
        lastNode = grid.NodeGrid[x, z];
        Ingredient lastIngridient = HomoBehaviour.Instantiate(breadPrefab, lastNode.Position, Quaternion.identity);
        lastIngridient.Coordinates = lastNode.Coordinates;

        lastNode.isBread = true;
        spawnedBreadNodes.Add(lastNode);
        lastNode.IngridientStack.Push(lastIngridient);
        lastNode.IngridientStack.Peek().name = "Bread_1";

        // spawn second bread
        List<Node> adjacentNodes = CheckValidAdjacentNodes(lastNode);

        lastNode.isBread = true;
        lastNode = adjacentNodes[Random.Range(0, adjacentNodes.Count)];
        lastIngridient = Object.Instantiate(breadPrefab, lastNode.Position, Quaternion.identity);
        lastIngridient.Coordinates = lastNode.Coordinates;

        spawnedBreadNodes.Add(lastNode);
        lastNode.IngridientStack.Push(lastIngridient);
        lastNode.IngridientStack.Peek().name = "Bread_2";
    }

    private void GenerateIngridients()
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

            if (adjacentNodes.Count <= 0)
            {
                i--;
                continue;
            }

            lastNode = adjacentNodes[Random.Range(0, adjacentNodes.Count)];
            spawnedIngredientNodes.Add(lastNode);

            lastIngridient = Object.Instantiate(ingridientPrefab, lastNode.Position, Quaternion.identity);
            lastIngridient.Coordinates = lastNode.Coordinates;
            lastNode.IngridientStack.Push(lastIngridient);
            lastIngridient.name = $"ingridient_{i + 1}";
        }
    }

    public Node GetNodeFormCoordinates(Vector2Int coordinates)
    {
        return grid.NodeGrid[coordinates.x, coordinates.y];
    }



}