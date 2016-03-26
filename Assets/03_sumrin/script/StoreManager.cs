using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;

public class StoreManager : MonoBehaviour {

	static StoreManager instance = null;
	public static StoreManager Instance
	{
		get
		{
			if(instance == null)
			{
				GameObject obj = GameObject.Find("StoreManager");
				if( obj == null )
				{
					obj = new GameObject("StoreManager");
				}

				instance = obj.GetComponent<StoreManager>();

				if(instance == null)
				{
					instance = obj.AddComponent<StoreManager>() as StoreManager;
				}
				instance.initialize ();
			}
			return instance;
		}
	}

	private void initialize(){
		DontDestroyOnLoad(gameObject);

		#if UNITY_ANDROID
		GoogleIAB.enableLogging (true);
		GoogleIAB.init ("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArGLKSb92Imt43S40ArCXfTmQ31c+pFQTM0Dza3j/Tn4cqjwccjQ/jej68GgVyGXGC2gT/EtbcVVA+bHugXmyv73lGBgmQlzBL41WYTKolO8Z6pVWTeHBtsT7RcHKukoKiONZ7NiQ9P5t6CCPBB2sXQOp1y3ryVbv01xXlM+hB6HkkKxrT6lIjTbtiVXCHAJvqPexPbqVIfGjBaXH/oHKxEBxYDaa6PTUsU3OP3MTx63ycTEnEMsQlA1W6ZuTFIa5Jd3cVlfQI7uovEzAbIlUfwcnxVOUWASiYe81eQiD1BMl+JeCRhfd5e8D4n0LOA12rHm1F3fC9ZoIEjpNB+BRhwIDAQAB");
		#endif

	}

	public void DummyCall(){
		Debug.Log ("Dummy!");
	}
	public void buy(string _strCode ){
		#if UNITY_ANDROID
		GoogleIAB.purchaseProduct (_strCode);
		#endif
	}
	/*
	public static Action<string,int> purchaseFailedEvent(string json){
		var dict = Json.decode<Dictionary<string,object>>( json );
		purchaseFailedEvent( dict["result"].ToString(), int.Parse( dict["response"].ToString() ) );
		Debug.Log ("StoreManager.purchaseFailedEvent");
		return;
	}
	*/

	// Update is called once per frame
	void Update () {
	
	}
}
