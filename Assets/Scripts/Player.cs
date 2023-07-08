using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputManager inputActions;
    [SerializeField] private AnimationManager animationManager;
    [SerializeField] private int moveSpeed = 8;

    private bool isRunning;

    void Update()
    {
        isRunning = inputActions.GetIsRunning();
        Vector2 inputVector = inputActions.GetVector2Normalized();
        Vector3 movedir = new Vector3(inputVector.x, 0 , inputVector.y);
        animationManager.SetAnimation(movedir, isRunning);

        //transform.position += movedir * moveSpeed * Time.deltaTime;

    }
}
