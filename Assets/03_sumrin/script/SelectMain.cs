using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectMain : ButtonManager {

	#region SerializeFieldでの設定が必要なメンバー変数
	[SerializeField]
	private UICenterOnChild m_csCenterOnChild;
	[SerializeField]
	private GameObject m_goScrollView;
	[SerializeField]
	private ButtonBase m_btnNext;
	[SerializeField]
	private ButtonBase m_btnPrev;
	[SerializeField]
	private UILabel m_lbExplain;
	[SerializeField]
	private ButtonBase m_btnReturn;
	#endregion
	private GameObject m_goCenter;


	public enum STEP
	{
		INIT		= 0,
		IDLE		,
		CLOSE		,
		END			,

		START_GAME	,

		GOTO_TITLE_WAIT	,
		GOTO_TITLE	,
		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	//private string STR_PAGE_ROOT = "LevelIconPage";
	private string STR_PAGE_ROOT = "stage_";
	private List<GameObject> m_goIconRootList = new List<GameObject> ();

	public int m_iPageIndex = 0;

	private float m_fTimer;
	private int PAGE_NUM = 11;
	private int PAGE_DISP_ICON = 9;
	// Use this for initialization
	void Start () {

		m_eStep = STEP.INIT;
		m_eStepPre = STEP.MAX;
	
	}
	
	// Update is called once per frame
	void Update () {

		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}
		switch (m_eStep) {

		case STEP.INIT:

			if (bInit) {
				m_fTimer = 0.0f;
				FadeInOut.Instance.Close (0.0f);
				m_csCenterOnChild.onCenter = DragBanner;

				if (DataManager.Instance.m_iPlayLevel == 0) {
					DataManager.Instance.m_iPlayLevel = 1;
				}

				// 初期化処理
				if (false == PlayerPrefs.HasKey (DefineProject.GetKeyStage (1))) {
					DataManager.Instance.SetStageStatus (1, DefineProject.STAGE_STATUS.NO_PLAY);
				}
				m_goIconRootList.Clear ();
				ButtonRefresh (PAGE_NUM * PAGE_DISP_ICON);

				for (int page = 0; page < PAGE_NUM; page++) {
					GameObject objList = PrefabManager.Instance.MakeObject ("prefab/LevelIconPage", m_goScrollView);

					objList.name = string.Format ("{0}{1}", STR_PAGE_ROOT, page);

					for (int icon = 0; icon < PAGE_DISP_ICON; icon++) {
						GameObject objIcon = PrefabManager.Instance.MakeObject ("prefab/LevelIcon", objList);

						AddButtonBase (page * PAGE_DISP_ICON + icon, objIcon);

						int iLevel = page * PAGE_DISP_ICON + icon + 1;
						objIcon.GetComponent<LevelIcon> ().Initialize (iLevel);
					}
					m_goIconRootList.Add (objList);
				}
				m_goScrollView.GetComponent<UIGrid> ().enabled = true;

				// 順番的にここでやらないと反応できない
				SetBanner ((DataManager.Instance.m_iPlayLevel - 1) / PAGE_DISP_ICON);

				ButtonInit ();
				TriggerClearAll ();
			}

			m_fTimer += Time.deltaTime;
			if (0.2f < m_fTimer) {

				m_eStep = STEP.IDLE;
			}
			break;

		case STEP.IDLE:
			if (bInit) {
				FadeInOut.Instance.Open (0.25f);
				m_btnReturn.TriggerClear ();

			}
			if (ButtonPushed) {
				Debug.Log (Index);
				TriggerClearAll ();

				int iLevel = Index + 1;

				DefineProject.STAGE_STATUS eStageStatus = DataManager.Instance.GetStageStatus (iLevel);
				if (eStageStatus == DefineProject.STAGE_STATUS.NONE) {
				} else {
					// 補正
					DataManager.Instance.m_iPlayLevel = Index + 1;
					m_eStep = STEP.START_GAME;
				}
			} else if (m_btnNext.ButtonPushed) {
				m_btnNext.TriggerClear ();
				m_iPageIndex += 1;
				m_iPageIndex %= PAGE_NUM;
				SetBanner (m_iPageIndex);
			} else if (m_btnPrev.ButtonPushed) {
				m_btnPrev.TriggerClear ();
				m_iPageIndex -= 1;
				if (m_iPageIndex < 0) {
					m_iPageIndex += PAGE_NUM;
				}
				SetBanner (m_iPageIndex);
			} else if (m_btnReturn.ButtonPushed) {
				m_eStep = STEP.GOTO_TITLE_WAIT;
			} else {
			}
			break;

		case STEP.START_GAME:
			if (bInit) {
				FadeInOut.Instance.Close (0.25f);
			}
			if (FadeInOut.Instance.IsIdle ()) {
				Application.LoadLevelAsync ("game");
			}
			break;

		case STEP.GOTO_TITLE_WAIT:
			if (bInit) {
				FadeInOut.Instance.Close (0.25f);
			}
			if (FadeInOut.Instance.IsIdle ()) {
				m_eStep = STEP.GOTO_TITLE;
			}
			break;

		case STEP.GOTO_TITLE:
			if (bInit) {
				Application.LoadLevelAsync ("title");
			}
			break;

		default:
			break;
		}


	

	}



	#region scroll関連
	// バナーがドラッグされて切り替わった際に呼ばれるイベント
	public void DragBanner(GameObject _goBanner) {
		//Debug.Log (_goBanner.name);
		int iBannerNo = 0;
		SetBanner(_goBanner);
		return;
	}
	public void SetBanner( int _iBannerId ){
		foreach (GameObject obj in m_goIconRootList) {
			int banner_id = int.Parse (obj.name.Replace (STR_PAGE_ROOT, ""));

			//Debug.Log (banner_id);
			if (banner_id == _iBannerId) {
				m_iPageIndex = _iBannerId;
				SetExplainText (m_iPageIndex);
				SetBanner (obj);
				break;
			}
		}
		return;
	}
	public void SetBanner( GameObject _goBanner ){
		//Debug.Log (_goBanner.name);
		if (m_goCenter != _goBanner) {
			m_goCenter = _goBanner;
			m_csCenterOnChild.CenterOn (_goBanner.transform);

			int banner_id = int.Parse (_goBanner.name.Replace (STR_PAGE_ROOT, ""));
			m_iPageIndex = banner_id;
			SetExplainText (m_goCenter);
		} else {
		}
	}

	public void SetExplainText( int _iPageIndex ){
		string strText = "";
		switch (_iPageIndex) {

		case 0:
			strText = string.Format ("初級コース\n20までの数字が選ばれる簡単なコースです");
			break;
		case 1:
			strText = string.Format ("らくらくコース\n30までの数字が選ばれる慣れてきた方向け");
			break;
		case 2:
			strText = string.Format ("いーくあるチャレンジコース\n40までの数字が選ばれる。ここが最初の山場！");
			break;
		case 3:
			strText = string.Format ("中級コース\n50までの数字が選ばれる。どんどんクリアしよう！");
			break;
		case 4:
			strText = string.Format ("中上級コース\n60までの数字が選ばれるコース");
			break;
		case 5:
			strText = string.Format ("中上級+コース\n70までの数字が選ばれるコース\n組み合わせが複雑に！");
			break;
		case 6:
			strText = string.Format ("いーくあるマイスターコース\n80までの数字が選ばれるコース。玄人が最も好む数字帯だとか何とか");
			break;
		case 7:
			strText = string.Format ("上級コース\n90までの数字が選ばれるコース。組み合わせの数がだいぶ減ってます");
			break;
		case 8:
			strText = string.Format ("超上級コース\n99までの数字が選ばれるコース。二桁最後の砦99先輩登場");
			break;
		case 9:
			strText = string.Format ("いーくありすとコース\n99までの数字が選ばれるコース。組み合わせの数が少ないものになっています");
			break;
		case 10:
			strText = string.Format ("いーくあるマスター\n99までの数字が選ばれるコース。組み合わせの少なさが難しさじゃない、いーくあるはそれを教えてくれるはず");
			break;

		default:
			break;
		}
		m_lbExplain.text = strText;

	}

	public void SetExplainText( GameObject _goBanner ){
		int banner_id = int.Parse (_goBanner.name.Replace (STR_PAGE_ROOT, ""));
		SetExplainText (banner_id);
		return;
	}


	#endregion


}
