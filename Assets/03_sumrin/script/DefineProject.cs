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
