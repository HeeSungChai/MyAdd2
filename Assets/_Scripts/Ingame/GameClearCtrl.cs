using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearCtrl : MonoBehaviour
{
    [Header("Adventure Clear")]
    public UILabel m_labelClear;
    public UILabel m_labelCongratulation;
    public UILabel m_labelResult;
    public UILabel m_labelScore;
    public UILabel m_labelScorePoint;
    public UILabel m_labelBonus;
    public UILabel m_labelBonusPoint;
    public UILabel m_labelTotal;
    public UILabel m_labelTotalPoint;

    [Header("Reward Item")]
    public UISprite m_sprItemEraser;
    public UISprite m_sprItemClock;
    public UISprite m_sprItemRecovery;
    public UISprite m_sprItemCoin;

    void Start ()
    {
        StartCoroutine("CoroutineShowResult_AdventureClear");
	}
	
    IEnumerator CoroutineShowResult_AdventureClear()
    {


        yield return null;
	}

    public void OnGoBackToLobby()
    {

    }

    public void OnGoToNextLevel()
    {

    }
}
