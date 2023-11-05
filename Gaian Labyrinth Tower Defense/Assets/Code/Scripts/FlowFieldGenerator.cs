using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlowFieldGenerator
{
    //Calculate distance from goal of each tile, as well as setting their predecessor and successor tiles, starting from the goal tile itself.
    //initDist is 0 when we are doing an initial calculation from the goal, and a different int if we are recalculating somewhere else in the map
    public void GenerateField(GridTile goal, int initDist)
    {
        List<GridTile> frontierQueue = new List<GridTile>();
        //list of calculated tiles, sorted by their distance from the goal.
        List<GridTile> sortedTileList = new List<GridTile>();

        goal.goalDist = initDist;
        frontierQueue.Add(goal);

        int count = 0;

        while (frontierQueue.Count > 0 && count < 990)
        {
            count++;
            Debug.Log(count);
            //GridTile currentTile = frontierQueue.OrderBy(x => x.F).First();    
            //if we end up giving tiles certain effects, like speed up and slow down, etc. (tower aoe slows won't count for pathing purposes)

            GridTile currentTile = frontierQueue[0];
            frontierQueue.RemoveAt(0);

            sortedTileList.Add(currentTile);

            //update dist values, push frontier queue along
            foreach (GridTile adjTile in currentTile.adjacentTiles)
            {
                
                if (adjTile.walkable && adjTile.goalDist > currentTile.goalDist) //walkable and not yet calculated (or needs to be recalculated)
                {
                    Debug.Log(currentTile);
                    //setting distance from goal of adjTile based on current tile's distance. Assuming distance between each tile is 1 for now.
                    adjTile.goalDist = currentTile.goalDist + 1;

                    //current tile becomes a successor of the adjacent tile
                    adjTile.successor = currentTile;

                    //adjacent tile becomes a predecessor of this tile
                    currentTile.predecessorList.Add(adjTile);

                    //setting up adjacent tile to be calculated soon
                    frontierQueue.Add(adjTile);
                }
            }
        }
    }

}
