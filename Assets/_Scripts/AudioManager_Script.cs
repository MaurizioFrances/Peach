using UnityEngine;
using System.Collections;

public class AudioManager_Script : MonoBehaviour {
	public AudioSource musicIntro;
	public AudioSource musicMain;

	public AudioSource sfxAnswerCorrect;
	public AudioSource sfxAnswerWrong;

	private bool readyMusicSwitch = false;
	private bool backToIntroMusic = false;

	// Use this for initialization
	void Start () {
		StartMusicIntroLoop();	
	}
	
	// Update is called once per frame
	void Update () {

		if(readyMusicSwitch){
			if(!musicIntro.isPlaying){
				readyMusicSwitch = false;
				StartMusicMainLoop();
			}
		}

		else if(backToIntroMusic){
			MainMusicFadeOut();
		}
		
	
	}

	public void StartMusicIntroLoop(){
		musicIntro.Play();
	}

	public void StartMusicSwitch(){
		musicIntro.loop = false;
		readyMusicSwitch = true;
	}

	public void StartMusicSwitchBack(){
		readyMusicSwitch = false;
		backToIntroMusic = true;
	}

	public void StartMusicMainLoop(){
		musicMain.Play();
	}

	public void PlayAnswerCorrect(){
		sfxAnswerCorrect.Play();
	}

	public void PlayAnswerWrong(){
		//audio.PlayOneShot(sfxAnswerWrong);
		sfxAnswerWrong.Play();
	}

	public void MuteSound(){
		musicIntro.mute = true;
		musicMain.mute = true;
		sfxAnswerCorrect.mute = true;
		sfxAnswerWrong.mute = true;
	}

	public void UnmuteSound(){
		musicIntro.mute = false;
		musicMain.mute = false;
		sfxAnswerCorrect.mute = false;
		sfxAnswerWrong.mute = false;
	}

	void MainMusicFadeOut(){
		if(musicMain.volume > 0.1){
			musicMain.volume -= 0.9f * Time.deltaTime;
		}

		else{
			backToIntroMusic = false;
			musicIntro.loop = true;
			musicMain.Stop();
			musicMain.volume = 1;

			if(!musicIntro.isPlaying){
				musicIntro.Play();
			}
			Debug.Log("PLAT INTRO");

		}
	}
}
