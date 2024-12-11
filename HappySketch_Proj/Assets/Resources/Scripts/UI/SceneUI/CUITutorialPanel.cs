using HakSeung;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CUITutorialPanel : CUIScene
{
    private const int TotalPlayersNum = 2;
    [SerializeField] private Transform[] playersUITransform = new Transform[TotalPlayersNum];
    [SerializeField] private TextMeshProUGUI[] playerActionCount = new TextMeshProUGUI[TotalPlayersNum];
    private Transform[] playersTransform = new Transform[TotalPlayersNum];

    protected override void InitUI()
    {
       
    }

    public void ActionCountSet(int playerNum, int actionCount)
    {
        if (playerNum >= TotalPlayersNum)
            return;
        if (actionCount >= 0)
            playerActionCount[playerNum].text = actionCount.ToString();
    }
}
