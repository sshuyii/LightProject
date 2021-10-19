using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{    
    //self properties
    public int Index;
    public int TeamIndex;

    private int score;
    public int Score
    {
        get{ return score;}
        set
        {
            score = value;
            teamManager.UpdateScore();
        }
    }

    private bool flashLightOn;
    public bool FlashLightOn
    {
        get{ return flashLightOn;}
        set
        {
            flashLightOn = value;
            if(flashLightOn) flashLight.enabled = true;
            else flashLight.enabled = false;
        }
    }
    [SerializeField] private Light flashLight;

    private float lightCollection;
    public float LightCollection
    {
        get{ return lightCollection;}
        set
        {
            if(value >= 100f) lightCollection = 100f;
            else if(value <= 0f) 
            {
                lightCollection = 0f;
                FlashLightOn = false;
            }
            else lightCollection = value;
        }
    }

    [SerializeField] private Image lightUI;

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

    private TeamManager teamManager;

    private void Start() 
    {
        myRB = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        // flashLight = GetComponentInChildren<Light>();
        FlashLightOn = false;

        teamManager = GameObject.Find("/Managers").GetComponent<TeamManager>();

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

        //set light collection bar
        if(lightUI != null) lightUI.fillAmount = lightCollection / 100f;

        //reduce light collection if flashlight is on
        if(flashLightOn == true)
        {
            LightCollection -= 8 * Time.deltaTime;
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

    public void FlashLight(InputAction.CallbackContext context)
    {
        if(context.performed && lightCollection == 100f)  FlashLightOn = true;

    }

}
