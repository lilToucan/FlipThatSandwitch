using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
public class InputManager : IManager
{
    private Camera mainCam;
    private bool isMovingDone = true;
    private Ingredient ingredientHit;

    private GetNode onGetNodeRequest;
    private ExecuteComand onExecuteCommandRequest;
    private bool isLevelDone = false;
    public ValidateMove onValidateMove;
    public Action OnDeleteLevel;

    public InputManager(ExecuteComand _executeCommand, GetNode _getNode)
    {
        onExecuteCommandRequest += _executeCommand;
        onGetNodeRequest += _getNode;
        mainCam = Camera.main;
    }

    public void AwakeFunction() { isLevelDone = false; }

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
        Node newNode = onGetNodeRequest?.Invoke(dir);

        if (newNode == null)
            return;
        if (!onValidateMove.Invoke(oldNode, newNode))
            return;

        FlipStackCommand flip = new(oldNode, newNode, 0.3f);
        onExecuteCommandRequest?.Invoke(flip);

        if (isLevelDone)
            OnDeleteLevel?.Invoke();

    }

    public void Won(Node _)
    {
        isLevelDone = true;
    } 

    private void OnTouchRelese(Finger _finger)
    {
        isMovingDone = true;
        ingredientHit = null;
    }
}
