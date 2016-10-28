using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HorrorHouse
{
	public class CellArray
	{

		private Cell[,] cells;

		private int width, height;

		public int Width { get { return width; } }
		public int Height { get { return height; } }

		public CellArray (int width, int height)
		{
			this.width = width;
			this.height = height;
			cells = new Cell[width, height];
		}

		public CellArray (Vector2i size)
		{
			this.width = size.x;
			this.height = size.y;
			cells = new Cell[width, height];
		}
	
		public void Add (Cell cell, int x, int y)
		{
			if (InBounds (x, y)) {
				cells [x, y] = cell;
			}
		}

		public Cell Get (Vector2i coord)
		{
			if (InBounds (coord)) {
				return cells [coord.x, coord.y];
			}

			return null;
		}

		public void Add (Cell cell, Vector2i coord)
		{
			Add (cell, coord.x, coord.y);
		}

		public bool InBounds (int x, int y)
		{
			return x > 0 && x < width && y > 0 && y < height;
		}

		public bool InBounds (Vector2i coord)
		{
			return InBounds (coord.x, coord.y);
		}

		public Vector2i GetRandomCoord ()
		{
			return new Vector2i (Random.Range (0, width), Random.Range (0, height));
		}
		
		public Cell GetRandomCell ()
		{
			var coord = GetRandomCoord ();
			
			return Get (coord);
		}

	}
}
