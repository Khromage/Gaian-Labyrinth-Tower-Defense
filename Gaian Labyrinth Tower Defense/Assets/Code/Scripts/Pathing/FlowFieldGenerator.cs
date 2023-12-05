using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlowFieldGenerator
{
    private List<GridTile> prevBottlenecks = new List<GridTile>();

    private GameObject[] fullTileSet = GameObject.FindGameObjectsWithTag("GridTile");

    private int useCounter = 0;


    //Calculate distance from goal of each tile, as well as setting their predecessor and successor tiles, starting from the goal tile itself.
    //initDist is 0 when we are doing an initial calculation from the goal, and a different int if we are recalculating somewhere else in the map
    public void GenerateField(GridTile goal, int initDist)
    {
        List<GridTile> frontierQueue = new List<GridTile>();
        //list of calculated tiles, sorted by their distance from the goal.
        List<GridTile> sortedTileList = new List<GridTile>();

        for (int i = 0; i < fullTileSet.Length; i++)
        {
            fullTileSet[i].GetComponent<GridTile>().goalDist = int.MaxValue;
            if (!fullTileSet[i].GetComponent<GridTile>().walkable)
            {
                fullTileSet[i].GetComponent<GridTile>().goalDistText.text = $"";
            }
        }

        goal.goalDist = initDist;
        frontierQueue.Add(goal);

        int count = 0;

        while (frontierQueue.Count > 0 && count < 1100)
        {
            count++;
            //Debug.Log(count);
            //GridTile currentTile = frontierQueue.OrderBy(x => x.F).First();    
            //if we end up giving tiles certain effects, like speed up and slow down, etc. (tower aoe slows won't count for pathing purposes)

            //Debug.Log($"Frontier Queue start of loop:\n {printQueue(frontierQueue)}");

            GridTile currentTile = frontierQueue[0];
            frontierQueue.RemoveAt(0);

            sortedTileList.Add(currentTile);
            currentTile.predecessorList.Clear();

            //update dist values, push frontier queue along
            foreach (GridTile adjTile in currentTile.adjacentTiles)
            {
                if (adjTile.walkable && adjTile.goalDist > currentTile.goalDist && !adjTile.fielded) //walkable and not yet calculated (or needs to be recalculated)
                {
                    //current tile becomes a successor of the adjacent tile
                    adjTile.successor = currentTile;
                    
                    /*
                    //current tile becomes a successor of the adjacent tile
                    if (initDist == 0)
                        adjTile.successor = currentTile;
                    //or set up a function to take the minimum of all 4 adjacents as its successor?
                    else if (initDist > 0)
                    {
                        Debug.Log("recalcing successor");
                        adjTile.successor = adjTile.recalcSuccessor();
                    }
                    */

                    //setting distance from goal of adjTile based on current tile's distance. Assuming distance between each tile is 1 for now.
                    adjTile.goalDist = adjTile.successor.goalDist + 1;

                    //adjTile.goalDist += useCounter;

                    //adjacent tile becomes a predecessor of this tile
                    currentTile.predecessorList.Add(adjTile);

                    //setting up adjacent tile to be calculated soon
                    frontierQueue.Add(adjTile);
                    adjTile.fielded = true;
                }
                else
                {
                    //Debug.Log($"didn't add adj tile: {adjTile}");
                }
            }
        }

        foreach (GridTile t in sortedTileList)
        {
            t.fielded = false;
            //Debug.Log($"tile goal dist pre increment: {t.goalDist}");
            //t.goalDist += useCounter;
            //Debug.Log($"tile goal dist post increment: {t.goalDist}");
            t.goalDistText.text = $"{t.goalDist.ToString()}";
        }
        Debug.Log($"generated use counter {useCounter}");
        useCounter++;
        Debug.Log($"Sorted tile list length: {sortedTileList.Count}");
        Debug.Log($"end queue: {printQueue(frontierQueue)}");
        determineBottlenecks(sortedTileList);
    }

    //Adjustment
    //check if it's the only tile of that distance with a predecessor
    //can have others at that distance if they don't have predecessors
    //if predecessorCount > 0 and all others at that distance have preecessorCount = 0, then you're a bottleneck

    //determine all tiles where placing a tower on them would cut off all possible paths for enemies.
    private void determineBottlenecks(List<GridTile> sortedTileList)
    {
        //reset bottlenecks from previous flow field calculation
        foreach (GridTile b in prevBottlenecks)
        {
            b.placeable = true;
            prevBottlenecks.Remove(b);
        }

        int i = 0;
        while (sortedTileList[i].goalDist == 0 && i < sortedTileList.Count)
            i++;
        if (i == sortedTileList.Count)
            Debug.Log("Something went very wrong in determining bottlenecks");
        for (int j = i; j < sortedTileList.Count - 1; j++)
        {
            //if the tile is placeable, and there are no other tiles of this distance, and this tile has a predecessor, this tile is a bottleneck.
            if (sortedTileList[j].placeable && sortedTileList[j].goalDist > sortedTileList[j - 1].goalDist && 
                sortedTileList[j].goalDist < sortedTileList[j+1].goalDist && 
                sortedTileList[j].predecessorList.Count > 0)
            {
                prevBottlenecks.Add(sortedTileList[j]);
                sortedTileList[j].placeable = false;
            }
        }

        //Debug.Log($"Bottlenecks: {printQueue(prevBottlenecks)}");
    }


    private string printQueue(List<GridTile> queue)
    {
        string queueStr = string.Empty;
        foreach (GridTile a in queue)
        {
            queueStr += a + " ";
        }
        return queueStr;
    }

}
