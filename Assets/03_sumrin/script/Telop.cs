using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Telop : MonoBehaviourEx {

	public enum STEP {
		NONE		= 0,
		IDLE		,
		OPEN		,
		MOVE		,
		CLOSE		,
		DELAY		,
		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	public UILabel m_lbMessage;
	private Queue<string> m_strMessageQueue = new Queue<string>();
	private string m_strMessage;
	private float m_fMove;

	private float m_fTimer;

	void Start(){
		m_eStep = STEP.IDLE;
		m_eStepPre = STEP.MAX;
		return;
	}

	public bool IsIdle(){
		return m_eStep == STEP.IDLE;
	}

	public void SetTelop( string _strMessage){
		m_strMessageQueue.Enqueue (_strMessage);
	}

	public float m_fFontSize= 70.0f;
	public float m_fSpeed = 1.0f;

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
				myTransform.localScale = new Vector3 (1.0f, 0.0f, 1.0f);
			}
			if (0 < m_strMessageQueue.Count) {
				m_eStep = STEP.OPEN;
			}
			break;
		case STEP.OPEN:
			if (bInit) {
				m_strMessage = m_strMessageQueue.Dequeue ();
				TweenScale ts = TweenScale.Begin (gameObject, 1.0f, Vector3.one);
				m_bEndTween = false;
				EventDelegate.Set (ts.onFinished, EndTween);

				m_lbMessage.text = m_strMessage;
				m_lbMessage.gameObject.transform.localPosition = new Vector3 (320.0f, 0.0f, 0.0f);
			}
			if (m_bEndTween) {
				m_eStep = STEP.MOVE;
			}
			break;
		case STEP.MOVE:
			if (bInit) {

				m_fFontSize = m_lbMessage.fontSize;
				TweenPosition tp = TweenPosition.Begin (m_lbMessage.gameObject, (m_strMessage.Length*m_fFontSize + 640.0f ) / m_fSpeed, new Vector3 (-320.0f - (m_strMessage.Length * 70.0f), 0.0f, 0.0f));
				m_bEndTween = false;
				EventDelegate.Set (tp.onFinished, EndTween);
				m_fTimer = Time.realtimeSinceStartup;
			}
			if (m_bEndTween) {
				//Debug.LogError( Time.realtimeSinceStartup - m_fTimer );
				m_eStep = STEP.CLOSE;
			}
			break;
		case STEP.CLOSE:
			if (bInit) {
				TweenScale ts = TweenScale.Begin (gameObject, 1.0f, new Vector3( 1.0f , 0.0f , 1.0f ));
				m_bEndTween = false;
				EventDelegate.Set (ts.onFinished, EndTween);
			}
			if (m_bEndTween) {
				m_eStep = STEP.DELAY;
			}
			break;
		case STEP.DELAY:
			if (bInit) {
				m_fTimer = 0.0f;
			}
			m_fTimer += Time.deltaTime;
			if (3.0f < m_fTimer) {
				m_eStep = STEP.IDLE;
			}
			break;
		case STEP.MAX:
		default:
			break;
		}
	
	}
}
