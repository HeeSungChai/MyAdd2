﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eOPERATOR
{
    ADDITION,
    SUBTRACTION,
    MULTIPLICATION,
    DIVISION,
}

public class InputCtrl : MonoBehaviour
{
    public GameObject[] m_arrObjLeftDigits;
    public GameObject[] m_arrObjRightDigits;
    public GameObject[] m_arrObjOperators;
    public float m_fInputDeactivateDuration;
    bool m_bInputAllowed;
    RePositioner[] m_arrScriptPositionerLeft;
    RePositioner[] m_arrScriptPositionerRight;
    InputButtonCtrl[] m_arrScriptInputButtonLeft;
    InputButtonCtrl[] m_arrScriptInputButtonRight;
    InputButtonCtrl[] m_arrScriptInputButtonOperator;
    public SuperGaugeCtrl m_scriptGaugeCtrl;
    public UILabel m_labelAnswer;
    
    int m_iCurAnswer;
    int m_iCorrectAnswerLeft;
    int m_iCorrectAnswerRight;
    int m_iCorrectAnswerOperator;
    int m_iSelectedDigitLeft;
    int m_iSelectedDigitRight;
    public eOPERATOR m_eSelected_Operator;
    int m_iIndexSelectedLeft;
    int m_iIndexSelectedRight;
    List<int> m_listLeftDigits;
    List<int> m_listRightDigits;
    List<int> m_listCandidates;

    bool m_bSelected_Left;
    bool m_bSelected_Right;
    bool m_bSelected_Operator;

    private void Awake()
    {
        m_listLeftDigits = new List<int>();
        m_listRightDigits = new List<int>();
        m_listCandidates = new List<int>();

        MyGlobals.InputCtrl = this;
        EventListener.AddListener("OnTargetChanged", this);
    }

    void Start ()
    {
        m_bInputAllowed = false;

        m_arrScriptPositionerLeft = new RePositioner[m_arrObjLeftDigits.Length];
        m_arrScriptInputButtonLeft = new InputButtonCtrl[m_arrObjLeftDigits.Length];
        for (int i = 0; i < m_arrObjLeftDigits.Length; ++i)
        {
            m_arrScriptPositionerLeft[i] = m_arrObjLeftDigits[i].GetComponentInChildren<RePositioner>();
            m_arrScriptInputButtonLeft[i] = m_arrObjLeftDigits[i].GetComponentInChildren<InputButtonCtrl>();
        }

        m_arrScriptPositionerRight = new RePositioner[m_arrObjRightDigits.Length];
        m_arrScriptInputButtonRight = new InputButtonCtrl[m_arrObjRightDigits.Length];
        for (int i = 0; i < m_arrObjRightDigits.Length; ++i)
        {
            m_arrScriptPositionerRight[i] = m_arrObjRightDigits[i].GetComponentInChildren<RePositioner>();
            m_arrScriptInputButtonRight[i] = m_arrObjRightDigits[i].GetComponentInChildren<InputButtonCtrl>();
        }

        m_arrScriptInputButtonOperator = new InputButtonCtrl[m_arrObjOperators.Length];
        for(int i = 0; i < m_arrObjOperators.Length; ++i)
        {
            m_arrScriptInputButtonOperator[i] = m_arrObjOperators[i].GetComponentInChildren<InputButtonCtrl>();
        }

        m_iIndexSelectedLeft = -1;
        m_iIndexSelectedRight = -1;
        m_bSelected_Left = false;
        m_bSelected_Right = false;
        m_bSelected_Operator = false;
        EventListener.Broadcast("OnDeselectAll");

        SetOperatorCondition();
    }

    public void SetOperatorCondition()
    {
        if (MyGlobals.StageMgr.GameType == INGAME_TYPE.ADVENTURE)
        {
            //PrefsMgr.Instance.InitializeAllState();
            //연산기호에 해당하는 캐릭터를 획득한 경우에만 해당 오퍼레이터 활성화
            //if (PrefsMgr.Instance.GetCharacterOpen(eCHARACTER.SUB) == false)
            //    m_arrScriptInputButtonOperator[(int)eOPERATOR.SUBTRACTION].DisableButton();
            //if (PrefsMgr.Instance.GetCharacterOpen(eCHARACTER.MUL) == false)
            //    m_arrScriptInputButtonOperator[(int)eOPERATOR.MULTIPLICATION].DisableButton();
            //if (PrefsMgr.Instance.GetCharacterOpen(eCHARACTER.DIV) == false)
            //    m_arrScriptInputButtonOperator[(int)eOPERATOR.DIVISION].DisableButton();
            if (MyGlobals.StageMgr.m_eMaxOperator < eOPERATOR.DIVISION)
                m_arrScriptInputButtonOperator[(int)eOPERATOR.DIVISION].DisableButton();
            else if (MyGlobals.StageMgr.m_eMaxOperator < eOPERATOR.MULTIPLICATION)
                m_arrScriptInputButtonOperator[(int)eOPERATOR.MULTIPLICATION].DisableButton();
            else if (MyGlobals.StageMgr.m_eMaxOperator < eOPERATOR.SUBTRACTION)
                m_arrScriptInputButtonOperator[(int)eOPERATOR.SUBTRACTION].DisableButton();
        }
    }
	
    public void OnTargetChanged()
    {
        ResetDigits();
    }

    void ResetDigits()
    {
        m_iCurAnswer = MyGlobals.DigitSpawner.LowestDigit;
        if (m_iCurAnswer == -1)
            return;

        if(MyGlobals.StageMgr.m_eInputType == eINPUT_TYPE.FROM_ONE_TO_EIGHTYONE)
            SetFormulaForAnswer();
        else
            SetFormulaForAnswer2();

        AllocateInputDigits();
    }

    //1~81의 숫자를 사용하는 버전
    void SetFormulaForAnswer()
    {
        //일거리. 빼기, 곱하기, 나누기 캐릭터가 오픈되지 않은 경우에는 중간 연산자는 더하기만 가능하도록 해야함
        m_iCorrectAnswerOperator = Random.Range(0, (int)eOPERATOR.DIVISION + 1);
        if (m_iCorrectAnswerOperator == (int)eOPERATOR.DIVISION && m_iCurAnswer > 45)
            m_iCorrectAnswerOperator = Random.Range(0, (int)eOPERATOR.DIVISION);
        else if (m_iCurAnswer == 0)
            m_iCorrectAnswerOperator = Random.Range(0, 2) == 0 ? (int)eOPERATOR.SUBTRACTION : (int)eOPERATOR.MULTIPLICATION;

        switch ((eOPERATOR)m_iCorrectAnswerOperator)
        {
            //더하기일 경우 (정답 - 정답보다 작은 랜덤 왼쪽숫자) = 오른쪽 숫자
            case eOPERATOR.ADDITION:
                {
                    m_iCorrectAnswerLeft = Random.Range(0, m_iCurAnswer+1);
                    m_iCorrectAnswerRight = m_iCurAnswer - m_iCorrectAnswerLeft;
                }
                break;
            //뺄셈일 경우 (정답보다 큰 랜덤 왼쪽숫자 - 정답) = 오른쪽 숫자
            case eOPERATOR.SUBTRACTION:
                {
                    m_iCorrectAnswerLeft = Random.Range(m_iCurAnswer + 1, MyGlobals.MaxValue + 1);
                    m_iCorrectAnswerRight = m_iCorrectAnswerLeft - m_iCurAnswer;
                }
                break;
            //곱하기일 경우 
            case eOPERATOR.MULTIPLICATION:
                {
                    m_listCandidates.Clear();
                    for (int i = 1; i <= m_iCurAnswer; ++i)
                    {
                        if (m_iCurAnswer % i == 0)
                            m_listCandidates.Add(i);
                    }
                    m_iCorrectAnswerLeft = m_listCandidates[Random.Range(0, m_listCandidates.Count)];
                    m_iCorrectAnswerRight = m_iCurAnswer / m_iCorrectAnswerLeft;
                }
                break;
            //나누기일 경우
            case eOPERATOR.DIVISION:
                {
                    m_listCandidates.Clear();
                    for (int i = 1; i <= MyGlobals.MaxValue; ++i)
                    {
                        if (m_iCurAnswer * i <= MyGlobals.MaxValue)
                            m_listCandidates.Add(i);
                    }
                    m_iCorrectAnswerRight = m_listCandidates[Random.Range(0, m_listCandidates.Count)];
                    m_iCorrectAnswerLeft = m_iCurAnswer * m_iCorrectAnswerRight;
                }
                break;
            default:
                break;
        }

        ShowAnswer();
    }

    void SetFormulaForAnswer2()//1~9의 숫자만 사용하는 버전
    {
        iAllocatingCount = 0;
        AllocateOperator();

        switch ((eOPERATOR)m_iCorrectAnswerOperator)
        {
            //더하기일 경우 (정답 - 정답보다 작은 랜덤 왼쪽숫자) = 오른쪽 숫자
            case eOPERATOR.ADDITION:
                {
                    if (m_iCurAnswer > MyGlobals.MaxInputValue)
                        m_iCorrectAnswerLeft = Random.Range(m_iCurAnswer - MyGlobals.MaxInputValue, MyGlobals.MaxInputValue);
                    else
                        m_iCorrectAnswerLeft = Random.Range(0, m_iCurAnswer+1);

                    m_iCorrectAnswerRight = m_iCurAnswer - m_iCorrectAnswerLeft;
                }
                break;
            //뺄셈일 경우 (정답보다 큰 랜덤 왼쪽숫자 - 정답) = 오른쪽 숫자
            case eOPERATOR.SUBTRACTION:
                {
                    m_iCorrectAnswerLeft = Random.Range(m_iCurAnswer, MyGlobals.MaxInputValue + 1);
                    m_iCorrectAnswerRight = m_iCorrectAnswerLeft - m_iCurAnswer;
                }
                break;
            //곱하기일 경우 
            case eOPERATOR.MULTIPLICATION:
                {
                    m_listCandidates.Clear();

                    if (m_iCurAnswer == 0)
                    {
                        for (int i = 1; i <= MyGlobals.MaxInputValue; ++i)
                        {
                            m_listCandidates.Add(i);
                        }
                        m_iCorrectAnswerRight = 0;
                    }
                    else
                    {
                        for (int i = 1; i <= MyGlobals.MaxInputValue; ++i)
                        {
                            if ((m_iCurAnswer % i == 0) && ((m_iCurAnswer / i) <= MyGlobals.MaxInputValue))
                                m_listCandidates.Add(i);
                        }
                        if (m_listCandidates.Count < 1)
                            m_iCorrectAnswerLeft = 1;
                        else
                            //Range Min을 0으로 두면 예를들어 15를 위해 왼쪽1 오른쪽 15가 될수도 있으므로 Min을 1로 두어 3*5등이 나오도록
                            m_iCorrectAnswerLeft = m_listCandidates[Random.Range(0, m_listCandidates.Count)];
                        m_iCorrectAnswerRight = m_iCurAnswer / m_iCorrectAnswerLeft;
                    }

                    if(Random.Range(0, 2) == 0)
                        MyUtility.Swap<int>(ref m_iCorrectAnswerLeft, ref m_iCorrectAnswerRight);
                }
                break;
            //나누기일 경우
            case eOPERATOR.DIVISION:
                {
                    m_listCandidates.Clear();
                    for (int i = 1; i <= MyGlobals.MaxInputValue; ++i)
                    {
                        if (m_iCurAnswer * i <= MyGlobals.MaxInputValue)
                            m_listCandidates.Add(i);
                    }
                    m_iCorrectAnswerRight = m_listCandidates[Random.Range(0, m_listCandidates.Count)];
                    m_iCorrectAnswerLeft = m_iCurAnswer * m_iCorrectAnswerRight;
                }
                break;
            default:
                break;
        }

        ShowAnswer();
    }

    int iAllocatingCount;
    void AllocateOperator()
    {
        if (m_iCurAnswer > 18)      //한자리 숫자만 사용하게 되면 18을 초과하는 수는 무조건 연산기호가 곱하기가 되고
            m_iCorrectAnswerOperator = (int)eOPERATOR.MULTIPLICATION;
        else if (m_iCurAnswer > 9)   //9를 초과하는 수는 빼기와 나누기가 답이 될 수 없음
        {
            if (m_iCurAnswer == 11 || m_iCurAnswer == 13 || m_iCurAnswer == 17)
                m_iCorrectAnswerOperator = (int)eOPERATOR.ADDITION;
            else
                m_iCorrectAnswerOperator = Random.Range(0, 2) == 0 ? (int)eOPERATOR.ADDITION : (int)eOPERATOR.MULTIPLICATION;
        }
        //else if (m_iCurAnswer > 8)
        //    m_iCorrectAnswerOperator = Random.Range(0, 3) == 0 ? (int)eOPERATOR.DIVISION : Random.Range(0, 2) == 0 ? (int)eOPERATOR.ADDITION : (int)eOPERATOR.MULTIPLICATION;
        else if (m_iCurAnswer == 0)
            m_iCorrectAnswerOperator = Random.Range(0, 2) == 0 ? (int)eOPERATOR.SUBTRACTION : (int)eOPERATOR.MULTIPLICATION;
        else
            //m_iCorrectAnswerOperator = Random.Range(0, (int)eOPERATOR.DIVISION + 1);
            m_iCorrectAnswerOperator = Random.Range(0, (int)MyGlobals.StageMgr.m_eMaxOperator + 1);

        if(iAllocatingCount > 10)//주어진 조건하에 도저히 답이 안나올 때에 대한 예외처리
        {
            MyUtility.DebugLog("Allocating too many times");
            MyGlobals.DigitSpawner.m_scriptTarget.SetDigitByForce(5);
            MyGlobals.DigitSpawner.LowestDigit = 5;
            m_iCurAnswer = 5;
            m_iCorrectAnswerOperator = (int)eOPERATOR.ADDITION;
            return;
        }

        //if (PrefsMgr.Instance.GetOperatorOpen((eOPERATOR)m_iCorrectAnswerOperator) == false)
        if (m_iCorrectAnswerOperator > (int)MyGlobals.StageMgr.m_eMaxOperator)
        {
            ++iAllocatingCount;
            AllocateOperator();
            return;
        }
    }

    string tempOperator;
    void ShowAnswer()
    {
#if DEBUG
        switch (m_iCorrectAnswerOperator)
        {
            case 0:
                tempOperator = " + ";
                break;
            case 1:
                tempOperator = " - ";
                break;
            case 2:
                tempOperator = " * ";
                break;
            case 3:
                tempOperator = " / ";
                break;
        }
        m_labelAnswer.text = m_iCorrectAnswerLeft.ToString() + tempOperator + m_iCorrectAnswerRight.ToString();
#endif
    }

    void AllocateInputDigits()
    {
        m_listLeftDigits.Clear();
        for (int i = 0; i < m_arrScriptPositionerLeft.Length; ++i)
        {
            m_listLeftDigits.Add(0);
        }
        //정답 숫자를 랜덤한 위치에 배치
        int iRandomIndex = Random.Range(0, m_arrScriptPositionerLeft.Length);
        m_arrScriptPositionerLeft[iRandomIndex].ResetDigit(m_iCorrectAnswerLeft);
        m_listLeftDigits[iRandomIndex] = m_iCorrectAnswerLeft;

        int iMax;
        if (MyGlobals.StageMgr.m_eInputType == eINPUT_TYPE.FROM_ONE_TO_NINE)
            iMax = MyGlobals.MaxInputValue + 1;
        else
            iMax = MyGlobals.MaxValue + 1;

        int iRandomValue;
        for (int i = 0; i < m_arrScriptPositionerLeft.Length; ++i)
        {
            if(i != iRandomIndex)
            {
                do
                {
                    iRandomValue = Random.Range(0, iMax);
                }
                while (m_listLeftDigits.Contains(iRandomValue));

                m_arrScriptPositionerLeft[i].ResetDigit(iRandomValue);
                m_listLeftDigits[i] = iRandomValue;
            }
        }

        m_listRightDigits.Clear();
        for (int i = 0; i < m_arrScriptPositionerRight.Length; ++i)
        {
            m_listRightDigits.Add(0);
        }
        iRandomIndex = Random.Range(0, m_arrScriptPositionerRight.Length);
        m_arrScriptPositionerRight[iRandomIndex].ResetDigit(m_iCorrectAnswerRight);
        m_listRightDigits[iRandomIndex] = m_iCorrectAnswerRight;

        for (int i = 0; i < m_arrScriptPositionerRight.Length; ++i)
        {
            if (i != iRandomIndex)
            {
                do
                {
                    iRandomValue = Random.Range(0, iMax);
                }
                while (m_listRightDigits.Contains(iRandomValue));

                m_arrScriptPositionerRight[i].ResetDigit(iRandomValue);
                m_listRightDigits[i] = iRandomValue;
            }
        }

        ResetSelection();
    }
    
    void CheckIsCorrectAnswer()
    {
        int iUserAnswer;
        if (m_bSelected_Left && m_bSelected_Right && m_bSelected_Operator)
        {
            switch(m_eSelected_Operator)
            {
                case eOPERATOR.ADDITION:
                    iUserAnswer = m_iSelectedDigitLeft + m_iSelectedDigitRight;
                    break;
                case eOPERATOR.SUBTRACTION:
                    iUserAnswer = m_iSelectedDigitLeft - m_iSelectedDigitRight;
                    break;
                case eOPERATOR.MULTIPLICATION:
                    iUserAnswer = m_iSelectedDigitLeft * m_iSelectedDigitRight;
                    break;
                case eOPERATOR.DIVISION:
                    {
                        if(m_iSelectedDigitRight == 0)
                        {
                            IsWrongAnswer();
                            return;
                        }
                        iUserAnswer = m_iSelectedDigitLeft / m_iSelectedDigitRight;
                    }
                    break;
                default:
                    iUserAnswer = 0;
                    break;
            }

            m_iCurAnswer = MyGlobals.DigitSpawner.LowestDigit;
            if (iUserAnswer == m_iCurAnswer)
            {
                IsCorrectAnswer();
                ResetSelection();
            }
            else
            {
                IsWrongAnswer();
                Invoke("ResetSelection", 0.2f);
                MyGlobals.SoundMgr.OnPlayFx(eSOUND_FX.WRONG_ANSWER);
            }
        }
    }

    void IsCorrectAnswer()
    {
        //해당 숫자 지우고, 점수 올리고, 필살기 게이지 채우고
        EventListener.Broadcast("OnCorrectAnswer", false);

        //일거리. 인피닛 모드면 콤보 수 누적

        MyUtility.DebugLog("IsCorrect");
    }    

    void IsWrongAnswer()
    {
        ResetSelection();

        MyUtility.DebugLog("IsWrong");
    }

    void ResetSelection()
    {
        m_iIndexSelectedLeft = -1;
        m_iIndexSelectedRight = -1;
        m_bSelected_Left = false;
        m_bSelected_Right = false;
        m_bSelected_Operator = false;
        EventListener.Broadcast("OnDeselectAll");
        StopCoroutine("CoroutineDeactivateInputDuringRelocation");
        StartCoroutine("CoroutineDeactivateInputDuringRelocation");
    }
    
    IEnumerator CoroutineDeactivateInputDuringRelocation()
    {
        m_bInputAllowed = false;

        yield return new WaitForSeconds(m_fInputDeactivateDuration);

        m_bInputAllowed = true;
    }

    void SelectLeft(int iIndex)
    {
        if (!m_bInputAllowed)
            return;

        //아무것도 선택되지 않은 경우
        if (m_iIndexSelectedLeft == -1)
        {
            m_arrScriptInputButtonLeft[iIndex].Select();
            m_bSelected_Left = true;
            m_iSelectedDigitLeft = m_listLeftDigits[iIndex];
            m_iIndexSelectedLeft = iIndex;
            CheckIsCorrectAnswer();
            MyGlobals.SoundMgr.OnPlayFx(eSOUND_FX.INPUT_TOUCH_FIRST);
            return;
        }

        //기존 선택과 다른걸 선택한 경우
        if (m_iIndexSelectedLeft != iIndex)
        {
            m_arrScriptInputButtonLeft[iIndex].Select();
            m_arrScriptInputButtonLeft[m_iIndexSelectedLeft].Deselect();
            m_bSelected_Left = true;
            m_iSelectedDigitLeft = m_listLeftDigits[iIndex];
            m_iIndexSelectedLeft = iIndex;
            CheckIsCorrectAnswer();
            MyGlobals.SoundMgr.OnPlayFx(eSOUND_FX.INPUT_TOUCH_FIRST);
        }
        else    //선택된 숫자를 한번 더 누른 경우에는 선택 해제
        {
            m_arrScriptInputButtonLeft[m_iIndexSelectedLeft].Deselect();
            m_bSelected_Left = false;
            m_iSelectedDigitLeft = 0;
            m_iIndexSelectedLeft = -1;
            MyGlobals.SoundMgr.OnPlayFx(eSOUND_FX.INPUT_TOUCH_SECOND);
        }
    }

    void SelectRight(int iIndex)
    {
        if (!m_bInputAllowed)
            return;

        if (m_iIndexSelectedRight == -1)
        {
            m_arrScriptInputButtonRight[iIndex].Select();
            m_bSelected_Right = true;
            m_iSelectedDigitRight = m_listRightDigits[iIndex];
            m_iIndexSelectedRight = iIndex;
            CheckIsCorrectAnswer();
            MyGlobals.SoundMgr.OnPlayFx(eSOUND_FX.INPUT_TOUCH_FIRST);
            return;
        }

        if (m_iIndexSelectedRight != iIndex)
        {
            m_arrScriptInputButtonRight[iIndex].Select();
            m_arrScriptInputButtonRight[m_iIndexSelectedRight].Deselect();
            m_bSelected_Right = true;
            m_iSelectedDigitRight = m_listRightDigits[iIndex];
            m_iIndexSelectedRight = iIndex;
            CheckIsCorrectAnswer();
            MyGlobals.SoundMgr.OnPlayFx(eSOUND_FX.INPUT_TOUCH_FIRST);
        }
        else
        {
            m_arrScriptInputButtonRight[m_iIndexSelectedRight].Deselect();
            m_bSelected_Right = false;
            m_iSelectedDigitRight = 0;
            m_iIndexSelectedRight = -1;
            MyGlobals.SoundMgr.OnPlayFx(eSOUND_FX.INPUT_TOUCH_SECOND);
        }
    }

    void SelectOperator(eOPERATOR eOperator)
    {
        if (!m_bInputAllowed)
            return;

        if (m_bSelected_Operator == false)
        {
            m_arrScriptInputButtonOperator[(int)eOperator].Select();
            m_bSelected_Operator = true;
            m_eSelected_Operator = eOperator;
            CheckIsCorrectAnswer();
            MyGlobals.SoundMgr.OnPlayFx(eSOUND_FX.INPUT_TOUCH_FIRST);
            return;
        }

        if (m_eSelected_Operator != eOperator)
        {
            m_arrScriptInputButtonOperator[(int)eOperator].Select();
            m_arrScriptInputButtonOperator[(int)m_eSelected_Operator].Deselect();
            m_bSelected_Operator = true;
            m_eSelected_Operator = eOperator;
            CheckIsCorrectAnswer();
            MyGlobals.SoundMgr.OnPlayFx(eSOUND_FX.INPUT_TOUCH_FIRST);
        }
        else
        {
            m_arrScriptInputButtonOperator[(int)m_eSelected_Operator].Deselect();
            m_bSelected_Operator = false;
            MyGlobals.SoundMgr.OnPlayFx(eSOUND_FX.INPUT_TOUCH_SECOND);
        }
    }

    public void OnSelect_Left_1()
    {
        SelectLeft(0);
    }

    public void OnSelect_Left_2()
    {
        SelectLeft(1);
    }

    public void OnSelect_Left_3()
    {
        SelectLeft(2);
    }

    public void OnSelect_Left_4()
    {
        SelectLeft(3);
    }

    public void OnSelect_Left_5()
    {
        SelectLeft(4);
    }

    public void OnSelect_Right_1()
    {
        SelectRight(0);
    }

    public void OnSelect_Right_2()
    {
        SelectRight(1);
    }

    public void OnSelect_Right_3()
    {
        SelectRight(2);
    }

    public void OnSelect_Right_4()
    {
        SelectRight(3);
    }

    public void OnSelect_Right_5()
    {
        SelectRight(4);
    }

    public void OnSelect_Operator_ADDITION()
    {
        SelectOperator(eOPERATOR.ADDITION);
    }

    public void OnSelect_Operator_SUBTRACTION()
    {
        SelectOperator(eOPERATOR.SUBTRACTION);
    }

    public void OnSelect_Operator_MULTIPLICATION()
    {
        SelectOperator(eOPERATOR.MULTIPLICATION);
    }

    public void OnSelect_Operator_DIVISION()
    {
        SelectOperator(eOPERATOR.DIVISION);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        EventListener.RemoveListener(this);
    }
}
