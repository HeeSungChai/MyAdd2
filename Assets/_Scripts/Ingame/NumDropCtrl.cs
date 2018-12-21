using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumDropCtrl : MonoBehaviour
{
    public UILabel m_label;
    public Transform m_transform;
    Vector3 m_vStartPos;
    Vector3 m_vTargetPos;
    float m_fFallDuration;
    bool m_bReachToBottom;

    //private void Awake()
    //{
    //    if (m_label == null)
    //        m_label = GetComponentInChildren<UILabel>();
    //    if (m_transform == null)
    //        m_transform = m_label.gameObject.transform;
    //}

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

        m_bReachToBottom = false;
        m_vStartPos = vSpawnPos;
        m_vTargetPos = m_vStartPos;
        m_vTargetPos.y = 0.0f;
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

            yield return null;
        }

        m_transform.localPosition = m_vTargetPos;
        m_bReachToBottom = true;
        //m_label.gameObject.SetActive(false);
        Invoke("DisableObj", 1.0f);
    }

    public bool IsReachToBottom()
    {
        return m_bReachToBottom;
    }

    public float GetHeight()
    {
        return m_transform.localPosition.y;
    }

    public int GetDigit()
    {
        return int.Parse(m_label.text);
    }

    void DisableObj()
    {
        this.gameObject.SetActive(false);
    }
}
