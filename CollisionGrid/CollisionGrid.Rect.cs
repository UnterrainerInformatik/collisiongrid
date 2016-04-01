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

using Utilities.Geometry;

namespace CollisionGrid
{
	public partial class CollisionGrid<T>
	{
		public T[] Get(Rect aabb)
		{
			lock (lockObject)
			{
				return Get(Rectangle(aabb));
			}
		}

		/// <summary>
		/// Adds a given item to the cells that are hit by the given Axis-Aligned-Bounding-Box.
		/// If the cell already contains the item, it is not added a second time.
		/// </summary>
		/// <param name="item">The item to add</param>
		/// <param name="aabb">The Axis-Aligned-Bounding-Box given in float-grid-coordinates.</param>
		public void Add(T item, Rect aabb)
		{
			lock (lockObject)
			{
				Add(item, Rectangle(aabb));
			}
		}

		/// <summary>
		/// Removes all items from the cells that are hit by the given Axis-Aligned-Bounding-Box.
		/// If the items don't occupy another cell, they are removed as well.
		/// </summary>
		/// <param name="aabb">The Axis-Aligned-Bounding-Box given in float-grid-coordinates.</param>
		public void Remove(Rect aabb)
		{
			lock (lockObject)
			{
				Remove(Rectangle(aabb));
			}
		}

		/// <summary>
		/// Removes all occurrences of the given item and re-adds it at the new cells that are hit by the given Axis-Aligned-Bounding-Box.
		/// If the item hasn't been in the grid before, this will just add it.
		/// </summary>
		/// <param name="item">The item to move</param>
		/// <param name="aabb">The Axis-Aligned-Bounding-Box given in float-grid-coordinates.</param>
		public void Move(T item, Rect aabb)
		{
			lock (lockObject)
			{
				Move(item, Rectangle(aabb));
			}
		}
	}
}