using UnityEngine;
using System.Collections;

public class TapStart : ButtonBase {

	[SerializeField]
	private UILabel m_lbTapStart;


	public enum STEP {
		NORMAL_BLINK	= 0,

		QUICK_BLINK		,

		MAX				,

	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	public bool m_bBlinkFlag;

	public float m_fTimer;
	public float m_fInterval;

	public void Show(){
		m_lbTapStart.enabled = true;
	}

	void Start(){
		m_eStep = STEP.NORMAL_BLINK;
		m_eStepPre = STEP.MAX;

		m_fTimer = 0.0f;
	}


	// Update is called once per frame
	void Update () {
		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}

		switch (m_eStep) {
		case STEP.NORMAL_BLINK:
			if (bInit) {
				m_fTimer = 0.0f;
				m_fInterval = 1.0f;
				m_bBlinkFlag = true;
			}

			float fRate = 1.0f;
			if (m_bBlinkFlag == false) {
				fRate = 3.0f;
			}

			m_fTimer += Time.deltaTime * fRate;
			if (m_fInterval < m_fTimer) {
				m_fTimer -= m_fInterval;
				m_bBlinkFlag = !m_bBlinkFlag;
				m_lbTapStart.enabled = m_bBlinkFlag;
			}

			if (ButtonPushed) {
				m_fInterval = 0.25f;
			} else {
				m_fInterval = 1.0f;
			}
			break;

		case STEP.QUICK_BLINK:
			break;

		case STEP.MAX:
		default:
			break;
		}
	
	}
}










