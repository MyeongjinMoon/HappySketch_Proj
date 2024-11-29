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

/*    public void uiSetting()
    {
        playersTransform[0] = GameObject.FindWithTag("Player1").transform;
        playersTransform[1] = GameObject.FindWithTag("Player2").transform;

        for (int playerNum = 0; playerNum < TotalPlayersNum; playerNum++)
        {
            Vector3 uiPos = new Vector3(
                Camera.main.WorldToScreenPoint(playersTransform[playerNum].position).x,
                playersUITransform[playerNum].position.y,
                playersUITransform[playerNum].position.z
                );

            playersUITransform[playerNum].position = uiPos;
        }
    }*/


    


}
