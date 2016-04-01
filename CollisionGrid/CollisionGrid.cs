// *************************************************************************** 
//  Copyright (c) 2016 by Unterrainer Informatik OG.
//  This source is licensed to Unterrainer Informatik OG.
//  All rights reserved.
//  
//  In other words:
//  YOU MUST NOT COPY, USE, CHANGE OR REDISTRIBUTE ANY ART, MUSIC, CODE OR
//  OTHER DATA, CONTAINED WITHIN THESE DIRECTORIES WITHOUT THE EXPRESS
//  PERMISSION OF Unterrainer Informatik OG.
// ---------------------------------------------------------------------------
//  Programmer: G U, 
//  Created: 2016-03-30
// ***************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Utilities.Geometry;

namespace CollisionGrid
{
	public partial class CollisionGrid<T>
	{
		private readonly object lockObject = new object();

		public float Width { get; private set; }
		public float Height { get; private set; }

		public float CellWidth { get; private set; }
		public float CellHeight { get; private set; }

		public int NumberOfCellsX { get; private set; }
		public int NumberOfCellsY { get; private set; }

		private Dictionary<Point, List<T>> grid = new Dictionary<Point, List<T>>();
		private Dictionary<T, List<Point>> items = new Dictionary<T, List<Point>>();
		private Queue<List<Point>> listOfPointQueue = new Queue<List<Point>>(); 

		public CollisionGrid(float width, float height, int numberOfCellsX, int numberOfCellsY)
		{
			Width = width;
			Height = height;
			NumberOfCellsX = numberOfCellsX;
			NumberOfCellsY = numberOfCellsY;
			CellWidth = Width/NumberOfCellsX;
			CellHeight = Height/NumberOfCellsY;

			items = new Dictionary<T, List<Point>>();
			grid = new Dictionary<Point, List<T>>();
			InitGrid(grid);
		}

		private void InitGrid(Dictionary<Point, List<T>> d)
		{
			for (int y = 0; y < NumberOfCellsY; y++)
			{
				for (int x = 0; x < NumberOfCellsX; x++)
				{
					d[new Point(x, y)] = new List<T>();
				}
			}
		}

		public Point[] Get(T item)
		{
			lock (lockObject)
			{
				List<Point> pl;
				items.TryGetValue(item, out pl);
				if (pl == null)
				{
					return new Point[0];
				}
				return pl.ToArray();
			}
		}

		/// <summary>
		/// Removes the given item from the grid.
		/// </summary>
		/// <param name="item">The item to remove</param>
		public void Remove(T item)
		{
			lock (lockObject)
			{
				List<Point> pl;
				items.TryGetValue(item, out pl);
				if (pl == null)
				{
					return;
				}

				foreach (Point p in pl)
				{
					grid[p].Remove(item);
				}

				pl.Clear();
				listOfPointQueue.Enqueue(pl);
				items.Remove(item);
			}
		}

		private Rectangle Rectangle(Rect rect)
		{
			Point tl = Cell(rect.TopLeft);
			Point br = Cell(rect.BottomRight);
			Point s = br - tl + new Point(1, 1);
			return new Rectangle(tl, s);
		}

		private Point Cell(Vector2 position)
		{
			return new Point((int)(position.X / CellWidth), (int)(position.Y / CellHeight));
		}

		private Rectangle Clamp(Rectangle rectangle)
		{
			Point tl = Clamp(rectangle.Location);
			Point br = Clamp(rectangle.Location + rectangle.Size - new Point(1, 1));
			Point s = br - tl + new Point(1, 1);
			return new Rectangle(tl, s);
		}

		private Point Clamp(Point p)
		{
			int nx = p.X;
			if (nx >= NumberOfCellsX)
			{
				nx = NumberOfCellsX - 1;
			}
			if (nx < 0)
			{
				nx = 0;
			}

			int ny = p.Y;
			if (ny >= NumberOfCellsY)
			{
				ny = NumberOfCellsY - 1;
			}
			if (ny < 0)
			{
				ny = 0;
			}
			return new Point(nx, ny);
		}

		public void Dispose()
		{
			lock (lockObject)
			{
				listOfPointQueue.Clear();
				listOfPointQueue = null;
				grid.Clear();
				grid = null;
				items.Clear();
				items = null;
			}
		}
	}
}