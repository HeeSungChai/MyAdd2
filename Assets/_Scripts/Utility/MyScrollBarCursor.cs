using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyScrollBarCursor : MonoBehaviour
{
    Transform m_transform;
    public UIScrollBar m_scriptScrollBar;
    public Transform m_transTop;
    public Transform m_transBottom;

    private void Awake()
    {
        m_transform = this.transform;
    }

	void Update ()
    {
        m_transform.position = Vector3.Lerp(m_transTop.position, m_transBottom.position, m_scriptScrollBar.value);
	}
}
