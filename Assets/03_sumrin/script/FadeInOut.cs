using UnityEngine;
using System.Collections;

public class FadeInOut : MonoBehaviourEx {

	public enum STEP
	{
		NONE		= 0,
		CLOSE		,
		CLEAR		,

		IDLE		,
		MOVE		,
		END			,
		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	[SerializeField]
	private UISprite m_sprCurtain;

	private static FadeInOut instance = null;

	public static FadeInOut Instance {
		get {
			if (instance == null) {
				GameObject obj = GameObject.Find ("FadeInOut");
				if (obj == null) {
					obj = new GameObject("FadeInOut");
					//Debug.LogError ("Not Exist AtlasManager!!");
				}
				instance = obj.GetComponent<FadeInOut> ();
				if (instance == null) {
					//Debug.LogError ("Not Exist AtlasManager Script!!");
					instance = obj.AddComponent<FadeInOut>() as FadeInOut;
				}
				instance.initialize ();
			}
			return instance;
		}
	}

	private void initialize(){

		return;
	}

	public void Alpha( float _fTime , float _fAlpha ){
		TweenAlpha ta = TweenAlphaAll (gameObject, _fTime, _fAlpha);
		m_bEndTween = false;
		EventDelegate.Set (ta.onFinished, EndTween);
	}
	public void Close( float _fTime ){
		Alpha (_fTime, 1.0f);
	}
	public void Open( float _fTime ){
		Alpha (_fTime, 0.0f);
	}

	public bool IsIdle(){
		return m_bEndTween;
	}




	// Update is called once per frame
	void Update () {
	
	}
}
