using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [SerializeField] float maxHeight;
    [SerializeField] float speed;

    [SerializeField] private float initialHeight;


    private bool underSun;
    public bool UnderLight
    {
        get{ return underSun;}
        set
        {
            underSun = value;

            StopCoroutine("Movement");
            StartCoroutine("Movement", underSun);
        }
    }

    IEnumerator Movement (bool up)
    {
        if(up)
        {
            while((maxHeight + initialHeight - transform.position.y) > 0.05f)
            {
                transform.position += new Vector3(0, speed * Time.deltaTime, 0);
                Debug.Log("Elevator is moving up");

                yield return null;
            }
        }
        else
        {
            while(transform.position.y -  initialHeight > 0.05f)
            {
                transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
                Debug.Log("Elevator is moving down");

                yield return null;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        initialHeight = transform.position.y;
    }


}
