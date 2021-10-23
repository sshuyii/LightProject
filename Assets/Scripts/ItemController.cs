using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField] private float spawnTimeLength;
    private float timer;
    [SerializeField] private List<GameObject> items;


    // Start is called before the first frame update
    void Start()
    {
        timer = spawnTimeLength;

        //item appear when game starts
        foreach(GameObject g in items)
        {
            g.GetComponent<ItemTimer>().Reset();
        }

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            foreach(GameObject g in items)
            {
                g.GetComponent<ItemTimer>().Reset();
            }

            timer = spawnTimeLength;
        }   
    }
}
