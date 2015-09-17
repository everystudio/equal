using UnityEngine;
using System.Collections;

public class TitleString : MonoBehaviourEx {

	[SerializeField]
	private UILabel m_lbText;

	[SerializeField]
	private UISprite m_sprBack;

	public enum STEP {
		NONE	= 0,
		WAIT	,
		IDLE	,
		MAX		,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	[SerializeField]
	private float m_fDelay;

	private float m_fTimer;

	private float m_fAngle;

	// Use this for initialization
	void Start () {

		m_fTimer = 0.0f;

		m_eStep = STEP.WAIT;
		m_eStepPre = STEP.MAX;

		myTransform.localScale = Vector3.zero;

	
	}
	
	// Update is called once per frame
	void Update () {
		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}

		switch (m_eStep) {
		case STEP.WAIT:
			if (bInit) {
				m_fTimer = 0.0f;
			}
			m_fTimer += Time.deltaTime;
			if (m_fDelay < m_fTimer) {
				m_eStep = STEP.IDLE;
			}
			break;

		case STEP.IDLE:
			if (bInit) {
				TweenScale.Begin (gameObject, 1.5f, new Vector3( 1.5f , 1.5f , 1.5f ));
			}
			myTransform.localEulerAngles = new Vector3 (0.0f, 0.0f, m_fAngle);

			m_fAngle += 1.0f;



			break;

		case STEP.MAX:
		default:
			break;
		}
	}
}













