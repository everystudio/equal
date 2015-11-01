using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;

public class StoreItem : IStoreAssets {

	/// <summary>
	/// see parent.
	/// </summary>
	public int GetVersion() {
		return 0;
	}

	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualCurrency[] GetCurrencies() {
		return new VirtualCurrency[]{};
	}

	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualGood[] GetGoods() {
		return new VirtualGood[] {COMIC_TICKET};
	}

	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualCurrencyPack[] GetCurrencyPacks() {
		return new VirtualCurrencyPack[] {TENMUFF_PACK};
	}

	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualCategory[] GetCategories() {
		return new VirtualCategory[]{GENERAL_CATEGORY};
	}


	public const string MUFFIN_CURRENCY_ITEM_ID      = "currency_muffin";
	public const string TENMUFF_PACK_PRODUCT_ID      = "android.test.refunded";

	public const string COMIC_TICKET_ID      = "comicticket100";

	public static VirtualCurrency MUFFIN_CURRENCY = new VirtualCurrency(
		"Muffins",										// name
		"",												// description
		MUFFIN_CURRENCY_ITEM_ID							// item id
	);

	/*	* Virtual Currency Packs **/

	public static VirtualCurrencyPack TENMUFF_PACK = new VirtualCurrencyPack(
		"10 Muffins",                                   // name
		"Test refund of an item",                       // description
		"muffins_10",                                   // item id
		10,												// number of currencies in the pack
		MUFFIN_CURRENCY_ITEM_ID,                        // the currency associated with this pack
		new PurchaseWithMarket(TENMUFF_PACK_PRODUCT_ID, 0.99)
	);




	public static VirtualGood COMIC_TICKET = new SingleUseVG(
		"comic_ticket",                                       		// name
		"komikkutiketto", // description
		"comick_ticket",                                       		// item id
		new PurchaseWithVirtualItem( COMIC_TICKET_ID , 100)); // the way this virtual good is purchased

	public const string MUFFINCAKE_ITEM_ID   = "fruit_cake";
	/** Virtual Categories **/
	// The muffin rush theme doesn't support categories, so we just put everything under a general category.
	public static VirtualCategory GENERAL_CATEGORY = new VirtualCategory(
		"General", new List<string>(new string[] { MUFFINCAKE_ITEM_ID })
	);


}

















