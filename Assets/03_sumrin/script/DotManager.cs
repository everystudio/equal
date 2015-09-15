using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DotManager : ButtonManager {

	[SerializeField]
	List<Dot> m_DotList = new List<Dot>();

	public int GetNumber( DefineProject.DOT_COLOR _eColor ){
		int iRet = 0;
		foreach (Dot dot in m_DotList) {
			if (dot.GetDotColor () == _eColor) {
				iRet += dot.Number;
			}
		}
		return iRet;
	}

	public void Initialize( int _iDotNum , int [] _iintialDot ){
		for (int i = 0; i < _iDotNum; i++) {
			m_DotList[i].SetNumber (_iintialDot [i]);
		}
	}

	public bool IsClear( ref int _iNumber , int [] _iCheckCount ){
		// 初期化
		_iNumber = 0;

		int iCheckIndexMaster = 0;
		int iCheckIndexCount = 0;
		// チェックする色を確認
		bool[] check_list = new bool[(int)DefineProject.DOT_COLOR.MAX];
		int[] tempNumber = new int[(int)DefineProject.DOT_COLOR.MAX];

		for (int i = 0; i < (int)DefineProject.DOT_COLOR.MAX; i++) {
			tempNumber [i] = 0;
			if (0 < _iCheckCount [i]) {
				check_list [i] = true;
				iCheckIndexCount += 1;
				if (iCheckIndexMaster == 0) {
					iCheckIndexMaster = i;
				}
			} else {
				check_list [i] = false;
			}
		}

		if (iCheckIndexCount < 2) {
			Debug.LogError ("less check dot");
		}

		foreach (Dot dot in m_DotList) {
			int iIndex = (int)dot.GetDotColor ();
			if (check_list [iIndex]) {
				tempNumber [iIndex] += dot.Number;
				_iCheckCount [iIndex] -= 1;
			}
		}

		bool bRet = true;
		for (int i = 0; i < (int)DefineProject.DOT_COLOR.MAX; i++) {
			if (0 < _iCheckCount [i]) {
				Debug.Log( string.Format( "選択数が足りません:{0}({1})" , (DefineProject.DOT_COLOR)i , _iCheckCount[i] ));
				bRet = false;
			}
		}

		if (bRet == false) {
			return bRet;
		}


		int iMasterNumber = tempNumber [iCheckIndexMaster];

		for (int i = 0; i < (int)DefineProject.DOT_COLOR.MAX; i++) {
			if (check_list [i] == true) {
				if (tempNumber [i] == iMasterNumber) {
					;// スルー
				} else {
					bRet = false;
					Debug.Log( string.Format( "数値が一致しません:{0}({1})" , (DefineProject.DOT_COLOR)i , tempNumber[i] ));
				}
			}
		}

		return bRet;


	}

	public void SetDotColor( int _iIndex , DefineProject.DOT_COLOR _eDotColor ){
		// require range check
		m_DotList [_iIndex].SetDotColor (_eDotColor);
		return;
	}

	public int GetCount( DefineProject.DOT_COLOR _eDotColor ){
		int iCount = 0;
		foreach (Dot dot in m_DotList) {
			if (dot.GetDotColor () == _eDotColor) {
				iCount += 1;
			}
		}
		return iCount;
	}

	public bool ChangeDotColor( int _iIndex ,int _iFullCount , bool _bInterigens = true ){

		Dot dot = m_DotList [_iIndex];

		DefineProject.DOT_COLOR eDotColor = dot.GetDotColor ();

		DefineProject.DOT_COLOR eNextDotColor = GetNextDotColor (eDotColor);

		if (_bInterigens) {

			// 白になってたらもう終わらせる
			while (eNextDotColor != DefineProject.DOT_COLOR.WHITE) {

				int iCount = GetCount (eNextDotColor);
				if (iCount < _iFullCount) {
					break;
				}
				eNextDotColor = GetNextDotColor (eNextDotColor);
			}
		}

		bool bRet = (eNextDotColor != eDotColor);
		if (bRet) {
			dot.SetDotColor (eNextDotColor);
		}

		// 違う色になってたら真
		return bRet;
	}

	public DefineProject.DOT_COLOR GetNextDotColor( DefineProject.DOT_COLOR _eDotColor ){
		DefineProject.DOT_COLOR eRet = DefineProject.DOT_COLOR.WHITE;
		switch (_eDotColor) {
		case DefineProject.DOT_COLOR.WHITE:
			eRet = DefineProject.DOT_COLOR.RED;
			break;
		case DefineProject.DOT_COLOR.RED:
			eRet = DefineProject.DOT_COLOR.BLUE;
			break;
		case DefineProject.DOT_COLOR.BLUE:
			eRet = DefineProject.DOT_COLOR.WHITE;
			break;
		default:
			eRet = DefineProject.DOT_COLOR.WHITE;
			break;
		}
		return eRet;
	}

}
