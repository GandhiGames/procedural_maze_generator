using UnityEngine;
using System.Collections;

public enum Direction
{
	NORTH,
	EAST,
	SOUTH,
	WEST
}

public class Directions : MonoBehaviour
{
	private static Directions _instance;
	public static Directions instance { get { return _instance; } }

	public const int Count = 4;

	public Vector2i[] vectors = {
		new Vector2i (0, 1),
		new Vector2i (1, 0),
		new Vector2i (0, -1),
		new Vector2i (-1, 0)
	};


	private Direction[] opposites = {
		Direction.SOUTH,
		Direction.WEST,
		Direction.NORTH,
		Direction.EAST
	};

	private Quaternion[] rotations = {
		Quaternion.identity,
		Quaternion.Euler(0f, 90f, 0f),
		Quaternion.Euler(0f, 180f, 0f),
		Quaternion.Euler(0f, 270f, 0f)
	};

	void Awake ()
	{
		_instance = this;
	}

	public Direction GetOpposite (Direction dir) {
		return opposites[(int)dir];
	}

	public Vector2i ToVector2i (Direction dir)
	{
		return vectors [(int)dir];
	}

	public Quaternion ToRotation (Direction dir)
	{
		return rotations[(int)dir];
	}


	public Direction RandomDirection {
		get {
			return (Direction)Random.Range (0, Count);
		}
	}
	
	public Direction GetNextClockwise (Direction direction) {
		return (Direction)(((int)direction + 1) % Count);
	}
	
	public Direction GetNextCounterclockwise (Direction direction) {
		return (Direction)(((int)direction + Count - 1) % Count);
	}

}
