using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitShadow : MonoBehaviour
{
    [SerializeField] private PlayerController myPlayerController;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {   
            PlayerController pc = other.gameObject.GetComponent<PlayerController>();
            if(!pc.InShadow)
            {
                pc.Score ++;
                pc.InShadow = true;

                Debug.Log("Player " + pc.Index + " hit the shadow of Player " + myPlayerController.Index);
            }
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.tag == "Player")   
        {
            PlayerController pc = other.gameObject.GetComponent<PlayerController>();

            pc.InShadow = false;
        } 
    }

}
