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
using Utilities.Geometry;

namespace CollisionGrid
{
    public partial class CollisionGrid<T>
    {
        private readonly object lockObject = new object();

        public float Width { get; }
        public float Height { get; }

        public float CellWidth { get; }
        public float CellHeight { get; }

        public int NumberOfCellsX { get; }
        public int NumberOfCellsY { get; }

        private Dictionary<Point, List<T>> Grid { get; set; }
        private Dictionary<T, List<Point>> ItemDictionary { get; set; }
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

            ItemDictionary = new Dictionary<T, List<Point>>();
            Grid = new Dictionary<Point, List<T>>();

            ListOfPointQueue = new Queue<List<Point>>();
            ListOfItemQueue = new Queue<List<T>>();
        }

        public Point[] Get(T item)
        {
            lock (lockObject)
            {
                List<Point> pl;
                ItemDictionary.TryGetValue(item, out pl);
                if (pl == null)
                {
                    return new Point[0];
                }
                return pl.ToArray();
            }
        }

        /// <summary>
        ///     Removes the given item from the grid.
        /// </summary>
        /// <param name="item">The item to remove</param>
        public void Remove(T item)
        {
            lock (lockObject)
            {
                List<Point> pl;
                ItemDictionary.TryGetValue(item, out pl);
                if (pl == null)
                {
                    return;
                }

                foreach (var p in pl)
                {
                    RemoveFromGrid(item, p);
                }

                pl.Clear();
                ListOfPointQueue.Enqueue(pl);
                ItemDictionary.Remove(item);
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
                    ListOfItemQueue.Enqueue(tl);
                    Grid.Remove(cell);
                }
            }
        }

        public IEnumerable<T> Items => ItemDictionary.Keys;

        public IEnumerable<Point> OccupiedCells => Grid.Keys;

        public int OccupiedCellCount => ItemDictionary.Keys.Count;

        public int ItemCount => Grid.Keys.Count;

        private Rectangle Rectangle(Rect rect)
        {
            var tl = Cell(rect.TopLeft);
            var br = Cell(rect.BottomRight);
            var s = br - tl + new Point(1, 1);
            return new Rectangle(tl, s);
        }

        private Point Cell(Vector2 position)
        {
            return new Point((int) (position.X/CellWidth), (int) (position.Y/CellHeight));
        }

        private Rectangle Clamp(Rectangle rectangle)
        {
            var tl = Clamp(rectangle.Location);
            var br = Clamp(rectangle.Location + rectangle.Size - new Point(1, 1));
            var s = br - tl + new Point(1, 1);
            return new Rectangle(tl, s);
        }

        private Point Clamp(Point p)
        {
            var nx = p.X;
            if (nx >= NumberOfCellsX)
            {
                nx = NumberOfCellsX - 1;
            }
            if (nx < 0)
            {
                nx = 0;
            }

            var ny = p.Y;
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
                ItemDictionary.Clear();
                ItemDictionary = null;
            }
        }
    }
}