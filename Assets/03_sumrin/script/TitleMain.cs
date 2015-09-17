using UnityEngine;
using System.Collections;
using NendUnityPlugin.AD;

public class TitleMain : MonoBehaviour {

	[SerializeField]
	private TapStart m_TapStart;
	[SerializeField]
	private TapStart m_StageSelect;
	[SerializeField]
	private TapStart m_ScoreAttack;

	[SerializeField]
	private NendAdIcon m_NendAdIcon;


	private float m_fTimer;

	public enum STEP {
		IDLE			= 0,
		DELAY			,
		GAME_START		,

		MODE_SELECT		,

		DELAY_STAGE_SELECT	,
		DELAY_SCORE_ATTACK	,

		STAGE_SELECT	,
		SCORE_ATTACK	,

		MAX				,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	void ShowAd(bool _bFlag){
		if (_bFlag) {
			m_NendAdIcon.Show ();
			m_NendAdIcon.Resume ();

		} else {
			m_NendAdIcon.Hide ();
			m_NendAdIcon.Pause ();
		}
	}

	void OnEnable(){
		ShowAd (true);
	}

	void OnDisable(){
		ShowAd (false);
	}

	// Use this for initialization
	void Start () {
		m_eStep = STEP.IDLE;
		m_eStepPre = STEP.MAX;
		FadeInOut.Instance.Close (0.0f);
		FadeInOut.Instance.Open (0.5f);
	
		m_StageSelect.gameObject.SetActive (false);
		m_ScoreAttack.gameObject.SetActive (false);

		DataManager.Instance.m_iPlayLevel = 0;

	}
	
	// Update is called once per frame
	void Update () {
		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}

		switch (m_eStep) {
		case STEP.IDLE:
			if (bInit) {
				m_TapStart.TriggerClear ();
			}
			if (m_TapStart.ButtonPushed) {
				m_eStep = STEP.DELAY;
			}
			break;
		case STEP.DELAY:
			if (bInit) {
				m_fTimer = 0.0f;
			}
			m_fTimer += Time.deltaTime;
			if (1.0f < m_fTimer) {
				m_eStep = STEP.MODE_SELECT;
			}
			break;

		case STEP.MODE_SELECT:
			if (bInit) {
				m_TapStart.gameObject.SetActive (false);
				m_StageSelect.gameObject.SetActive (true);
				m_ScoreAttack.gameObject.SetActive (true);

				m_StageSelect.TriggerClear ();
				m_ScoreAttack.TriggerClear ();
			}
			if (m_StageSelect.ButtonPushed) {
				m_eStep = STEP.DELAY_STAGE_SELECT;
			} else if (m_ScoreAttack.ButtonPushed) {
				m_eStep = STEP.DELAY_SCORE_ATTACK;
			} else {
			}
			break;

		case STEP.DELAY_STAGE_SELECT:
			if (bInit) {
				m_fTimer = 0.0f;
			}
			m_fTimer += Time.deltaTime;
			if (1.0f < m_fTimer) {
				m_eStep = STEP.STAGE_SELECT;
			}
			break;
		case STEP.DELAY_SCORE_ATTACK:
			if (bInit) {
				m_fTimer = 0.0f;
			}
			m_fTimer += Time.deltaTime;
			if (1.0f < m_fTimer) {
				m_eStep = STEP.SCORE_ATTACK;
			}
			break;

		case STEP.STAGE_SELECT:
			if (bInit) {
				m_StageSelect.Show ();
				ShowAd (false);
				FadeInOut.Instance.Close (0.25f);
			}
			if (FadeInOut.Instance.IsIdle () && DataManager.Instance.IsReadyStageData) {
				Application.LoadLevelAsync ("select");
			}
			break;
		case STEP.SCORE_ATTACK:
			if (bInit) {
				//Application.LoadLevelAsync ("score_attack");
				m_ScoreAttack.gameObject.GetComponent<UILabel> ().text = "準備中です";
				m_ScoreAttack.Show ();
				m_ScoreAttack.enabled = false;

			}
			m_eStep = STEP.MODE_SELECT;
			break;

		default:
			break;

		}
	}
}
















