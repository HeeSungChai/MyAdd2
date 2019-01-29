using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eUSER_TITLE
{
    BEGINNER = 100,
    KINDERGARTEN,
    ELEMENTARY_SCHOOL,
    MIDDLE_SCHOOL,
    HIGH_SCHOOL,
    UNIVERSITY,
    DOCTOR,
    GOD_OF_MATH
}

public enum eCHARACTER
{
    ADD = 200,
    SUB,
    MUL,
    DIV,
    INFINITY_CHALLENGER,
}

public class LobbyMgr : MonoBehaviour
{
    [Header("Gold Info")]
    public UILabel m_labelGoldAmount;

    [Header("Title Info")]
    private eTABLE_LIST m_eTableTitle;
    public UISprite m_sprTitleIcon;
    public UILabel m_labelTitle;
    public UILabel m_labelGrade;
    eKEY_TABLEDB m_eKeyTitleName;
    //eKEY_TABLEDB m_eKeyGradeName;

    [Header("Character Info")]
    private eTABLE_LIST m_eTableChatacter;
    private eTABLE_LIST m_eTableCharacterLv;
    public UILabel m_labelCharName;
    public UILabel m_labelCharAbility;
    eKEY_TABLEDB m_eKeyCharName;
    eKEY_TABLEDB m_eKeyCharAbility;
    int m_iCurLevel;
    public GameObject m_objAdd;
    public GameObject m_objMi;
    public GameObject m_objDouble;
    public GameObject m_objDividivi;

    public UILabel m_labelBtnCharSelect;
    public UILabel m_labelBtnShop;
    public UILabel m_labelBtnStartAdventure;
    public UILabel m_labelBtnStartAdventure2;
    public UILabel m_labelBtnStartInfinite;

    private void Awake()
    {
        EventListener.AddListener("OnGoldAmountChanged", this);
        EventListener.AddListener("OnLanguageChanged", this);
        EventListener.AddListener("OnCharacterChanged", this);
    }

    void Start()
    {
        m_eTableTitle = eTABLE_LIST.TITLE_MARK;
        m_eTableChatacter = eTABLE_LIST.CHAR_INFO;
       
        OnGoldAmountChanged();
        OnLanguageChanged();
    }

    void OnGoldAmountChanged()
    {
        m_labelGoldAmount.text = MyUtility.CommaSeparateDigit(PrefsMgr.Instance.GetCoinAmount());
    }

    void OnLanguageChanged()
    {
        //언어설정에 따라 다른 키값 사용
        if (LanguageMgr.Instance.GetLanguage() == eLANGUAGE.KOREAN)
        {
            m_eKeyTitleName = eKEY_TABLEDB.s_TITLE_NAME_KR;
            //m_eKeyGradeName = eKEY_TABLEDB.s_GRADE_NAME_KR;
            m_eKeyCharName = eKEY_TABLEDB.s_CHAR_NAME_KR;
            m_eKeyCharAbility = eKEY_TABLEDB.s_SKILL_KR;
        }
        else
        {
            m_eKeyTitleName = eKEY_TABLEDB.s_TITLE_NAME_US;
            //m_eKeyGradeName = eKEY_TABLEDB.s_GRADE_NAME_US;
            m_eKeyCharName = eKEY_TABLEDB.s_CHAR_NAME_US;
            m_eKeyCharAbility = eKEY_TABLEDB.s_SKILL_US;
        }

        //m_labelBtnCharSelect.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.CHARACTER_SELECT_BUTTON);
        //m_labelBtnShop.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.SHOP_BUTTON);
        m_labelBtnStartAdventure.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_DATA.CHALLENGE);
        m_labelBtnStartAdventure2.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_DATA.CHALLENGE);
        m_labelBtnStartInfinite.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_DATA.INFINITY_CHALLENGE);

        OnCharacterChanged();
    }

    void OnCharacterChanged()
    {
        RefreshGradeInfo();

        RefreshCharacterInfo();

        ShowCharacter();
    }

    void RefreshGradeInfo()
    {
        m_labelTitle.text = (string)TableDB.Instance.GetData(m_eTableTitle,
            (int)MyGlobals.UserState.m_eTitle, m_eKeyTitleName);

        m_sprTitleIcon.spriteName = (string)TableDB.Instance.GetData(m_eTableTitle,
            (int)MyGlobals.UserState.m_eTitle, eKEY_TABLEDB.s_RESOURCE);
        m_sprTitleIcon.MakePixelPerfect();

        //string tempStr = (string)TableDB.Instance.GetData(m_eTableTitle,
        //    (int)MyGlobals.UserState.m_eTitle, m_eKeyGradeName);

        //if (tempStr == null || tempStr == "noText")
        //    m_labelGrade.text = "";
        //else
        //    m_labelGrade.text = tempStr;
    }

    void RefreshCharacterInfo()
    {
        UpdateCharacterTable();

        //m_labelCharName.text = (string)TableDB.Instance.GetData(m_eTableChatacter,
        //    (int)PrefsMgr.Instance.GetChoosenCharacter(), m_eKeyCharName);

        m_labelCharAbility.text = string.Format(
            (string)TableDB.Instance.GetData(m_eTableChatacter,
                (int)PrefsMgr.Instance.GetChoosenCharacter(), m_eKeyCharAbility),
            TableDB.Instance.GetData(m_eTableCharacterLv,
                (int)m_iCurLevel, eKEY_TABLEDB.i_SKILL_VALUE));
    }

    void UpdateCharacterTable()
    {
        switch (PrefsMgr.Instance.GetChoosenCharacter())
        {
            case eCHARACTER.ADD:
                m_eTableCharacterLv = eTABLE_LIST.CHAR_LEVEL_ADD;
                m_iCurLevel = MyGlobals.UserState.m_iLvAdd;
                break;
            case eCHARACTER.SUB:
                m_eTableCharacterLv = eTABLE_LIST.CHAR_LEVEL_SUB;
                m_iCurLevel = MyGlobals.UserState.m_iLvSub;
                break;
            case eCHARACTER.MUL:
                m_eTableCharacterLv = eTABLE_LIST.CHAR_LEVEL_MUL;
                m_iCurLevel = MyGlobals.UserState.m_iLvMul;
                break;
            case eCHARACTER.DIV:
                m_eTableCharacterLv = eTABLE_LIST.CHAR_LEVEL_DIV;
                m_iCurLevel = MyGlobals.UserState.m_iLvDiv;
                break;
            default:
                m_eTableCharacterLv = eTABLE_LIST.CHAR_LEVEL_ADD;
                m_iCurLevel = MyGlobals.UserState.m_iLvAdd;
                break;
        }
    }

    public void OnPress_Coin()
    {

    }

    public void OnPress_Character()
    {

    }

    void ShowCharacter()
    {
        m_objAdd.SetActive(false);
        m_objMi.SetActive(false);
        m_objDouble.SetActive(false);
        m_objDividivi.SetActive(false);
        switch (PrefsMgr.Instance.GetChoosenCharacter())
        {
            case eCHARACTER.ADD:
                m_objAdd.SetActive(true);
                break;
            case eCHARACTER.SUB:
                m_objMi.SetActive(true);
                break;
            case eCHARACTER.MUL:
                m_objDouble.SetActive(true);
                break;
            case eCHARACTER.DIV:
                m_objDividivi.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void OnPress_CharacterLeft()
    {
        switch (PrefsMgr.Instance.GetChoosenCharacter())
        {
            case eCHARACTER.ADD:
                //PrefsMgr.Instance.geto
                //PrefsMgr.Instance.SetChoosenCharacter(eCHARACTER.SUB);
                break;
            case eCHARACTER.SUB:
                PrefsMgr.Instance.SetChoosenCharacter(eCHARACTER.ADD);
                break;
            case eCHARACTER.MUL:
                PrefsMgr.Instance.SetChoosenCharacter(eCHARACTER.SUB);
                break;
            case eCHARACTER.DIV:
                PrefsMgr.Instance.SetChoosenCharacter(eCHARACTER.MUL);
                break;
            default:
                break;
        }
        ShowCharacter();
        RefreshCharacterInfo();
    }

    public void OnPress_CharacterRight()
    {
        switch(PrefsMgr.Instance.GetChoosenCharacter())
        {
            case eCHARACTER.ADD:
                if(PrefsMgr.Instance.GetCharacterOpen(eCHARACTER.SUB))
                    PrefsMgr.Instance.SetChoosenCharacter(eCHARACTER.SUB);
                break;
            case eCHARACTER.SUB:
                if (PrefsMgr.Instance.GetCharacterOpen(eCHARACTER.MUL))
                    PrefsMgr.Instance.SetChoosenCharacter(eCHARACTER.MUL);
                break;
            case eCHARACTER.MUL:
                if (PrefsMgr.Instance.GetCharacterOpen(eCHARACTER.DIV))
                    PrefsMgr.Instance.SetChoosenCharacter(eCHARACTER.DIV);
                break;
            default:
                break;
        }
        ShowCharacter();
        RefreshCharacterInfo();
    }

    public void OnPress_CharacterChoice()
    {

    }

    public void OnPress_Shop()
    {

    }

    public void OnPress_StartAdventure()
    {

    }

    public void OnPress_StartInfinity()
    {

    }
}
