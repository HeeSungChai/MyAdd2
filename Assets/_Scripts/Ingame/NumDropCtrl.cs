using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eEVALUATION
{
    GREAT,
    COOL,
    NICE,
    FAIL
}

public class NumDropCtrl : MonoBehaviour
{
    public UILabel m_label;
    public Color m_colorDefault;
    public Transform m_transform;
    public GameObject m_objCorrectStar;
    public GameObject m_objFailStar;
    public GameObject[] m_objEvaluation;
    public GameObject m_objLockOn;
    public GameObject m_objFire;
    Vector3 m_vStartPos;
    Vector3 m_vTargetPos;
    float m_fFallDuration;
    bool m_bReachToBottom;
    bool m_bCorrect;
    public int LineID { get; set; }
    public int LineID_Infinite { get; set; }
    bool m_bCorrectOrFailed = false;
    public int m_iDepthOrigin = 100;
    public int m_iDepthFocus = 500;

    private void Awake()
    {
        m_colorDefault = m_label.color;
        //EventListener.AddListener("OnActivateSuperSkill", this);
        EventListener.AddListener("OnDeactivateSuperSkill", this);
    }

    private void OnEnable()
    {
        m_label.depth = m_iDepthOrigin;
        m_label.gameObject.SetActive(true);

        StopCoroutine("CoroutineFall");
        StartCoroutine("CoroutineFall");
    }

    //public void Init (Vector3 vSpawnPos, int num, float fFallDuration, int iLineID = 0)
    //{
    //    //m_label.gameObject.SetActive(true);
    //    if (m_label == null)
    //        m_label = GetComponentInChildren<UILabel>();
    //    if (m_transform == null)
    //        m_transform = m_label.gameObject.transform;

    //    m_bCorrectOrFailed = false;
    //    m_bCorrect = false;
    //    m_bReachToBottom = false;
    //    m_vStartPos = vSpawnPos;
    //    m_vTargetPos = m_vStartPos;
    //    m_vTargetPos.y = MyGlobals.DigitSpawner.m_fHeightFail;
    //    m_transform.localPosition = m_vStartPos;
    //    m_label.text = num.ToString();
    //    m_fFallDuration = fFallDuration;
    //    LineID = iLineID;
    //    m_objFire.SetActive(true);
    //}

    public void Init(Vector3 vSpawnPos, int num, float fFallDuration, int iLineID = 0, int iLineIDForInfinite = 0)
    {
        //m_label.gameObject.SetActive(true);
        if (m_label == null)
            m_label = GetComponentInChildren<UILabel>();
        if (m_transform == null)
            m_transform = m_label.gameObject.transform;

        m_label.depth = m_iDepthOrigin;
        m_bCorrectOrFailed = false;
        m_bCorrect = false;
        m_bReachToBottom = false;
        m_vStartPos = vSpawnPos;
        m_vTargetPos = m_vStartPos;
        m_vTargetPos.y = MyGlobals.DigitSpawner.m_fHeightFail;
        m_transform.localPosition = m_vStartPos;
        m_label.text = num.ToString();
        m_fFallDuration = fFallDuration;
        LineID = iLineID;
        LineID_Infinite = iLineIDForInfinite;
        m_objFire.SetActive(true);
        m_objLockOn.SetActive(false);
    }

    IEnumerator CoroutineFall()
    {
        float fElased = 0.0f;
        while(fElased < m_fFallDuration)
        {
            if (MyGlobals.StageMgr.IsPauseDrop)
            {
                yield return new WaitForFixedUpdate();
                continue;
            }

            fElased += Time.deltaTime;

            m_transform.localPosition = Vector3.Lerp(m_vStartPos, m_vTargetPos, fElased / m_fFallDuration);

            if (m_bCorrect)
                yield break;

            yield return new WaitForFixedUpdate();
        }

        m_transform.localPosition = m_vTargetPos;
        m_bReachToBottom = true;
        Failed();
        //m_label.gameObject.SetActive(false);
        Invoke("DisableObj", 1.0f);
    }

    public bool IsReachToBottom()
    {
        return m_bReachToBottom;
    }

    public bool IsCorrect()
    {
        return m_bCorrect;
    }

    public int GetLineID()
    {
        if (MyGlobals.StageMgr.IsAdventure())
            return LineID;
        else
            return LineID_Infinite;
    }

    public float GetHeight()
    {
        return m_transform.localPosition.y;
    }

    public void SetDigitByForce(int iForcedDigit)
    {
        m_label.text = iForcedDigit.ToString();
    }

    public int GetDigit()
    {
        return int.Parse(m_label.text);
    }

    public void SetTextColor(Color _color)
    {
        m_label.color = _color;
    }

    public void SetTextColorDefault()
    {
        m_label.color = m_colorDefault;
    }

    public void LockOn()
    {
        m_label.depth = m_iDepthFocus;
        m_objLockOn.SetActive(true);
    }

    public bool IsLockedOn()
    {
        if (m_objLockOn.activeSelf)
            return true;
        else
            return false;
    }

    public void Correct(eEVALUATION eEvaluation)
    {
        if (m_bCorrectOrFailed)
            return;

        m_bCorrectOrFailed = true;
        //정답인 경우 레이블 비활성화, Great여부 판정,
        m_bCorrect = true;
        m_label.gameObject.SetActive(false);
        m_objLockOn.SetActive(false);
        m_objFire.SetActive(false);
        ActivateCorrectEffect(eEvaluation);
        Invoke("DisableObj", 1.0f);
        --MyGlobals.DigitSpawner.DigitsCount;
        ++MyGlobals.DigitSpawner.TargetID;

        MyGlobals.SoundMgr.OnPlayFx(eSOUND_FX.CORRECT_ANSWER);

        if (!MyGlobals.StageMgr.IsAdventure())
        {
            ++MyGlobals.StageMgr.ComboCount;
            if (MyGlobals.StageMgr.ComboCount > 0)//두번째 성공부터 콤보로 침
                ++MyGlobals.StageMgr.TotalComboCount;
        }

        EventListener.Broadcast("OnChangeTarget");
    }

    void ActivateCorrectEffect(eEVALUATION eEvaluation)
    {
        m_objCorrectStar.transform.localPosition = m_transform.localPosition;
        m_objCorrectStar.SetActive(true);

        m_objEvaluation[(int)eEvaluation].transform.localPosition = m_transform.localPosition;
        m_objEvaluation[(int)eEvaluation].SetActive(true);
    }

    void Failed()
    {
        if (m_bCorrectOrFailed)
            return;

        m_bCorrectOrFailed = true;
        m_bCorrect = false;
        m_label.gameObject.SetActive(false);
        m_objFire.SetActive(false);
        ActivateFailEffect();
        Invoke("DisableObj", 1.0f);
        EventListener.Broadcast("OnFailed");
        --MyGlobals.DigitSpawner.DigitsCount;
        ++MyGlobals.DigitSpawner.TargetID;

        MyGlobals.SoundMgr.OnPlayFx(eSOUND_FX.FAILED);

        //가장 낮은놈이 타겟일때는 그놈이 fail되면 숫자입력부 갱신해야되지만 
        //기획 바뀌어 LineID 기반으로 타깃을 정하므로 ChargeTarget해줄필요 없어짐
        //그랬더니 타깃인애가 땅에 닿아도 타깃이 안바뀌어 현재 타깃이면 changeTarget하도록 함
        if (m_objLockOn.activeSelf == true)
            EventListener.Broadcast("OnChangeTarget");

        m_objLockOn.SetActive(false);

        if (!MyGlobals.StageMgr.IsAdventure())
        {
            MyGlobals.StageMgr.ComboCount = -1;

            MyGlobals.DigitSpawner.m_fSpawnDelay += 0.1f;
            MyGlobals.DigitSpawner.m_fSpawnDelay = Mathf.Clamp(MyGlobals.DigitSpawner.m_fSpawnDelay, 
                                                                MyGlobals.StageMgr.m_fSpawnDelayMin, 
                                                                MyGlobals.StageMgr.m_fSpawnDelayMax);
        }
    }

    void ActivateFailEffect()
    {
        m_objFailStar.transform.localPosition = m_transform.localPosition;
        m_objFailStar.SetActive(true);

        m_objEvaluation[(int)eEVALUATION.FAIL].transform.localPosition = m_transform.localPosition;
        m_objEvaluation[(int)eEVALUATION.FAIL].SetActive(true);
    }

    float fCurHeight;
    eEVALUATION eEvaluation;
    void OnDeactivateSuperSkill()
    {
        fCurHeight = GetHeight();

        if (fCurHeight >= MyGlobals.DigitSpawner.m_fHeightGreat)
            eEvaluation = eEVALUATION.GREAT;
        else if (fCurHeight >= MyGlobals.DigitSpawner.m_fHeightCool)
            eEvaluation = eEVALUATION.COOL;
        else
            eEvaluation = eEVALUATION.NICE;

        Correct(eEvaluation);

        MyGlobals.ScoreMgr.UpdateScore(eEvaluation, false, true);
    }

    void DisableObj()
    {
        m_objCorrectStar.SetActive(false);
        m_objFailStar.SetActive(false);
        for (int i = 0; i < m_objEvaluation.Length; ++i)
        {
            m_objEvaluation[i].SetActive(false);
        }

        this.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        EventListener.RemoveListener(this);
    }
}
