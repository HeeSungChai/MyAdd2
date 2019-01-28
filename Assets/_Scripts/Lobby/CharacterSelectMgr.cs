using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class CharacterSelectMgr : MonoBehaviour
{
    [Header("Gold Info")]
    public UILabel m_labelGoldAmount;

    [Header("Character Btn")]
    public GameObject m_objAdd;
    public GameObject m_objSub;
    public GameObject m_objMul;
    public GameObject m_objDiv;
    public CharacterBtnCtrl m_scriptAddBtn;
    public CharacterBtnCtrl m_scriptSubBtn;
    public CharacterBtnCtrl m_scriptMulBtn;
    public CharacterBtnCtrl m_scriptDivBtn;

    [Header("Character Profile")]
    public UILabel m_labelName;
    public UILabel m_labelCharName;
    public UILabel m_labelCharStory;
    public UILabel m_labelSkillValue;
    public UILabel m_labelSkillExplanation;
    public TypeWriterReset m_scriptTypewriter;
    public UILabel m_labelSkill;
    public UILabel m_labelSkillLvCur;
    public UILabel m_labelSkillBonusCur;
    public UILabel m_labelSkillLvNext;
    public UILabel m_labelSkillBonusNext;
    public UILabel m_labelAquiredCoinAmount;
    public UILabel m_labelBtnUpgrade;
    private eTABLE_LIST m_eTableChatacter;
    private eTABLE_LIST m_eTableCharacterLv;
    eKEY_TABLEDB m_eKeyCharName;
    eKEY_TABLEDB m_eKeyCharStory;
    eKEY_TABLEDB m_eKeyCharSkillExplanation;

    private void Awake()
    {
        EventListener.AddListener("OnGoldAmountChanged", this);
        EventListener.AddListener("OnLanguageChanged", this);
        EventListener.AddListener("OnCharacterChanged", this);
    }

    void Start()
    {
        m_eTableChatacter = eTABLE_LIST.CHAR_INFO;

        OnGoldAmountChanged();
        OnLanguageChanged();
        //OnCharacterChanged();
    }

    void OnGoldAmountChanged()
    {
        m_labelGoldAmount.text = MyUtility.CommaSeparateDigit(MyGlobals.UserState.m_iCoinAmount);
    }

    void OnLanguageChanged()
    {
        if (LanguageMgr.Instance.GetLanguage() == eLANGUAGE.KOREAN)
        {
            m_eKeyCharName = eKEY_TABLEDB.s_CHAR_NAME_KR;
            m_eKeyCharStory = eKEY_TABLEDB.s_CHAR_STORY_KR;
            m_eKeyCharSkillExplanation = eKEY_TABLEDB.s_SKILL_KR;
        }
        else
        {
            m_eKeyCharName = eKEY_TABLEDB.s_CHAR_NAME_US;
            m_eKeyCharStory = eKEY_TABLEDB.s_CHAR_STORY_US;
            m_eKeyCharSkillExplanation = eKEY_TABLEDB.s_SKILL_US;
        }

        //if(m_labelName)
        //    m_labelName.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_ID.CHARACTER_SELECT_NAME);
        if(m_labelSkill)
            m_labelSkill.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_DATA.SKILL);
        if(m_labelBtnUpgrade)
            m_labelBtnUpgrade.text = LanguageMgr.Instance.GetLanguageData(eLANGUAGE_DATA.LEVEL_UP);

        RefreshCharacterInfo();
    }

    void OnCharacterChanged()
    {
        RefreshCharacterInfo();
    }

    void RefreshCharacterInfo()
    {
        RetargetCharacterSelect();

        int iChoosenCharacter = (int)PrefsMgr.Instance.GetChoosenCharacter();

        m_labelCharName.text = (string)TableDB.Instance.GetData(m_eTableChatacter,
            iChoosenCharacter, m_eKeyCharName);

        m_labelCharStory.text = (string)TableDB.Instance.GetData(m_eTableChatacter,
            iChoosenCharacter, m_eKeyCharStory);
        //m_scriptTypewriter.OnManualReset();
        
        int iSkillLevelCur = MyGlobals.UserState.GetCurSkillLv();
        int iSkillLevelNext = iSkillLevelCur + 1;

        m_labelSkillValue.text = ((int)TableDB.Instance.GetData(m_eTableCharacterLv,
                                    iSkillLevelCur, eKEY_TABLEDB.i_SKILL_VALUE)).ToString();

        m_labelSkillExplanation.text = string.Format(
            (string)TableDB.Instance.GetData(m_eTableChatacter,
                                    iChoosenCharacter, m_eKeyCharSkillExplanation),
                                TableDB.Instance.GetData(m_eTableCharacterLv,
                                    iSkillLevelCur, eKEY_TABLEDB.i_SKILL_VALUE));

        m_labelSkillLvCur.text = MyUtility.GetLevelText(MyGlobals.UserState.GetCurSkillLv());
        m_labelSkillBonusCur.text = "(+" + ((int)TableDB.Instance.GetData(m_eTableCharacterLv,
                                    iSkillLevelCur, eKEY_TABLEDB.i_SKILL_VALUE)).ToString() + ")";

        if (iSkillLevelNext <= 10)
        {
            m_labelSkillLvNext.text = MyUtility.GetLevelText(iSkillLevelNext);
            m_labelSkillBonusNext.text = "(+" + ((int)TableDB.Instance.GetData(m_eTableCharacterLv,
                                    iSkillLevelNext, eKEY_TABLEDB.i_SKILL_VALUE)).ToString() + ")";
            m_labelAquiredCoinAmount.text = ((int)TableDB.Instance.GetData(m_eTableCharacterLv,
                                    iSkillLevelNext, eKEY_TABLEDB.i_AMOUNT)).ToString();
        }
        else
        {
            m_labelSkillLvNext.text = "";
            m_labelSkillBonusNext.text = "";
            m_labelAquiredCoinAmount.text = "";
        }

        //tempString = tempString.Replace("\\n", "\n");
    }

    void RetargetCharacterSelect()
    {
        m_objAdd.SetActive(false);
        m_objSub.SetActive(false);
        m_objMul.SetActive(false);
        m_objDiv.SetActive(false);
        m_scriptAddBtn.OnDeactivate();
        m_scriptSubBtn.OnDeactivate();
        m_scriptMulBtn.OnDeactivate();
        m_scriptDivBtn.OnDeactivate();

        //EventListener.Broadcast("OnCharacterChanged", PrefsMgr.Instance.GetChoosenCharacter());

        switch (PrefsMgr.Instance.GetChoosenCharacter())
        {
            case eCHARACTER.ADD:
                m_eTableCharacterLv = eTABLE_LIST.CHAR_LEVEL_ADD;
                m_objAdd.SetActive(true);
                m_scriptAddBtn.OnActivate();
                break;
            case eCHARACTER.SUB:
                m_eTableCharacterLv = eTABLE_LIST.CHAR_LEVEL_SUB;
                m_objSub.SetActive(true);
                m_scriptSubBtn.OnActivate();
                break;
            case eCHARACTER.MUL:
                m_eTableCharacterLv = eTABLE_LIST.CHAR_LEVEL_MUL;
                m_objMul.SetActive(true);
                m_scriptMulBtn.OnActivate();
                break;
            case eCHARACTER.DIV:
                m_eTableCharacterLv = eTABLE_LIST.CHAR_LEVEL_DIV;
                m_objDiv.SetActive(true);
                m_scriptDivBtn.OnActivate();
                break;
            default:
                m_eTableCharacterLv = eTABLE_LIST.CHAR_LEVEL_ADD;
                m_objAdd.SetActive(true);
                m_scriptAddBtn.OnActivate();
                break;
        }
    }

    public void OnPress_Add()
    {
        //if (PrefsMgr.Instance.GetCharacterOpen(eCHARACTER.ADD) == false)
        //    return;

        //MyGlobals.UserState.m_eCurCharacter = eCHARACTER.ADD;
        PrefsMgr.Instance.SetChoosenCharacter(eCHARACTER.ADD);

        //OnCharacterChanged();
        EventListener.Broadcast("OnCharacterChanged");
    }

    public void OnPress_Sub()
    {
        if (PrefsMgr.Instance.GetCharacterOpen(eCHARACTER.SUB) == false)
            return;

        //MyGlobals.UserState.m_eCurCharacter = eCHARACTER.SUB;
        PrefsMgr.Instance.SetChoosenCharacter(eCHARACTER.SUB);

        //OnCharacterChanged();
        EventListener.Broadcast("OnCharacterChanged");
    }

    public void OnPress_Mul()
    {
        if (PrefsMgr.Instance.GetCharacterOpen(eCHARACTER.MUL) == false)
            return;

        //MyGlobals.UserState.m_eCurCharacter = eCHARACTER.MUL;
        PrefsMgr.Instance.SetChoosenCharacter(eCHARACTER.MUL);

        //OnCharacterChanged();
        EventListener.Broadcast("OnCharacterChanged");
    }

    public void OnPress_Div()
    {
        if (PrefsMgr.Instance.GetCharacterOpen(eCHARACTER.DIV) == false)
            return;

        //MyGlobals.UserState.m_eCurCharacter = eCHARACTER.DIV;
        PrefsMgr.Instance.SetChoosenCharacter(eCHARACTER.DIV);

        //OnCharacterChanged();
        EventListener.Broadcast("OnCharacterChanged");
    }
}
