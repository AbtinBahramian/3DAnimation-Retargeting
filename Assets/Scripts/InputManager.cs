using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public event EventHandler<OnRunEventArgs> OnRun;
    public class OnRunEventArgs: EventArgs{
        public bool isRunning;
        public Vector3 inputVector;
    }

    private InputActionManager inputActions;
    private Vector2 inputVector;
    private bool isRunning = false;

    private void Awake() {
        inputActions = new InputActionManager();
        inputActions.Player.Enable();
        inputActions.Player.Run.started += SetRun;
        inputActions.Player.Run.canceled += SetRun;
    }

    private void Update() {
        inputVector = inputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
    }

    private void SetRun(InputAction.CallbackContext context)
    {
        OnRun?.Invoke(this, new OnRunEventArgs { 
            isRunning = context.started,
            inputVector = new Vector3(inputVector.x, 0f, inputVector.y)});
        
        isRunning = context.started;
    }

    public Vector2 GetVector2Normalized(){
        
        return inputVector;
    }

    public bool GetIsRunning(){
        return isRunning;
    }
}
