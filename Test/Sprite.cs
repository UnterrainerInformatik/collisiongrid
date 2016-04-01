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
using System.Data.Odbc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Utilities;
using Utilities.Geometry;

namespace Test
{
	internal class Sprite : DrawableGameComponent
	{
		public SpriteBatch SpriteBatch { get; set; }

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

		public Rect GetAABB()
		{
			return new Rect(Position.X - Width/2f, Position.Y - Height/2f, Width, Height);
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteBatch.Begin();
			SpriteBatch.FillRectangle(GetAABB().ToRectangle(), Color.White);
			SpriteBatch.End();
		}
	}
}