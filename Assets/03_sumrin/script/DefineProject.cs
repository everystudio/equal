using UnityEngine;
using System.Collections;

public class DefineProject : Define {

	public enum DOT_COLOR {
		WHITE		= 0,
		RED			,
		BLUE		,
		GREEN		,

		MAX			,
	}

	#if UNITY_ANDROID
	public static readonly string NEND_AD_INTER_API_KEY = "9c88701a441d43893c5eac2ba485b56804c0f61c";
	public static readonly string NEND_AD_INTER_SPOT_ID = "443851";
	public static readonly string INSTALL_URL = "https://play.google.com/store/apps/details?id=jp.everystudio.equal&hl=ja";
	#elif UNITY_IPHONE
	public static readonly string NEND_AD_INTER_API_KEY = "321f03d70fb7e3455da1ac687be72d49354b2972";
	public static readonly string NEND_AD_INTER_SPOT_ID = "447212";
	public static readonly string INSTALL_URL = "https://itunes.apple.com/us/app/i-kuaru/id1041959470?mt=8";
	#else
	public static readonly string NEND_AD_INTER_API_KEY = "321f03d70fb7e3455da1ac687be72d49354b2972";
	public static readonly string NEND_AD_INTER_SPOT_ID = "447212";
	public static readonly string INSTALL_URL = "https://itunes.apple.com/us/app/i-kuaru/id1041959470?mt=8";
	#endif

	public static readonly string KEYNAME_STAGE = "stage";
	public static string GetKeyStage( int _iStage ){
		return string.Format ("{0}_{1}", KEYNAME_STAGE, _iStage);
	}
	public static readonly string TWEET_TAG = "#い〜くある";

	public static readonly string TWEET_MESSAGE_CLEAR = string.Format( "シンプル＆簡単パズルゲーム！数を揃えるだけなのになぜかはまっちゃう！？ {0} {1}" , TWEET_TAG , INSTALL_URL ); 

	public enum STAGE_STATUS
	{
		NONE		= 0,
		NO_PLAY		,
		CHALLENGING	,
		CLEARED		,
		MAX			,
	}

	public static float COLOR_CHANGE_TIME = 0.1f;
	public static float NUMBER_CHANGE_TIME = 10.0f;

	public static Color[] ColorTable = new Color[(int)DOT_COLOR.MAX] {
		Color.white,
		new Color(1.0f,0.78f,0.78f),
		new Color(0.78f,0.78f,1.0f),
		new Color(0.78f,1.0f,0.78f),
	};
	public static Color GetColor( DOT_COLOR _eDotColor ){
		return ColorTable [(int)_eDotColor];
	}

}
