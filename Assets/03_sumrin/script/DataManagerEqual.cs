using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataManagerEqual : DataManagerBase<DataManagerEqual> {
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

	public override void Initialize ()
	{
		base.Initialize ();
		Debug.LogError ("call");
		DontDestroyOnLoad (this);			// 削除させない
		/*
		m_masterTableAudio.Load ("nouse");
		m_masterTablePrefab.Load ("nouse");
		m_masterTableSprite.Load ("nouse");
		*/

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
	#endregion
}
