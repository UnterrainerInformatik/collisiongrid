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
//  Created: 2016-03-30
// ***************************************************************************

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utilities;
using Utilities.Randomizing;

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
			Rectangle? screenBounds = Utils.InitGraphicsMode(this, GraphicsDeviceManager, MIN_SCREEN_RESOLUTION_WIDTH,
				MIN_SCREEN_RESOLUTION_HEIGHT, false);
			if (screenBounds == null)
			{
				Console.Out.WriteLine("Severe error opening and initializing window.");
				Exit();
			}

			DrawableGrid grid = new DrawableGrid(this, new SpriteBatch(GraphicsDevice), 700, 700, 40, 40);
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