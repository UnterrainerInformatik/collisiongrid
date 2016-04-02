```
/**************************************************************************
 * 
 * by Unterrainer Informatik OG.
 * This is free and unencumbered software released into the public domain.
 * Anyone is free to copy, modify, publish, use, compile, sell, or
 * distribute this software, either in source code form or as a compiled
 * binary, for any purpose, commercial or non-commercial, and by any
 * means.
 *
 * In jurisdictions that recognize copyright laws, the author or authors
 * of this software dedicate any and all copyright interest in the
 * software to the public domain. We make this dedication for the benefit
 * of the public at large and to the detriment of our heirs and
 * successors. We intend this dedication to be an overt act of
 * relinquishment in perpetuity of all present and future rights to this
 * software under copyright law.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
 * OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * For more information, please refer to <http://unlicense.org>
 * 
 * (In other words you may copy, use, change, redistribute and sell it without
 * any restrictions except for not suing me because it broke something.)
 * 
 ***************************************************************************/

```

# General  

This section contains various useful projects that should help your development-process.  

This section of our GIT repositories is free. You may copy, use or rewrite every single one of its contained projects to your hearts content.  
In order to get help with basic GIT commands you may try [the GIT cheat-sheet][coding] on our [homepage][homepage].  

This repository located on our  [homepage][homepage] is private since this is the master- and release-branch. You may clone it, but it will be read-only.  
If you want to contribute to our repository (push, open pull requests), please use the copy on github located here: [the public github repository][github]  

# CollisionGrid  

This class implements a collision-grid.  
When doing game development you've all come across a point when you'd like to do some collision-checks and that's usually the time when you realize that just checking all sprites against each other just doesn't cut it.  
The problem is that the number of checks grow very fast (N² for N sprites) when the number of your sprites grow.  
  
So you somehow have to narrow down your collision-candidates.
This piece of software does that for you. It does not do collision checking itself. It just tells you if a sprite is near enough to a second one to maybe collide which allows you to do a collision test for those two, or three, or five...
  
The first thing is: You have to set-up a grid. Usually you'd take your game-coordinate-system's bounds.
Then you have to tell the grid how many cells it has by setting the number of cells on the X and Y axis.
  
Then you may add your sprites or other objects to the grid by calling `Add(object, Point/Vector2/Rectangle/Rect)` or `Move(object, Point/Vector2/Rectangle/Rect)`. Move removes the item first if it was present on the grid.  
  
### Parameters
The first parameter is your item. The grid is generic and there are no constraints for that.  
The second parameter is always one of the following:
| Parameter | Description | Info |
|:----------|:------------|:-----|
|Point|This is an int-point.|By specifying this you tell the grid you mean the cell at exactly this position.|
|Vector2|This is a float-vector.|By specifying this you tell the grid that you mean the cell that contains these game-coordinates.|
|Rectangle|This is a basic int-rectangle. It is not rotated and therefore axis-alinged. So it's an Axis-Aligned-Bounding-Box or AABB.|By specifying this you give the grid a rectangle in the cell-coordinate-system (0-numberOfCellsX, 0-numberOfCellsY).|
|Rect|This is a special parameter in our utility-package. It's essentially a Rectangle, but with all float parameters.|By specifying this you give the grid a rectangle in the game-coordinate-system.|

All rectangles this grid works with are axis-aligned.  

You're free to remove them at any time by using one of the remove-methods `Remove(Point/Vector2/Rectangle/Rect)`.
  
The method `Get(Point/Vector2/Rectangle/Rect)` returns an array of your items with all of them that the grid has encountered in the area you've specified when calling `Get`. If it doesn't find any it returns an empty array.
  
![Position Test][testposition]
  
![Rectangle Test][testrectangle]

#### Example  
    
Set up the collision-grid:
```csharp
public CollisionGrid<Sprite> Grid;

public DrawableGrid(Game game, SpriteBatch spriteBatch, float width, float height, int numberOfCellsX, int numberOfCellsY) : base(game)
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
The move-method adds an item on the given position (cell that encapsulates the given game-coordinate)

[homepage]: http://www.unterrainer.info
[coding]: http://www.unterrainer.info/Home/Coding
[github]: https://github.com/UnterrainerInformatik/collisiongrid
[testrectangle]: https://github.com/UnterrainerInformatik/collisiongrid/blob/master/testrectangle.gif
[testposition]: https://github.com/UnterrainerInformatik/collisiongrid/blob/master/testposition.gif