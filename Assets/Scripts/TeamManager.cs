using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public int[] TeamScore = new int[2];
    public List<PlayerController> TeamOnePlayer;
    public List<PlayerController> TeamTwoPlayer;

    private void Start() 
    {
        TeamScore = new int[]{0, 0};
    }
    public void UpdateScore()
    {
        foreach(PlayerController pc in TeamOnePlayer)
        {
            TeamScore[0] += pc.Score;
        }

        foreach(PlayerController pc in TeamTwoPlayer)
        {
            TeamScore[1] += pc.Score;
        }
    }
}
