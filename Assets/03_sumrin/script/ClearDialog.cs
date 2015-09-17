﻿using UnityEngine;
using System.Collections;

public class ClearDialog : MonoBehaviour {

	[SerializeField]
	private ButtonBase m_btnNext;

	[SerializeField]
	private ButtonBase m_btnShare;

	private int m_iLevel;
	private int m_iScore;

	public void Initialize( int _iLevel , int _iScore ){
		m_iLevel = _iLevel;
		m_iScore = _iScore;

		m_btnNext.TriggerClear ();
		m_btnShare.TriggerClear ();
		return;
	}

	public bool IsNext(){
		return m_btnNext.ButtonPushed;
	}

	// Update is called once per frame
	void Update () {
	
		if (m_btnNext.ButtonPushed) {
		} else if (m_btnShare.ButtonPushed) {
			m_btnShare.TriggerClear ();
			OnShareBtn ();
		} else {
		}
		return;
	}

	void OnShareBtn() {
		StartCoroutine("WaitSS");
	}

	IEnumerator WaitSS () {


		// レイアウト設定のために1フレーム待つ
		yield return new WaitForEndOfFrame ();

		Application.CaptureScreenshot("image.png");


		// キャプチャを保存する処理として１フレーム待つ
		yield return new WaitForEndOfFrame ();

		#if UNITY_ANDROID
		Debug.Log (Application.persistentDataPath);
		SocialConnector.Share(
			string.Format( "Level{0}を{1}い〜くあるでクリア！ {1}" , m_iLevel , m_iScore, DefineProject.TWEET_MESSAGE_CLEAR )
		);
		#elif UNITY_IPHONE
		SocialConnector.Share(
			DefineProject.TWEET_MESSAGE_CLEAR,
			"", 
			Application.persistentDataPath + "/image.png"
		);   
		#endif
	}

}








