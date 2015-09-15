using UnityEngine;
using System.Collections;

public class UnderNumber : MonoBehaviour {

	public enum STEP
	{
		NONE		= 0,
		IDLE		,
		CHANGE		,
		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	[SerializeField]
	private UILabel m_lbNumber;

	[SerializeField]
	private UISprite m_sprBackCircle;

	private int m_iNumber;
	private float m_fNumberTemp;			// 移動中の一時的な数字
	private float m_fNumberTarget;			// 目標の数字(本当はint型の方がいい気がする)
	private float m_fNumberDiv;				// いちいち計算で出さない

	public void SetNumber( int _iNumber ){
		// 処理をするのは違った時だけ
		if (m_iNumber != _iNumber) {
			// 実数を変更
			m_iNumber = _iNumber;
			m_fNumberTarget = (float)_iNumber;

			m_fNumberDiv = m_fNumberTarget - m_fNumberTemp;
			m_fNumberDiv = m_fNumberDiv / DefineProject.NUMBER_CHANGE_TIME;

			m_eStep = STEP.CHANGE;
		}
	}

	public void Initialize( int _iNumber ){
		m_iNumber = _iNumber;
		m_eStep = STEP.IDLE;
		return;
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
				m_lbNumber.text = m_iNumber.ToString ();
			}
			break;

		case STEP.CHANGE:
			bool bChangeEnd = false;

			float fNumberSum = m_fNumberTemp + m_fNumberDiv;

			if (0.0f < m_fNumberDiv) {
				if (m_fNumberTarget <= fNumberSum) {
					bChangeEnd = true;
				}
			} else {
				if (fNumberSum <= m_fNumberTarget) {
					bChangeEnd = true;
				}
			}
			if (bChangeEnd) {
				m_fNumberTemp = m_fNumberTarget;
				m_eStep = STEP.IDLE;
			} else {
				m_fNumberTemp = fNumberSum;
			}
			m_lbNumber.text = ((int)(m_fNumberTemp)).ToString ();
			break;

		case STEP.MAX:
		default:
			break;
		}
	}
}














