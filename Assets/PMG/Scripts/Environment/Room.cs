using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : ScriptableObject
{

	public int SettingsIndex;
	
	public RoomSettings Settings;
	
	private List<Cell> cells = new List<Cell> ();
	public List<Cell> Cells { get { return cells; } }
	
	public void Add (Cell cell)
	{
		cell._Room = this;
		cells.Add (cell);
	}
	
	public void Combine (Room room)
	{
		for (int i = 0; i < room.cells.Count; i++) {
			Add (room.cells [i]);
		}
	}
	
	public void Hide ()
	{
		foreach (var c in cells) {
			c.Hide ();
		}
	}
	
	public void Show ()
	{
		foreach (var c in cells) {
			c.Show ();
		}
	}
}
