using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;


public delegate Node GetNode(Vector2Int _coordinates);
public delegate void ExecuteComand(ICommand _command);

public class InputManager : IManager
{
    private Camera mainCam;
    private bool isMovingDone = true;
    private Ingredient ingredientHit;


    private GetNode onGetNodeRequest;
    private ExecuteComand onExecuteCommandRequest;
    private ValidateMove onValidateMove;



    public InputManager(ExecuteComand _executeCommand, GetNode _getNode,ref ValidateMove _onValidateMove)
    {
        onExecuteCommandRequest += _executeCommand;
        onGetNodeRequest += _getNode;
        onValidateMove = _onValidateMove;
        mainCam = Camera.main;
    }

    public void AwakeFunction() { }

    public void OnEnableFunction()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();

        EnhancedTouch.Touch.onFingerDown += OnTouch;
        EnhancedTouch.Touch.onFingerMove += OnTouchMove;
        EnhancedTouch.Touch.onFingerUp += OnTouchRelese;
    }


    public void OnDisableFunction()
    {
        onExecuteCommandRequest -= onExecuteCommandRequest;
        onGetNodeRequest -= onGetNodeRequest;
        onValidateMove -= onValidateMove;

        EnhancedTouch.Touch.onFingerDown -= OnTouch;
        EnhancedTouch.Touch.onFingerMove -= OnTouchMove;
        EnhancedTouch.Touch.onFingerUp -= OnTouchRelese;

        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
    }


    private void OnTouch(Finger _finger)
    {
        Debug.Log(_finger.index);
        Ray ray = mainCam.ScreenPointToRay(_finger.screenPosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 10f);
        Physics.Raycast(ray, out RaycastHit hit);
        if (hit.transform == null)
            return;

        hit.transform.TryGetComponent(out ingredientHit);
    }

    private void OnTouchMove(Finger _finger)
    {
        if (!isMovingDone || ingredientHit == null)
            return;

        Vector2 touchStartPos = _finger.currentTouch.startScreenPosition;
        Vector2 touchEndPos = _finger.currentTouch.screenPosition;

        if (Vector2.Distance(touchStartPos, touchEndPos) < 30f)
            return;

        Vector2 touchDirection = (touchEndPos - touchStartPos).normalized;
        Vector2 dirNorm = touchDirection;

        // checks if the direction is diagonal 
        if (touchDirection.x == 0.5f)
            return;

        isMovingDone = false;

        Node oldNode = onGetNodeRequest?.Invoke(ingredientHit.Coordinates);

        if (Mathf.Abs(touchDirection.x) > Mathf.Abs(touchDirection.y))
        {
            dirNorm.y = 0;
        }
        else
        {
            dirNorm.x = 0;
        }

        dirNorm = dirNorm.normalized;
        float dot = Vector2.Dot(dirNorm, touchDirection);

        if (dot < 0.7f) // the dot needs to be > 70 
            return;

        Vector2Int dir = new(oldNode.Coordinates.x + (int)dirNorm.x, oldNode.Coordinates.y + (int)dirNorm.y);
        Debug.Log(dir);
        Node newNode = onGetNodeRequest?.Invoke(dir);

        if (newNode == null || !onValidateMove.Invoke(oldNode, newNode))
            return;

        FlipStackCommand flip = new(oldNode, newNode, 0.3f);
        onExecuteCommandRequest?.Invoke(flip);

    }

    private void OnTouchRelese(Finger _finger)
    {
        isMovingDone = true;
        ingredientHit = null;
    }
}
