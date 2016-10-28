using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour
{
	public Vector2i Coordinates { get; set; }

	private CellEdge[] edges = new CellEdge[Directions.Count];

	private int edgeCount;
	
	public bool IsFullyInitialised {
		get {
			return edgeCount == Directions.Count;
		}
	}
	
	public Room _Room;
	
	public void Initialise (Room room)
	{
		room.Add (this);
		transform.GetChild (0).GetComponent<Renderer> ().material = room.Settings.FloorMaterial;
	}

	public Direction? RandomUninitialisedDirection {
		get {
			int skips = Random.Range (0, Directions.Count - edgeCount);
			for (int i = 0; i < Directions.Count; i++) {
				if (edges [i] == null) {
					if (skips == 0) {
						return (Direction)i;
					}
					skips -= 1;
				}
			}

			return null;

		}
	}

	
	public CellEdge GetEdge (Direction direction)
	{
		return edges [(int)direction];
	}
	
	public void SetEdge (Direction direction, CellEdge edge)
	{
		edges [(int)direction] = edge;
		edgeCount++;
	}

	public void OnPlayerEnter ()
	{
		_Room.Show ();
		for (int i = 0; i < edges.Length; i++) {
			edges [i].OnPlayerEnter ();
		}
	}
	
	public void OnPlayerExit ()
	{
		_Room.Hide ();
		foreach (var e in edges) {
			e.OnPlayerExit ();
		}
	}

	public void Show ()
	{
		gameObject.SetActive (true);
	}
	
	public void Hide ()
	{
		gameObject.SetActive (false);
	}

}
