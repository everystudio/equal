using UnityEngine;
using System.Collections;
using Soomla;
using Soomla.Store;

public class StoreManager : MonoBehaviour {

	public bool m_bInitialized = false;

	public const string COMIC_TICKET_10 = "comickticket10";

	protected static StoreManager instance = null;
	public static StoreManager Instance {
		get {
			if (instance == null) {
				GameObject obj = GameObject.Find ("StoreManager");
				if (obj == null) {
					obj = new GameObject("StoreManager");
					//Debug.LogError ("Not Exist AtlasManager!!");
				}
				instance = obj.GetComponent<StoreManager> ();
				if (instance == null) {
					//Debug.LogError ("Not Exist AtlasManager Script!!");
					instance = obj.AddComponent<StoreManager>() as StoreManager;
				}
			}
			return instance;
		}
	}
	// Use this for initialization
	void Start () {

	}

	public void Initialize(){
		if (m_bInitialized) {
			return;
		}

		AndroidInAppPurchaseManager.ActionProductPurchased += OnProductPurchased;  
		AndroidInAppPurchaseManager.ActionProductConsumed  += OnProductConsumed;
		AndroidInAppPurchaseManager.ActionBillingSetupFinished += OnBillingConnected;
		AndroidInAppPurchaseManager.instance.loadStore();

		m_bInitialized = true;
		Debug.Log (" StoreManager Initialize");
	}

	private static void OnBillingConnected(BillingResult result) {
		AndroidInAppPurchaseManager.ActionBillingSetupFinished -= OnBillingConnected;

		if(result.isSuccess) {
			//Store connection is Successful. Next we loading product and customer purchasing details
			AndroidInAppPurchaseManager.instance.retrieveProducDetails();
			AndroidInAppPurchaseManager.ActionRetrieveProducsFinished += OnRetriveProductsFinised;
		} 

		AndroidMessage.Create("Connection Responce", result.response.ToString() + " " + result.message);
		Debug.Log ("Connection Responce: " + result.response.ToString() + " " + result.message);
	}

	public bool _isInited;

	private static void OnRetriveProductsFinised(BillingResult result) {
		AndroidInAppPurchaseManager.ActionRetrieveProducsFinished -= OnRetriveProductsFinised;

		if(result.isSuccess) {
			instance._isInited = true;
			AndroidMessage.Create("Success", "Billing init complete inventory contains: " + AndroidInAppPurchaseManager.instance.inventory.purchases.Count + " products");
		} else {
			AndroidMessage.Create("Connection Responce", result.response.ToString() + " " + result.message);
		}
		Debug.Log ("Connection Responce: " + result.response.ToString() + " " + result.message);
	}

	private static void OnProcessingPurchasedProduct(GooglePurchaseTemplate purchase) {
		//some stuff for processing product purchse. Add coins, unlock track, etc
		switch (purchase.SKU) {
		case COMIC_TICKET_10:
			Debug.LogError ("success:comickticket10 GET");
			break;
		default:
			break;
		}
	}

	private static void OnProcessingConsumeProduct(GooglePurchaseTemplate purchase) {
		//some stuff for processing product consume. Reduse tip anount, reduse gold token, etc
		switch(purchase.SKU) {
		case COMIC_TICKET_10:
			AndroidInAppPurchaseManager.instance.consume(COMIC_TICKET_10);
			break;
		default:
			Debug.LogError (string.Format ("unknown:sku{0}", purchase.SKU));
			break;
		}
	}


	private static void OnProductPurchased(BillingResult result) {
		if(result.isSuccess) {
			AndroidMessage.Create ("Product Purchased", result.purchase.SKU+ "\n Full Response: " + result.purchase.originalJson);
			OnProcessingPurchasedProduct (result.purchase);
		} else {
			AndroidMessage.Create("Product Purchase Failed", result.response.ToString() + " " + result.message);
		}

		Debug.Log ("Purchased Responce: " + result.response.ToString() + " " + result.message);
		Debug.Log (result.purchase.originalJson);
	}

	private static void OnProductConsumed(BillingResult result) {

		if(result.isSuccess) {
			AndroidMessage.Create ("Product Consumed", result.purchase.SKU + "\n Full Response: " + result.purchase.originalJson);
			OnProcessingConsumeProduct (result.purchase);
		} else {
			AndroidMessage.Create("Product Cousume Failed", result.response.ToString() + " " + result.message);
		}

		Debug.Log ("Cousume Responce: " + result.response.ToString() + " " + result.message);
	}



	// Update is called once per frame
	void Update () {
	
	}
}
