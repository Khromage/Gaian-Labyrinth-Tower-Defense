using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder
{
    
    public List<GridTile> FindPath(GridTile start, GridTile end)
    {
        List<GridTile> openList = new List<GridTile>();
        List<GridTile> closedList = new List<GridTile>();

        openList.Add(start);

        while (openList.Count > 0)
        {
            //current tile becomes the tile in openList (unvisited adjacents) with the lowest F value (dist from start + dist from end)
            GridTile currentTile = openList.OrderBy(x => x.F).First();

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            if (currentTile == end)
            {
                return GetFinishedList(start, end);
            }

            //sets each neighbor's F and adds them to the openList
            foreach (GridTile adjTile in currentTile.adjacentTiles)
            {
                //Debug.Log("adjTile " + adjTile);
                
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
        return new List<GridTile>();
    }

    //gets the distance assuming we're using purely cardinal directions, no diagonals/shortcuts
    //this might need to be a much more complex calculation. I'm skeptical it will account for dead-ends and such...
    private int GetManhattanDistance(GridTile start, GridTile adjTile)
    {
        return Mathf.Abs(start.Coords.x - adjTile.Coords.x)
            + Mathf.Abs(start.Coords.y - adjTile.Coords.y)
            + Mathf.Abs(start.Coords.z - adjTile.Coords.z);
    }

    //returns the final list of tiles to path through, using the chain of each tile's "previous" value from the end backwards
    private List<GridTile> GetFinishedList(GridTile start, GridTile end)
    {
        List<GridTile> finishedList = new List<GridTile>();

        GridTile currentTile = end;

        while (currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.previous;
        }

        finishedList.Reverse();

        return finishedList;
    }
}
