using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WindowCleaner
{

	public class Window : MonoBehaviour {

		public CharacterController cleanedBy;
		SpriteRenderer spriteRenderer;


		void Start () 
		{
			spriteRenderer = GetComponent<SpriteRenderer> ();

			SetCleaned (null);
		}

		public void SetCleaned(CharacterController cleaner)
		{
			this.cleanedBy = cleaner;

			if (cleaner != null)
			{
				spriteRenderer.color = cleaner.color;
				Sprite newSprite = Resources.Load<Sprite> ("Clean_Window");
				spriteRenderer.sprite = newSprite;

			}
		}

		public void Reset(){
			this.cleanedBy = null;
			spriteRenderer.color = Color.white;
		}
		
	
	}

}