using UnityEngine;
using System.Collections;

public class GameManager_Script : MonoBehaviour {

	enum GameStates{Menu, Info, Instruction, Play, Pause, Gameover};
    
    private GameStates currentGameState = GameStates.Menu;
    private bool ShowInstruction = true;

	private ShapeManager_Script SMS;
	private ColourManager_Script CMS;
	private AudioManager_Script AMS;
	private SaveLoad_Script SLS;
	private Twitter_Script TWS;
	private Analytics_Script GAS;
	private AdMob_Script ADS;

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

	private bool audio = true;

	private int amountOfShapes;

	private GameObject Panel_Menu;
	private UITweener PanelTween_Menu;

	private GameObject Panel_Gameover;
	private UITweener PanelTween_Gameover;

	private GameObject Panel_Gameplay;
	private UITweener PanelTween_Gameplay;
	private UITweener PanelTween_Gameplay_NonGameover;

	private GameObject Panel_Pause;
	private UITweener PanelTween_Pause;

	private GameObject Panel_Darklayer;
	private UITweener PanelTween_Darklayer;

	private GameObject Panel_Info;
	private UITweener PanelTween_Info;

	private UITweener Instruction_Text;
	private UITweener Instruction_Shape_L;
	private UITweener Instruction_Shape_R;

	private TweenPosition shapes_TweenPos;
	private TweenScale shapes_TweenScale;
	private TweenPosition score_TweenPos;
	private TweenPosition gameoverText_TweenPos;
	private TweenPosition twitterBtn_TweenPos;
	private UITweener chosenShapeText_L;
	private UITweener chosenShapeText_R;

	private TweenAlpha score_TweenAlpha;
	private TweenAlpha highscore_TweenAlpha;


	public int[] difficultyTier; //How much score is needed to get to this tier.
	public int[] difficultyRange; //How many puzzles are allowed in this tier.

	private int currentDifficulty = 0;

	// Use this for initialization
	void Start () {

		Panel_Menu = GameObject.Find("Panel_Menu");
		PanelTween_Menu = Panel_Menu.GetComponent<UITweener>();

		Panel_Gameover = GameObject.Find("Panel_Gameover");
		PanelTween_Gameover = Panel_Gameover.GetComponent<UITweener>();

		Panel_Gameplay = GameObject.Find("Panel_Gameplay");
		PanelTween_Gameplay = Panel_Gameplay.GetComponent<UITweener>();
		PanelTween_Gameplay_NonGameover = GameObject.Find("NonGameover_Container").GetComponent<UITweener>();

		Panel_Pause = GameObject.Find("Panel_Pause");
		PanelTween_Pause = Panel_Pause.GetComponent<UITweener>();

		Panel_Darklayer = GameObject.Find("Panel_Darklayer");
		PanelTween_Darklayer = Panel_Darklayer.GetComponent<UITweener>();

		Panel_Info = GameObject.Find("Panel_Info");
		PanelTween_Info = Panel_Info.GetComponent<UITweener>();

		Instruction_Text = GameObject.Find("Instruction_Text").GetComponent<UITweener>();
		Instruction_Shape_L = GameObject.Find("Shape_Left").GetComponent<UITweener>();
		Instruction_Shape_R = GameObject.Find("Shape_Right").GetComponent<UITweener>();

		//shapes_TweenPos = GameObject.Find("Panel_Shapes").GetComponent<TweenPosition>();
		//shapes_TweenScale = GameObject.Find("Panel_Shapes").GetComponent<TweenScale>();

		shapes_TweenPos = GameObject.Find("Shape_Move_Container").GetComponent<TweenPosition>();
		shapes_TweenScale = GameObject.Find("Shape_Container").GetComponent<TweenScale>();

		score_TweenPos = GameObject.Find("Scores_Container").GetComponent<TweenPosition>();
		gameoverText_TweenPos = GameObject.Find("GameoverSprite").GetComponent<TweenPosition>();
		twitterBtn_TweenPos = GameObject.Find("Control_TwitterButton").GetComponent<TweenPosition>();
		chosenShapeText_L = GameObject.Find("ChosenShape_Text_L").GetComponent<UITweener>();
		chosenShapeText_R = GameObject.Find("ChosenShape_Text_R").GetComponent<UITweener>();

		score_TweenAlpha = scoreLbl.GetComponent<TweenAlpha>();
		highscore_TweenAlpha = highScoreLbl.GetComponent<TweenAlpha>();

		SMS = this.GetComponent<ShapeManager_Script>();
		CMS = this.GetComponent<ColourManager_Script>();
		SLS = this.GetComponent<SaveLoad_Script>();
		TWS = this.GetComponent<Twitter_Script>();
		GAS = this.GetComponent<Analytics_Script>();

		ADS = Camera.main.GetComponent<AdMob_Script>();

		AMS = GameObject.Find("AudioManager").GetComponent<AudioManager_Script>();

		amountOfShapes = SMS.GetAmountOfShapes();

		currentCountdown = maxCountdown;

		LoadHighScore();
		UpdateHighscore();

		if(currentGameState == GameStates.Menu){
			GAS.SetScreen("Menu");
		}
	}

	void Update(){
		//Shuffle shapes if on main menu
		if(currentGameState == GameStates.Menu){
			//How fast the shapes will shuffle.
			if( (Time.frameCount%12) == 0){
				SMS.ShuffleMidShape();
			}
		}

		if(currentGameState == GameStates.Play){
			Debug.Log("WHY IS THIS STILL HERE:" + currentGameState);
			CountdownTimer();
		}

		//DO NATIVE BACK BUTTON STUFF HERE
		#if UNITY_ANDROID || UNITY_WP8

			 if (Input.GetKeyUp(KeyCode.Escape)){

			 	Debug.Log("DEBUG: ESC");
			 	//If in the game menu, exit the game
			 	if(currentGameState == GameStates.Menu){
			 		Debug.Log("DEBUG: QUIT");
			 		Application.Quit();
			 	}

			 	else if(currentGameState == GameStates.Info){
			 		Debug.Log("DEBUG: INFO");
			 		ToggleInfo();
			 	}

			 	else if(currentGameState == GameStates.Pause){
			 		Debug.Log("DEBUG: PAUSE");
			 		TogglePause();
			 	}

			 	//If playing the game (or gameover or intructions)
			 	else{
			 		Debug.Log("DEBUG: BACK");
			 		BackToMainMenu();
			 	}

            }

		#endif
	}

	public void StartGame(){
			Debug.Log("STARTGAME");
			SMS.ToggleAnswerShapes(true);

			if(ShowInstruction){
				GAS.SetScreen("Instruction");

				currentGameState = GameStates.Instruction;

				nextCorrectAnswer = 50;
				SetWrongShape();

				Instruction_Text.PlayForward();
				Instruction_Shape_L.Play();
				Instruction_Shape_R.Play();
			}

			else{
				GAS.SetScreen("Play");

				currentGameState = GameStates.Play;
				SetCorrectAnswer();
			}
			
	}

	void CheckDifficulty(){
		for(int i = currentDifficulty; i<difficultyTier.Length-1;i++){
			if(score>difficultyTier[i+1]){
				currentDifficulty++;
			}
		}

		Debug.Log("CurrentDiff: " + currentDifficulty);
	}

	//Select the next correct shape.
	void SetCorrectAnswer(){
		CheckDifficulty();

		int randomDifficulty = Random.Range(0, difficultyRange[currentDifficulty]);
		nextCorrectAnswer = SMS.GetShapeOfDifficulty(randomDifficulty);
		Debug.Log("ShapeDiff: " + randomDifficulty + "    Shape: " + nextCorrectAnswer);
		//nextCorrectAnswer = SMS.GetShapeOfDifficulty(Random.Range(0, difficultyRange[currentDifficulty]));

		//nextCorrectAnswer = Random.Range(0, amountOfShapes);
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

		if(currentGameState == GameStates.Play || currentGameState == GameStates.Instruction){
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

		if(currentGameState == GameStates.Play || currentGameState == GameStates.Instruction){
			if(SMS.IsRightCorrect()){
				//MoveLeftFish();
				ChooseAnswer(true);
			}

			else{
				ChooseAnswer(false);
			}

		}
	}

	void ChooseAnswer(bool wasCorrect){
		if(wasCorrect){

			if(currentGameState == GameStates.Instruction){

				GAS.SetScreen("Play");

				ShowInstruction = false;
				currentGameState = GameStates.Play;

				Instruction_Text.PlayReverse();

				Instruction_Shape_L.ResetToBeginning();
				Instruction_Shape_R.ResetToBeginning();

				Instruction_Shape_L.enabled = false;
				Instruction_Shape_R.enabled = false;
			}

			//Do Correct Stats here
			//RecordShapeStats(nextCorrectAnswer,(maxCountdown - currentCountdown),true,false);
			GAS.SetShapeStats(nextCorrectAnswer,true,false);

			AMS.PlayAnswerCorrect();
			Debug.Log("CORRECT");
			score ++;
			SetCorrectAnswer();
			ResetTimer();
		}

		else{

			//Do fail stats here
			//RecordShapeStats(nextCorrectAnswer,(maxCountdown - currentCountdown),false,false);
			GAS.SetShapeStats(nextCorrectAnswer,false,false);
			
			GameOver();

			ShowGameoverShapeText();
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


	//Timer stuff//
	void CountdownTimer(){
		Debug.Log("COUNTDOWN TIMER");
		currentCountdown -= Time.deltaTime;

		if(currentCountdown<0){
			//Took too long shape stats
			//RecordShapeStats(nextCorrectAnswer,(maxCountdown - currentCountdown),false,true);
			GAS.SetShapeStats(nextCorrectAnswer,false,true);

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

		GAS.SetScreen("Gameover");
		GAS.SetScore(score);

		if(ShowInstruction){
			Instruction_Text.PlayReverse();
			Instruction_Shape_L.enabled = false;
			Instruction_Shape_R.enabled = false;
		}

		currentGameState = GameStates.Gameover;

		AMS.PlayAnswerWrong();

		UpdateHighscore();

		PanelTween_Gameplay_NonGameover.PlayForward();

		PanelTween_Darklayer.PlayForward();
		PanelTween_Gameover.PlayForward();

		shapes_TweenPos.PlayForward();
		shapes_TweenScale.PlayForward();
		score_TweenPos.PlayForward();
		gameoverText_TweenPos.PlayForward();
		twitterBtn_TweenPos.PlayForward();

		score_TweenAlpha.PlayForward();
		highscore_TweenAlpha.PlayForward();

		ADS.ShowAd();
	}

	void ShowGameoverShapeText(){
		//Player chose the R shape
		if(SMS.IsLeftCorrect()){
			chosenShapeText_R.PlayForward();
		}

		//Player chose L shape
		else{
			chosenShapeText_L.PlayForward();
		}

	}

	void ClearGameoverShapeText(){
		chosenShapeText_L.PlayReverse();
		chosenShapeText_R.PlayReverse();
	}

	public void RestartGame(){

		ADS.HideAd();

		PanelTween_Gameplay_NonGameover.PlayReverse();

		shapes_TweenPos.PlayReverse();
		shapes_TweenScale.PlayReverse();
		score_TweenPos.PlayReverse();
		gameoverText_TweenPos.PlayReverse();
		twitterBtn_TweenPos.PlayReverse();

		score_TweenAlpha.PlayReverse();
		highscore_TweenAlpha.PlayReverse();

		PanelTween_Darklayer.PlayReverse();
		PanelTween_Gameover.PlayReverse();

		ClearGameoverShapeText();

		ResetTimer();

		currentDifficulty = 0;
		score = 0;
		UpdateScore();

		StartGame();

	}

	public void StartFromMenu(){


		PanelTween_Menu.PlayForward();
		PanelTween_Gameplay.PlayForward();
		AMS.StartMusicSwitch();

		ResetTimer();

		currentDifficulty = 0;
		score = 0;
		UpdateScore();

		Invoke("StartGame",PanelTween_Menu.duration);
	}

	public void TogglePause(){
		/*
		pause = !pause;

		if(pause){
			PanelTween_Pause.PlayForward();
			PanelTween_Darklayer.PlayForward();
		}

		else{
			PanelTween_Pause.PlayReverse();
			PanelTween_Darklayer.PlayReverse();
		}
		*/

		if(currentGameState == GameStates.Play || currentGameState == GameStates.Instruction){

			GAS.SetScreen("Pause");
			currentGameState = GameStates.Pause;

			PanelTween_Pause.PlayForward();
			PanelTween_Darklayer.PlayForward();
		}

		else if(currentGameState == GameStates.Pause){

			if(ShowInstruction){
				GAS.SetScreen("Instruction");
				currentGameState = GameStates.Instruction;
			}
			else{
				GAS.SetScreen("Play");
				currentGameState = GameStates.Play;
			}

			

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

	public void BackToMainMenu(){
		GAS.SetScreen("Menu");

		//If going Home from Pause
		if(currentGameState == GameStates.Pause){
			PanelTween_Pause.PlayReverse();
		}

		//If going Home from Gameover
		else if(currentGameState == GameStates.Gameover){
			ADS.HideAd();

			ClearGameoverShapeText();

			shapes_TweenPos.PlayReverse();
			shapes_TweenScale.PlayReverse();
			score_TweenPos.PlayReverse();
			gameoverText_TweenPos.PlayReverse();
			twitterBtn_TweenPos.PlayReverse();

			score_TweenAlpha.PlayReverse();
			highscore_TweenAlpha.PlayReverse();
			PanelTween_Gameover.PlayReverse();

			EventDelegate.Add (PanelTween_Gameover.onFinished, PanelTween_Gameplay_NonGameover.PlayReverse, true);
		}

		currentGameState = GameStates.Menu;

		PanelTween_Menu.PlayReverse();
		PanelTween_Gameplay.PlayReverse();		
		PanelTween_Darklayer.PlayReverse();

		SMS.ToggleAnswerShapes(false);
		AMS.StartMusicSwitchBack();

	}

	public void ToggleInfo(){
		if(currentGameState == GameStates.Menu){

			GAS.SetScreen("Info");
			currentGameState = GameStates.Info;

			PanelTween_Info.PlayForward();
			PanelTween_Info.PlayForward();
		}

		else if(currentGameState == GameStates.Info){
			GAS.SetScreen("Menu");
			currentGameState = GameStates.Menu;

			PanelTween_Info.PlayReverse();
			PanelTween_Info.PlayReverse();
		}

	}

	public void ShareToTwitter(){
		GAS.SetShare(score);
		TWS.ShareScore(score);
	}

	public void FieroWebsiteLink(){
		Application.OpenURL("http://www.fiero.co.nz");
	}
}
