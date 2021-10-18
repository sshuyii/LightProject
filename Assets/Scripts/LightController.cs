using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] private float direction;
    public float Direction
    {
        get{ return direction;}
        set
        {
            direction = value;
            UpdateLightDir(direction);
        }
    }

    [SerializeField] private float angle;
    public float Angle
    {
        get{ return angle;}
        set
        {
            angle = value;
            UpdateLightAngle(angle);
        }
    }

    private void UpdateLightAngle(float angle)
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, angle, transform.localEulerAngles.y);
    }

    private void UpdateLightDir(float dir)
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, direction, transform.localEulerAngles.y);
    }

}
