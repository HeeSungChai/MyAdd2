using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public string m_strScene = "<Insert scene name>";
    public float m_fDuration = 1.0f;
    public Color m_color = Color.black;

    public void PerformTransition()
    {
        Transition.LoadLevel(m_strScene, m_fDuration, m_color);
    }
}
