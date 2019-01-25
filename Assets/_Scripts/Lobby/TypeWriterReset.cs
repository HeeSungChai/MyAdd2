using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeWriterReset : MonoBehaviour
{
    public TypewriterEffect m_typeWriter;

    void Awake()
    {
        if (m_typeWriter == null)
            m_typeWriter = GetComponentInChildren<TypewriterEffect>();

        m_typeWriter.gameObject.SetActive(false);
    }

	void OnEnable ()
    {
        OnManualReset();
    }

    public void OnManualReset()
    {
        if (m_typeWriter)
        {
            m_typeWriter.gameObject.SetActive(true);
            m_typeWriter.StartNewWrite();
        }
    }

    private void OnDisable()
    {
        if (m_typeWriter)
        {
            m_typeWriter.gameObject.SetActive(false);
        }
    }
}
