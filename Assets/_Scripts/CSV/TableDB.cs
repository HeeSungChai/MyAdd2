using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // for format Exception

public class TableDB// : UnityEditor.Editor
{
    #region Singleton Pattern Implementation
    private static TableDB instance;

    public static TableDB Instance
    {
        get
        {
            if (instance == null)
                instance = new TableDB();
            return instance;
        }
    }
    #endregion

    private Dictionary<eTABLE_LIST, Dictionary<int, Dictionary<eKEY_TABLEDB, object>>> m_TableDB;
    private List<string> m_Keys;
    public List<string> KEYS
    {
        get
        {
            return m_Keys;
        }
    }

    private ThreadProcessor<KeyValuePair<string, string>> m_threadProcessor;
    private bool m_bInit = false;
    private bool m_bEndLoad = false;
    public bool IsEndLoad { get { return m_bEndLoad; } }

    public const string m_strAssetPathDialogueTable = "Table/";

    private TableDB()
    {
        Reset();
        LoadAllInGameDB();
    }

    public void Reset()
    {
        m_bInit = false;

        if (m_TableDB == null)
            m_TableDB = new Dictionary<eTABLE_LIST, Dictionary<int, Dictionary<eKEY_TABLEDB, object>>>();
        else
            m_TableDB.Clear();

        if (m_Keys == null)
            m_Keys = new List<string>();
        else
            m_Keys.Clear();
    }

    public void LoadAllInGameDB()
    {
        if (m_bInit == true)
            return;

        UnityEngine.Object[] arrayTable;
        arrayTable = Resources.LoadAll(m_strAssetPathDialogueTable, typeof(TextAsset));

        if (arrayTable != null && arrayTable.Length > 0)
        {
            m_bInit = true;
            m_bEndLoad = false;

            if (m_threadProcessor == null)
                m_threadProcessor = new ThreadProcessor<KeyValuePair<string, string>>();

            List<KeyValuePair<string, string>> DBTableList = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < arrayTable.Length; ++i)
            {
                TextAsset textAsset = arrayTable[i] as TextAsset;
                DBTableList.Add(new KeyValuePair<string, string>(textAsset.name, textAsset.text));
            }

            //  thread load
            m_threadProcessor.Process(DBTableList, LoadInGameDBTable, EndLoadInGameDBTable, true);
        }
    }

    private void LoadInGameDBTable(KeyValuePair<string, string> _dbTable)
    {
        eTABLE_LIST inGameDBTable = MyUtility.ParsingStringToEnumType<eTABLE_LIST>(_dbTable.Key, true);
        if (string.IsNullOrEmpty(_dbTable.Key) == true
            || string.IsNullOrEmpty(_dbTable.Value) == true
            || m_TableDB.ContainsKey(inGameDBTable) == true)
            return;

        string[] lines = _dbTable.Value.Split('\n');
        string[] keys = lines[0].Split('\t');
        for (int i = 0; i < keys.Length; ++i)
        {
            string key = keys[i].Trim();
            keys[i] = key;
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(key) || m_Keys.Contains(key))
                continue;
            m_Keys.Add(key);
#endif
        }

        Dictionary<int, Dictionary<eKEY_TABLEDB, object>> dicInGameDB = new Dictionary<int, Dictionary<eKEY_TABLEDB, object>>();

        for (int i = 1; i < lines.Length; ++i)
        {
            //한 row의 전체 값들
            string[] arrayData = lines[i].Split('\t');
            int id = -1;
            try
            {
                id = int.Parse(arrayData[0]);
            }
            catch (FormatException)
            {
                break;
            }
            if (id < 0)
                continue;
            for (int j = 1; j < keys.Length; ++j)
            {
                string data = arrayData[j].Trim();
                arrayData[j] = data;
                string dataKey = keys[j];

                //기획변경으로 테이블에서 -1값을 실제 활용하게 되어 해당 조건은 뺌
                //if (string.IsNullOrEmpty(data) || data == "-1" || data == "null")
                if (string.IsNullOrEmpty(data) || data == "null")
                    continue;
                object value;
                try
                {
                    value = dataKey[0] == 'i' ? (object)int.Parse(data) : (dataKey[0] == 'f' ? (object)float.Parse(data) : data);
                }
                catch (FormatException)
                {
                    MyUtility.DebugLog(string.Format("Wrong Data Format - Table : {0} / Line : {1} / ID : {2} / Key : {3} / Data : {4}",
                        _dbTable.Key, i, id, dataKey, data));
                    continue;
                }
                eKEY_TABLEDB key;
                try
                {
                    key = MyUtility.ParsingStringToEnumType<eKEY_TABLEDB>(dataKey);
                }
                catch (ArgumentException)
                {
                    MyUtility.DebugLog("Undefined Key Data: " + "Key:" + dataKey);
                    continue;
                }
                if (!dicInGameDB.ContainsKey(id))
                    dicInGameDB.Add(id, new Dictionary<eKEY_TABLEDB, object>());
                //한 row의 전체 값들
                if (dicInGameDB[id].ContainsKey(key))
                    dicInGameDB[id][key] = value;
                else
                    dicInGameDB[id].Add(key, value);
            }
        }

        m_TableDB.Add(inGameDBTable, dicInGameDB);
    }

    private void EndLoadInGameDBTable()
    {
        MyUtility.DebugLog("########## End load InGameDB table ##########");
        m_Keys.Sort();
        m_bEndLoad = true;
    }

    public object GetData(eTABLE_LIST inGameDBTable, int id, string key)
    {
        try
        {
            Dictionary<int, Dictionary<eKEY_TABLEDB, object>> dicInGameDB;
            //인게임 db에 있는 테이블들 중 enum값 inGameDBTable에 해당하는 db를 가져옴
            if (m_TableDB.TryGetValue(inGameDBTable, out dicInGameDB) == true)
            {
                eKEY_TABLEDB inGameDBKey = MyUtility.ParsingStringToEnumType<eKEY_TABLEDB>(key);
                Dictionary<eKEY_TABLEDB, object> dicDB;
                object data;

                //그 db의 id행 한줄을 읽어오고(dicDB), 그 행의 inGameDBKey열 값을 리턴
                if (dicInGameDB.TryGetValue(id, out dicDB) == true && dicDB.TryGetValue(inGameDBKey, out data) == true)
                    return data;
            }
        }
        catch (Exception)
        {
            MyUtility.DebugLog("TableDB : Wrong eKEY_TABLEDB - " + key);
        }

        switch (MyUtility.ConvertToString(key).ToLower()[0])
        {
            case 'i':
                return -100;//기획변경으로 -1값을 테이블에서 사용하게 되어 읽기 실패시 -1 => -100리턴으로 변경
            case 'f':
                return 0.0f;
            case 's':
                return "noText";
            default:
                return null;
        }
    }

    public object GetData(eTABLE_LIST inGameDBTable, int id, eKEY_TABLEDB key)
    {
        return GetData(inGameDBTable, id, MyUtility.ConvertToString(key));
    }

    public int GetRowCount(eTABLE_LIST inGameDBTable)
    {
        Dictionary<int, Dictionary<eKEY_TABLEDB, object>> dicInGameDB;
        if (m_TableDB.TryGetValue(inGameDBTable, out dicInGameDB) == true)
        {
            return dicInGameDB.Count;
        }
        else
        {
            MyUtility.DebugLog("TableDB : Row count 0");
            return 0;
        }
    }

    public void Clear()
    {
        m_TableDB.Clear();
        m_Keys.Clear();
        instance = null;
    }    
}