using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ShadowType
{
    Spot,
    Directional
};

public class PlayerShadow : MonoBehaviour
{
    [SerializeField] private ShadowType myST;
    private GameObject currentLight;
    private LightController currentLC;

    [SerializeField] private PlayerController myPlayerController;
    private Transform playerTransform;
    private float playerHeight;

    [SerializeField]private Transform mainCollider;
    [SerializeField]private Transform subCollider1;


    
    private Vector2 spotLightPos;
    private float dist;
    private float angle;

    void Start()
    {
        if(myST == ShadowType.Directional) currentLight = GameObject.Find("Lights/Directional Light");
        else if(myST == ShadowType.Spot) currentLight = GameObject.Find("Spot Light");

        currentLC = currentLight.GetComponent<LightController>();

        playerTransform = myPlayerController.gameObject.transform;
        playerHeight = myPlayerController.Height;
    }

    void Update()
    {
        if(myST == ShadowType.Directional)
        {
            //set shadow rotation to light
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentLight.transform.eulerAngles.y, transform.eulerAngles.z);

            //set shadow length
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, playerHeight * Mathf.Tan(currentLC.Angle));
        }
        else if(myST == ShadowType.Spot)
        {
            mainCollider.position = SpotlightShadowPos();

            //set shadow rotation to light
            subCollider1.eulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
             
            //set shadow length
            subCollider1.localScale = new Vector3(subCollider1.localScale.x, subCollider1.localScale.y, 2 * SpotlightShadowLength());
        }
    }

   
    private Vector3 SpotlightShadowPos()
    {
        Vector2 spotLightPos = new Vector3(currentLight.transform.position.x, currentLight.transform.position.z);
        Vector2 playerPos = new Vector3(playerTransform.position.x, playerTransform.position.z);

        Vector2 relative =  playerPos - spotLightPos;

        float dist = 2 * Vector2.Distance(spotLightPos, playerPos);
        Debug.Log("Dist = " + dist);

        angle = Vector2.SignedAngle(relative, new Vector2(0, 1));
        Debug.Log("Angle = " + angle);

        Vector3 pos = new Vector3(playerTransform.position.x + Mathf.Sin(Mathf.Deg2Rad * angle) * dist, 0, 
            playerTransform.position.z + Mathf.Cos(Mathf.Deg2Rad * angle) * dist);

        return pos;
    }

    private float SpotlightShadowLength()
    {
        //this is the distance between center point
        //not precise but acceptable for the prototype stage
        Vector2 spotLightPos = new Vector3(currentLight.transform.position.x, currentLight.transform.position.z);
        Vector2 playerPos = new Vector3(playerTransform.position.x, playerTransform.position.z);

        float dist = 2 * Vector2.Distance(spotLightPos, playerPos);
        
        return dist;
    }
}

