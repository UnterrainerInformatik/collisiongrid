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

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utilities;

namespace Test
{
    internal class Main : Game
    {
        public const int MIN_SCREEN_RESOLUTION_WIDTH = 1024;
        public const int MIN_SCREEN_RESOLUTION_HEIGHT = 768;

        public GraphicsDeviceManager GraphicsDeviceManager;

        public Main()
        {
            Content.RootDirectory = "Content";

            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            GraphicsDeviceManager.PreferredBackBufferWidth = MIN_SCREEN_RESOLUTION_WIDTH;
            GraphicsDeviceManager.PreferredBackBufferHeight = MIN_SCREEN_RESOLUTION_HEIGHT;
            GraphicsDeviceManager.IsFullScreen = false;
        }

        protected override void Initialize()
        {
            var screenBounds = Utils.InitGraphicsMode(this, GraphicsDeviceManager, MIN_SCREEN_RESOLUTION_WIDTH,
                MIN_SCREEN_RESOLUTION_HEIGHT, false);
            if (screenBounds == null)
            {
                Console.Out.WriteLine("Severe error opening and initializing window.");
                Exit();
            }

            var grid = new DrawableGrid(this, new SpriteBatch(GraphicsDevice), 700, 700, 40, 40);
            Components.Add(grid);
            grid.Initialize();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}