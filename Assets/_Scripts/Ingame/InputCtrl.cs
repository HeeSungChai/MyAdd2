using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eOPERATOR
{
    ADDITION,
    SUBTRACTION,
    MULTIPLICATION,
    DIVISION,
}

public enum eINPUT_TYPE
{
    FROM_ONE_TO_NINE,
    FROM_ONE_TO_EIGHTYONE,
}

public class InputCtrl : MonoBehaviour
{
    public eINPUT_TYPE m_eInputType;
    public GameObject[] m_arrObjLeftDigits;
    public GameObject[] m_arrObjRightDigits;
    public GameObject[] m_arrObjOperators;
    public float m_fInputDeactivateDuration;
    bool m_bInputAllowed = true;
    RePositioner[] m_arrScriptPositionerLeft;
    RePositioner[] m_arrScriptPositionerRight;
    InputButtonCtrl[] m_arrScriptInputButtonLeft;
    InputButtonCtrl[] m_arrScriptInputButtonRight;
    InputButtonCtrl[] m_arrScriptInputButtonOperator;
    public SuperGaugeCtrl m_scriptGaugeCtrl;

    int m_iCurAnswer;
    int m_iCorrectAnswerLeft;
    int m_iCorrectAnswerRight;
    int m_iCorrectAnswerOperator;
    int m_iSelectedDigitLeft;
    int m_iSelectedDigitRight;
    eOPERATOR m_eSelected_Operator;
    int m_iIndexSelectedLeft;
    int m_iIndexSelectedRight;
    //int[] m_listLeftDigits;
    //int[] m_listRightDigits;
    List<int> m_listLeftDigits;
    List<int> m_listRightDigits;
    List<int> m_listCandidates;

    bool m_bSelected_Left;
    bool m_bSelected_Right;
    bool m_bSelected_Operator;

    private void Awake()
    {
        //m_listLeftDigits = new int[m_arrObjLeftDigits.Length];
        //m_listRightDigits = new int[m_arrObjLeftDigits.Length];
        m_listLeftDigits = new List<int>();
        m_listRightDigits = new List<int>();
        m_listCandidates = new List<int>();

        EventListener.AddListener("OnLowestChanged", this);
    }

    void Start ()
    {
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

        ResetSelection();
        ResetDigits();
    }

    public void SetOperatorCondition()
    {
        //일거리. 연산기호에 해당하는 캐릭터를 획득한 경우에만 해당 오퍼레이터 활성화
    }
	
    public void OnLowestChanged()
    {
        ResetDigits();
    }

    void ResetDigits()
    {
        m_iCurAnswer = MyGlobals.DigitSpawner.LowestDigit;
        if (m_iCurAnswer == 0)
            return;

        if(m_eInputType == eINPUT_TYPE.FROM_ONE_TO_EIGHTYONE)
            SetFormulaForAnswer();
        else
            SetFormulaForAnswer2();

        AllocateInputDigits();
        //Invoke("AllocateInputDigits", 0.2f);
    }

    //1~81의 숫자를 사용하는 버전
    void SetFormulaForAnswer()
    {
        //일거리. 빼기, 곱하기, 나누기 캐릭터가 오픈되지 않은 경우에는 중간 연산자는 더하기만 가능하도록 해야함
        m_iCorrectAnswerOperator = Random.Range(0, (int)eOPERATOR.DIVISION + 1);
        if (m_iCorrectAnswerOperator == (int)eOPERATOR.DIVISION && m_iCurAnswer > 45)
            m_iCorrectAnswerOperator = Random.Range(0, (int)eOPERATOR.DIVISION);

        switch ((eOPERATOR)m_iCorrectAnswerOperator)
        {
            //더하기일 경우 (정답 - 정답보다 작은 랜덤 왼쪽숫자) = 오른쪽 숫자
            case eOPERATOR.ADDITION:
                {
                    m_iCorrectAnswerLeft = Random.Range(1, m_iCurAnswer);
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
    }

    void SetFormulaForAnswer2()//1~9의 숫자만 사용하는 버전
    {
        if (m_iCurAnswer > 18)      //한자리 숫자만 사용하게 되면 18을 초과하는 수는 무조건 연산기호가 곱하기가 되고
            m_iCorrectAnswerOperator = (int)eOPERATOR.MULTIPLICATION;
        else if (m_iCurAnswer > 9)   //9를 초과하는 수는 빼기와 나누기가 답이 될 수 없음
        {
            if(m_iCurAnswer == 11 || m_iCurAnswer == 13 || m_iCurAnswer == 17)
                m_iCorrectAnswerOperator = (int)eOPERATOR.ADDITION;
            else
                m_iCorrectAnswerOperator = Random.Range(0, 2) == 0 ? (int)eOPERATOR.ADDITION : (int)eOPERATOR.MULTIPLICATION;
        }
        else if (m_iCurAnswer > 8)
            m_iCorrectAnswerOperator = Random.Range(0, 3) == 0 ? (int)eOPERATOR.DIVISION : Random.Range(0, 2) == 0 ? (int)eOPERATOR.ADDITION : (int)eOPERATOR.MULTIPLICATION;
        else
            m_iCorrectAnswerOperator = Random.Range(0, (int)eOPERATOR.DIVISION + 1);

        switch ((eOPERATOR)m_iCorrectAnswerOperator)
        {
            //더하기일 경우 (정답 - 정답보다 작은 랜덤 왼쪽숫자) = 오른쪽 숫자
            case eOPERATOR.ADDITION:
                {
                    if (m_iCurAnswer > MyGlobals.MaxInputValue)
                        m_iCorrectAnswerLeft = Random.Range(m_iCurAnswer - MyGlobals.MaxInputValue, MyGlobals.MaxInputValue);
                    else
                        m_iCorrectAnswerLeft = Random.Range(1, m_iCurAnswer);

                    m_iCorrectAnswerRight = m_iCurAnswer - m_iCorrectAnswerLeft;
                }
                break;
            //뺄셈일 경우 (정답보다 큰 랜덤 왼쪽숫자 - 정답) = 오른쪽 숫자
            case eOPERATOR.SUBTRACTION:
                {
                    m_iCorrectAnswerLeft = Random.Range(m_iCurAnswer + 1, MyGlobals.MaxInputValue + 1);
                    m_iCorrectAnswerRight = m_iCorrectAnswerLeft - m_iCurAnswer;
                }
                break;
            //곱하기일 경우 
            case eOPERATOR.MULTIPLICATION:
                {
                    m_listCandidates.Clear();
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

        //1~81
        //int iMax = MyGlobals.MaxValue + 1;
        //1~9
        //int iMax = MyGlobals.MaxInputValue + 1;
        int iMax;
        if (m_eInputType == eINPUT_TYPE.FROM_ONE_TO_NINE)
            iMax = MyGlobals.MaxInputValue + 1;
        else
            iMax = MyGlobals.MaxValue + 1;

        int iRandomValue;
        for (int i = 0; i < m_arrScriptPositionerLeft.Length; ++i)
        {
            if(i != iRandomIndex)
            {
                //iRandomValue = Random.Range(1, iMax);
                do
                {
                    iRandomValue = Random.Range(1, iMax);
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
                //iRandomValue = Random.Range(1, iMax);
                do
                {
                    iRandomValue = Random.Range(1, iMax);
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
                    iUserAnswer = m_iSelectedDigitLeft / m_iSelectedDigitRight;
                    break;
                default:
                    iUserAnswer = 0;
                    break;
            }

            m_iCurAnswer = MyGlobals.DigitSpawner.LowestDigit;
            if (iUserAnswer == m_iCurAnswer)
            {
                IsCorrectAnswer();
            }
            else
                IsWrongAnswer();

            //ResetSelection();
            Invoke("ResetSelection", 0.2f);
        }
    }

    void IsCorrectAnswer()
    {
        ////정답인 경우 
        //해당 숫자 지우고 
        EventListener.Broadcast("OnCorrectAnswer");

        //점수 올리고 


        //필살기 게이지 채우고
        //m_scriptGaugeCtrl.IncreaseGauge();

        //그 다음 타겟에 맞춰 숫자 입력부 재설정


        //인피닛 모드면 콤보 수 누적


        Debug.Log("IsCorrect");
    }

    void IsWrongAnswer()
    {
        ////오답인 경우 


        //해당 숫자 유지하고


        //숫자 입력부 선택 해제


        Debug.Log("IsWrong");
    }

    void ResetSelection()
    {
        //Debug.Log("ResetSelection");
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

        if (m_iIndexSelectedLeft == -1)
        {
            m_arrScriptInputButtonLeft[iIndex].Select();
        }

        if (m_iIndexSelectedLeft != -1 && m_iIndexSelectedLeft != iIndex)
        {
            m_arrScriptInputButtonLeft[iIndex].Select();
            m_arrScriptInputButtonLeft[m_iIndexSelectedLeft].Deselect();
        }

        m_bSelected_Left = true;
        m_iSelectedDigitLeft = m_listLeftDigits[iIndex];
        m_iIndexSelectedLeft = iIndex;

        CheckIsCorrectAnswer();
    }

    void SelectRight(int iIndex)
    {
        if (!m_bInputAllowed)
            return;

        if (m_iIndexSelectedRight == -1)
        {
            m_arrScriptInputButtonRight[iIndex].Select();
        }

        if (m_iIndexSelectedRight != -1 && m_iIndexSelectedRight != iIndex)
        {
            m_arrScriptInputButtonRight[iIndex].Select();
            m_arrScriptInputButtonRight[m_iIndexSelectedRight].Deselect();
        }
        m_bSelected_Right = true;
        m_iSelectedDigitRight = m_listRightDigits[iIndex];
        m_iIndexSelectedRight = iIndex;

        CheckIsCorrectAnswer();
    }

    void SelectOperator(eOPERATOR eOperator)
    {
        if (!m_bInputAllowed)
            return;

        if (m_bSelected_Operator == false)
        {
            m_arrScriptInputButtonOperator[(int)eOperator].Select();
        }

        if (m_bSelected_Operator && m_eSelected_Operator != eOperator)
        {
            m_arrScriptInputButtonOperator[(int)eOperator].Select();
            m_arrScriptInputButtonOperator[(int)m_eSelected_Operator].Deselect();
        }
        
        m_bSelected_Operator = true;
        m_eSelected_Operator = eOperator;
        CheckIsCorrectAnswer();
    }

    public void OnSelect_Left_1()
    {
        SelectLeft(0);
        Debug.Log("OnSelect_Left_1");        
    }

    public void OnSelect_Left_2()
    {
        SelectLeft(1);
        Debug.Log("OnSelect_Left_2");
    }

    public void OnSelect_Left_3()
    {
        SelectLeft(2);
        Debug.Log("OnSelect_Left_3");
    }

    public void OnSelect_Left_4()
    {
        SelectLeft(3);
        Debug.Log("OnSelect_Left_4");
    }

    public void OnSelect_Left_5()
    {
        SelectLeft(4);
        Debug.Log("OnSelect_Left_5");
    }

    public void OnSelect_Right_1()
    {
        SelectRight(0);
        Debug.Log("OnSelect_Right_1");
    }

    public void OnSelect_Right_2()
    {
        SelectRight(1);
        Debug.Log("OnSelect_Right_2");
    }

    public void OnSelect_Right_3()
    {
        SelectRight(2);
        Debug.Log("OnSelect_Right_3");
    }

    public void OnSelect_Right_4()
    {
        SelectRight(3);
        Debug.Log("OnSelect_Right_4");
    }

    public void OnSelect_Right_5()
    {
        SelectRight(4);
        Debug.Log("OnSelect_Right_5");
    }

    public void OnSelect_Operator_ADDITION()
    {
        SelectOperator(eOPERATOR.ADDITION);
        Debug.Log("OnSelect_Operator_ADDITION");
    }

    public void OnSelect_Operator_SUBTRACTION()
    {
        SelectOperator(eOPERATOR.SUBTRACTION);
        Debug.Log("OnSelect_Operator_SUBTRACTION");
    }

    public void OnSelect_Operator_MULTIPLICATION()
    {
        SelectOperator(eOPERATOR.MULTIPLICATION);
        Debug.Log("OnSelect_Operator_MULTIPLICATION");
    }

    public void OnSelect_Operator_DIVISION()
    {
        SelectOperator(eOPERATOR.DIVISION);
        Debug.Log("OnSelect_Operator_DIVISION");
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        EventListener.RemoveListener(this);
    }
}
