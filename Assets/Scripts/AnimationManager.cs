using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] InputManager inputManager;
    private Animator animator;

    [SerializeField] private float acceleration = 2f; // for acceleration the velocity and blending two animation
    [SerializeField] private float deceleration = 2f; // for deceleration the velocity and blending two animation
    private float velocityX = 0;
    private float velocityZ = 0;
    private float maxWalkVelocity = 0.5f;
    private float maxRunVelocity = 2f;
    private int velocity_X_Hash;
    private int velocity_Z_Hash;


    
    private void Awake() {
        animator = this.GetComponent<Animator>();
        velocity_X_Hash = Animator.StringToHash("Velocity_X");
        velocity_Z_Hash = Animator.StringToHash("Velocity_Z");
    }

    private void Start() {
        inputManager.OnRun += InputManager_OnRun;
    }

    private void InputManager_OnRun(object sender, InputManager.OnRunEventArgs e)
    {
            
       SetAnimation(e.inputVector, e.isRunning);
    }

    public void SetAnimation(Vector3 inputVector, bool isRunning){
        //defining the current velocity based on if speed is pressed
        float currentMaxVelocity = isRunning ? maxRunVelocity : maxWalkVelocity;

        ChangeVelocity(inputVector, isRunning, currentMaxVelocity);
        LockOrResetVelocity(inputVector, isRunning, currentMaxVelocity);

        animator.SetFloat(velocity_X_Hash, velocityX);
        animator.SetFloat(velocity_Z_Hash, velocityZ);
    }

    private void ChangeVelocity(Vector3 inputVector, bool isRunning, float currentMaxVelocity){
        if(inputVector.z > 0 && velocityZ < currentMaxVelocity){
            velocityZ += Time.deltaTime * acceleration;
        }

        if(inputVector.x > 0 && velocityX < currentMaxVelocity){
            velocityX += Time.deltaTime * acceleration;
        }

        if(inputVector.x < 0 && velocityX > -currentMaxVelocity){
            velocityX -= Time.deltaTime * acceleration;
        }

        //decrease velocityZ
        if(inputVector.z == 0 && velocityZ > 0 ){
            velocityZ -= Time.deltaTime * deceleration;
        }

        //decrease right movement velocity
        if(inputVector.x == 0 && velocityX > 0){
            velocityX -= Time.deltaTime * deceleration;
        }
        //decrease left movement velocity
        if(inputVector.x == 0 && velocityX < 0){
            velocityX += Time.deltaTime * deceleration;
        }
    }

    private void LockOrResetVelocity(Vector3 inputVector, bool isRunning, float currentMaxVelocity){
        //reset velocityZ
        if(inputVector.z == 0 && velocityZ < 0){
            velocityZ = 0;
        }
        
        //reseting velocityX
        if(inputVector.x == 0 && velocityX != 0.0f && (velocityX > -0.05f && velocityX < 0.05f)){
            velocityX = 0.0f;
        }
        //lock forward
        if(inputVector.z > 0 && isRunning && velocityZ > currentMaxVelocity){
            velocityZ = currentMaxVelocity;
        }else if(inputVector.z > 0 && velocityZ > currentMaxVelocity){ // decrease to the max walking velocity
            velocityZ -= Time.deltaTime * deceleration;
            //round to the currentMaxVelocity if within offset from top
            if(velocityZ > currentMaxVelocity && velocityZ < (currentMaxVelocity +0.05f)){
                velocityZ = currentMaxVelocity;
            }
            //round to the currentMaxVelocity if within offset from bottom
        }else if(inputVector.z > 0 && velocityZ < currentMaxVelocity && velocityZ > (currentMaxVelocity -0.05f)){
            velocityZ = currentMaxVelocity;
        }

        //lock left
        if(inputVector.x < 0 && isRunning && velocityX < -currentMaxVelocity){
            velocityX = -currentMaxVelocity;
        }else if(inputVector.x < 0 && velocityX < -currentMaxVelocity){ // decrease to the max walking velocity
            velocityX += Time.deltaTime * deceleration;
            //round to the currentMaxVelocity if within offset from top
            if(velocityX < -currentMaxVelocity && velocityX > (-currentMaxVelocity - 0.05f)){
                velocityX = -currentMaxVelocity;
            }
            //round to the currentMaxVelocity if within offset from bottom
        }else if(inputVector.x < 0 && velocityX > -currentMaxVelocity && velocityX < (-currentMaxVelocity + 0.05f)){
            velocityX = -currentMaxVelocity;
        }

        //lock Right
        if(inputVector.x > 0 && isRunning && velocityX > currentMaxVelocity){
            velocityX = currentMaxVelocity;
        }else if(inputVector.x > 0 && velocityX > currentMaxVelocity){ // decrease to the max walking velocity
            velocityX -= Time.deltaTime * deceleration;
            //round to the currentMaxVelocity if within offset from top
            if(velocityX > currentMaxVelocity && velocityX < (currentMaxVelocity +0.05f)){
                velocityX = currentMaxVelocity;
            }
            //round to the currentMaxVelocity if within offset from bottom
        }else if(inputVector.x > 0 && velocityX < currentMaxVelocity && velocityX > (currentMaxVelocity -0.05f)){
            velocityX = currentMaxVelocity;
        }
    }
}
