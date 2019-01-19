using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //for enum
using System.Text;

public class MyUtility
{
    static public void DebugLog(string str)
    {
#if DEBUG
        Debug.Log(str);
#endif
    }

    static public void ActivateAll(GameObject[] arrObj, bool bActivate = true)
    {
        for (int i = 0; i < arrObj.Length; ++i)
        {
            arrObj[i].SetActive(bActivate);
        }
    }

    static public string GetLevelText(int iLevel)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Lv. ");
        sb.Append(iLevel.ToString());
        return sb.ToString();
    }

    static public string GetBonusText(int iLevel)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Lv. ");
        sb.Append(iLevel.ToString());
        return sb.ToString();
    }

    static public string CommaSeparateDigit(int iDigit)
    {
        StringBuilder sb = new StringBuilder();

        while (iDigit >= 10)
        {
            sb.Append((iDigit % 10).ToString());
            iDigit /= 10;
        }
        sb.Append(iDigit.ToString());

        // Revers
        StringBuilder sb2 = new StringBuilder();
        for (int i = sb.Length - 1; i >= 0; --i)
        {
            sb2.Append(sb[i]);

            if (i != 0 && i % 3 == 0)
                sb2.Append(",");
        }

        return sb2.ToString();
    }

    static public void Swap<T>(ref T left, ref T right)
    {
        T tempT = left;
        left = right;
        right = tempT;
    }

    static public T ParsingStringToEnumType<T>(string _strValue, bool bUseTryCatch = false)
    {
        T enumType;
        if (string.IsNullOrEmpty(_strValue) == false)
        {
            if (bUseTryCatch == true)
            {
                try
                {
                    enumType = (T)Enum.Parse(typeof(T), _strValue);
                }
                catch (Exception e)
                {
                    enumType = default(T);
                    Debug.Log(e.Message);
                    Debug.LogError(e.StackTrace);
                }
            }
            else
                enumType = (T)Enum.Parse(typeof(T), _strValue);
        }
        else
            enumType = default(T);

        return enumType;
    }

    static public string ConvertToString(object _value)
    {
        return Convert.ToString(_value);
    }

    //static public float GetDistance(Vector3 vPos1, Vector3 vPos2)
    //{
    //    float fDistance = Vector3.Distance(vPos1, vPos2);
    //    return fDistance;
    //}

    //public static Vector3 NewPosition(Vector3 vOriginPos, float fX, float fY, float fZ)
    //{
    //    Vector3 vNewPos = new Vector3(fX, fY, fZ);
    //    vOriginPos += vNewPos;
    //    return vOriginPos;
    //}

    //public static Coroutine WaitCoroutine(this MonoBehaviour parent, float waitTime, Action callback)
    //{
    //    return parent.gameObject.activeInHierarchy == false ? null : parent.StartCoroutine(WaitThenCallback(waitTime, callback));
    //}

    //private static IEnumerator WaitThenCallback(float time, Action callback)
    //{
    //    yield return new WaitForSeconds(time);
    //    callback();
    //}

    //public static IEnumerator CoroutineInvoke()
    //{

    //}

    //public static IEnumerator CoroutineSlerp(object[] _object)
    //{
    //    Vector3 vOrigin = (Vector3)_object[0];
    //    Vector3 vTarget = (Vector3)_object[1];
    //    float fDuration = (float)_object[2];

    //    float fTime = 0.0f;

    //    while (fTime < fDuration)
    //    {
    //        fTime += Time.deltaTime;
    //        m_transform.position = Vector3.Slerp(vOrigin, vTarget, fTime / fDuration);
    //        yield return null;
    //    }

    //    m_transform.position = vTarget;

    //    yield return null;
    //}
}
