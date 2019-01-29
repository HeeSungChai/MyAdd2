using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eITEM_ID
{
    COIN = 400,
    ERASER = 401,
    CLOCK = 402,
    RECOVERY = 403
}

public class RewardMgr : MonoBehaviour
{
    eTABLE_LIST m_eTableName = eTABLE_LIST.REWARD_TABLE;
    public RewardCtrl[] m_scriptRewardCtrl;

    int iIndex;
    int m_iReward1;
    int m_iReward1_Value;
    int m_iReward2;
    int m_iReward2_Value;
    int m_iReward3;
    int m_iReward3_Value;
    
    void OnEnable()
    {
        iIndex = 0;
        for (int i = 0; i < m_scriptRewardCtrl.Length; ++i)
        {
            m_scriptRewardCtrl[i].gameObject.SetActive(false);
        }

        GetRewardData();
    }

    void GetRewardData()
    {
        m_iReward1 = (int)TableDB.Instance.GetData(m_eTableName, MyGlobals.StageMgr.StageNum, eKEY_TABLEDB.i_REWARD_1);
        if (m_iReward1 != -1)
        {
            m_iReward1_Value = (int)TableDB.Instance.GetData(m_eTableName, MyGlobals.StageMgr.StageNum, eKEY_TABLEDB.i_REWARD_1_VALUE);
            ShowReward(iIndex, m_iReward1, m_iReward1_Value);
            ++iIndex;
        }

        m_iReward2 = (int)TableDB.Instance.GetData(m_eTableName, MyGlobals.StageMgr.StageNum, eKEY_TABLEDB.i_REWARD_2);
        if (m_iReward2 != -1)
        {
            m_iReward2_Value = (int)TableDB.Instance.GetData(m_eTableName, MyGlobals.StageMgr.StageNum, eKEY_TABLEDB.i_REWARD_2_VALUE);
            ShowReward(iIndex, m_iReward2, m_iReward2_Value);
            ++iIndex;
        }

        m_iReward3 = (int)TableDB.Instance.GetData(m_eTableName, MyGlobals.StageMgr.StageNum, eKEY_TABLEDB.i_REWARD_3);
        if (m_iReward3 != -1)
        {
            m_iReward3_Value = (int)TableDB.Instance.GetData(m_eTableName, MyGlobals.StageMgr.StageNum, eKEY_TABLEDB.i_REWARD_3_VALUE);
            ShowReward(iIndex, m_iReward3, m_iReward2_Value);
            ++iIndex;
        }        
    }

    void ShowReward(int iIndex, int iRewardID, int iValue)
    {
        m_scriptRewardCtrl[iIndex].gameObject.SetActive(true);
        m_scriptRewardCtrl[iIndex].Init((eITEM_ID)iRewardID, m_iReward1_Value);
    }    
}
