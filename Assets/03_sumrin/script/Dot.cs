using UnityEngine;
using System.Collections;

public class Dot : ButtonBase {

	public int m_iNumber;
	public int Number{
		get{ return m_iNumber; }
	}
	public void SetNumber( int _iNumber ){
		m_iNumber = _iNumber;
		m_lbNumber.text = m_iNumber.ToString ();
		return;
	}

	[SerializeField]
	private UISprite m_sprBackBoard;

	[SerializeField]
	private UILabel m_lbNumber;

	DefineProject.DOT_COLOR m_eDotColor;
	public void SetDotColor( DefineProject.DOT_COLOR _eColor ){
		if (m_eDotColor != _eColor) {
			//Debug.Log (string.Format ("change color : {0}", _eColor));
			//iTween.ColorTo (m_sprBackBoard.gameObject, DefineProject.GetColor (_eColor), DefineProject.COLOR_CHANGE_TIME);

			TweenColor.Begin (m_sprBackBoard.gameObject, DefineProject.COLOR_CHANGE_TIME, DefineProject.GetColor (_eColor));
			m_eDotColor = _eColor;
		} else {
			//Debug.Log ("same color");
		}
		return;
	}
	public DefineProject.DOT_COLOR GetDotColor(){
		return m_eDotColor;
	}



}














