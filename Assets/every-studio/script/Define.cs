using UnityEngine;
using System.Collections;

/// <summary>
/// 基本的には直接使わないようにする
/// </summary>

public abstract class Define : MonoBehaviour {

	public enum ENVIROMENT
	{
		PRODUCTION			= 0,
		LOCAL				,
		STREAMING_ASSETS	,
		MAX					,
	}

	#if UNITY_IPHONE
	public const string ASSET_BUNDLE_PREFIX             = "iphone";
	public const string ASSET_BUNDLES_ROOT              = "AssetBundles/iOS";
	#elif UNITY_ANDROID
	public const string ASSET_BUNDLE_PREFIX             = "android";
	public const string ASSET_BUNDLES_ROOT              = "AssetBundles/Android";
	#endif

	#if UNITY_IPHONE
	public const string S3_SERVER_HEADER = "http://ad.xnosserver.com/apps/doubutsumikke_data/iOS/assets/assetbundleresource";
	#elif UNITY_ANDROID
	public const string S3_SERVER_HEADER = "https://s3-ap-northeast-1.amazonaws.com/every-studio/app/turnright/AssetBundles/Android/assets/assetbundleresource";
	#endif

	//sound/bgm_room_game_day.unity3d

	#if UNITY_IPHONE
	public static string strFooterAdUrl = "http://ad.xnosserver.com/apps/osakanamikke_ios/ad.html";
	#elif UNITY_ANDROID
	public static string strFooterAdUrl = "http://ad.xnosserver.com/apps/osakanamikke_android/ad.html";
	#endif




}






























