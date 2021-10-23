using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmbrellaController : MonoBehaviour
{
    private PlayerController myPC;
    private ItemTimer myIT;

    // Start is called before the first frame update
    void Start()
    {
        myIT = GetComponent<ItemTimer>();
        myPC = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(myIT.Timer == 0)
        {
            myPC.Invincible = false;
        }
        
    }
}
