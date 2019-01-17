using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class MyRenamer : MonoBehaviour
{
    public Sprite[] m_arrSprite;
    public string m_strName;

	void Start ()
    {
        Rename();
	}
	
	void Rename ()
    {
		for(int i = 0; i < m_arrSprite.Length; ++i)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(m_strName);
            sb.Append(i.ToString());
            m_arrSprite[i].texture.name = sb.ToString();
        }
	}
}
