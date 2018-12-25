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
    Vector3 m_vStartPos;
    Vector3 m_vTargetPos;
    float m_fFallDuration;
    bool m_bReachToBottom;
    bool m_bCorrect;

    private void Awake()
    {
        m_colorDefault = m_label.color;
        EventListener.AddListener("OnActivateSuperSkill", this);
    }

    private void OnEnable()
    {
        m_label.gameObject.SetActive(true);

        StopCoroutine("CoroutineFall");
        StartCoroutine("CoroutineFall");
    }

    public void Init (Vector3 vSpawnPos, int num, float fFallDuration)
    {
        //m_label.gameObject.SetActive(true);
        if (m_label == null)
            m_label = GetComponentInChildren<UILabel>();
        if (m_transform == null)
            m_transform = m_label.gameObject.transform;

        m_bCorrect = false;
        m_bReachToBottom = false;
        m_vStartPos = vSpawnPos;
        m_vTargetPos = m_vStartPos;
        m_vTargetPos.y = MyGlobals.DigitSpawner.m_fHeightFail;
        m_transform.localPosition = m_vStartPos;
        m_label.text = num.ToString();
        m_fFallDuration = fFallDuration;
    }

    IEnumerator CoroutineFall()
    {
        float fElased = 0.0f;
        while(fElased < m_fFallDuration)
        {
            fElased += Time.deltaTime;

            m_transform.localPosition = Vector3.Lerp(m_vStartPos, m_vTargetPos, fElased / m_fFallDuration);

            if (m_bCorrect)
                yield break;

            yield return null;
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

    public float GetHeight()
    {
        return m_transform.localPosition.y;
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

    public void Correct(eEVALUATION eEvaluation)
    {
        //정답인 경우 레이블 비활성화, Great여부 판정,
        m_bCorrect = true;
        m_label.gameObject.SetActive(false);
        ActivateCorrentEffect(eEvaluation);
        Invoke("DisableObj", 1.0f);
    }

    void ActivateCorrentEffect(eEVALUATION eEvaluation)
    {
        m_objCorrectStar.transform.localPosition = m_transform.localPosition;
        m_objCorrectStar.SetActive(true);

        m_objEvaluation[(int)eEvaluation].transform.localPosition = m_transform.localPosition;
        m_objEvaluation[(int)eEvaluation].SetActive(true);
    }

    void Failed()
    {
        m_bCorrect = true;
        m_label.gameObject.SetActive(false);
        ActivateFailEffect();
        Invoke("DisableObj", 1.0f);
        EventListener.Broadcast("OnFailed");
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
    void OnActivateSuperSkill()
    {
        fCurHeight = GetHeight();

        if (fCurHeight >= MyGlobals.DigitSpawner.m_fHeightGreat)
            eEvaluation = eEVALUATION.GREAT;
        else if (fCurHeight >= MyGlobals.DigitSpawner.m_fHeightCool)
            eEvaluation = eEVALUATION.COOL;
        else
            eEvaluation = eEVALUATION.NICE;

        Correct(eEvaluation);
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
