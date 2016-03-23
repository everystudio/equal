using UnityEngine;
using System.Collections;

public class LevelIcon : ButtonBase {

	[SerializeField]
	private UISprite m_sprBack;

	[SerializeField]
	private UILabel m_lbText;

	//private int m_iLevel;

	public void Initialize( int _iLevel ){

		//m_iLevel = _iLevel;

		m_lbText.text = _iLevel.ToString ();

		DefineProject.STAGE_STATUS eStageStatus = DataManagerEqual.Instance.GetStageStatus (_iLevel);

		switch( eStageStatus){
		case DefineProject.STAGE_STATUS.NONE:
			m_sprBack.color = new Color (0.5f, 0.5f, 0.5f);
			break;

		case DefineProject.STAGE_STATUS.NO_PLAY:
			m_sprBack.color = Color.white;
			break;

		case DefineProject.STAGE_STATUS.CHALLENGING:
			m_sprBack.color = Color.white;
			break;

		case DefineProject.STAGE_STATUS.CLEARED:
			m_sprBack.color = Color.yellow;
			break;

		case DefineProject.STAGE_STATUS.MAX:
		default:
			break;

		}


	}

	// Update is called once per frame
	void Update () {
	
	}
}
