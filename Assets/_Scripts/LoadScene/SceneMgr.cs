using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    public INGAME_TYPE eGameType;
    public int iStageNum;

    public void StartInGame()
    {
        if (MyGlobals.EnteringIngame)
            return;

        StartCoroutine("CoroutineEnterIngame");
    }

    IEnumerator CoroutineEnterIngame()
    {
        MyGlobals.GameType = eGameType;
        if (eGameType == INGAME_TYPE.ADVENTURE)
        {
            MyGlobals.StageNum = iStageNum;
        }

        MyGlobals.EnteringIngame = true;
        MyGlobals.EnterIngameFromOutgame = true;

        EventListener.Broadcast("OnEnterIngame");

        yield return new WaitForSeconds(2.0f);

        var oper = SceneManager.LoadSceneAsync("Ingame");

        oper.allowSceneActivation = false;

        while (oper.progress < 0.9f)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1.0f);

        oper.allowSceneActivation = true;
    }
}
