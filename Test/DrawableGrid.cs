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

using System.Collections.Generic;
using CollisionGrid;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utilities;
using Utilities.Geometry;
using Utilities.Randomizing;

namespace Test
{
	internal class DrawableGrid : DrawableGameComponent
	{
		private const int NUMBER_OF_SPRITES = 50;

		public CollisionGrid<Sprite> Grid;
		public Vector2 Position { get; set; }
		public SpriteBatch SpriteBatch { get; set; }

		private readonly List<Sprite> sprites = new List<Sprite>(); 
		private readonly float width;
		private readonly float height;

		public DrawableGrid(Game game, SpriteBatch spriteBatch, float width, float height, int numberOfCellsX, int numberOfCellsY) : base(game)
		{
			SpriteBatch = spriteBatch;
			this.width = width;
			this.height = height;
			Grid = new CollisionGrid<Sprite>(width, height, numberOfCellsX, numberOfCellsY);
		}

		public override void Initialize()
		{
			base.Initialize();
			IRandomNumberGenerator rand = RandomizerController.Randomizer;
			for (int i = 0; i < NUMBER_OF_SPRITES; i++)
			{
				Sprite s = new Sprite(Game, SpriteBatch, new Point((int)width, (int)height));
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
			foreach (Sprite s in sprites)
			{
				Grid.Move(s, s.Position);
			}
		}

		public override void Draw(GameTime gameTime)
		{
			if (Visible)
			{
				SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
				Rectangle r = new Rectangle((int)Position.X, (int)Position.Y, (int)width, (int)height);			
				SpriteBatch.DrawRectangle(r, Color.Yellow);
				for (int x = 0; x < Grid.NumberOfCellsX; x++)
				{
					for (int y = 0; y < Grid.NumberOfCellsY; y++)
					{
						Rectangle cell = new Rectangle((int) (Position.X + x*Grid.CellWidth), (int) (Position.Y + y*Grid.CellHeight),
							(int) Grid.CellWidth, (int) Grid.CellHeight);
						int l = Grid.Get(new Point(x, y)).Length;
						if (l > 0)
						{
							float f = l/4f;
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