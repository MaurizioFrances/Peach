using UnityEngine;
using System.Collections;

public class Tutorial_Script : MonoBehaviour {

	private Animation tute_anim;

	// Use this for initialization
	void Start () {
		tute_anim = GameObject.Find("Panel_Tutorial").GetComponent<Animation>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartAnim(){
		tute_anim.Play();
	}

	public void StopAnim(){
		tute_anim.Stop();
	}
}
