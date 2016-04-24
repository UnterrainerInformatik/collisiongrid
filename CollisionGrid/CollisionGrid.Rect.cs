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
        ///     Gets the first item encountered in the cells that are hit by the given Axis-Aligned-Bounding-Box.
        /// </summary>
        /// <param name="aabb">The Axis-Aligned-Bounding-Box given in int-cell-coordinates</param>
        /// <returns>
        ///     The item or default(T)
        /// </returns>
        public T First(Rect aabb)
        {
            lock (lockObject)
            {
                return First(Rectangle(aabb));
            }
        }

        /// <summary>
        ///     Adds a given item to the cells that are hit by the given Axis-Aligned-Bounding-Box.
        ///     If the cell already contains the item, it is not added a second time.
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
        ///     Removes all items from the cells that are hit by the given Axis-Aligned-Bounding-Box.
        ///     If the items don't occupy another cell, they are removed as well.
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
        ///     Removes all occurrences of the given item and re-adds it at the new cells that are hit by the given
        ///     Axis-Aligned-Bounding-Box.
        ///     If the item hasn't been in the grid before, this will just add it.
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

        public bool IsEmpty(Rect aabb)
        {
            lock (lockObject)
            {
                return IsEmpty(Rectangle(aabb));
            }
        }
    }
}