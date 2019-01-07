using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterLobby : MonoBehaviour
{
    public void EnterLobbyScene()
    {
        StartCoroutine("CoroutineEnterLobby");
    }

    IEnumerator CoroutineEnterLobby()
    {
        EventListener.Broadcast("OnEnterIngame");

        //yield return new WaitForSeconds(2.0f);

        var oper = SceneManager.LoadSceneAsync("Lobby");

        oper.allowSceneActivation = false;

        while (oper.progress < 0.9f)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1.0f);

        oper.allowSceneActivation = true;
    }
}
