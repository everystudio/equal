using UnityEngine;
using System.Collections;

public class ReviewDialog : MonoBehaviourEx {

	public enum STEP
	{
		IDLE		= 0,
		REVIEW		,
		SHINAI		,
		LATER		,
		END			,
		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	[SerializeField]
	private ButtonBase m_btnReview;
	[SerializeField]
	private ButtonBase m_btnNone;
	[SerializeField]
	private ButtonBase m_btnLater;
	[SerializeField]
	private ButtonBase m_btnReviewed;

	private bool m_bIsEnd;

	public void Initialize(){
		m_btnReview.TriggerClear ();
		m_btnNone.TriggerClear ();
		m_btnLater.TriggerClear ();
		m_eStep = STEP.IDLE;
		m_eStepPre = STEP.MAX;
		m_bIsEnd = false;
	}

	public bool IsEnd(){
		return m_bIsEnd;
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
			}

			if (m_btnReview.ButtonPushed) {
				m_eStep = STEP.REVIEW;
			} else if (m_btnNone.ButtonPushed) {
				m_eStep = STEP.SHINAI;
			} else if (m_btnLater.ButtonPushed) {
				m_eStep = STEP.LATER;
			} else {
			}

			break;

		case STEP.REVIEW:
			if (bInit) {
				m_btnReviewed.gameObject.SetActive (true);
				TweenAlphaAll (m_btnReviewed.gameObject, 0.0f, 0.0f);
				TweenAlphaAll (m_btnReviewed.gameObject, 1.0f, 1.0f);
				m_btnReviewed.TriggerClear ();
				PlayerPrefs.SetInt ("review", 1);

				Application.OpenURL ("https://play.google.com/store/apps/details?id=jp.everystudio.equal");
			}
			if (m_btnReviewed.ButtonPushed) {
				TweenAlphaAll (m_btnReviewed.gameObject, 0.5f, 0.0f);
				m_eStep = STEP.END;
			}

			break;
		case STEP.SHINAI:
			PlayerPrefs.SetInt ("review", 2);
			m_eStep = STEP.END;
			break;
		case STEP.LATER:
			m_eStep = STEP.END;
			break;
		case STEP.END:
			if (bInit) {
				TweenAlpha ta = TweenAlphaAll (gameObject, 0.5f, 0.0f);
				EventDelegate.Set (ta.onFinished, EndTween);
			}
			if (m_bEndTween) {
				m_bIsEnd = true;
			}
			break;

		case STEP.MAX:
		default:
			break;
		}
	
	}
}

















