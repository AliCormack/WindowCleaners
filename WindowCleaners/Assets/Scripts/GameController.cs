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

		public int numPlayers = 2;
		public GameObject PlayerPrefab;

		public int PointsPerWindow = 50;

		List<Window> windows;
		List<CharacterController> characters;

		void Start () 
		{
			characters = new List<CharacterController> ();

			for (int i = 0; i <= numPlayers; i++)
			{
				// Instantiate Player
				GameObject playerGameObject = GameObject.Instantiate(PlayerPrefab, new Vector3(1, 0, 0) * i * 2, Quaternion.identity);

				// Need to add Rigidbody2D at runtime as adding it at compile time causes a crash with unity
				Rigidbody2D rb2d = playerGameObject.AddComponent<Rigidbody2D> ();
				rb2d.mass = 30;
				rb2d.gravityScale = 2;
				rb2d.sleepMode = RigidbodySleepMode2D.NeverSleep;
				rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;

				CharacterController player = playerGameObject.GetComponent<CharacterController> ();
				player.color = colors [i];
				characters.Add (player);
			}

			windows = Object.FindObjectsOfType<Window> ().ToList ();

		}
		
		// Update is called once per frame
		void Update () 
		{

			foreach (CharacterController controller in characters)
			{
				controller.cleanedWindows = 0;
			}

			foreach (Window window in windows)
			{
				window.cleanedBy.cleanedWindows += 1;
			}
		}
	}

}