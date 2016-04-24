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