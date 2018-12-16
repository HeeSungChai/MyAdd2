using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eOPERATOR
{
    ADDITION,
    SUBTRACTION,
    MULTIPLICATION,
    DIVISION,
}

public class InputCtrl : MonoBehaviour
{
    public GameObject[] m_arrObjLeftDigits;
    public GameObject[] m_arrObjRightDigits;
    public GameObject[] m_arrObjOperators;

    void Start ()
    {
        EventListener.AddListener("OnDigitThrowed", this);
        EventListener.AddListener("OnSelect_Left", this);
        //EventListener.AddListener("OnSelect_Left2", this);
        //EventListener.AddListener("OnSelect_Left3", this);
        //EventListener.AddListener("OnSelect_Left4", this);
        //EventListener.AddListener("OnSelect_Left5", this);
        EventListener.AddListener("OnSelect_Right", this);
        //EventListener.AddListener("OnSelect_Right2", this);
        //EventListener.AddListener("OnSelect_Right3", this);
        //EventListener.AddListener("OnSelect_Right4", this);
        //EventListener.AddListener("OnSelect_Right5", this);
    }

    public void SetOperatorCondition()
    {

    }
	
	public void OnDigitThrowed ()
    {
		
	}

    void ReplaceDigits()
    {

    }

    void OnSelect_Left(int iIndex)
    {

    }

    void OnSelect_Right(int iIndex)
    {

    }

    void OnSelect_Operator(eOPERATOR eOperator)
    {

    }
}
