using UnityEngine;
using System.Collections;

public class CellDoor : CellPassage
{
	public Transform Hinge;
	
	private CellDoor OtherSideOfDoor {
		get {
			return (CellDoor)connectingCell.GetEdge (Directions.instance.GetOpposite (direction));
		}
	}
	
	private static readonly Quaternion
		normalRotation = Quaternion.Euler (0f, -90f, 0f),
		mirroredRotation = Quaternion.Euler (0f, 90f, 0f);
	
	private bool isMirrored;
	
	public override void Initialise (Cell primary, Cell connectingCell, Direction dir)
	{
		base.Initialise (primary, connectingCell, dir);
		if (OtherSideOfDoor != null) {
			isMirrored = true;
			Vector3 s = Hinge.localScale;
			Hinge.localScale = new Vector3 (s.x * -1f, s.y, s.z);
			Vector3 p = Hinge.localPosition;
			Hinge.localPosition = new Vector3 (p.x * -1, p.y, p.z);
		}
		
		for (int i = 0; i < transform.childCount; i++) {
			Transform child = transform.GetChild (i);
			if (child != Hinge) {
				child.GetComponent<Renderer> ().material = cell._Room.Settings.WallMaterial;
			}
		}
	}
	
	public override void OnPlayerEnter ()
	{
		OtherSideOfDoor.Hinge.localRotation = Hinge.localRotation = isMirrored ? mirroredRotation : normalRotation;
		OtherSideOfDoor.cell._Room.Show ();
	}
	
	public override void OnPlayerExit ()
	{
		OtherSideOfDoor.Hinge.localRotation = Hinge.localRotation = Quaternion.identity;
		OtherSideOfDoor.cell._Room.Hide ();
	}
}
