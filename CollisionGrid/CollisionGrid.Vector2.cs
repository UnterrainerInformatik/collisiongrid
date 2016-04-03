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

using Microsoft.Xna.Framework;

namespace CollisionGrid
{
	public partial class CollisionGrid<T>
	{
		public T[] Get(Vector2 position)
		{
			lock (lockObject)
			{
				return Get(Cell(position));
			}
		}

		/// <summary>
		///     Gets the first item encountered in the cell that contains the given position.
		/// </summary>
		/// <param name="position">The position</param>
		/// <returns>
		///     The item or default(T)
		/// </returns>
		public T First(Vector2 position)
		{
			lock (lockObject)
			{
				return First(Cell(position));
			}
		}

		/// <summary>
		///     Adds a given item to a cell that contains the given position.
		///     If the cell already contains the item, it is not added a second time.
		/// </summary>
		/// <param name="item">The item to add</param>
		/// <param name="position">The position.</param>
		public void Add(T item, Vector2 position)
		{
			lock (lockObject)
			{
				Add(item, Cell(position));
			}
		}

		/// <summary>
		///     Removes all items from the cell that contains the given position.
		///     If the items don't occupy another cell, they are removed as well.
		/// </summary>
		/// <param name="position">The position.</param>
		public void Remove(Vector2 position)
		{
			lock (lockObject)
			{
				Remove(Cell(position));
			}
		}

		/// <summary>
		///     Removes all occurrences of the given item and re-adds it at the new cell that contains the given position.
		///     If the item hasn't been in the grid before, this will just add it.
		/// </summary>
		/// <param name="item">The item to move</param>
		/// <param name="position">The position.</param>
		public void Move(T item, Vector2 position)
		{
			lock (lockObject)
			{
				Move(item, Cell(position));
			}
		}

		public bool IsEmpty(Vector2 position)
		{
			lock (lockObject)
			{
				return IsEmpty(Cell(position));
			}
		}
	}
}