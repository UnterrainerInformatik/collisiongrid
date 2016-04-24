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
        private readonly List<Point> lop = new List<Point>();
        private readonly List<T> result = new List<T>();

        private void FillList(Rectangle aabb)
        {
            var r = Clamp(aabb);
            lop.Clear();
            for (var y = 0; y < r.Size.Y; y++)
            {
                for (var x = 0; x < r.Size.X; x++)
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
                foreach (var p in lop)
                {
                    foreach (var i in Get(p))
                    {
                        if (!result.Contains(i))
                        {
                            result.Add(i);
                        }
                    }
                }
                return result.ToArray();
            }
        }

        /// <summary>
        ///     Gets the first item encountered in the cells that are hit by the given Axis-Aligned-Bounding-Box.
        /// </summary>
        /// <param name="aabb">The Axis-Aligned-Bounding-Box given in int-cell-coordinates</param>
        /// <returns>
        ///     The item or default(T)
        /// </returns>
        public T First(Rectangle aabb)
        {
            lock (lockObject)
            {
                FillList(aabb);
                result.Clear();
                foreach (var p in lop)
                {
                    var content = First(p);
                    if (!content.Equals(default(T)))
                    {
                        return content;
                    }
                }
                return default(T);
            }
        }

        /// <summary>
        ///     Adds a given item to the cells that are hit by the given Axis-Aligned-Bounding-Box.
        ///     If the cell already contains the item, it is not added a second time.
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <param name="aabb">The Axis-Aligned-Bounding-Box given in int-cell-coordinates</param>
        public void Add(T item, Rectangle aabb)
        {
            lock (lockObject)
            {
                FillList(aabb);
                foreach (var p in lop)
                {
                    Add(item, p);
                }
            }
        }

        /// <summary>
        ///     Removes all items from the cells that are hit by the given Axis-Aligned-Bounding-Box.
        ///     If the items don't occupy another cell, they are removed as well.
        /// </summary>
        /// <param name="aabb">The Axis-Aligned-Bounding-Box given in int-cell-coordinates</param>
        public void Remove(Rectangle aabb)
        {
            lock (lockObject)
            {
                FillList(aabb);
                foreach (var p in lop)
                {
                    Remove(p);
                }
            }
        }

        /// <summary>
        ///     Removes all occurrences of the given item and re-adds it at the new cells that are hit by the given
        ///     Axis-Aligned-Bounding-Box.
        ///     If the item hasn't been in the grid before, this will just add it.
        /// </summary>
        /// <param name="item">The item to move</param>
        /// <param name="aabb">The Axis-Aligned-Bounding-Box given in int-cell-coordinates</param>
        public void Move(T item, Rectangle aabb)
        {
            lock (lockObject)
            {
                Remove(item);
                FillList(aabb);
                foreach (var p in lop)
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
                foreach (var p in lop)
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