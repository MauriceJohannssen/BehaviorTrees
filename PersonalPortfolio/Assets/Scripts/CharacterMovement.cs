using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{   
    //Public
    public float acceleration = 5.0f, maxVelocity = 10.0f, gravity = 1.0f;
    public float jumpHeight = 10.0f, inAirMovementMultiplier = 0.4f;
    public float mouseXSensitivity = 0.1f, friction = 0.95f;

    //"Public"
    //[SerializeField] private RepeatingSound walkingSound;
    //[SerializeField] private float walkingSoundVelocityThreshold = 0.1f;
    
    //Private
    private Transform _transform;
    private CharacterController _characterController;
    private Vector3 _acceleration, _velocityXZ, _velocityY, _gravity;

    private void Start()
    {
        //Components
        _transform = transform;
        _characterController = GetComponent<CharacterController>();
        //Movement vectors
        _acceleration = Vector3.zero;
        _velocityXZ = Vector3.zero;
        _velocityY = Vector3.zero;
        _gravity = new Vector3(0, -gravity, 0);
    }

    private void Update()
    {
        _acceleration = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * acceleration;

        //Rotates character
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * mouseXSensitivity, 0));

        //Apply friction when no movement-related key's pressed
        //Info: Friction is only applied to XZ-plane
        if (!MovementKeyPressed()) ApplyFriction();

        //Jump when character on ground and space pressed
        if (_characterController.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            _velocityY = new Vector3(0, jumpHeight, 0);
            // TODO add jump sound
        }

        //Constrains velocity if velocity > maxVelocity
        ConstrainVelocity();

        //Semi-implicit euler integration
        //Movement on ground
        if (_characterController.isGrounded) _velocityXZ += _acceleration;
        //Movement in air
        else _velocityXZ += _acceleration * inAirMovementMultiplier;

        //Apply gravity vector
        if(!_characterController.isGrounded) _velocityY += _gravity * Time.deltaTime;

        //walkingSound.Running = _velocityXZ.magnitude >= walkingSoundVelocityThreshold;

        //Move character
        _characterController.Move(_transform.rotation * _velocityXZ * Time.smoothDeltaTime);
        _characterController.Move(_velocityY * Time.smoothDeltaTime);
    }

    private void ConstrainVelocity()
    {
        //Normalizes the velocity if the threshold maxVelocity is exceeded
        if (_velocityXZ.magnitude < maxVelocity) return;
        _velocityXZ.Normalize();
        _velocityXZ *= maxVelocity;
    }

    private void ApplyFriction()
    {
        _velocityXZ *= friction;
    }

    private bool MovementKeyPressed()
    {
        //Checks whether one or more input keys are currently pressed
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) ||
               Input.GetKey(KeyCode.D);
    }
}