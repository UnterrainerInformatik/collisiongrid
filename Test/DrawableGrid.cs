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
using CollisionGrid;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utilities;
using Utilities.Randomizing;

namespace Test
{
    internal class DrawableGrid : DrawableGameComponent
    {
        private const int NUMBER_OF_SPRITES = 50;

        public CollisionGrid<Sprite> Grid;
        public Vector2 Position { get; set; }
        public SpriteBatch SpriteBatch { get; }

        private readonly List<Sprite> sprites = new List<Sprite>();
        private readonly float width;
        private readonly float height;

        public DrawableGrid(Game game, SpriteBatch spriteBatch, float width, float height, int numberOfCellsX,
            int numberOfCellsY) : base(game)
        {
            SpriteBatch = spriteBatch;
            this.width = width;
            this.height = height;
            Grid = new CollisionGrid<Sprite>(width, height, numberOfCellsX, numberOfCellsY);
        }

        public override void Initialize()
        {
            base.Initialize();
            var rand = RandomizerController.Randomizer;
            for (var i = 0; i < NUMBER_OF_SPRITES; i++)
            {
                var s = new Sprite(Game, SpriteBatch, new Point((int) width, (int) height));
                s.Trajectory = new Vector2(rand.RandomBetween(-1f, 1f), rand.RandomBetween(-1f, 1f));
                s.Trajectory.Normalize();
                s.Position = new Vector2(rand.RandomBetween(0f, 700f), rand.RandomBetween(0f, 700f));
                s.Velocity = rand.RandomBetween(.1f, 4f);
                s.Width = rand.RandomBetween(5, 53);
                s.Height = rand.RandomBetween(5, 53);

                s.Initialize();
                Game.Components.Add(s);
                sprites.Add(s);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (var s in sprites)
            {
                Grid.Move(s, s.GetAabb());
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
                var r = new Rectangle((int) Position.X, (int) Position.Y, (int) width, (int) height);
                SpriteBatch.DrawRectangle(r, Color.Yellow);
                for (var x = 0; x < Grid.NumberOfCellsX; x++)
                {
                    for (var y = 0; y < Grid.NumberOfCellsY; y++)
                    {
                        var cell = new Rectangle((int) (Position.X + x*Grid.CellWidth),
                            (int) (Position.Y + y*Grid.CellHeight),
                            (int) Grid.CellWidth, (int) Grid.CellHeight);
                        var l = Grid.Get(new Point(x, y)).Length;
                        if (l > 0)
                        {
                            var f = l/4f;
                            if (f > 1)
                            {
                                f = 1;
                            }
                            SpriteBatch.FillRectangle(cell, Utils.SetTransparencyOnColor(Color.Red, f));
                        }
                        else
                        {
                            SpriteBatch.DrawRectangle(cell, Utils.SetTransparencyOnColor(Color.Yellow, .5f));
                        }
                    }
                }
                SpriteBatch.End();
            }
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            Grid.Dispose();
        }
    }
}