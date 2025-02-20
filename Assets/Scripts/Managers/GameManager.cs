using System;
using UnityEngine;

public class GameManager : HomoBehaviour
{
    [SerializeField] public Ingredient breadPrefab;
    [SerializeField] public Ingredient ingridientPrefab;

    [SerializeField] public Vector2Int ingredientAmountRange;

    [SerializeField] public float ingredientSize = 1.5f;
   
    IManager[] managers;

    Grid gameGrid;
    CommandInvoker invoker;

    private void Awake()
    {
        gameGrid = new(Vector3.zero, ingredientSize);
        managers = new IManager[] { new GameplayManager(ExecuteCommand, GetNodeFromCoordinates), new LevelManager(gameGrid,breadPrefab,ingridientPrefab,ingredientAmountRange,ingredientSize) };
        invoker = new();
        LevelGeneration();
    }

    private void OnEnable()
    {
        foreach(var manager in managers)
        {
            manager.OnEnableFunction();
        }
    }

    private void OnDisable()
    {
        foreach (var manager in managers)
        {
            manager.OnDisableFunction();
        }
    }


    public void LevelGeneration()
    {
        foreach (var manager in managers)
        {
            manager.AwakeFunction();
        }
    }

    private Node GetNodeFromCoordinates(Vector2Int coordinates) => gameGrid.NodeGrid[coordinates.x, coordinates.y];

    private void ExecuteCommand(ICommand command) => invoker.ExecuteCommand(command);

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (gameGrid == null)
            gameGrid = new(Vector3.zero, ingredientSize);
        foreach (Node node in gameGrid.NodeGrid)
        {
            Gizmos.DrawWireCube(node.Position, Vector3.one);
        }
    }
#endif
}
