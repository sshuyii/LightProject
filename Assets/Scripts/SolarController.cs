using UnityEngine;

public class SolarController : MonoBehaviour
{
    [SerializeField] private int teamIndex;
    [SerializeField] private float speed;

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

    public bool UnderLight;

    public float Score;

    // Start is called before the first frame update
    void Start()
    {
        Acceleration = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(UnderLight)
        {
            Score += Acceleration * speed * Time.deltaTime;
        }

        Debug.Log("Player" + teamIndex + " acceleration = " + Acceleration);

    }
}
