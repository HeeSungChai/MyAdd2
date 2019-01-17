using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardCtrl : MonoBehaviour
{
    public UISprite m_spriteRewardIcon;
    public UILabel m_labelRewardCount;

    public void Init(eREWARD_ID eRewardID, int iCount)
    {
        SetIcon(eRewardID);
        SetRewardCount(iCount);
    }

    void SetIcon(eREWARD_ID eRewardID)
    {
        switch(eRewardID)
        {
            case eREWARD_ID.COIN:
                m_spriteRewardIcon.spriteName = "Item_Clock";
                break;
            case eREWARD_ID.ERASER:
                m_spriteRewardIcon.spriteName = "Item_Eraser";
                break;
            case eREWARD_ID.CLOCK:
                m_spriteRewardIcon.spriteName = "Item_Clock";
                break;
            case eREWARD_ID.RECOVERY:
                m_spriteRewardIcon.spriteName = "Item_Recovery";
                break;
            default:
                break;
        }
    }

    void SetRewardCount(int iCount)
    {
        m_labelRewardCount.text = iCount.ToString();
    }
}
