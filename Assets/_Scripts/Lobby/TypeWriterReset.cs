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
    }

	void OnEnable ()
    {
        if (m_typeWriter)
        {
            m_typeWriter.StartNewWrite();
        }
    }
}
