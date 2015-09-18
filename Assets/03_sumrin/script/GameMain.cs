﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NendUnityPlugin.AD;

public class GameMain : MonoBehaviourEx {

	[SerializeField]
	private DotManager m_DotManager;

	[SerializeField]
	private NendAdBanner m_NendAdBanner;
	[SerializeField]
	private NendAdIcon m_NendAdIcon;
	//[SerializeField]
	//private NendAdInterstitial m_NendAdInterstitial;


	[SerializeField]
	private UILabel m_lbHeaderLevel;

	[SerializeField]
	private UnderNumber m_UnderNumberRed;
	[SerializeField]
	private UnderNumber m_UnderNumberBlue;

	[SerializeField]
	private ClearDialog m_ClearDialog;

	[SerializeField]
	private ButtonBase m_btnBack;

	public enum STEP {
		NONE			= 0,
		START			,
		RESUME_CHECK	,

		SKIT			,

		IDLE			,

		CLICK_DOT		,
		CLEAR_CHECK		,

		CLEAR			,

		NEXT			,

		BACK_SELECT		,

		MAX				,

	}
	public STEP m_eStep;
	public STEP m_eStepPre;
	public float m_fTimer;
	public int m_iClearScore;
	public bool m_bDispFlag;

	void ShowAd(bool _bFlag){

		if (m_bDispFlag == _bFlag) {
			return;
		} else {
			m_bDispFlag = _bFlag;
		}

		if (_bFlag) {
			m_NendAdBanner.Show ();
			m_NendAdBanner.Resume ();
			m_NendAdIcon.Show ();
			m_NendAdIcon.Resume ();

		} else {
			m_NendAdBanner.Hide ();
			m_NendAdBanner.Pause ();
			m_NendAdIcon.Hide ();
			m_NendAdIcon.Pause ();
		}
	}

	void OnEnable(){
		Debug.Log ("OnEnable");
		ShowAd (true);
	}

	void OnDisable(){
		Debug.Log ("OnDisable");
		ShowAd (false);
	}

	void Start(){
		m_bDispFlag = false;
		// 先にロード
		NendAdInterstitial.Instance.Load (DefineProject.NEND_AD_INTER_API_KEY, DefineProject.NEND_AD_INTER_SPOT_ID);

		FadeInOut.Instance.Close (0.0f);

		m_eStep = STEP.START;
		m_eStepPre = STEP.MAX;

		m_ClearDialog.gameObject.SetActive (false);
	}

	void Update(){

		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}

		switch (m_eStep) {
		case STEP.START:

			if (bInit) {
				m_fTimer = 0.0f;

				m_UnderNumberRed.Initialize (m_DotManager.GetNumber (DefineProject.DOT_COLOR.RED));
				m_UnderNumberBlue.Initialize (m_DotManager.GetNumber (DefineProject.DOT_COLOR.BLUE));

				if (DataManager.Instance.m_iPlayLevel == 0) {
					DataManager.Instance.m_iPlayLevel = 1;
				}
				CsvLevelData level_data = DataManager.Instance.GetLevelData (DataManager.Instance.m_iPlayLevel);
				m_DotManager.Initialize (level_data);

				m_UnderNumberRed.SetNumber (m_DotManager.GetNumber (DefineProject.DOT_COLOR.RED));
				m_UnderNumberBlue.SetNumber (m_DotManager.GetNumber (DefineProject.DOT_COLOR.BLUE));

				m_lbHeaderLevel.text = string.Format ("Level {0}", DataManager.Instance.m_iPlayLevel);
			}
			m_fTimer += Time.deltaTime;
			if (0.5f < m_fTimer) {
				m_eStep = STEP.SKIT;
			}
			break;
		case STEP.SKIT:
			if (bInit) {
				FadeInOut.Instance.Open (0.25f);
			}
			if (FadeInOut.Instance.IsIdle ()) {
				ShowAd (true);

				m_eStep = STEP.IDLE;
			}
			break;
		case STEP.IDLE:
			if (bInit) {
				m_DotManager.ButtonInit ();
				m_DotManager.TriggerClearAll ();

				m_btnBack.TriggerClear ();
			}

			if (m_DotManager.ButtonPushed) {
				m_eStep = STEP.CLICK_DOT;
			} else if (m_btnBack.ButtonPushed) {
				m_eStep = STEP.BACK_SELECT;
			} else {
			}
			break;
		case STEP.CLICK_DOT:
			if (bInit) {

				int iClickedDotIndex = m_DotManager.Index;
				m_DotManager.ChangeDotColor (iClickedDotIndex , 3 , true );

				m_UnderNumberRed.SetNumber (m_DotManager.GetNumber (DefineProject.DOT_COLOR.RED));
				m_UnderNumberBlue.SetNumber (m_DotManager.GetNumber (DefineProject.DOT_COLOR.BLUE));

			}

			m_eStep = STEP.CLEAR_CHECK;
			break;
		case STEP.CLEAR_CHECK:
			if (m_DotManager.IsClear (ref m_iClearScore, new int[(int)DefineProject.DOT_COLOR.MAX] {0, 3, 3, 0})) {
				m_eStep = STEP.CLEAR;
			} else {
				m_eStep = STEP.IDLE;
			}
			break;
		case STEP.CLEAR:
			if (bInit) {

				// サーバー側で設定した回数で表示される
				NendAdInterstitial.Instance.Show (DefineProject.NEND_AD_INTER_SPOT_ID);

				DataManager.Instance.SetStageStatus (DataManager.Instance.m_iPlayLevel, DefineProject.STAGE_STATUS.CLEARED);
				if (DataManager.Instance.GetStageStatus (DataManager.Instance.m_iPlayLevel + 1) == DefineProject.STAGE_STATUS.NONE) {
					DataManager.Instance.SetStageStatus (DataManager.Instance.m_iPlayLevel+1, (DefineProject.STAGE_STATUS.NO_PLAY));
				}

				m_ClearDialog.gameObject.SetActive (true);
				TweenAlphaAll (m_ClearDialog.gameObject, 0.0f, 0.0f);
				TweenAlphaAll (m_ClearDialog.gameObject, 0.5f, 1.0f);
				m_ClearDialog.Initialize (DataManager.Instance.m_iPlayLevel , m_iClearScore );

				m_btnBack.TriggerClear ();
				ShowAd (false);

			}
			if (m_ClearDialog.IsNext ()) {

				m_eStep = STEP.NEXT;
				TweenAlphaAll (m_ClearDialog.gameObject, 0.5f, 0.0f);
			}

			if (m_btnBack.ButtonPushed) {
				m_eStep = STEP.BACK_SELECT;
			} 
			break;
		case STEP.NEXT:
			if (bInit) {
				DataManager.Instance.m_iPlayLevel += 1;
				m_eStep = STEP.START;
			}
			break;

		case STEP.BACK_SELECT:
			if (bInit) {
				ShowAd (false);
				FadeInOut.Instance.Close (0.25f);
			}
			if (FadeInOut.Instance.IsIdle ()) {
				Application.LoadLevelAsync ("select");
			}
			break;
		case STEP.MAX:
		default:
			break;
		}

	}

}






















