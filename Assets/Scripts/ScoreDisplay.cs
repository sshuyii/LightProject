using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    private TextMeshProUGUI myTMP;

    
    // Start is called before the first frame update
    void Start()
    {
        myTMP = GetComponent<TextMeshProUGUI>();

        myTMP.text = "Player" + playerController.Idx + " Score = " + playerController.Score;
    }

    // Update is called once per frame
    void Update()
    {
        myTMP.text = "Player" + playerController.Idx + " Score = " + playerController.Score;
    }
}
