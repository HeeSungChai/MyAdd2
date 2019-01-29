using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardCtrl : MonoBehaviour
{
    public UISprite m_spriteRewardIcon;
    public UILabel m_labelRewardCount;

    public void Init(eITEM_ID eRewardID, int iCount)
    {
        SetIcon(eRewardID);
        SetRewardCount(iCount);
        PrefsMgr.Instance.IncreaseItemAmount(eRewardID, iCount);
    }

    void SetIcon(eITEM_ID eRewardID)
    {
        switch(eRewardID)
        {
            case eITEM_ID.COIN:
                m_spriteRewardIcon.spriteName = "Item_Coin";
                break;
            case eITEM_ID.ERASER:
                m_spriteRewardIcon.spriteName = "Item_Eraser";
                break;
            case eITEM_ID.CLOCK:
                m_spriteRewardIcon.spriteName = "Item_Clock";
                break;
            case eITEM_ID.RECOVERY:
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
