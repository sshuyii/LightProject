using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeDisplay;
    [SerializeField] private float timeLength;
    private float timer = 0;
    public static float ItemScore = 5;

    private LightController lightController;

    private void Start() 
    {   
        lightController = GameObject.Find("Lights/Directional Light").GetComponent<LightController>();
        
        StartCoroutine(lightController.RotateAngleFixedTime(30));
    }

    private void Update() 
    {
        if(timer < timeLength ) timer += Time.deltaTime;
        else
        {
            timer = timeLength;

            // EventManager.current.LevelEnd();
        }

        //display level time count down
        timeDisplay.text = FormatTime(timeLength - timer);

        //light rotate around Y axix constantly
        lightController.RotateDirFixedTime();

    }

    private string FormatTime(float time)
    {
        int minutes = (int) time / 60 ;
        int seconds = (int) time / 1 - 60 * minutes;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
