using UnityEngine;
using System.Collections;

public class HelpMain : PageBase {

	public UILabel m_lbReturn;
	public UILabel m_lbScale;
	public ButtonBase m_btnReturn;
	public ButtonBase m_btnScale;
	public UI2DSprite m_sprScale;

	public bool m_bBig;

	public override void PageStart ()
	{
		base.PageStart ();
		m_lbReturn.text = "戻";
		m_lbScale.text = "大";
		m_bBig = false;

		m_btnReturn.TriggerClear ();
		m_btnScale.TriggerClear ();

	}

	public override void PageEnd ()
	{
		base.PageEnd ();
	}

	void Update(){

		if (m_btnReturn.ButtonPushed) {
			m_bIsEnd = true;
			m_btnReturn.TriggerClear ();
		}
		if (m_btnScale.ButtonPushed) {
			m_btnScale.TriggerClear ();

			if (m_bBig == false) {
				m_sprScale.width = 1000;
				m_lbScale.text = "小";
			} else {
				m_sprScale.width = 640;
				m_lbScale.text = "大";
			}

			m_bBig = !m_bBig;
		}
		
	}

}
