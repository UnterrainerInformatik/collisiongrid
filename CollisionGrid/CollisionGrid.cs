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

		private Dictionary<Point, List<T>> Grid { get; set; }
		private Dictionary<T, List<Point>> Items { get; set; }
		private Queue<List<Point>> ListOfPointQueue { get; set; }
		private Queue<List<T>> ListOfItemQueue { get; set; }

		public CollisionGrid(float width, float height, int numberOfCellsX, int numberOfCellsY)
		{
			Width = width;
			Height = height;
			NumberOfCellsX = numberOfCellsX;
			NumberOfCellsY = numberOfCellsY;
			CellWidth = Width/NumberOfCellsX;
			CellHeight = Height/NumberOfCellsY;

			Items = new Dictionary<T, List<Point>>();
			Grid = new Dictionary<Point, List<T>>();

			ListOfPointQueue = new Queue<List<Point>>();
			ListOfItemQueue = new Queue<List<T>>();
		}

		public Point[] Get(T item)
		{
			lock (lockObject)
			{
				List<Point> pl;
				Items.TryGetValue(item, out pl);
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
				Items.TryGetValue(item, out pl);
				if (pl == null)
				{
					return;
				}

				foreach (Point p in pl)
				{
					RemoveFromGrid(item, p);
				}

				pl.Clear();
				ListOfPointQueue.Enqueue(pl);
				Items.Remove(item);
			}
		}

		private void RemoveFromGrid(T item, Point cell)
		{
			List<T> tl;
			Grid.TryGetValue(cell, out tl);
			if (tl != null)
			{
				tl.Remove(item);
				if (tl.Count == 0)
				{
					if (ListOfItemQueue.Count == 0)
					{
						Console.Out.WriteLine("Yippie!");
					}
					ListOfItemQueue.Enqueue(tl);
					Grid.Remove(cell);
				}
			}
		}

		public IEnumerable<T> AllItems()
		{
			return Items.Keys;
		}

		public IEnumerable<Point> AllOccupiedCells()
		{
			return Grid.Keys;
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
				ListOfPointQueue.Clear();
				ListOfPointQueue = null;
				ListOfItemQueue.Clear();
				ListOfItemQueue = null;
				Grid.Clear();
				Grid = null;
				Items.Clear();
				Items = null;
			}
		}
	}
}