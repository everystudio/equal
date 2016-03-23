using UnityEngine;
using System.Collections;
using NendUnityPlugin.AD;

using Prime31;

public class TitleMain : MonoBehaviourEx {

	[SerializeField]
	private TapStart m_TapStart;
	[SerializeField]
	private TapStart m_StageSelect;
	[SerializeField]
	private TapStart m_ScoreAttack;
	[SerializeField]
	private ReviewDialog m_ReviewDialog;
	#if UNITY_ANDROID
	[SerializeField]
	private NendAdIcon m_NendAdIcon;
	#endif

	private float m_fTimer;

	public enum STEP {
		IDLE			= 0,
		DELAY			,
		GAME_START		,

		REVIEW			,
		REVIEW_THANKYOU	,
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
		#if UNITY_ANDROID
		if (_bFlag) {
			if( m_NendAdIcon != null ){
				m_NendAdIcon.Show ();
				m_NendAdIcon.Resume ();
			}

		} else {
			if( m_NendAdIcon != null ){
				m_NendAdIcon.Hide ();
				m_NendAdIcon.Pause ();
			}
		}
		#endif
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

		DataManagerEqual.Instance.m_iPlayLevel = 0;
		string strReviewCount = "review_count";
		string strReview = "review";

		StoreManager.Instance.DummyCall ();


		int iCheckCount = 4;
		if (PlayerPrefs.HasKey (strReview) == false) {
			PlayerPrefs.SetInt (strReview, 0); 
		}
		if (0 == PlayerPrefs.GetInt (strReview)) {
			if (PlayerPrefs.HasKey (strReviewCount) == false) {
				PlayerPrefs.SetInt (strReviewCount, 1);
			} else {
				int iReviewCount = PlayerPrefs.GetInt (strReviewCount);
				iReviewCount += 1;
				if (iCheckCount < iReviewCount) {
					m_eStep = STEP.REVIEW;
					iReviewCount = 0;
				}
				PlayerPrefs.SetInt (strReviewCount, iReviewCount);
			}
		}
		Debug.Log ("Initialized");
	}
	
	// Update is called once per frame
	void Update () {
		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}

		switch (m_eStep) {

		case STEP.REVIEW:
			if (bInit) {
				m_ReviewDialog.gameObject.SetActive (true);
				m_ReviewDialog.Initialize ();
				TweenAlphaAll (m_ReviewDialog.gameObject, 0.0f, 0.0f);
				TweenAlphaAll (m_ReviewDialog.gameObject, 0.5f, 1.0f);
			}
			if (m_ReviewDialog.IsEnd ()) {
				m_eStep = STEP.IDLE;
			}
			break;

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
				//m_ScoreAttack.gameObject.SetActive (true);

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
			if (FadeInOut.Instance.IsIdle () && DataManagerEqual.Instance.IsReadyStageData) {
				Application.LoadLevelAsync ("select");
			}
			break;
		case STEP.SCORE_ATTACK:
			if (bInit) {
				//Application.LoadLevelAsync ("score_attack");
				m_ScoreAttack.gameObject.GetComponent<UILabel> ().text = "準備中です";
				m_ScoreAttack.Show ();
				m_ScoreAttack.enabled = false;

				if (m_bChange == true) {
					GoogleIAB.purchaseProduct ("comicticket100");
				} else {
					GoogleIAB.purchaseProduct ("equal.month.item001");
				}
				m_bChange = !m_bChange;
			}



			m_eStep = STEP.MODE_SELECT;
			break;

		default:
			break;

		}
	}
	public bool m_bChange;
}
















