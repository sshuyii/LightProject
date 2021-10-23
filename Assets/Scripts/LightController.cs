using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] private float dirSpeed;
    [SerializeField] private float angleSpeed;

    [SerializeField] private float timePerRound;    //for fixed time direction rotation
    [SerializeField] private float timeAngle;    //for fixed time angle rotation

    
    private float initialAngle;
    private float initialDir;

    private float direction;
    public float Direction
    {
        get{ return direction;}
        set
        {
            direction = value;
            StopCoroutine("UpdateDirection");
            StartCoroutine("UpdateDirection", direction);
        }
    }

    private float angle;
    public float Angle
    {
        get{ return angle;}
        set
        {
            angle = value;
            StopCoroutine("UpdateAngle");
            StartCoroutine("UpdateAngle", angle);
        }
    }

    IEnumerator UpdateDirection (float f)
    {
        while(Mathf.Abs(transform.localEulerAngles.y - f)  > 0.05f)
        {
            Vector3 target = new Vector3(transform.localEulerAngles.x, f, transform.localEulerAngles.z);
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, target, dirSpeed * Time.deltaTime);

            Debug.Log("Updating light direction");
            yield return null;
        }
    }

    IEnumerator UpdateAngle (float f)
    {
        while(Mathf.Abs(transform.localEulerAngles.x - f)  > 0.05f)
        {    
            Vector3 target = new Vector3(f, transform.localEulerAngles.y, transform.localEulerAngles.z);
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, target, angleSpeed * Time.deltaTime);

            Debug.Log("Updating light angle");

            yield return null;
        }
    }

    private void Start() 
    {
        initialAngle = transform.localEulerAngles.x;
        initialDir = transform.localEulerAngles.y;

    }

    
    public void RotateDirFixedTime()
    {
        float speed = 360 / timePerRound;

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 
            transform.localEulerAngles.y + speed * Time.deltaTime, transform.localEulerAngles.z);    
    }

    public IEnumerator RotateAngleFixedTime(float angle)
    {
        float speed = (angle - initialAngle ) / timeAngle;

        while(Mathf.Abs(transform.localEulerAngles.x - angle)  > 0.5f)
        {    
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x + speed * Time.deltaTime, 
                transform.localEulerAngles.y, transform.localEulerAngles.z);    

                Debug.Log("speed = " + speed);

            yield return null;
        }  
    }
}
