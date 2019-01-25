using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eSOUND_FX
{
    BUTTON,
    GAME_CLEAR,
    GAME_OVER,
    LETS_GO,
    SUPER_POWER_MAX,
    SUPER_POWER_ACTIVATE,
    ITEM_ERASER,
    ITEM_TIME,
    ITEM_RECOVERY,
    INPUT_TOUCH_FIRST,
    INPUT_TOUCH_SECOND,
    WRONG_ANSWER,
    CORRECT_ANSWER,
    FAILED,
}

public enum eSOUND_BGM
{
    TITLE,
    LOBBY,
    STAGE_NORMAL,
    STAGE_BOSS,
    GAME_CLEAR,
    GAME_OVER,
}

public class SoundMgr : MonoBehaviour
{
    public AudioSource m_audioSourceFx;
    public AudioSource m_audioSourceBGM;
    public AudioClip[] m_arrAudioClipFx;
    public AudioClip[] m_arrAudioClipBGM;
    public float m_fMinVolumnFx;
    public float m_fMaxVolumnFx;
    public float m_fMinVolumnBGM;
    public float m_fMaxVolumnBGM;
    [SerializeField]
    private float m_fVolumnFx;
    public float VolumnFx { get { return m_fVolumnFx; } set { m_fVolumnFx = value; } }
    public float VolumnFxTemp;
    [SerializeField]
    private float m_fVolumnBGM;
    public float VolumnBGM { get { return m_fVolumnBGM; } set { m_fVolumnBGM = value; } }
    public float VolumnBGMTemp;

    private void Awake()
    {
        VolumnFx = PrefsMgr.Instance.GetFloat(PrefsMgr.strVolumnFX, 1f);
        VolumnBGM = PrefsMgr.Instance.GetFloat(PrefsMgr.strVolumnBGM, 1f);
        VolumnFxTemp = VolumnFx;
        VolumnBGMTemp = VolumnBGM;
    }

    void Start ()
    {
        MyGlobals.SoundMgr = this;
	}

    public void OnSetVolumnFx(float fVolumn)
    {
        VolumnFx = fVolumn * 0.1f;
        VolumnFx = Mathf.Clamp01(VolumnFx);
        PrefsMgr.Instance.SetFloat(PrefsMgr.strVolumnFX, VolumnFx);
    }
	
    public void OnFxVolumnUp()
    {
        VolumnFxTemp += 0.1f;
        VolumnFxTemp = Mathf.Clamp01(VolumnFx);
        //PrefsMgr.Instance.SetFloat(PrefsMgr.strVolumnFX, VolumnFx);
    }

    public void OnFxVolumnDown()
    {
        VolumnFxTemp -= 0.1f;
        VolumnFxTemp = Mathf.Clamp01(VolumnFx);
        //PrefsMgr.Instance.SetFloat(PrefsMgr.strVolumnFX, VolumnFx);
    }

    public void OnSetVolumnBGM(float fVolumn)
    {
        VolumnBGM = fVolumn * 0.1f;
        VolumnBGM = Mathf.Clamp01(VolumnBGM);
        PrefsMgr.Instance.SetFloat(PrefsMgr.strVolumnBGM, VolumnBGM);
        m_audioSourceBGM.volume = VolumnBGM;
    }

    public void OnBGMVolumnUp()
    {
        VolumnBGM += 0.1f;
        VolumnBGM = Mathf.Clamp01(VolumnBGM);
    }

    public void OnBGMVolumnDown()
    {
        VolumnBGM -= 0.1f;
        VolumnBGM = Mathf.Clamp01(VolumnBGM);
    }

    public void OnPlayFx(eSOUND_FX eSound)
    {
        NGUITools.PlaySound(m_arrAudioClipFx[(int)eSound], VolumnFx, 1f);
    }

    public void OnPlayFxTemp(eSOUND_FX eSound)
    {
        NGUITools.PlaySound(m_arrAudioClipFx[(int)eSound], VolumnFxTemp, 1f);
    }

    public void OnPlayFx(eSOUND_FX eSound, float fVolumn)
    {
        NGUITools.PlaySound(m_arrAudioClipFx[(int)eSound], fVolumn, 1f);
    }

    public void OnPlayBGM(eSOUND_BGM eSound)
    {
        //m_audioSourceFx.clip = m_arrAudioClipFx[(int)eSound];
        //m_audioSourceFx.Play();
        NGUITools.PlaySound(m_arrAudioClipBGM[(int)eSound], VolumnBGM, 1f);
    }

    public void OnPlayGameClear()
    {
        m_audioSourceBGM.clip = m_arrAudioClipBGM[(int)eSOUND_BGM.GAME_CLEAR];
        m_audioSourceBGM.volume = VolumnBGM;
        m_audioSourceBGM.Play();
    }

    public void OnPlayGameOver()
    {
        m_audioSourceBGM.clip = m_arrAudioClipBGM[(int)eSOUND_BGM.GAME_OVER];
        m_audioSourceBGM.volume = VolumnBGM;
        m_audioSourceBGM.Play();
    }
}
