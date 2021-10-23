using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public SolarController[] SolarArray = new SolarController[2];
    public float[] TeamScoreArray = new float[2];
    public int[] ItemCountArray;

    public List<PlayerController> TeamOnePlayer;
    public List<PlayerController> TeamTwoPlayer;

    private void Start() 
    {
        TeamScoreArray = new float[]{0f, 0f};
        ItemCountArray = new int[]{0, 0};
    }

    public void UpdateScore()
    {
        foreach(PlayerController pc in TeamOnePlayer)
        {
            TeamScoreArray[0] += pc.Score;
        }

        foreach(PlayerController pc in TeamTwoPlayer)
        {
            TeamScoreArray[1] += pc.Score;
        }
    }

    private void Update() 
    {
        for(int i = 0; i < TeamScoreArray.Length; i++)
        {
            TeamScoreArray[i] = SolarArray[i].Score + ItemCountArray[i] * LevelManager.ItemScore;
        }
    }
}
