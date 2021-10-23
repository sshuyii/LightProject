using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShadowDetector : MonoBehaviour
{
    public enum ObjectType
    {
        Player,
        Item,
        Battery,
        Solar,
        Umbrella
    }

    public ObjectType myObjectType;
    private GameObject light;
    private MeshRenderer myMesh;
    private RaycastHit hit;

    public bool UnderDirLight = false;
    public bool UnderSpotLight = false;

    private bool hitByPlayer = false;


    private Vector3 objectBottom;

    private PlayerController myPC;
    private ElevatorController myElevator;
    private SolarController mySolar;

    private MeshRenderer myMR;

    private LevelManager levelManager;

    private float height;
     
     // Use this for initialization
    void Start () 
    {

        light = GameObject.Find("Lights/Directional Light");
        levelManager = GameObject.Find("Managers").GetComponent<LevelManager>();

        myMR = GetComponent<MeshRenderer>();
        myMesh = GetComponent<MeshRenderer>();

        height = transform.localScale.y;

        if(myObjectType == ObjectType.Battery)
        {
            myElevator = transform.parent.GetComponent<ElevatorController>();
        }
        else if(myObjectType == ObjectType.Solar)
        {
            mySolar = transform.parent.GetComponent<SolarController>();
        }
        else if(myObjectType == ObjectType.Player)
        {
            myPC = GetComponent<PlayerController>();
        }
    }
     
    // Update is called once per frame
    void Update () 
    {
        hitByPlayer = false;

        objectBottom = new Vector3(transform.position.x, transform.position.y - height/2, transform.position.z);

        //check whether this gameObject is under directional light
        UnderDirLight = false;
        Vector3 sunDir = light.transform.forward;

        // Debug.Log("sun direction = " + sunDir );
        sunDir.Normalize();
        sunDir *= 100;
    
        Debug.DrawLine(objectBottom, objectBottom - sunDir, Color.red);

        if (!Physics.Raycast(objectBottom, - sunDir, out hit, 30, LayerMask.GetMask("Wall"))
            && !Physics.Raycast(objectBottom, - sunDir, out hit, 30, LayerMask.GetMask("Player")))
        {
           UnderDirLight = true;
        }
        if(Physics.Raycast(objectBottom, - sunDir, out hit, 30, LayerMask.GetMask("Player"))
            && hit.transform.GetComponent<PlayerController>().UnderLight)
        {
            hitByPlayer = true;
        }

        //detect whether this gameObject is in the shadow of any spotlight
        //spot light is optional
        UnderSpotLight = false;

        Collider[] spotLights = Physics.OverlapSphere(transform.position, 5f, LayerMask.GetMask("Light"));
        if(spotLights.Length > 0)
        {
            foreach(Collider c in spotLights)
            {
                Vector3 playerToLight = transform.position - c.transform.position;
                // Debug.Log("player to light direction = " + playerToLight );
                playerToLight.Normalize();
                playerToLight *= 100;

                Debug.DrawLine(objectBottom, objectBottom - playerToLight, Color.yellow);

                if (!Physics.Raycast(objectBottom, - playerToLight, out hit, 30, LayerMask.GetMask("Wall"))
                    && !Physics.Raycast(objectBottom, - playerToLight, out hit, 30, LayerMask.GetMask("Player")))
                {
                    UnderSpotLight = true;
                }

                if(Physics.Raycast(objectBottom, - sunDir, out hit, 30, LayerMask.GetMask("Player"))
                    && hit.transform.GetComponent<PlayerController>().UnderLight)
                {
                    hitByPlayer = true;
                }
            }
        }

        NotifyObject();

    }

    //todo: use event system, this script should only be used to trigger relevant event
    private void NotifyObject()
    {
        switch (myObjectType)
        {
            case ObjectType.Player:

                if(!UnderSpotLight && !UnderDirLight)
                {
                    myPC.UnderLight = false;

                    if(hitByPlayer)
                    {
                        //detect whether the player is under the shadow of wall or other player
                        PlayerController losePlayer = hit.transform.GetComponent<PlayerController>();
                        losePlayer.Dead();

                        Debug.Log("Player" + myPC.Index + " hit Player" + losePlayer.Index);
                    }
                }
                else
                {
                    myPC.UnderLight = true;
                }
                break;

            case ObjectType.Item:
                if(!UnderSpotLight && !UnderDirLight)
                {
                    if(hitByPlayer && myMR.enabled)
                    {
                        PlayerController pc = hit.transform.GetComponent<PlayerController>();
                        Debug.Log("Player" + pc.Index + " collect this item");

                        //player can only pick up items when the items are under light                        {
                        if(pc.Score < 10)
                        {
                            //increase player score
                            pc.Score++;

                            //this item should disappear if it is picked up
                            myMR.enabled = false;
                        }
                    } 
                }
                break;
            
            case ObjectType.Battery:
                if(!UnderDirLight && !UnderSpotLight)
                {
                    myElevator.UnderLight = false;
                    Debug.Log("Battery is in the shadow");
                }
                else
                {
                    myElevator.UnderLight = true;
                    Debug.Log("Battery is under sun");

                }
                break;
                
            case ObjectType.Solar:
                if(!UnderDirLight && !UnderSpotLight)
                {
                    mySolar.UnderLight = false;
                    Debug.Log("Solar system is in the shadow");
                }
                else
                {
                    mySolar.UnderLight = true;
                    Debug.Log("Solar system is under sun");

                }
                break;
            case ObjectType.Umbrella:
                if(!UnderSpotLight && !UnderDirLight)
                {
                    if(hitByPlayer)                         
                    {
                        PlayerController pc = hit.transform.GetComponent<PlayerController>();

                        //this item should disappear if it is picked up
                        myMR.enabled = false;

                        pc.Umbrellable = true;
                    }
                }
                break;

        }
    }
}