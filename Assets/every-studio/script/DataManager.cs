using UnityEngine;
using System.Collections;
using System.Collections.Generic;

			// リスト使うなら追加しないといけないっ！
using System;

public class DataCopyUtil
{
	static public bool checkContainsValue (IDictionary _iDict, string _strKey, bool _bLog)
	{
		if (_iDict.Contains (_strKey) && !(_iDict [_strKey] == null)) {
			return true;
		}
		if (_bLog) {
			string error_msg = "error key name :[" + _strKey + "]";
			////Debug.Log( error_msg );
		}
		Debug.Log ("ダメな処理のキー洗い出し" + _strKey);
		return false;
	}

	static public bool copyLongToInt (ref int _lData, IDictionary _iDict, string _strKey, bool _bLog = true)
	{
		if (DataCopyUtil.checkContainsValue (_iDict, _strKey, _bLog)) {
			_lData = (int)(long)_iDict [_strKey];
			return true;
		}
		return false;
	}

	static public bool copyString (ref string _strData, IDictionary _iDict, string _strKey, bool _bLog = true)
	{
		if (DataCopyUtil.checkContainsValue (_iDict, _strKey, _bLog)) {
			_strData = (string)_iDict [_strKey];
			return true;
		}
		return false;
	}

	static public bool copyLong (ref long _strData, IDictionary _iDict, string _strKey, bool _bLog = true)
	{
		if (DataCopyUtil.checkContainsValue (_iDict, _strKey, _bLog)) {
			_strData = (long)_iDict [_strKey];
			return true;
		}
		return false;
	}

	static public bool copyFloat (ref float _strData, IDictionary _iDict, string _strKey, bool _bLog = true)
	{
		if (DataCopyUtil.checkContainsValue (_iDict, _strKey, _bLog)) {
			_strData = (float)_iDict [_strKey];
			return true;
		}
		return false;
	}

	static public bool copyFloatV (ref float _strData, IDictionary _iDict, string _strKey, bool _bLog = true)
	{
		if (DataCopyUtil.checkContainsValue (_iDict, _strKey, _bLog)) {
			var temp = _iDict [_strKey];
			_strData = float.Parse (temp.ToString ());
			return true;
		}
		return false;
	}

	static public bool copyBool (ref bool _strData, IDictionary _iDict, string _strKey, bool _bLog = true)
	{
		if (DataCopyUtil.checkContainsValue (_iDict, _strKey, _bLog)) {
			_strData = (bool)_iDict [_strKey];
			return true;
		}
		return false;
	}
}

public class DebugData{
	public bool bDebugGoldRush;
	public DebugData(){
	}
}

public class DataManager : MonoBehaviour
{
	// ====================================================================================
	// 以下シングルトン宣言
	private static DataManager instance = null;

	public static DataManager Instance {
		get {
			if (instance == null) {
				GameObject obj = GameObject.Find ("DataManager");
				if (obj == null) {
					obj = new GameObject("DataManager");
					//Debug.LogError ("Not Exist AtlasManager!!");
				}
				instance = obj.GetComponent<DataManager> ();
				if (instance == null) {
					//Debug.LogError ("Not Exist AtlasManager Script!!");
					instance = obj.AddComponent<DataManager>() as DataManager;
				}
				instance.initialize ();
			}
			return instance;
		}
	}
	//これがないとネットワークソースの方でエラーがでるので。
	public string uuid;
	public long uid;
	public string sid;
	public string game_return;
	public DebugData tDebugData = new DebugData();

	public int m_iCurrentStageId;

	public int CopySid (IDictionary _iDict)
	{
		if (_iDict.Contains ("sid")) {
			DataCopyUtil.copyString (ref this.sid, _iDict, "sid");
		}
		return 0;
	}


	private void initialize(){
		DontDestroyOnLoad (this);			// 削除させない

		m_masterTableAudio.Load ();
		m_masterTablePrefab.Load ();
		m_masterTableSprite.Load ();

		m_csvLevel.Load ();

		m_bLoadedStageStatus = false;
		StartCoroutine (load_stage_status());
	}


	public CsvAudio m_masterTableAudio = new CsvAudio();
	static public List<CsvAudioData> master_audio_list {
		get{ 
			return Instance.m_masterTableAudio.All;
		}
	}
	public CsvPrefab m_masterTablePrefab = new CsvPrefab();
	static public List<CsvPrefabData> master_prefab_list {
		get{ 
			return Instance.m_masterTablePrefab.All;
		}
	}
	public CsvSprite m_masterTableSprite = new CsvSprite();
	static public List<CsvSpriteData> master_sprite_list {
		get{ 
			return Instance.m_masterTableSprite.All;
		}
	}

	#region Project依存 // ---------------------------------------------
	public int m_iPlayLevel;
	public CsvLevel m_csvLevel = new CsvLevel();
	static public List<CsvLevelData> csv_level_list {
		get{ 
			return Instance.m_csvLevel.All;
		}
	}
	public CsvLevelData GetLevelData( int _iLevel ){
		foreach (CsvLevelData data in csv_level_list) {
			if (data.level == _iLevel) {
				return data;
			}
		}
		return new CsvLevelData ();
	}

	public bool m_bLoadedStageStatus;
	public IEnumerator load_stage_status (){
		for (int i = 1; i < 100; i++) {
			int iStageStatus = PlayerPrefs.GetInt (DefineProject.GetKeyStage (i));
			m_iStageStatusArr [i] = iStageStatus;
		}
		yield return 0;
		Debug.Log ("loaded");
		m_bLoadedStageStatus = true;
		yield return 0;
	}
	public bool IsReadyStageData{
		get{ return m_bLoadedStageStatus; }
	}

	public int [] m_iStageStatusArr = new int[100];
	public DefineProject.STAGE_STATUS GetStageStatus( int _iLevel ){
		return (DefineProject.STAGE_STATUS)m_iStageStatusArr[_iLevel];
	}
	public void SetStageStatus( int _iLevel , DefineProject.STAGE_STATUS _eStatus ){
		PlayerPrefs.SetInt (DefineProject.GetKeyStage (_iLevel), (int)_eStatus);
		m_iStageStatusArr [_iLevel] = (int)_eStatus;
		return;
	}


	#endregion // ------------------------------------------------------



}









