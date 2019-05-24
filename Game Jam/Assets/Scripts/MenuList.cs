using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuList : MonoBehaviour
{
    // References to all the requires text boxes for highscores
    public TextMeshProUGUI score;
    public TextMeshProUGUI minutescore;
    public HighScoreManager hs;

    private void Update()
    {
        Highscores();
    }

    public void Highscores()
    {
        hs.LoadScoresFromFile();
        score.text = hs.scoreArray[0].ToString();
        minutescore.text = hs.scoreArray[1].ToString();
    }
}

