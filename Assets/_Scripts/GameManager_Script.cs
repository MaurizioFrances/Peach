using UnityEngine;
using System.Collections;

public class GameManager_Script : MonoBehaviour {

	enum GameStates{Menu, Play, Gameover};
    
    private GameStates currentGameState = GameStates.Menu;

	private ShapeManager_Script SMS;
	private ColourManager_Script CMS;
	private AudioManager_Script AMS;
	private SaveLoad_Script SLS;

	private Tutorial_Script TTS;

	private int nextCorrectAnswer = 0; //The next correct shape number
	private int nextWrongShape = 0; //The next incorrect shape number (will be +1 or -1 the correct number)

	private int score = 0;
	private int highScore = 0;
	public UILabel scoreLbl;
	public UILabel highScoreLbl;

	public UISlider countdownUI;
	public UISlider countdownUI2;

	public float maxCountdown;
	private float currentCountdown;

	//REPLACE WITH STATE MANAGER
	private bool gameover = true;
	private bool shouldStart = true;

	private int amountOfShapes;

	private bool audio = true;
	private bool pause = false;

	private GameObject Panel_Menu;
	private UITweener PanelTween_Menu;

	private GameObject Panel_Gameover;
	private UITweener PanelTween_Gameover;

	private GameObject Panel_Gameplay;
	private UITweener PanelTween_Gameplay;

	private GameObject Panel_Pause;
	private UITweener PanelTween_Pause;

	private GameObject Panel_Darklayer;
	private UITweener PanelTween_Darklayer;

	private GameObject Panel_Debug;
	private UITweener PanelTween_Debug;
	private string debugTxt = "";
	private UITextList debugTextlist;

	private GameObject Panel_Tutorial;
	private UITweener PanelTween_Tutorial;

	// Use this for initialization
	void Start () {

		Panel_Menu = GameObject.Find("Panel_Menu");
		PanelTween_Menu = Panel_Menu.GetComponent<UITweener>();

		Panel_Gameover = GameObject.Find("Panel_Gameover");
		PanelTween_Gameover = Panel_Gameover.GetComponent<UITweener>();

		Panel_Gameplay = GameObject.Find("Panel_Gameplay");
		PanelTween_Gameplay = Panel_Gameplay.GetComponent<UITweener>();

		Panel_Pause = GameObject.Find("Panel_Pause");
		PanelTween_Pause = Panel_Pause.GetComponent<UITweener>();

		Panel_Darklayer = GameObject.Find("Panel_Darklayer");
		PanelTween_Darklayer = Panel_Darklayer.GetComponent<UITweener>();

		Panel_Debug = GameObject.Find("Panel_Debug");
		PanelTween_Debug = Panel_Debug.GetComponent<UITweener>();

		Panel_Tutorial = GameObject.Find("Panel_Tutorial");
		PanelTween_Tutorial = Panel_Tutorial.GetComponent<UITweener>();

		debugTextlist = GameObject.Find("Debug_Lbl").GetComponent<UITextList>();;

		SMS = this.GetComponent<ShapeManager_Script>();
		CMS = this.GetComponent<ColourManager_Script>();
		SLS = this.GetComponent<SaveLoad_Script>();
		TTS = this.GetComponent<Tutorial_Script>();

		AMS = GameObject.Find("AudioManager").GetComponent<AudioManager_Script>();

		amountOfShapes = SMS.GetAmountOfShapes();

	//	gameOverLbl.SetActive(false);
	//	restartBtn.SetActive(false);

		//SetCorrectAnswer();

		currentCountdown = maxCountdown;

		LoadHighScore();
		UpdateHighscore();
	}

	void Update(){
		//Shuffle shapes if on main menu
		if(currentGameState == GameStates.Menu){
			//How fast the shapes will shuffle.
			if( (Time.frameCount%12) == 0){
				SMS.ShuffleMidShape();
			}
		}

		if( (!gameover) && (!pause) ){
			CountdownTimer();
		}
	}

	public void StartGame(){
		if(shouldStart){
			SMS.ToggleAnswerShapes(true);

			currentGameState = GameStates.Play;
			SetCorrectAnswer();
			gameover = false;
		}
	}

	//Select the next correct shape.
	void SetCorrectAnswer(){
		//nextCorrectAnswer = Random.Range(0, 2);
		nextCorrectAnswer = Random.Range(0, amountOfShapes);
		SetWrongShape();
	}

	void SetWrongShape(){
		
		//if shape pairs
		//If is an even number
		if( ((nextCorrectAnswer+2)%2)==0){
			nextWrongShape = nextCorrectAnswer+1;
		}

		//An odd number
		else{
			nextWrongShape = nextCorrectAnswer-1;
		}
		
		SetShapes();



/*
		nextWrongShape = Random.Range(0, amountOfShapes);

		if(nextCorrectAnswer==nextWrongShape){
			SetWrongShape();
		}

		else{
			SetShapes();
		}
		*/
	}

	void SetShapes(){
		if(score>0){
			CMS.NewColour();

			int tempInvertRandom = Random.Range(0, 2);

			if(tempInvertRandom==0){
				SMS.SetColourInvert(true);
			}
			else{
				SMS.SetColourInvert(false);
			}

			SMS.SetShapeColours();
		}
		SMS.SetMidShapeNum(nextCorrectAnswer);

		//Pick if left or right is correct
		int tempRandom = Random.Range(0, 2);

		//If left
		if(tempRandom==0){
			SMS.SetLeftShapeNum(nextCorrectAnswer);
			SMS.SetRightShapeNum(nextWrongShape);

		}
		else{
			SMS.SetRightShapeNum(nextCorrectAnswer);
			SMS.SetLeftShapeNum(nextWrongShape);
		}
	

	}

	public void ChooseLeft(){
		if((!gameover) && (!pause)){
			if(SMS.IsLeftCorrect()){
				//MoveLeftFish();
				ChooseAnswer(true);
			}

			else{
				ChooseAnswer(false);
			}

		}
	}

	public void ChooseRight(){
		if((!gameover) && (!pause)){

			if(SMS.IsRightCorrect()){
				//MoveRightFish();
				ChooseAnswer(true);
			}

			else{
				ChooseAnswer(false);
			}


		}
	}

	void ChooseAnswer(bool wasCorrect){
		if(wasCorrect){

			//Do Correct Stats here
			RecordShapeStats(nextCorrectAnswer,(maxCountdown - currentCountdown),true,false);

			AMS.PlayAnswerCorrect();
			Debug.Log("CORRECT");
			score ++;
			SetCorrectAnswer();
			ResetTimer();
		}

		else{

			//Do fail stats here
			RecordShapeStats(nextCorrectAnswer,(maxCountdown - currentCountdown),false,false);
			
			GameOver();
		}

		UpdateScore();

		
	}

	void UpdateScore(){
		scoreLbl.text = ""+score;

	}

	void UpdateHighscore(){
		if(score>highScore){
			highScore=score;
			SaveHighScore();
		}

		highScoreLbl.text = "BEST SCORE: " + highScore;
		//highScoreLbl.text = "" + highScore;


	}

	void LoadHighScore(){
		highScore = SLS.LoadInt("highscore");
	}

	void SaveHighScore(){
		SLS.SaveInt("highscore", highScore);
	}

	void RecordShapeStats(int shapeNumber, float timeTaken, bool correct, bool timeUp){
		int tempAmount;
		float tempTime;
		int tempFinalScore;

		//If clicked an answer
		if(!timeUp){
			//If the answer was correct
			if(correct){
				tempAmount = SLS.LoadInt("shape" + shapeNumber + "_correct_amount");
				tempTime = SLS.LoadFloat("shape" + shapeNumber + "_correct_totaltime");

				tempAmount++;
				tempTime = tempTime + timeTaken;

				SLS.SaveInt("shape" + shapeNumber + "_correct_amount", tempAmount);
				SLS.SaveFloat("shape" + shapeNumber + "_correct_totaltime", tempTime);

			}

			//If the answer was wrong
			else{
				tempAmount = SLS.LoadInt("shape" + shapeNumber + "_incorrect_amount");
				tempTime = SLS.LoadFloat("shape" + shapeNumber + "_incorrect_totaltime");
				tempFinalScore = SLS.LoadInt("finalscore_" + score);

				tempAmount++;
				tempTime = tempTime + timeTaken;
				tempFinalScore++;

				SLS.SaveInt("shape" + shapeNumber + "_incorrect_amount", tempAmount);
				SLS.SaveFloat("shape" + shapeNumber + "_incorrect_totaltime", tempTime);
				SLS.SaveInt("finalscore_" + score, tempFinalScore);

			}
		}

		//Took too long to answer
		else{
				tempAmount = SLS.LoadInt("shape" + shapeNumber + "_timeup_amount");
				tempFinalScore = SLS.LoadInt("finalscore_" + score);

				tempAmount++;
				tempFinalScore++;

				SLS.SaveInt("shape" + shapeNumber + "_timeup_amount", tempAmount);
				SLS.SaveInt("finalscore_" + score, tempFinalScore);

		}
	}


	public void ShowDebug(){
		PrintShapeStats();
		PanelTween_Debug.PlayForward();
	}

	public void HideDebug(){
		PanelTween_Debug.PlayReverse();
	}

	void PrintShapeStats(){
		debugTxt = "";

		debugTxt += "HIGHSCORE:" + SLS.LoadInt("highscore") + "\n\n";

		/*for(int i=0; i<amountOfShapes; i++){
			debugTxt += "SHAPE" + i + " - Correct:" +
			SLS.LoadInt("shape" + i + "_correct_amount") +
			"     Total Time:" + 
			SLS.LoadFloat("shape" + i + "_correct_totaltime") +
			"     Incorrect:" + 
			SLS.LoadInt("shape" + i + "_incorrect_amount") +
			"     Total Time:" + 
			SLS.LoadFloat("shape" + i + "_incorrect_totaltime") +
			"     Out of Time:" +
			SLS.LoadInt("shape" + i + "_timeup_amount") +
			"\n";
		}*/

		for(int i=0; i<amountOfShapes; i++){
			debugTxt += "SHAPE";

			if(i<10){debugTxt += "0";}

			debugTxt += ""+ i + " - Correct:";

			if(SLS.LoadInt("shape" + i + "_correct_amount")<10){debugTxt += "0";}

			debugTxt += "" + SLS.LoadInt("shape" + i + "_correct_amount") +
			"     Total Time:" + 
			SLS.LoadFloat("shape" + i + "_correct_totaltime").ToString("F3") +
			"     Incorrect:";

			if(SLS.LoadInt("shape" + i + "_incorrect_amount")<10){debugTxt += "0";}

			debugTxt += "" + SLS.LoadInt("shape" + i + "_incorrect_amount") +
			"     Total Time:" + 
			SLS.LoadFloat("shape" + i + "_incorrect_totaltime").ToString("F3") +
			"     Out of Time:";

			if(SLS.LoadInt("shape" + i + "_timeup_amount")<10){debugTxt += "0";}

			debugTxt += "" + SLS.LoadInt("shape" + i + "_timeup_amount") +
			"\n";
		}

		debugTxt += "\n";

		for(int j=0; j <= SLS.LoadInt("highscore"); j++){
			debugTxt += "Score:" + j + "     Frequency:" + SLS.LoadInt("finalscore_" + j) + "\n";
		}



		debugTextlist.Add(debugTxt);

	}

	//Timer stuff//
	void CountdownTimer(){
		currentCountdown -= Time.deltaTime;

		if(currentCountdown<0){
			//Took too long shape stats
			RecordShapeStats(nextCorrectAnswer,(maxCountdown - currentCountdown),false,true);
			GameOver();
		}

		SetTimerUI();

	}

	void ResetTimer(){
		currentCountdown = maxCountdown;
		SetTimerUI();

	}

	void SetTimerUI(){

		float normalizedValue = Mathf.InverseLerp(0, maxCountdown, currentCountdown);
		countdownUI.value = normalizedValue;
		countdownUI2.value = normalizedValue;

	}



	//Timer stuff//

	void GameOver(){

		AMS.PlayAnswerWrong();

		UpdateHighscore();
		shouldStart = false;
		Debug.Log("WRONG NOOB");
		gameover = true;
	//	gameOverLbl.SetActive(true);
	//	restartBtn.SetActive(true);

		PanelTween_Darklayer.PlayForward();
		PanelTween_Gameover.PlayForward();
	}

	public void RestartGame(){
		shouldStart = true;
		PanelTween_Darklayer.PlayReverse();
		PanelTween_Gameover.PlayReverse();

	//	gameOverLbl.SetActive(false);
	//	restartBtn.SetActive(false);

		//SetCorrectAnswer();

		//gameover = false;
		//currentCountdown = maxCountdown;
		ResetTimer();

		score = 0;
		UpdateScore();

	}

	public void StartFromMenu(){
		PanelTween_Menu.PlayForward();
		PanelTween_Gameplay.PlayForward();
		AMS.StartMusicSwitch();
	}

	public void ShowTute(){
		PanelTween_Tutorial.PlayForward();
		TTS.StartAnim();
	}

	public void HideTute(){
		PanelTween_Tutorial.PlayReverse();
		TTS.StopAnim();
	}

	public void TogglePause(){
		pause = !pause;

		if(pause){
			PanelTween_Pause.PlayForward();
			PanelTween_Darklayer.PlayForward();
		}

		else{
			PanelTween_Pause.PlayReverse();
			PanelTween_Darklayer.PlayReverse();
		}


	}

	public void ToggleAudio(){
		audio = !audio;

		if(audio){
			AMS.UnmuteSound();

		}

		else{
			AMS.MuteSound();
		}
	}
}
