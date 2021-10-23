using System.Collections.Generic;
using UnityEngine;

public class SolarController : MonoBehaviour
{
    [SerializeField] private int teamIndex;

    //todo: use static fields or scriptable object to keep the same value in all instances
    [SerializeField] private float selfSpeed;
    [SerializeField] private float accelerationPerPlayer;

    private ShadowDetector[] solarArray;

    [HideInInspector] public bool UnderLight;

    [HideInInspector] public float Score;

    private float acceleration;
    public float Acceleration
    {
        get{ return acceleration;}
        set
        {
            acceleration = value;
            if(acceleration <= 1) acceleration = 1;
        }
    }

   

    // Start is called before the first frame update
    void Start()
    {
        Acceleration = 1;

        solarArray = GetComponentsInChildren<ShadowDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(ShadowDetector sd in solarArray)
        {
            if(sd.UnderDirLight || sd.UnderSpotLight)
            {
                Score += Acceleration * selfSpeed * Time.deltaTime;
            }
        }
        Debug.Log("Player" + teamIndex + " acceleration = " + Acceleration);

    }

    public void AccelerationArea(bool playerIn)
    {
        if(playerIn) Acceleration += accelerationPerPlayer;
        else Acceleration -= accelerationPerPlayer;
    }
}
