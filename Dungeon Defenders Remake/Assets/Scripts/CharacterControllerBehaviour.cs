using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
[RequireComponent(typeof(CharacterController))]
public class CharacterControllerBehaviour : MonoBehaviour
{
    [Header("Locomotion Parameters")]
    [SerializeField]
    private float _mass = 75; // [kg] 
    [SerializeField]
    private float _acceleration = 5; // [m/s^2] 
    [Header("Dependencies")]
    [SerializeField, Tooltip("What should determine the absolute forward when a player presses forward.")]
    private Transform _absoluteForward;
    [SerializeField]
    private float _maxRunningSpeed = (30.0f * 1000) / (60 * 60); // [m/s], 30 km/h
    [SerializeField]
    private Animator ani;

    private bool _jump;
    private CharacterController _characterController;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _movement = Vector3.zero;
    private float _dragOnGround = 5f;
    private float _jumpHeight = 2f;
    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        #if DEBUG
        Assert.IsNotNull(_characterController, "Dependency Error: This component needs a CharachterController to work.");
        #endif
    }

    void FixedUpdate() {
        ApplyGravity();
        ApplyGround();
        ApplyMovement();
        ApplyGroundDrag();
        ApplyJump();
        LimitMaximumRunningSpeed();
        _characterController.Move(_velocity * Time.deltaTime);
    }
    // Update is called once per frame
    void Update() {
        _movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //Debug.Log("z" + _movement.z);
        //Debug.Log("x" + _movement.x);
        if (_movement.z > 0.01f)
        {
            ani.SetBool("Walking_f", true);
        } else if (_movement.z < -0.01f)
        {
            ani.SetBool("Walking_b", true);
        } else
        {
            ani.SetBool("Walking_b", false);
            ani.SetBool("Walking_f", false);
        }

        if (_movement.x > 0.01f)
        {
            ani.SetBool("Walking_r", true);
        }
        else if (_movement.x < -0.01f)
        {
            ani.SetBool("Walking_l", true);
        } else
        {
            ani.SetBool("Walking_l", false);
            ani.SetBool("Walking_r", false);
        }

        if (Input.GetButtonDown("Jump")) {
            _jump = true;
            ani.SetBool("Jumping", true);
        }
    }
    private void ApplyGroundDrag() {
        if (_characterController.isGrounded) {
            _velocity = _velocity * (1 - Time.deltaTime * _dragOnGround);
        }
    }

    private void ApplyGravity()
    {
        if (!_characterController.isGrounded){
            _velocity += Physics.gravity * Time.deltaTime; // g[m/s^2] * t[s]   
        } 
    }

    private void ApplyGround() {
        if (_characterController.isGrounded){
            _velocity -= Vector3.Project(_velocity, Physics.gravity.normalized);
        }
    }


    private void ApplyMovement()
    {
        if (_characterController.isGrounded)
        {
            Vector3 xzAbsoluteForward = Vector3.Scale(_absoluteForward.forward, new Vector3(1, 0, 1));
        
            Quaternion forwardRotation = 
                Quaternion.LookRotation(xzAbsoluteForward);

            Vector3 relativeMovement = forwardRotation * _movement;

            _velocity += relativeMovement * _acceleration * Time.deltaTime; // F(= m.a) [m/s^2] * t [s]    

        }
    }

    private void ApplyJump() {
        if (_jump && _characterController.isGrounded) {
            _velocity += -Physics.gravity.normalized * Mathf.Sqrt(2 * Physics.gravity.magnitude * _jumpHeight);
            _jump = false;
        }
    }

    private void LimitMaximumRunningSpeed() {
        Vector3 yVelocity = Vector3.Scale(_velocity, new Vector3(0, 1, 0));
        Vector3 xzVelocity = Vector3.Scale(_velocity, new Vector3(1, 0, 1));
        Vector3 clampedXzVelocity = Vector3.ClampMagnitude(xzVelocity, _maxRunningSpeed);
        _velocity = yVelocity + clampedXzVelocity;
    }
}
