using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class CheckTransform : EditorWindow {


	static List<int> GetChoiceSumList( int _iChoiceNum , List<int> _list , int _iSum ){
		List<int > ret_list = new List<int> ();
		for (int i = 0; i < _list.Count - _iChoiceNum+1 ; i++) {
			for (int j = i + 1; j < _list.Count - _iChoiceNum+2; j++) {
				for (int k = j + 1; k < _list.Count; k++) {
					if (i != j && j != k && k != i) {
						int iSum = _list [i] + _list [j] + _list [k];

						if (_iSum == iSum) {
							//Debug.Log (string.Format ("sum({0} ({1},{2},{3})", iSum, _list [i], _list [j], _list [k]));
						}

						ret_list.Add (iSum);
					}
				}
			}
		}
		return ret_list;
	}

	[MenuItem("Tools/ReferenceAtlas")]
	static void Open(){

		GameObject obj = Selection.activeGameObject;
		if (obj == null) {
			Debug.Log ("Please Select Object");
			return;
		}

		int iRange = 100;
		Debug.Log ("start");
		// とりあえず１００個作る
		for (int i = 0; i < 1000; i++) {

			List< int> result_list = new List<int> ();
			result_list.Clear ();

			int[] table = new int[iRange];
			for (int table_index = 0; table_index < iRange; table_index++) {

				int iProbParam = 100;

				if (table_index == 0) {
					iProbParam = 0;
				} else if (table_index < 10) {
					iProbParam /= 3;
				} else {
				}
				table [table_index] = iProbParam;
			}

			int iNumberTemp = 0;
			int iNumberResult = 0;

			for( int result_num = 0 ; result_num < 9 ; result_num++ ){
				iNumberTemp = UtilRand.GetIndex (table);
				iNumberResult += iNumberTemp;
				table [iNumberTemp] = 0;
				result_list.Add (iNumberTemp);
			}

			int iHitCount = 0;

			for (int x = 0; x < result_list.Count - 2 ; x++) {
				for (int y = x + 1; y < result_list.Count - 1; y++) {
					for (int z = y + 1; z < result_list.Count ; z++) {
						if (x != y && y != z && z != x) {
							int iSum = result_list [x] + result_list [y] + result_list [z];

							List<int> check_list = new List<int> ();

							for (int check = 0; check < result_list.Count; check++) {
								if (check == x) {
								} else if (check == y) {
								} else if (check == z) {
								} else {
									check_list.Add (result_list [check]);
								}
							}

							bool temphit = false;
							List<int> sum_list = GetChoiceSumList (3, check_list , iSum );
							foreach (int sum_result in sum_list) {
								if (iSum == sum_result) {
									iHitCount += 1;
									temphit = true;
								}
							}
							if (temphit == true) {
								//Debug.Log (string.Format ("sum={0} ({1},{2},{3})", iSum, result_list [x], result_list [y], result_list [z]));
							}


							//ret_list.Add (iSum);
						}
					}
				}
			}

			if ( iHitCount == 2 ) 
			{
				Debug.Log (string.Format ("HitCount:{0} ({1},{2},{3},{4},{5},{6},{7},{8},{9})",
					iHitCount,
					result_list [0],
					result_list [1],
					result_list [2],
					result_list [3],
					result_list [4],
					result_list [5],
					result_list [6],
					result_list [7],
					result_list [8]));
				Textreader.Append( "/../text.csv" , string.Format ("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",
					iHitCount,
					result_list [0],
					result_list [1],
					result_list [2],
					result_list [3],
					result_list [4],
					result_list [5],
					result_list [6],
					result_list [7],
					result_list [8]));
			}




		}
		Debug.Log ("end");

		/*
		foreach (Transform child in obj.transform) {
			Debug.Log (child.gameObject.name);
			Textreader.write (string.Format ("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}" ,
				child.name,
				child.localPosition.x,
				child.localPosition.y,
				child.localPosition.z,
				child.localEulerAngles.x,
				child.localEulerAngles.y,
				child.localEulerAngles.z,
				child.localScale.x,
				child.localScale.y,
				child.localScale.z
			));
		}
		*/
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
