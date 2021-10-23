using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeDisplay;
    [SerializeField] private float timeLength;

    [SerializeField] private CanvasGroup restartButton;

    private float timer = 0;
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
        while (timer > 0) 
        {
            Time.timeScale = 1;

            timer -= Time.deltaTime;
            UIExpansion.Hide(restartButton);

            yield return null;
        }

        if(timer <= 0)
        {
            Debug.Log("Leven ends");

            timer = 0;
            UIExpansion.Show(restartButton);

            Time.timeScale = 0;
            EventManager.current.LevelEnd();
        }
    }

    public static float ItemScore = 5;

    private LightController lightController;

    private void Start() 
    {   
        Timer = timeLength;

        lightController = GameObject.Find("Lights/Directional Light").GetComponent<LightController>();
        StartCoroutine(lightController.RotateAngleFixedTime(30));
    }

    private void Update() 
    {
        //display level time count down
        timeDisplay.text = FormatTime(Timer);

        //light rotate around Y axix constantly
        lightController.RotateDirFixedTime();

    }

    private string FormatTime(float time)
    {
        int minutes = (int) time / 60 ;
        int seconds = (int) time / 1 - 60 * minutes;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Level");
    }
}
