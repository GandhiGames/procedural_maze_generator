using UnityEngine;
using System.Collections;

namespace HorrorHouse
{
	public class Director : MonoBehaviour
	{
		public Environment EnvironmentPrefab;
		public Player PlayerPrefab;
		public Map HUD;
		
		private Environment envirInstance;
		private Player playerInstance;

		void Awake ()
		{
			envirInstance = (Environment)Instantiate (EnvironmentPrefab);
		}

		void Start()
		{
			Generate ();
		}

		void Update ()
		{
			if (Input.GetKeyUp (KeyCode.R)) {
				Generate ();
			} 
		}
		
		private void Generate ()
		{
			GenerateMaze ();
			GeneratePlayer ();
			HUD.Initialise ();
		}
	
		private void GenerateMaze ()
		{
			envirInstance.Generate ();
		}
		
		private void GeneratePlayer ()
		{
			if (playerInstance != null) {
				Destroy (playerInstance);
			}
			
			playerInstance = (Player)Instantiate (PlayerPrefab);
			playerInstance.SetLocation (envirInstance.Cells.GetRandomCell ());
		}
		
	}
}
