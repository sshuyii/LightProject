using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTimer : MonoBehaviour
{
    [SerializeField] private float timeLength;
    private MeshRenderer myMR;

    private float timer;
    public float Timer
    {
        get{ return timer;}
        set 
        {   
            timer = value;
            StopCoroutine("CalTime");
            StartCoroutine("CalTime");
           
        }
    }

    IEnumerator CalTime()
    {
        myMR.enabled = true;

        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        
        if(timer <= 0)
        {
            timer = 0;
            myMR.enabled = false;
        }
    
    }

    // Start is called before the first frame update
    void Awake()
    {
        myMR = GetComponent<MeshRenderer>();

    }

    public void Reset()
    {
        Timer = timeLength;
    }
}
