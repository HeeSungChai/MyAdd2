using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // for File

#if UNITY_EDITOR
public class TableDbEditor : UnityEditor.Editor
{
    [UnityEditor.MenuItem("Update Table/Dialogue Table/Update Dialogue DB")]
    static public void UpdateDefineIngameDB()
    {
        UnityEngine.Object[] DBdata = Resources.LoadAll(TableDB.m_strAssetPathDialogueTable, typeof(TextAsset));
        List<string> keyList = new List<string>();

        //테이블 맨 윗줄을 가져와 키값들을 분리해냄
        foreach (UnityEngine.Object textAsset in DBdata)
        {
            string[] lines = (textAsset as TextAsset).text.Split('\n');
            if (lines != null && lines.Length > 0)
            {
                string[] keys = lines[0].Split('\t');
                foreach (string key in keys)
                {
                    string trimKey = key.Trim();
                    if (keyList.Contains(trimKey) == false)
                        keyList.Add(trimKey);
                }
            }
        }

        string[] defaultTableDBText =  {
                                            "public enum eKEY_TABLEDB",
                                            "{",
                                            "\tEND",
                                            "}"
                                        };

        //기존 eKEY_TABLEDB 에 정의된 키값과 비교해 업데이트
        bool bChangedInGameDB = false;
        string strTableDBfileName = Application.dataPath + "/_Scripts/CSV/TableList.cs";
        string[] arrayKeyLine = File.ReadAllLines(strTableDBfileName);
        if (arrayKeyLine != null)
        {
            bool bFindEnum = false;
            int nFindEnumIndex = 0;
            int nKeyCount = 0;
            string enumName = defaultTableDBText[0];
            string enumEnd = defaultTableDBText[2];

            for (int i = 0; i < arrayKeyLine.Length; ++i)
            {
                string line = arrayKeyLine[i];
                string trimLine = line.Trim(' ', ',', '\t');
                if (bFindEnum == false && trimLine.Equals(enumName) == true)
                {
                    bFindEnum = true;
                    nFindEnumIndex = i + 1;
                }

                if (bFindEnum == true && i > nFindEnumIndex)
                {
                    if (line.Equals(enumEnd) == true)
                        break;

                    nKeyCount++;

                    bool bExist = false;
                    foreach (string dbKey in keyList)
                    {
                        if (trimLine.Equals(dbKey.Trim()) == true)
                        {
                            bExist = true;
                            break;
                        }
                    }

                    if (bExist == false)
                    {
                        bChangedInGameDB = true;
                        break;
                    }
                }
            }

            if (bChangedInGameDB == false && nKeyCount != keyList.Count)
                bChangedInGameDB = true;
        }

        //뭔가 바뀐 내용이 있으면 => 스테이지 레벨 테이블들은 키값이 모두 같아 새로운 테이블 추가해도 업데이트 안되므로 이 조건 생략
        //if (bChangedInGameDB == true)
        //{
            string[] inGameDBTableText =    {
                                                "public enum eTABLE_LIST",
                                                "{",
                                                "\tEND",
                                                "}"
                                            };

            keyList.Sort();

            StreamWriter fileWriter = File.CreateText(strTableDBfileName);

            //테이블 리스트(eTABLE_LIST) 갱신하고
            fileWriter.WriteLine(inGameDBTableText[0]);
            fileWriter.WriteLine(inGameDBTableText[1]);
            foreach (UnityEngine.Object textAsset in DBdata)
                fileWriter.WriteLine("\t" + (textAsset as TextAsset).name + ",");
            fileWriter.WriteLine(inGameDBTableText[2]);
            fileWriter.WriteLine(inGameDBTableText[3]);
            fileWriter.WriteLine("");

            //테이블 키값(eKEY_TABLEDB) 갱신
            fileWriter.WriteLine(defaultTableDBText[0]);
            fileWriter.WriteLine(defaultTableDBText[1]);
            foreach (string key in keyList)
                fileWriter.WriteLine("\t" + key.Trim() + ",");
            fileWriter.WriteLine(defaultTableDBText[2]);
            fileWriter.WriteLine(defaultTableDBText[3]);

            foreach (UnityEngine.Object textAsset in DBdata)
            {
                string[] lines = (textAsset as TextAsset).text.Split('\n');
                string[] keys = lines[0].Split('\t');
                fileWriter.WriteLine("");
                fileWriter.WriteLine("public enum " + (textAsset as TextAsset).name.ToUpper());
                fileWriter.WriteLine("{");
                foreach (string key in keys)
                    fileWriter.WriteLine("\t" + key.Trim() + ",");
                fileWriter.WriteLine("\tEND");
                fileWriter.WriteLine("}");
            }

            fileWriter.Close();
            UnityEditor.AssetDatabase.Refresh();

            MyUtility.DebugLog("Reimported Keys from Ingame DB" + strTableDBfileName);
        //}
    }
}
#endif
