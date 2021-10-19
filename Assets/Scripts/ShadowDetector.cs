using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShadowDetector : MonoBehaviour
{
    public enum ObjectType
    {
        Player,
        Item
    }

    public ObjectType myObjectType;
    private GameObject light;
    private MeshRenderer mesh;
    private RaycastHit hit;

    private bool underSun = false;
    private Vector3 objectBottom;

    private PlayerController myPC;
     
     // Use this for initialization
    void Start () 
    {
        mesh = GetComponent<MeshRenderer>();

        light = GameObject.Find("Lights/Directional Light");
        myPC = GetComponent<PlayerController>();
    }
     
    // Update is called once per frame
    void Update () 
    {
        underSun = false;
        Vector3 sunDir = light.transform.forward;

        Debug.Log("sun direction = " + sunDir );
        sunDir.Normalize();
        sunDir *= 100;
    
        objectBottom = new Vector3(transform.position.x, 0, transform.position.z);

        Debug.DrawLine(objectBottom, objectBottom - sunDir, Color.red);

        if (!Physics.Raycast(objectBottom, - sunDir, out hit, 30, LayerMask.GetMask("Wall")))
        {
            underSun = true;
        }

        switch (myObjectType)
        {
            case ObjectType.Player:
                if(Physics.Raycast(objectBottom, - sunDir, out hit, 30, LayerMask.GetMask("Player")))
                {         
                    if(hit.transform != transform)
                    {
                        //detect whether the player is under the shadow of wall or other player
                        int winPlayer = transform.GetComponent<PlayerController>().Index;
                        int losePlayer = hit.transform.GetComponent<PlayerController>().Index;

                        Debug.Log("Player" + winPlayer + " hit Player" + losePlayer);
                    }
                }

                if(underSun)
                {
                    mesh.material.color = Color.red;
                    myPC.LightCollection += 2 * Time.deltaTime;
                }
                else 
                {
                    mesh.material.color = Color.green;
                    // myPC.LightCollection -= Time.deltaTime;
                }
                break;

            case ObjectType.Item:
                if(Physics.Raycast(objectBottom, - sunDir, out hit, 30, LayerMask.GetMask("Player")))
                {
                    int player = hit.transform.GetComponent<PlayerController>().Index;
                    Debug.Log("Player" + player + " collect this item");

                    //player score increases
                    hit.transform.GetComponent<PlayerController>().Score++;

                    //this item should disappear after being picked up
                    gameObject.SetActive(false);
                }
                break;

        }

     }
}