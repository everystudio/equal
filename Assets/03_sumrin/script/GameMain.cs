using UnityEngine;
using System.Collections;

public class GameMain : MonoBehaviour {

	[SerializeField]
	private DotManager m_DotManager;

	[SerializeField]
	private UnderNumber m_UnderNumberRed;
	[SerializeField]
	private UnderNumber m_UnderNumberBlue;

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

		MAX				,

	}
	public STEP m_eStep;
	public STEP m_eStepPre;


	void Start(){
		m_eStep = STEP.START;
		m_eStepPre = STEP.MAX;
	}

	void Update(){

		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}

		switch (m_eStep) {
		case STEP.START:
			m_eStep = STEP.SKIT;

			m_UnderNumberRed.Initialize (m_DotManager.GetNumber (DefineProject.DOT_COLOR.RED));
			m_UnderNumberBlue.Initialize (m_DotManager.GetNumber (DefineProject.DOT_COLOR.BLUE));

			m_DotManager.Initialize ( 9 , new int[9]{ 1, 2, 3, 4, 5, 6, 7, 8, 9 });


			break;
		case STEP.SKIT:
			m_eStep = STEP.IDLE;
			break;
		case STEP.IDLE:
			if (bInit) {
				m_DotManager.ButtonInit ();
				m_DotManager.TriggerClearAll ();
			}

			if (m_DotManager.ButtonPushed) {
				m_eStep = STEP.CLICK_DOT;
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
			int iNumber = 0;
			if (m_DotManager.IsClear (ref iNumber, new int[(int)DefineProject.DOT_COLOR.MAX] {0, 3, 3, 0})) {
				m_eStep = STEP.CLEAR;
			} else {
				m_eStep = STEP.IDLE;
			}
			break;
		case STEP.CLEAR:
			break;
		case STEP.NEXT:
			break;
		case STEP.MAX:
		default:
			break;
		}

	}

}






















