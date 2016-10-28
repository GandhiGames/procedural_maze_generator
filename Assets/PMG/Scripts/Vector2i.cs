using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Vector2i
{
	public int x, y;

	public Vector2i (int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public static Vector2i operator + (Vector2i a, Vector2i b)
	{
		a.x += b.x;
		a.y += b.y;
		return a;
	}
}
