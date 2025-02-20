using System;
using UnityEngine;

public class GameManager : HomoBehaviour
{
    [SerializeField] private Ingredient breadPrefab;
    [SerializeField] private Ingredient ingridientPrefab;

    [SerializeField] private Vector2Int ingredientAmountRange;

    [SerializeField] private float ingredientSize = 1.5f;

    [SerializeField] private GameObject winPannel;

    IManager[] managers;

    Grid gameGrid;
    CommandInvoker invoker;
    int ingridientAmount;

    private ValidateMove onValidate;
    private Action<int> onGetIngridientAmount;



    private void ExecuteCommand(ICommand command) => invoker.ExecuteCommand(command);

    private void Awake()
    {
        gameGrid = new(Vector3.zero, ingredientSize);
        managers = new IManager[] { new RulesManager(ref onValidate, ref onGetIngridientAmount, winPannel), new InputManager(ExecuteCommand, GetNodeFromCoordinates, ref onValidate), new LevelManager(gameGrid, breadPrefab, ingridientPrefab, ingredientAmountRange, ingredientSize, ref onGetIngridientAmount) };
        invoker = new();
        LevelGeneration();
    }


    private void OnEnable()
    {
        foreach (var manager in managers)
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

    private Node GetNodeFromCoordinates(Vector2Int coordinates)
    {
        if (coordinates.x >= 4 || coordinates.x < 0 || coordinates.y >= 4 || coordinates.y < 0)
            return null;

        return gameGrid.NodeGrid[coordinates.x, coordinates.y];
    }




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
