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
using Microsoft.Xna.Framework.Graphics;
using Utilities;
using Utilities.Geometry;

namespace Test
{
    internal class Sprite : DrawableGameComponent
    {
        public SpriteBatch SpriteBatch { get; }

        public Vector2 Position { get; set; }
        public Vector2 Trajectory { get; set; }
        public float Velocity { get; set; }

        public float Width { get; set; }
        public float Height { get; set; }

        private Point bounds;

        public Sprite(Game game, SpriteBatch spriteBatch, Point bounds) : base(game)
        {
            SpriteBatch = spriteBatch;
            this.bounds = bounds;
            Width = 11;
            Height = 11;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Position = Position + Trajectory*Velocity;

            if (Position.X <= 0)
            {
                Position = new Vector2(0, Position.Y);
                Trajectory = new Vector2(-Trajectory.X, Trajectory.Y);
            }
            if (Position.X >= bounds.X)
            {
                Position = new Vector2(bounds.X, Position.Y);
                Trajectory = new Vector2(-Trajectory.X, Trajectory.Y);
            }
            if (Position.Y <= 0)
            {
                Position = new Vector2(Position.X, 0);
                Trajectory = new Vector2(Trajectory.X, -Trajectory.Y);
            }
            if (Position.Y >= bounds.Y)
            {
                Position = new Vector2(Position.X, bounds.Y);
                Trajectory = new Vector2(Trajectory.X, -Trajectory.Y);
            }
        }

        public Rect GetAabb()
        {
            return new Rect(Position.X - Width/2f, Position.Y - Height/2f, Width, Height);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.FillRectangle(GetAabb().ToRectangle(), Color.White);
            SpriteBatch.End();
        }
    }
}