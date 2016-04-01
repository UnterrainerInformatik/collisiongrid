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

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CollisionGrid
{
	public partial class CollisionGrid<T>
	{
		public T[] Get(Point cell)
		{
			lock (lockObject)
			{
				return grid[Clamp(cell)].ToArray();
			}
		}

		/// <summary>
		/// Adds a given item to a given cell.
		/// If the cell already contains the item, it is not added a second time.
		/// </summary>
		/// <param name="item">The item to add</param>
		/// <param name="cell">The cell to add the item to</param>
		public void Add(T item, Point cell)
		{
			lock (lockObject)
			{
				Point c = Clamp(cell);
				AddToGrid(item, c);
				AddToItems(item, c);
			}
		}

		private void AddToGrid(T item, Point cell)
		{
			List<T> l = grid[cell];
			if (!l.Contains(item))
			{
				l.Add(item);
			}
		}

		private void AddToItems(T item, Point cell)
		{
			List<Point> pl;
			items.TryGetValue(item, out pl);
			if (pl == null)
			{
				if (listOfPointQueue.Count > 0)
				{
					pl = listOfPointQueue.Dequeue();
				}
				else
				{
					pl = new List<Point>();
				}
				pl.Add(cell);
				items.Add(item, pl);
			}
			else
			{
				if (!pl.Contains(cell))
				{
					pl.Add(cell);
				}
			}
		}

		/// <summary>
		/// Removes all items from the given cell.
		/// If the items don't occupy another cell, they are removed as well.
		/// </summary>
		/// <param name="cell">The cell to remove items from</param>
		public void Remove(Point cell)
		{
			lock (lockObject)
			{
				Point c = Clamp(cell);
				List<T> l = grid[c];

				foreach (T i in l)
				{
					List<Point> pl;
					items.TryGetValue(i, out pl);
					if (pl != null)
					{
						pl.Remove(c);
						if (pl.Count == 0)
						{
							listOfPointQueue.Enqueue(pl);
							items.Remove(i);
						}
					}
				}

				l.Clear();
			}
		}

		/// <summary>
		/// Removes all occurrences of the given item and re-adds it at the new given cell.
		/// If the item hasn't been in the grid before, this will just add it.
		/// </summary>
		/// <param name="item">The item to move</param>
		/// <param name="cell">The cell to move it to</param>
		public void Move(T item, Point cell)
		{
			lock (lockObject)
			{
				Remove(item);
				Add(item, cell);
			}
		}
	}
}