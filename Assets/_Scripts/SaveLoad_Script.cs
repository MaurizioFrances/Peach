using UnityEngine;
using System.Collections;

public class SaveLoad_Script : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public void SaveInt(string name, int value){
		PlayerPrefs.SetInt(name, value);
	}

	public int LoadInt(string name){
		return PlayerPrefs.GetInt(name);
	}

	public void SaveFloat(string name, float value){
		PlayerPrefs.SetFloat(name, value);
	}

	public float LoadFloat(string name){
		return PlayerPrefs.GetFloat(name);
	}

	public void SaveString(string name, string value){
		PlayerPrefs.SetString(name, value);
	}

	public string LoadString(string name){
		return PlayerPrefs.GetString(name);
	}

}
