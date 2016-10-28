using UnityEngine;
using System.Collections;

public abstract class CellEdge : MonoBehaviour
{

	public Cell cell, connectingCell;

	public Direction direction;

	public virtual void Initialise (Cell cell, Cell connectingCell, Direction direction)
	{
		this.cell = cell;
		this.connectingCell = connectingCell;
		this.direction = direction;
		cell.SetEdge (direction, this);
		transform.parent = cell.transform;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Directions.instance.ToRotation (direction);
	}
	
	public virtual void OnPlayerEnter ()
	{
	}
	
	public virtual void OnPlayerExit ()
	{
	}
	
}
