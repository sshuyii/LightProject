using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeDisplay;
    [SerializeField] private float timeLength;
    [SerializeField] private float timer = 0;

    private LightController lightController;
    private void Start() 
    {   
        lightController = GameObject.Find("Lights/Directional Light").GetComponent<LightController>();
        
    }
    private void Update() 
    {
        if(timer < timeLength ) timer += Time.deltaTime;
        else
        {
            timer = timeLength;

            // EventManager.current.LevelEnd();
        }

        //display 
        timeDisplay.text = FormatTime(timeLength - timer);

        //change light direction and angle according to time
        if(timer < 30f)
        {
            lightController.Direction = 20;
        }
        else
        {
            lightController.Direction = 45;
        }
    }

    private string FormatTime(float time)
    {
        int minutes = (int) time / 60 ;
        int seconds = (int) time / 1 - 60 * minutes;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
