using UnityEngine;
using System.Collections;

public class CsvLevel : CsvBase<CsvLevelData> 
{
	//private static readonly string FilePath = "CSV/master_animal";
	/*
	public void Load( int _iLevel ) {
		string filename = string.Format ("level_{0:D3}", _iLevel);
		Load( "csv/" + filename );
	}
	*/
	public void Load(){
		Load ("csv/level");
	}
}


public class CsvLevelData : MasterBase
{
	public int level { get; private set; }
	public int number1 { get; private set; }
	public int number2 { get; private set; }
	public int number3 { get; private set; }
	public int number4 { get; private set; }
	public int number5 { get; private set; }
	public int number6 { get; private set; }
	public int number7 { get; private set; }
	public int number8 { get; private set; }
	public int number9 { get; private set; }

	public enum TYPE{
		NONE		= 0,
		START		= 1,
		GOAL		,
		STAY		,
		NORMAL		= 100,
		PLAYER		= 1000,
		PLAYER_GOAL	,
		PLAYER_FALL	,
		GOAL_GOAL	,
		MAX			,
	}

}







