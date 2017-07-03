using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WindowCleaner
{

	public class GameController : MonoBehaviour 
	{

		static Color player1Color = Color.blue;
		static Color player2Color = Color.red;
		static Color[] colors = new Color[]{player1Color, player2Color};

		public int numPlayers = 1;
		public GameObject PlayerPrefab;
		public GameObject GondolaPrefab;

		public int PointsPerWindow = 50;

		List<Window> windows;
		List<CharacterController> characters;

		bool gameStarted = false;

		public float TimeLimit;
		private float timeLeft;


		void Start () 
		{
			characters = new List<CharacterController> ();

			for (int i = 0; i < numPlayers; i++)
			{
				// Instantiate Player
				GameObject playerGameObject = GameObject.Instantiate(PlayerPrefab, new Vector3(1, 0, 0), Quaternion.identity);
				GameObject gondolaGameObject = GameObject.Instantiate(GondolaPrefab, new Vector3(1, 5, 0), Quaternion.identity);

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
			gameStarted = true;

		}
		
		// Update is called once per frame
		void Update () 
		{
			timeLeft -= Time.deltaTime;


			foreach (CharacterController controller in characters)
			{
				controller.cleanedWindows = 0;
			}

			foreach (Window window in windows)
			{
				if (window.cleanedBy != null) {
					window.cleanedBy.cleanedWindows += 1;
				}
			}
		}
	}

}