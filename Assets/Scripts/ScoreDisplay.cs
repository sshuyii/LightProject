using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    private TeamManager teamManager;


    [SerializeField] private TextMeshProUGUI playerTMP;
    [SerializeField] private TextMeshProUGUI teamTMP0;
    [SerializeField] private TextMeshProUGUI teamTMP1;
    [SerializeField] private CanvasGroup umbrella;

    
    // Start is called before the first frame update
    void Start()
    {
        teamManager = GameObject.Find("/Managers").GetComponent<TeamManager>();
    }

    // Update is called once per frame
    void Update()
    {
        playerTMP.text = playerController.Score.ToString();
        teamTMP0.text = Mathf.RoundToInt(teamManager.TeamScoreArray[0]).ToString();
        teamTMP1.text = Mathf.RoundToInt(teamManager.TeamScoreArray[1]).ToString();

        if(playerController.Umbrellable) umbrella.alpha = 1;
        else umbrella.alpha = 0;
    }
}
