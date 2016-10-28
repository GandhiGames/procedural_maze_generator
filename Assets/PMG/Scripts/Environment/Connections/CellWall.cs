using UnityEngine;
using System.Collections;

public class CellWall : CellEdge
{
	public Transform Child;
	
	public override void Initialise (Cell cell, Cell otherCell, Direction direction)
	{
		base.Initialise (cell, otherCell, direction);
		Child.GetComponent<Renderer> ().material = cell._Room.Settings.WallMaterial;
	}
}
