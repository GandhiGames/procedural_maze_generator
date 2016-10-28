using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

	private Cell currentCell;
	private Direction currentDirection;
	
	public void SetLocation (Cell cell)
	{
		if (currentCell != null) {
			currentCell.OnPlayerExit ();
		}
		
		currentCell = cell;
		transform.localPosition = cell.transform.localPosition;
		currentCell.OnPlayerEnter ();
	}
	
	private void Move (Direction dir)
	{
		var edge = currentCell.GetEdge (dir);
		
		if (edge is CellPassage) {
			SetLocation (edge.connectingCell);
		}
	}
	
	private void Rotate (Direction dir)
	{
		transform.localRotation = Directions.instance.ToRotation (dir);
		currentDirection = dir;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyUp (KeyCode.UpArrow) || Input.GetKeyUp (KeyCode.W)) {
			Move (currentDirection);
		} else if (Input.GetKeyUp (KeyCode.LeftArrow) || Input.GetKeyUp (KeyCode.A)) {
			Move (Directions.instance.GetNextCounterclockwise (currentDirection));
		} else if (Input.GetKeyUp (KeyCode.DownArrow) || Input.GetKeyUp (KeyCode.S)) {
			Move (Directions.instance.GetOpposite (currentDirection));
		} else if (Input.GetKeyUp (KeyCode.RightArrow) || Input.GetKeyUp (KeyCode.D)) {
			Move (Directions.instance.GetNextClockwise (currentDirection));
		} else if (Input.GetKeyDown (KeyCode.Q)) {
			Rotate (Directions.instance.GetNextCounterclockwise (currentDirection));
		} else if (Input.GetKeyDown (KeyCode.E)) {
			Rotate (Directions.instance.GetNextClockwise (currentDirection));
		}
	}
	

}
