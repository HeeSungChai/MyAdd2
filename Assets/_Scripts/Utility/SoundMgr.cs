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
}

public enum eSOUND_BGM
{
    TITLE,
    LOBBY,
    STAGE_NORMAL,
    STAGE_BOSS,
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
    [SerializeField]
    private float m_fVolumnBGM;
    public float VolumnBGM { get { return m_fVolumnBGM; } set { m_fVolumnBGM = value; } }
    //eSOUND_FX m_eSoundFx;
    //eSOUND_BGM m_eSoundBGM;

    //#region Singleton Pattern Implementation
    //private static SoundMgr instance;

    //public static SoundMgr Instance
    //{
    //    get
    //    {
    //        if (instance == null)
    //            instance = new SoundMgr();
    //        return instance;
    //    }
    //}
    //#endregion

    private void Awake()
    {
        VolumnFx = PlayerPrefs.GetFloat("VolumnFx", 1f);
        VolumnBGM = PlayerPrefs.GetFloat("VolumnBGM", 1f);
    }

    void Start ()
    {
        MyGlobals.SoundMgr = this;
	}

    public void OnSetVolumnFx(float fVolumn)
    {
        VolumnFx = fVolumn;
        VolumnFx = Mathf.Clamp01(VolumnFx);
        //m_audioSourceFx.volume = VolumnFx;
        PlayerPrefs.SetFloat("VolumnFx", VolumnFx);
    }
	
    public void OnFxVolumnUp()
    {
        VolumnFx += 0.1f;
        VolumnFx = Mathf.Clamp01(VolumnFx);
        //m_audioSourceFx.volume = VolumnFx;
        PlayerPrefs.SetFloat("VolumnFx", VolumnFx);
    }

    public void OnFxVolumnDown()
    {
        VolumnFx -= 0.1f;
        VolumnFx = Mathf.Clamp01(VolumnFx);
        //m_audioSourceFx.volume = VolumnFx;
        PlayerPrefs.SetFloat("VolumnBGM", VolumnBGM);
    }

    public void OnSetVolumnBGM(float fVolumn)
    {
        VolumnBGM = fVolumn;
        VolumnBGM = Mathf.Clamp01(VolumnFx);
        //m_audioSourceBGM.volume = VolumnFx;
        PlayerPrefs.SetFloat("VolumnBGM", VolumnBGM);
    }

    public void OnBGMVolumnUp()
    {
        VolumnBGM += 0.1f;
        VolumnBGM = Mathf.Clamp01(VolumnBGM);
        //m_audioSourceBGM.volume = VolumnBGM;
    }

    public void OnBGMVolumnDown()
    {
        VolumnBGM -= 0.1f;
        VolumnBGM = Mathf.Clamp01(VolumnBGM);
        //m_audioSourceBGM.volume = VolumnBGM;
    }

    public void OnPlayFx(eSOUND_FX eSound)
    {
        //m_audioSourceFx.clip = m_arrAudioClipFx[(int)eSound];
        //m_audioSourceFx.Play();
        NGUITools.PlaySound(m_arrAudioClipFx[(int)eSound], VolumnFx, 1f);
    }


}
