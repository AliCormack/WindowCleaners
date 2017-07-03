using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace WindowCleaner
{

	public class GameController : MonoBehaviour 
	{

		public enum GameState{
			Starting,
			Playing,
			Ended	
		}
		private GameState currentState;

		static Color player1Color = Color.blue;
		static Color player2Color = Color.red;
		static Color[] colors = new Color[]{player1Color, player2Color};

		public int numPlayers = 1;
		public GameObject PlayerPrefab;
		public GameObject GondolaPrefab;

		public int PointsPerWindow = 50;

		List<Window> windows;
		List<CharacterController> characters;

		public float TimeLimit;
		private float timeLeft;

		public Text TimerText; 
		public List<Text> ScoreText;
		public Text GameEndText;

		void Start () 
		{
			currentState = GameState.Starting;
			characters = new List<CharacterController> ();

			for (int i = 0; i < numPlayers; i++)
			{
				// Instantiate Player
				GameObject playerGameObject = GameObject.Instantiate(PlayerPrefab, new Vector3(1, 0, 0) * i * 2, Quaternion.identity);
				GameObject gondolaGameObject = GameObject.Instantiate(GondolaPrefab, new Vector3(1, 0, 0) * i * 2, Quaternion.identity);

				// Need to add Rigidbody2D at runtime as adding it at compile time causes a crash with unity
				Rigidbody2D rb2d = playerGameObject.AddComponent<Rigidbody2D> ();
				rb2d.mass = 30;
				rb2d.gravityScale = 2;
				rb2d.sleepMode = RigidbodySleepMode2D.NeverSleep;
				rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;

				CharacterController player = playerGameObject.GetComponent<CharacterController> ();
				player.color = colors [i];
				player.SetPlayerNumber (i+1);
				characters.Add (player);

				GondolaController gondola = gondolaGameObject.GetComponent<GondolaController> ();
				gondola.SetPlayerNumber (i+1);
			}

			windows = Object.FindObjectsOfType<Window> ().ToList ();

			timeLeft = TimeLimit;
			currentState = GameState.Playing;

		}
		
		// Update is called once per frame
		void Update () 
		{
			if (currentState == GameState.Playing) {
				// Timer update
				timeLeft -= Time.deltaTime;
				TimerText.text = timeLeft.ToString ("F1");

				if (timeLeft <= 0) {
					currentState = GameState.Ended;
				}


				// Scores update
				foreach (CharacterController controller in characters) {
					controller.cleanedWindows = 0;
				}

				foreach (Window window in windows) {
					if (window.cleanedBy != null) {
						window.cleanedBy.cleanedWindows += 1;
					}
				}

				for (int i = 0; i < ScoreText.Count; i++) {
					ScoreText [i].text = "Player "+ (i+1) +": $" + characters [i].cleanedWindows * PointsPerWindow;
				}
			}
			else if(currentState == GameState.Ended){
				foreach (var controller in characters) {
					controller.enabled = false;
				}	
				//Find out what the top score is and which players have it
				int topScore = characters.Max (character => character.cleanedWindows);
				List<CharacterController> winners = characters.FindAll (character => character.cleanedWindows == topScore);
				
				if (winners.Count == 1) {
					GameEndText.text = "Player " + (winners[0].PlayerNumber) + " wins!";
				} else {
					string outText = "Players ";
					for (int i = 0; i < winners.Count; i++) {
						outText += "" + (winners[i].PlayerNumber) + " ";
						if (i != winners.Count-1) {
							outText += "and ";
						}
					}
					outText += "win!";
					GameEndText.text = outText;		
				}
				GameEndText.enabled = true;

			}

			
		}
	}

}