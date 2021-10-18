using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{    
    //self properties
    public int Idx;
    public int Score;
    public float Height;
    public float Width;

    public bool UnderSpotLight = false;
    public bool InShadow;//one shadow may be consisted of more than one collider, but score can only be calculated once

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    
    

    private Rigidbody myRB;
    private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;
    private bool isMoving;
    private bool isGrounded = true;
    [SerializeField] private float maxSpeed;


    private void Start() 
    {
        myRB = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

    }
    private void Update() 
    {
        if(isMoving)
        {
            Vector2 input = playerInputActions.Player.Movement.ReadValue<Vector2>();
            Vector3 move = new Vector3(input.x, 0, input.y);

            myRB.AddForce(move * moveSpeed, ForceMode.Force);   

            //limited max speed
            if(myRB.velocity.magnitude > maxSpeed)
            {
                myRB.velocity = myRB.velocity.normalized * maxSpeed;
            }

            //rotate to moving direction
            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground")) isGrounded = true;
    }
 
    public void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground")) isGrounded = false;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("SpotLight"))
        {
            UnderSpotLight = true;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.CompareTag("SpotLight"))
        {
            UnderSpotLight = false;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if(context.performed) isMoving = true;
        else if(context.canceled) isMoving = false;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed && isGrounded)
        {                    
            Debug.Log(context);
            Debug.Log("Jump");
            myRB.AddForce(Vector3.up * 6f, ForceMode.Impulse);

        }
    }

}
