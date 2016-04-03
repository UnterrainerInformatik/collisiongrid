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
//  Created: 2016-04-01
// ***************************************************************************

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CollisionGrid
{
	public partial class CollisionGrid<T>
	{
		private readonly List<Point> lop = new List<Point>();
		private readonly List<T> result = new List<T>(); 

		private void FillList(Rectangle aabb)
		{
			Rectangle r = Clamp(aabb);
			lop.Clear();
			for (int y = 0; y < r.Size.Y; y++)
			{
				for (int x = 0; x < r.Size.X; x++)
				{
					lop.Add(new Point(r.X + x, r.Y + y));
				}
			}
		}

		public T[] Get(Rectangle aabb)
		{
			lock (lockObject)
			{
				FillList(aabb);
				result.Clear();
				foreach (Point p in lop)
				{
					result.AddRange(Get(p));
				}
				return result.ToArray();
			}
		}

		/// <summary>
		/// Gets the first item encountered in the cells that are hit by the given Axis-Aligned-Bounding-Box.
		/// </summary>
		/// <param name="aabb">The Axis-Aligned-Bounding-Box given in int-cell-coordinates</param>
		/// <returns>
		/// The item or default(T)
		/// </returns>
		public T First(Rectangle aabb)
		{
			lock (lockObject)
			{
				FillList(aabb);
				result.Clear();
				foreach (Point p in lop)
				{
					T content = First(p);
					if (!content.Equals(default(T)))
					{
						return content;
					}
				}
				return default(T);
			}
		}

		/// <summary>
		/// Adds a given item to the cells that are hit by the given Axis-Aligned-Bounding-Box.
		/// If the cell already contains the item, it is not added a second time.
		/// </summary>
		/// <param name="item">The item to add</param>
		/// <param name="aabb">The Axis-Aligned-Bounding-Box given in int-cell-coordinates</param>
		public void Add(T item, Rectangle aabb)
		{
			lock (lockObject)
			{
				FillList(aabb);
				foreach (Point p in lop)
				{
					Add(item, p);
				}
			}
		}

		/// <summary>
		/// Removes all items from the cells that are hit by the given Axis-Aligned-Bounding-Box.
		/// If the items don't occupy another cell, they are removed as well.
		/// </summary>
		/// <param name="aabb">The Axis-Aligned-Bounding-Box given in int-cell-coordinates</param>
		public void Remove(Rectangle aabb)
		{
			lock (lockObject)
			{
				FillList(aabb);
				foreach (Point p in lop)
				{
					Remove(p);
				}
			}
		}

		/// <summary>
		/// Removes all occurrences of the given item and re-adds it at the new cells that are hit by the given Axis-Aligned-Bounding-Box.
		/// If the item hasn't been in the grid before, this will just add it.
		/// </summary>
		/// <param name="item">The item to move</param>
		/// <param name="aabb">The Axis-Aligned-Bounding-Box given in int-cell-coordinates</param>
		public void Move(T item, Rectangle aabb)
		{
			lock (lockObject)
			{
				Remove(item);
				FillList(aabb);
				foreach (Point p in lop)
				{
					Add(item, p);
				}
			}
		}

		public bool IsEmpty(Rectangle aabb)
		{
			lock (lockObject)
			{
				FillList(aabb);
				foreach (Point p in lop)
				{
					if (!IsEmpty(p))
					{
						return false;
					}
				}
				return true;
			}
		}
	}
}