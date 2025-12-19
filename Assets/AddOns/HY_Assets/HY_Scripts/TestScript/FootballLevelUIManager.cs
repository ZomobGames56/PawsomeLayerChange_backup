using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FootballLevelUIManager : MonoBehaviour
{
    public static FootballLevelUIManager Instance;
    
    
    public TextMeshProUGUI redTeamScoreTxt, blueTeamScoreTxt;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
   
}
