using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder
{
    
    public List<GridTileScript> FindPath(GridTileScript start, GridTileScript end)
    {
        List<GridTileScript> openList = new List<GridTileScript>();
        List<GridTileScript> closedList = new List<GridTileScript>();

        openList.Add(start);

        while (openList.Count > 0)
        {
            //current tile becomes the tile in openList (unvisited adjacents) with the lowest F value (dist from start + dist from end)
            GridTileScript currentTile = openList.OrderBy(x => x.F).First();

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            if (currentTile == end)
            {
                return GetFinishedList(start, end);
            }

            //sets each neighbor's F and adds them to the openList
            foreach (var adjTile in currentTile.adjacentTiles)
            {
                //if not in the running for next tile on path, nothing happens
                if (adjTile.walkable && !closedList.Contains(adjTile)) //walkable and not already in path
                {
                    //set the F value for the adjTile (to determine it's relative "distance" to the end
                    adjTile.G = GetManhattanDistance(start, adjTile);
                    adjTile.H = GetManhattanDistance(end, adjTile);

                    //create a chain of previous tiles that we have "traversed" as our currentTiles
                    adjTile.previous = currentTile;

                    //
                    if (!openList.Contains(adjTile))
                    {
                        openList.Add(adjTile);
                    }
                }
            }
        }
        //don't care about this return. The important one is when currentTile == end
        return new List<GridTileScript>();
    }

    //gets the distance assuming we're using purely cardinal directions, no diagonals/shortcuts
    //this might need to be a much more complex calculation. I'm skeptical it will account for dead-ends and such...
    private int GetManhattanDistance(GridTileScript start, GridTileScript adjTile)
    {
        return Mathf.Abs(start.Coords.x - adjTile.Coords.x)
            + Mathf.Abs(start.Coords.y - adjTile.Coords.y)
            + Mathf.Abs(start.Coords.z - adjTile.Coords.z);
    }

    //returns the final list of tiles to path through, using the chain of each tile's "previous" value from the end backwards
    private List<GridTileScript> GetFinishedList(GridTileScript start, GridTileScript end)
    {
        List<GridTileScript> finishedList = new List<GridTileScript>();

        GridTileScript currentTile = end;

        while (currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.previous;
        }

        finishedList.Reverse();

        return finishedList;
    }
}
