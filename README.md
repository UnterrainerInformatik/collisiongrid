[![NuGet](https://img.shields.io/nuget/v/CollisionGrid.svg?maxAge=2592000)](https://www.nuget.org/packages/CollisionGrid/)
 [![license](https://img.shields.io/github/license/unterrainerinformatik/collisiongrid.svg?maxAge=2592000)](http://unlicense.org)  [![Twitter Follow](https://img.shields.io/twitter/follow/throbax.svg?style=social&label=Follow&maxAge=2592000)](https://twitter.com/throbax)  

# General  

This section contains various useful projects that should help your development-process.  

This section of our GIT repositories is free. You may copy, use or rewrite every single one of its contained projects to your hearts content.  
In order to get help with basic GIT commands you may try [the GIT cheat-sheet][coding] on our [homepage][homepage].  

This repository located on our  [homepage][homepage] is private since this is the master- and release-branch. You may clone it, but it will be read-only.  
If you want to contribute to our repository (push, open pull requests), please use the copy on github located here: [the public github repository][github]  

# ![Icon](https://github.com/UnterrainerInformatik/collisiongrid/raw/master/icon.png)CollisionGrid

This class is a PCL library for MonoGame that implements a collision-grid.  
When doing game development you've probably come across a point when you'd liked to do some collision-checks and that's usually the time when you've realize that just checking all sprites against each other doesn't cut it.  
The problem with that brute-force-approach is, that the number of checks grow very fast (N² for N sprites) when the number of your sprites increase.  

So you somehow have to narrow down your collision-candidates.  
This piece of software does that for you. It does not do the collision checks themself. It just tells you if a sprite may be near enough to a second one to maybe collide with it, which allows you to do a collision test for those two, or three, or five sprites instead of the whole bunch.  

> **If you like this repo, please don't forget to star it.**
> **Thank you.**



## What It Really Does...

...is a simple trade-off.  
You may query it about the sprites around your coordinate or rectangle, but your sprites have to register with it and update their position/AABB on every update.
But all in all this is a lot faster than a simple brute-force check.  

## Getting Started
The first thing is: You have to set-up a grid. Usually you'd take your game-coordinate-system's bounds.
Then you have to tell the grid how many cells it has by setting the number of cells on the X and Y axis.

Then you may add your sprites or other objects to the grid by calling `Add(object, Point/Vector2/Rectangle/Rect)` or `Move(object, Point/Vector2/Rectangle/Rect)`. Move removes the item first if it was present on the grid.  

### Parameters
    The first parameter is your item. The grid is generic and there are no constraints for that.  
    The second parameter is always one of the following:
#### Point  
    This is an int-point.  
    By specifying this you tell the grid you mean the cell at exactly this position.  
#### Vector2  
    This is a float-vector.  
    By specifying this you tell the grid that you mean the cell that contains these game-coordinates.  
#### Rectangle  
    This is a basic int-rectangle.  
    It is not rotated and therefore axis-aligned. So it's an Axis-Aligned-Bounding-Box or AABB.  
    By specifying this you give the grid a rectangle in the cell-coordinate-system
    (0-numberOfCellsX, 0-numberOfCellsY).  
#### Rect  
    This is a special parameter in our utility-package. It's essentially a Rectangle, but with all float parameters.  
    By specifying this you give the grid a rectangle in the game-coordinate-system.  

All rectangles this grid works with are axis-aligned.  

You're free to remove them at any time by using one of the remove-methods `Remove(Point/Vector2/Rectangle/Rect)`.

The method `Get(Point/Vector2/Rectangle/Rect)` returns an array of your items with all of them that the grid has encountered in the area you've specified when calling `Get`. If it doesn't find any it returns an empty array.

If you add your sprites by their position, then this is what the grid will basically do:  
![Position Test][testposition]

If you add your sprites by their AABBs, then this is what the grid will basically do:  
![Rectangle Test][testrectangle]

#### Example  

Set up the collision-grid:
```csharp
public CollisionGrid<Sprite> Grid;

public DrawableGrid(Game game, SpriteBatch spriteBatch,
                    float width, float height, int numberOfCellsX,
                    int numberOfCellsY) : base(game)
{
	Grid = new CollisionGrid<Sprite>(width, height, numberOfCellsX, numberOfCellsY);
}
```
Place your sprites on to the grid in your update-method:
```csharp
public override void Update(GameTime gameTime)
{
	base.Update(gameTime);
	foreach (Sprite s in sprites)
	{
		Grid.Move(s, s.Position);
	}
}
```
The move-method adds an item on the given position (cell that encapsulates the given game-coordinate).
This is the code to achieve the output in the first GIF.  

To achieve the output in the second one, just change the Move-line to the following one:  
```csharp
        Grid.Move(s, s.getAABB());
```
Whereas of course your sprite has to return the axis-aligned-bounding-box as a Rect.

Please don't forget to clean up afterwards. There are a few data-structures the grid has to dispose of:  
```csharp
protected override void UnloadContent()
{
	base.UnloadContent();
	Grid.Dispose();
}
```

## So What's A QuadTree Then?
Maybe you've heard of such a data-structure that does essentially exactly the same things as this grid with one major difference:  

The QuadTree divides the space all by itself, dynamically whenever you add new items.  
It doesn't need a fixed uniform grid, but divides space unevenly and only when another partition is needed.  
And that's good and bad at the same time.
The good thing is that it can cope with unevenly distributed items very well.
The bad thing is that it costs a lot more time (the updating of this data-structure); At least when compared to this grid here.  
[Here's a very good implementation of a QuadTree on GitHub with an excellent explanation what it exactly does.][quadtree]  

The good news about the QuadTree is that it's exactly what you're looking for if you are thinking...
> Oh! Nice thing this grid. But the space I have to check is REALLY BIG and my sprites are very unevenly distributed. Most of the time they are clustered with much space in between those clusters. So my cells become way too big to segment those clusters in a helpful way.

...when reading the explanation of the CollisionGrid.


[homepage]: http://www.unterrainer.info
[coding]: http://www.unterrainer.info/Home/Coding
[github]: https://github.com/UnterrainerInformatik/collisiongrid
[quadtree]: https://github.com/ChevyRay/QuadTree
[testrectangle]: https://github.com/UnterrainerInformatik/collisiongrid/blob/master/testrectangle.gif
[testposition]: https://github.com/UnterrainerInformatik/collisiongrid/blob/master/testposition.gif
