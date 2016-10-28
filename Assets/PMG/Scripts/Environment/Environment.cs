using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HorrorHouse
{
	public class Environment : MonoBehaviour
	{
		public Vector2i Size;
		public Cell CellPrefab;
		public CellPassage PassagePrefab;
		public CellWall WallPrefab;
		public CellDoor DoorPrefab;
		
		[Range (0f, 1f)]
		public float
			ChanceToAddDoor = 0.3f;
			
		public RoomSettings[] RoomTypes;
		private List<Room> rooms;
		
		private GameObject parent;
		private CellArray cells;

		public CellArray Cells { get { return cells; } }

		// Use this for initialization
		void Awake ()
		{
			parent = new GameObject ("Cells");
		}

		public void Generate ()
		{
			ObjectManager.instance.RemoveObjects ();

			cells = new CellArray (new Vector2i ((int)Size.x, (int)Size.y));
			rooms = new List<Room> ();

			InstantiateCells ();
			MergeSmallRooms ();
			//HideRooms ();
		}

		public void InstantiateCells ()
		{
			var activeCells = new List<Cell> ();
	
			DoFirstGenerationStep (activeCells);

			while (activeCells.Count > 0) {
				DoNextGenerationStep (activeCells);
			}
		}

		private Cell CreateCell (Vector2i coord)
		{
			var cellObj = ObjectManager.instance.AddObject (CellPrefab.name);
			var cell = cellObj.GetComponent<Cell> ();
			cell.Coordinates = coord;
			cells.Add (cell, coord);
			cellObj.transform.SetParent (parent.transform, false);
			cellObj.transform.localPosition = new Vector3 (coord.x - cells.Width * 0.5f + 0.5f, 0f, coord.y - cells.Height * 0.5f + 0.5f);
			cellObj.SetActive (true);
			return cell;
		}

		private void DoFirstGenerationStep (List<Cell> activeCells)
		{
			var cell = CreateCell (cells.GetRandomCoord ());
			cell.Initialise (CreateRoom (-1));
			activeCells.Add (cell);
		}

		private void DoNextGenerationStep (List<Cell> activeCells)
		{
			int currentIndex = activeCells.Count - 1;
			var currentCell = activeCells [currentIndex];

			if (currentCell.IsFullyInitialised) {
				activeCells.RemoveAt (currentIndex);
				return;
			}

			var directionVal = currentCell.RandomUninitialisedDirection;

			if (!directionVal.HasValue) {
				Debug.LogError ("Cell contains no uninitialised directions left");
				return;
			}

			var direction = directionVal.Value;

			Vector2i coordinates = currentCell.Coordinates + Directions.instance.ToVector2i (direction);

			if (cells.InBounds (coordinates)) {
				var neighbour = cells.Get (coordinates);
				if (neighbour == null) {
					neighbour = CreateCell (coordinates);
					CreatePassage (currentCell, neighbour, direction);
					activeCells.Add (neighbour);
				} else if (currentCell._Room.SettingsIndex == neighbour._Room.SettingsIndex) {
					CreatePassageInSameRoom (currentCell, neighbour, direction);
				} else {
					CreateWall (currentCell, neighbour, direction);
					
				}
			} else {
				CreateWall (currentCell, null, direction);
			}
		}

		private void HideRooms ()
		{
			foreach (var r in rooms) {
				r.Hide ();
			}
		}

		private void MergeSmallRooms ()
		{
			for (int passes = 0; passes < 10; passes++) {
				for (int i = 0; i < rooms.Count; i++) {
					var r = rooms [i];
					if (r.Cells.Count < 4) {
						var neighbouringRoom = GetNeighbouringRoom (r);
					
						if (neighbouringRoom != null) {
							Debug.Log ("here");
							neighbouringRoom.Combine (r);
							rooms.Remove (r);
							Destroy (r);
						}
					}	
				}
			}
		}

		private Room GetNeighbouringRoom (Room room)
		{
			foreach (var c in room.Cells) {
				foreach (var v in Directions.instance.vectors) {
				
					var neighbour = cells.Get (c.Coordinates + v);
					
					if (neighbour != null && neighbour._Room.SettingsIndex != room.SettingsIndex) {
						return neighbour._Room;
					}

				}
			}
			
			return null;
		}

		private void CreatePassage (Cell currentCell, Cell neighbourCell, Direction direction)
		{
/*			InitPassage (currentCell, neighbourCell, direction);
			InitPassageWithRoom (neighbourCell, currentCell, Directions.instance.GetOpposite (direction));*/
			
			var prefab = Random.value < ChanceToAddDoor ? DoorPrefab : PassagePrefab;
			var passageObj = ObjectManager.instance.AddObject (prefab.name);
			passageObj.SetActive (true);
			var passage = passageObj.GetComponent<CellPassage> ();
			passage.Initialise (currentCell, neighbourCell, direction);
			
			passageObj = ObjectManager.instance.AddObject (prefab.name);
			passageObj.SetActive (true);
			passage = passageObj.GetComponent<CellPassage> ();
			if (passage is CellDoor) {
				neighbourCell.Initialise (CreateRoom (currentCell._Room.SettingsIndex));
			} else {
				neighbourCell.Initialise (currentCell._Room);
			}
			passage.Initialise (neighbourCell, currentCell, Directions.instance.GetOpposite (direction));
		}

		private void CreatePassageInSameRoom (Cell cell, Cell otherCell, Direction direction)
		{
			var passageObj = ObjectManager.instance.AddObject (PassagePrefab.name);
			passageObj.SetActive (true);
			var passage = passageObj.GetComponent<CellPassage> ();
			passage.Initialise (cell, otherCell, direction);
			passageObj = ObjectManager.instance.AddObject (PassagePrefab.name);
			passageObj.SetActive (true);
			passage = passageObj.GetComponent<CellPassage> ();
			passage.Initialise (otherCell, cell, Directions.instance.GetOpposite (direction));
			
			if (cell._Room != otherCell._Room) {
				var roomToCombine = otherCell._Room;
				cell._Room.Combine (roomToCombine);
				rooms.Remove (roomToCombine);
				Destroy (roomToCombine);
			}
		}

		private void InitPassage (Cell currentCell, Cell neighbourCell, Direction direction)
		{
			var prefab = (Random.value < ChanceToAddDoor) ? DoorPrefab : PassagePrefab;
			var passageObj = ObjectManager.instance.AddObject (prefab.name); 
			passageObj.SetActive (true);
			passageObj.SetActive (true);
			var passage = passageObj.GetComponent<CellPassage> ();
			passage.Initialise (currentCell, neighbourCell, direction);
		}

		private void InitPassageWithRoom (Cell currentCell, Cell neighbourCell, Direction direction)
		{
			var prefab = (Random.value < ChanceToAddDoor) ? DoorPrefab : PassagePrefab;
			var passageObj = ObjectManager.instance.AddObject (prefab.name); //Instantiate(PassagePrefab) as MazePassage;
			passageObj.SetActive (true);
			var passage = passageObj.GetComponent<CellPassage> ();
		
			if (passage is CellDoor) {
				neighbourCell.Initialise (CreateRoom (currentCell._Room.SettingsIndex));
			} else {
				neighbourCell.Initialise (currentCell._Room);
			}
			
			passage.Initialise (currentCell, neighbourCell, direction);
		}

		private void CreateWall (Cell currentCell, Cell neighbourCell, Direction direction)
		{
			InitWall (currentCell, neighbourCell, direction);
			if (neighbourCell != null) {
				InitWall (neighbourCell, currentCell, Directions.instance.GetOpposite (direction));
			}
		}

		private void InitWall (Cell currentCell, Cell neighbourCell, Direction direction)
		{
			var wallObj = ObjectManager.instance.AddObject (WallPrefab.name); 
			wallObj.SetActive (true);
			var wall = wallObj.GetComponent<CellWall> ();
			wall.Initialise (currentCell, neighbourCell, direction);
		}

		private Room CreateRoom (int indexToExclude)
		{
			var newRoom = ScriptableObject.CreateInstance<Room> ();
			newRoom.SettingsIndex = Random.Range (0, RoomTypes.Length);
			if (newRoom.SettingsIndex == indexToExclude) {
				newRoom.SettingsIndex = (newRoom.SettingsIndex + 1) % RoomTypes.Length;
			}
			newRoom.Settings = RoomTypes [newRoom.SettingsIndex];
			rooms.Add (newRoom);
			return newRoom;
		}
	}
}
