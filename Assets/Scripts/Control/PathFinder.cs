using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Path.AI
{
    public struct PathResult
    {
        public Vector2Int[] path;
        public bool success;

        public PathResult(Vector2Int[] path, bool success)
        {
            this.path = path;
            this.success = success;
        }
    }


    public class PathFinder
    {

        private int GetDistance(Vector2 startNode, Vector2 endNode)
        {
            int x = Mathf.Abs((int)(startNode.x - endNode.x));
            int y = Mathf.Abs((int)(startNode.y - endNode.y));
            if (x > y)
            {
                return 10 * (x - y) + 14 * y;
            }
            else
            {
                return 10 * (y - x) + 14 * x;
            }
        }

      


        public static PathResult GetPath(MapManage manage, Vector2Int startPosition, Vector2Int endPosition)
        {
            if (manage.width< startPosition.x)
            {
                
            }
            MapTile start = manage.map[startPosition.x, startPosition.y];
            MapTile end = manage.map[endPosition.x,endPosition.y];
            bool success = false;
            Vector2Int[] path = new Vector2Int[0];

            if (!start.isWall && !end.isWall)
            {
                Queue<MapTile> openSet = new Queue<MapTile>();
               // SimplePriorityQueue<TileProperty> openSet = new SimplePriorityQueue<TileProperty>();
                HashSet<MapTile> closedSet = new HashSet<MapTile>();

               // openSet.Enqueue(start, start.CostF);
                openSet.Enqueue(start);
                while (openSet.Count > 0)
                {
                    MapTile current = openSet.Dequeue();
                    if (current == end)
                    {
                        success = true;
                        break;
                    }
                    closedSet.Add(current);
                    for (int i = 0; i < 8; i++)
                    {
                        //MapTile neighbour = Loki.map[current.position + DirectionUtils.neighbours[i]];
                        //if (neighbour == null || neighbour.blockPath || closedSet.Contains(neighbour))
                        //{
                        //    continue;
                        //}
                        //float neighbourCost = current.gCost + Distance(current.position, neighbour.position) + neighbour.pathCost;
                        //if (neighbourCost > neighbour.gCost || !openSet.Contains(neighbour))
                        //{
                        //    neighbour.gCost = neighbourCost;
                        //    neighbour.hCost = Distance(neighbour.position, end.position);
                        //    neighbour.parent = current;

                        //    if (!openSet.Contains(neighbour))
                        //    {
                        //        openSet.Enqueue(neighbour, neighbour.fCost);
                        //    }
                        //    else
                        //    {
                        //        openSet.UpdatePriority(neighbour, neighbour.fCost);
                        //    }
                        //}
                    }
                }
            }

            if (success)
            {
                path = PathFinder.CalcPath(start, end);
                success = path.Length > 0;
            }
            return new PathResult(path, success);
        }

        public static Vector2Int[] CalcPath(MapTile start, MapTile end)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            MapTile current = end;
            while (current != start)
            {
                path.Add(new Vector2Int(current.posX,current.posY));
               // current = current.parent;
            }
            Vector2Int[] result = path.ToArray();
            System.Array.Reverse(result);
            return result;
        }

        public float Distance(Vector2Int a, Vector2Int b)
        {
            if (
                Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) == 1
            )
            {
                return 1f;
            }

            if (
                Mathf.Abs(a.x - b.x) == 1 &&
                Mathf.Abs(a.y - b.y) == 1
            )
            {
                return 1.41121356237f;
            }

            return Mathf.Sqrt(
                Mathf.Pow((float)a.x - (float)b.x, 2) +
                Mathf.Pow((float)a.y - (float)b.y, 2)
            );
        }
    }
}
