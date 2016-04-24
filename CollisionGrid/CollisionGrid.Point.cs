// *************************************************************************** 
// This is free and unencumbered software released into the public domain.
// 
// Anyone is free to copy, modify, publish, use, compile, sell, or
// distribute this software, either in source code form or as a compiled
// binary, for any purpose, commercial or non-commercial, and by any
// means.
// 
// In jurisdictions that recognize copyright laws, the author or authors
// of this software dedicate any and all copyright interest in the
// software to the public domain. We make this dedication for the benefit
// of the public at large and to the detriment of our heirs and
// successors. We intend this dedication to be an overt act of
// relinquishment in perpetuity of all present and future rights to this
// software under copyright law.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// 
// For more information, please refer to <http://unlicense.org>
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
                List<T> contents;
                Grid.TryGetValue(Clamp(cell), out contents);
                if (contents == null)
                {
                    return new T[0];
                }
                return contents.ToArray();
            }
        }

        /// <summary>
        ///     Gets the first item encountered on the given cell.
        /// </summary>
        /// <param name="cell">The cell to search</param>
        /// <returns>The item or default(T)</returns>
        public T First(Point cell)
        {
            lock (lockObject)
            {
                List<T> contents;
                Grid.TryGetValue(Clamp(cell), out contents);
                if (contents != null && contents.Count > 0)
                {
                    return contents[0];
                }
                return default(T);
            }
        }

        /// <summary>
        ///     Adds a given item to a given cell.
        ///     If the cell already contains the item, it is not added a second time.
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <param name="cell">The cell to add the item to</param>
        public void Add(T item, Point cell)
        {
            lock (lockObject)
            {
                var c = Clamp(cell);
                AddToGrid(item, c);
                AddToItems(item, c);
            }
        }

        private void AddToGrid(T item, Point cell)
        {
            List<T> l;
            Grid.TryGetValue(cell, out l);
            if (l == null)
            {
                if (ListOfItemQueue.Count > 0)
                {
                    l = ListOfItemQueue.Dequeue();
                }
                else
                {
                    l = new List<T>();
                }
                l.Add(item);
                Grid.Add(cell, l);
            }
            else
            {
                if (!l.Contains(item))
                {
                    l.Add(item);
                }
            }
        }

        private void AddToItems(T item, Point cell)
        {
            List<Point> pl;
            ItemDictionary.TryGetValue(item, out pl);
            if (pl == null)
            {
                if (ListOfPointQueue.Count > 0)
                {
                    pl = ListOfPointQueue.Dequeue();
                }
                else
                {
                    pl = new List<Point>();
                }
                pl.Add(cell);
                ItemDictionary.Add(item, pl);
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
        ///     Removes all items from the given cell.
        ///     If the items don't occupy another cell, they are removed as well.
        /// </summary>
        /// <param name="cell">The cell to remove items from</param>
        public void Remove(Point cell)
        {
            lock (lockObject)
            {
                var c = Clamp(cell);
                List<T> l;
                Grid.TryGetValue(c, out l);

                if (l != null)
                {
                    foreach (var i in l)
                    {
                        List<Point> pl;
                        ItemDictionary.TryGetValue(i, out pl);
                        if (pl != null)
                        {
                            pl.Remove(c);
                            if (pl.Count == 0)
                            {
                                ListOfPointQueue.Enqueue(pl);
                                ItemDictionary.Remove(i);
                            }
                        }
                    }
                    l.Clear();
                    ListOfItemQueue.Enqueue(l);
                    Grid.Remove(cell);
                }
            }
        }

        /// <summary>
        ///     Removes all occurrences of the given item and re-adds it at the new given cell.
        ///     If the item hasn't been in the grid before, this will just add it.
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

        public bool IsEmpty(Point cell)
        {
            lock (lockObject)
            {
                return Get(cell).Length == 0;
            }
        }
    }
}