using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    private TeamManager teamManager;


    [SerializeField] private TextMeshProUGUI playerTMP;
    [SerializeField] private TextMeshProUGUI teamTMP;


    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Team Index = " + playerController.TeamIndex);

        teamManager = GameObject.Find("/Managers").GetComponent<TeamManager>();

        playerTMP.text = "Player" + playerController.Index + " Score = 0";
        teamTMP.text = "Team0 Score = 0";

    }

    // Update is called once per frame
    void Update()
    {
        playerTMP.text = "Player" + playerController.Index + " Score = " + playerController.Score;
        teamTMP.text = "Team" + teamManager.TeamScoreArray[playerController.TeamIndex];

    }
}
