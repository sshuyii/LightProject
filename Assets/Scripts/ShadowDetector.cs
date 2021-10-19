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
        Solar
    }

    public ObjectType myObjectType;
    private GameObject light;
    private MeshRenderer mesh;
    private RaycastHit hit;

    private bool underLight = false;

    private Vector3 objectBottom;

    private PlayerController myPC;
    private ElevatorController myElevator;
    private SolarController mySolar;


    private float height;
     
     // Use this for initialization
    void Start () 
    {
        mesh = GetComponent<MeshRenderer>();

        light = GameObject.Find("Lights/Directional Light");
        myPC = GetComponent<PlayerController>();

        height = transform.localScale.y;

        if(myObjectType == ObjectType.Battery)
        {
            myElevator = transform.parent.GetComponent<ElevatorController>();
        }
        else if(myObjectType == ObjectType.Solar)
        {
            mySolar = GetComponent<SolarController>();
        }
    }
     
    // Update is called once per frame
    void Update () 
    {
        objectBottom = new Vector3(transform.position.x, transform.position.y - height/2, transform.position.z);


        //check whether this gameObject is under directional light
        underLight = false;
        Vector3 sunDir = light.transform.forward;

        Debug.Log("sun direction = " + sunDir );
        sunDir.Normalize();
        sunDir *= 100;
    
        Debug.DrawLine(objectBottom, objectBottom - sunDir, Color.red);

        if (!Physics.Raycast(objectBottom, - sunDir, out hit, 30, LayerMask.GetMask("Wall"))
            && !Physics.Raycast(objectBottom, - sunDir, out hit, 30, LayerMask.GetMask("Player")))
        {
            if(hit.transform != transform) underLight = true;
        }

        CheckUnderShadow(- sunDir);


        //detect whether this gameObject is in the shadow of any spotlight

        Collider[] spotLights = Physics.OverlapSphere(transform.position, 5f, LayerMask.GetMask("Light"));
        if(spotLights.Length > 0)
        {
            foreach(Collider c in spotLights)
            {
                Vector3 playerToLight = transform.position - c.transform.position;
                Debug.Log("player to light direction = " + playerToLight );
                playerToLight.Normalize();
                playerToLight *= 100;

                Debug.DrawLine(objectBottom, objectBottom - playerToLight, Color.yellow);

                if (!Physics.Raycast(objectBottom, - playerToLight, out hit, 30, LayerMask.GetMask("Wall"))
                    && !Physics.Raycast(objectBottom, - playerToLight, out hit, 30, LayerMask.GetMask("Player")))
                {
                     if(hit.transform != transform) underLight = true;
                }

                CheckUnderShadow(playerToLight);
            }
        }

        
    }

    private void CheckUnderShadow(Vector3 dir)
    {
        switch (myObjectType)
        {
            case ObjectType.Player:
                PlayerController thisPlayer = transform.GetComponent<PlayerController>();

                if(Physics.Raycast(objectBottom, dir, out hit, 30, LayerMask.GetMask("Player"))
                    || Physics.Raycast(objectBottom, dir, out hit, 30, LayerMask.GetMask("Wall")))
                {         
                    if(hit.transform != transform)
                    {
                        thisPlayer.UnderLight = false;
                        Debug.Log("Player" + thisPlayer.Index + " is under shadow");

                        if(Physics.Raycast(objectBottom, dir, out hit, 30, LayerMask.GetMask("Player")))
                        {
                            //detect whether the player is under the shadow of wall or other player
                            PlayerController losePlayer = hit.transform.GetComponent<PlayerController>();

                            Debug.Log("Player" + thisPlayer.Index + " hit Player" + losePlayer.Index);
                        }
                    } 
                }
                else
                {
                    thisPlayer.UnderLight = true;
                }
                break;

            case ObjectType.Item:
                if(Physics.Raycast(objectBottom, dir, out hit, 30, LayerMask.GetMask("Player")))
                {
                    int player = hit.transform.GetComponent<PlayerController>().Index;
                    Debug.Log("Player" + player + " collect this item");

                    //increase player score
                    hit.transform.GetComponent<PlayerController>().Score++;

                    //this item should disappear if it is picked up
                    gameObject.SetActive(false);
                }
                break;
            
            case ObjectType.Battery:
                if(Physics.Raycast(objectBottom, dir, out hit, 30, LayerMask.GetMask("Player"))
                    || Physics.Raycast(objectBottom, dir, out hit, 30, LayerMask.GetMask("Wall")))
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
                if(Physics.Raycast(objectBottom, dir, out hit, 30, LayerMask.GetMask("Player"))
                    || Physics.Raycast(objectBottom, dir, out hit, 30, LayerMask.GetMask("Wall")))
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
        }
    }
}