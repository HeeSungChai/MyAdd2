using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarActivator : MonoBehaviour
{
    public GameObject m_objHpFive;
    public GameObject m_objHpSix;
    public GameObject m_objHpSeven;
    public GameObject m_objHpEight;
    public GameObject m_objHpNine;
    public GameObject m_objHpTen;

    int m_iCharacterHP;

    void Start ()
    {
        int iCharacterLv = MyGlobals.StageMgr.m_iCharacterLv;
        //= PrefsMgr.Instance.GetCharacterLevel(MyGlobals.StageMgr.m_eCharacter);

        eTABLE_LIST eTable;
        switch (MyGlobals.StageMgr.m_eCharacter)
        {
            case eCHARACTER.ADD:
                eTable = eTABLE_LIST.CHAR_LEVEL_ADD;
                break;
            case eCHARACTER.SUB:
                eTable = eTABLE_LIST.CHAR_LEVEL_SUB;
                break;
            case eCHARACTER.MUL:
                eTable = eTABLE_LIST.CHAR_LEVEL_MUL;
                break;
            case eCHARACTER.DIV:
                eTable = eTABLE_LIST.CHAR_LEVEL_DIV;
                break;
            default:
                eTable = eTABLE_LIST.CHAR_LEVEL_ADD;
                break;
        }
        m_iCharacterHP = ((int)TableDB.Instance.GetData(eTable, iCharacterLv, eKEY_TABLEDB.i_LIFE_VALUE));

        ActivateAppropriateHPBar();
    }
	
	void ActivateAppropriateHPBar()
    {
        m_objHpFive.SetActive(false);
        m_objHpSix.SetActive(false);
        m_objHpSeven.SetActive(false);
        m_objHpEight.SetActive(false);
        m_objHpNine.SetActive(false);
        m_objHpTen.SetActive(false);

        switch (m_iCharacterHP)
        {
            case 5:
                m_objHpFive.SetActive(true);
                break;
            case 6:
                m_objHpSix.SetActive(true);
                break;
            case 7:
                m_objHpSeven.SetActive(true);
                break;
            case 8:
                m_objHpEight.SetActive(true);
                break;
            case 9:
                m_objHpNine.SetActive(true);
                break;
            case 10:
                m_objHpTen.SetActive(true);
                break;
            default:
                m_objHpFive.SetActive(true);
                break;
        }
	}
}
