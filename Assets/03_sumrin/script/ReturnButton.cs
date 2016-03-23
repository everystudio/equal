using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ReturnButton : ButtonBase {

	public string m_strNextScene;

	// Use this for initialization
	void Start () {
		TriggerClear ();
	}
	
	// Update is called once per frame
	void Update () {
		if (ButtonPushed) {
			if (m_strNextScene.Equals ("") == false) {
				TriggerClear ();
				SceneManager.LoadScene (m_strNextScene);
			}
		}
	}
}
