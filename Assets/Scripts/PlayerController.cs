using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{    
    [Header("Self Properties")]
    public int Index;
    public int TeamIndex;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpSpeed;
    [HideInInspector] public bool UnderLight = false;
    [HideInInspector] public bool Umbrellable = false;
    [HideInInspector] public bool Invincible = false;

    private Vector3 initialPosition;
    private bool isMoving;
    private bool isGrounded = true;
    private bool levelEnd;

    

    [Header("References")]
    [SerializeField] private Light flashLight;
    [SerializeField] private GameObject Umbrella;
    [SerializeField] private GameObject itemPrefab;

    private Rigidbody myRB;
    private MeshRenderer myMR;
    private TeamManager teamManager;


    [Header("UI")]
    [SerializeField] private Image lightUI;
    
   
    private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;
    

    private int score;
    public int Score
    {
        get{ return score;}
        set
        {
            score = value;
            if(score >= 10) score = 10;
            else if(score <= 0) 
            {
                score = 0;
                Throwable = false;
            }
        }
    }

    private bool flashLightOn;
    public bool FlashLightOn
    {
        get{ return flashLightOn;}
        set
        {
            flashLightOn = value;
            if(flashLightOn) 
            {
                flashLight.enabled = true;
                flashLight.gameObject.layer = LayerMask.NameToLayer("Light");
            }
            else 
            {
                flashLight.enabled = false;
                flashLight.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
    }

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

  
    private bool throwable;
    public bool Throwable
    {
        get{ return throwable;}
        set
        {
            throwable = value;
            if(Score == 0) throwable = false;
        }
    }


    private void Start() 
    {
        myRB = GetComponent<Rigidbody>();
        myMR = GetComponent<MeshRenderer>();
        playerInput = GetComponent<PlayerInput>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        Umbrella.GetComponent<MeshRenderer>().enabled = false;
        teamManager = GameObject.Find("/Managers").GetComponent<TeamManager>();

        initialPosition = transform.position;

        //subscribe to events
        EventManager.current.OnLevelEnd += LevelEnd;

        Reset();
    }

    private void OnDestroy() 
    {
        //unsubscribe to events
        EventManager.current.OnLevelEnd -= LevelEnd;

    }

    private void Update() 
    {        
        if(isMoving)
        {
            Vector2 input = playerInputActions.Player.Movement.ReadValue<Vector2>();
            Vector3 move = new Vector3(input.x, 0, input.y);
            Vector3 camDir = Camera.main.transform.forward;
        
            move = Camera.main.transform.TransformDirection(move);
            move = new Vector3(move.x, 0, move.z);

            myRB.AddForce(move * moveSpeed, ForceMode.Force);   

            //limited max speed
            if(myRB.velocity.magnitude > maxSpeed)
            {
                myRB.velocity = myRB.velocity.normalized * maxSpeed;
            }

            //rotate to camera forword direction
            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

        }

        //set light collection bar
        if(lightUI != null) lightUI.fillAmount = lightCollection / 100f;

        //reduce light collection if flashlight is on
        if(FlashLightOn == true) LightCollection -= 8 * Time.deltaTime;

        if(UnderLight)
        {
            myMR.material.color = Color.green;
            LightCollection += 2 * Time.deltaTime;
        }
        else myMR.material.color = Color.black;

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
        if(other.gameObject.CompareTag("Trigger"))
        {
            teamManager.SolarArray[TeamIndex].AccelerationArea(true);
        }
        else if(other.gameObject.CompareTag("Solar"))
        {
            if(Score > 0) Throwable = true;
        }
        else if(other.gameObject.CompareTag("Dead"))
        {
            Dead();
        }
    }


    private void OnTriggerExit(Collider other) 
    {
        //more players in the trigger do more acceleration
        if(other.gameObject.CompareTag("Trigger"))
        {
            teamManager.SolarArray[TeamIndex].AccelerationArea(false);
        }
        else if(other.gameObject.CompareTag("Solar"))
        {
            Throwable = false;
        }
    }
    
    public void Dead()
    {
        if(!Invincible)
        {
            for(int i = 0; i < Score; i++)
            {
                Instantiate(itemPrefab, transform.position, Quaternion.identity);
            }

            Reset();        
        }
       
    }

    private void Reset()
    {
        FlashLightOn = false;
        Throwable = false;
        Score = 0;
        LightCollection = 0;
        Umbrellable = false;

        transform.position = initialPosition;
        
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
            // Debug.Log(context);
            Debug.Log("Jump");
            myRB.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }
    }

    public void FlashLight(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            FlashLightOn = !FlashLightOn;
        }
    }

    public void Throw(InputAction.CallbackContext context)
    {
        //throw item onto solar system
        if(context.performed && Throwable)  
        {
            Score--;
            teamManager.ItemCountArray[TeamIndex] ++;
        }
    }

    public void Item(InputAction.CallbackContext context)
    {
        //unfold umbrella
        if(context.performed && Umbrellable)  
        {
            Umbrella.GetComponent<ItemTimer>().Reset();
            Umbrellable = false;
            Invincible = true;

            Debug.Log("Umbrealla unfold");
        }
    }

    private void LevelEnd()
    {
        
    }

}
