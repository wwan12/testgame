using System;
using UnityEngine;
using AI;
using System.Collections.Generic;
using System.Collections;

namespace Deftly
{
    public class Agent : MonoBehaviour
    {
        public float remainingDistance;
        public float stoppingDistance;
        public Vector3 desiredVelocity;
        public Vector3 destination;
        public bool isPathStale;
        public float speed;

        private SpatialAStar<MyPathNode, System.Object> aStar;
        private LinkedList<MyPathNode> path;
        private Rigidbody2D _rb;
        private bool isRunning = false;

        public void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Start()
        {
            RefreshMap();
        }

        public void RefreshMap()
        {
            MyPathNode[,] grid = new MyPathNode[100, 100];

            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    Boolean isWall = ((y % 2) != 0) && (UnityEngine.Random.Range(0, 10) != 8);

                    grid[x, y] = new MyPathNode()
                    {
                        IsWall = false,
                        X = x,
                        Y = y,
                    };
                }
            }

            aStar = new SpatialAStar<MyPathNode, System.Object>(grid);
        }

        public void CalculatePath()
        {
            Vector2Int pos = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));
            Vector2Int target = new Vector2Int(Mathf.FloorToInt(destination.x), Mathf.FloorToInt(destination.y));
            path = aStar.Search(pos.x, pos.y, target.x, target.y, null);
        }

        public void RotateTowards(Vector3 target)
        {
            Vector2 dir = target - transform.position;
            Quaternion fin = Quaternion.Euler(0, 0, (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg));
            transform.rotation = Quaternion.Slerp
                        (transform.rotation, fin, Time.deltaTime * 5);
        }

        public void Resume()
        {
            if(!isRunning)
                StartCoroutine(SimpleMove());
        }

        public void Stop()
        {
            StopCoroutine(SimpleMove());
            isRunning = false;
        }

        public IEnumerator MoveAlongNodes()
        {
            isRunning = true;
            foreach (var node in path)
            {
                Vector2 target = new Vector2(node.X, node.Y);
                Vector2 position = new Vector2(transform.position.x, transform.position.y);
                while (Mathf.Abs(target.x - position.x) > 0.1f && Mathf.Abs(target.y - position.y) > 0.1f)
                {
                    _rb.AddRelativeForce((target - position).normalized);
                }
                Debug.Log("Moving to " + target.ToString());
                yield return true;
            }
            isRunning = false;
        }

        public IEnumerator SimpleMove()
        {
            Vector2 target = destination;
            Vector2 position = new Vector2(transform.position.x, transform.position.y);
            while (Mathf.Abs(target.x - position.x) > 1f && Mathf.Abs(target.y - position.y) > 1f)
            {
                if(_rb.velocity.magnitude<1)
                    _rb.AddRelativeForce((target - position).normalized);

                yield return null;
            }
        }
    }

    public class MyPathNode : IPathNode<System.Object>
    {
        public Int32 X { get; set; }
        public Int32 Y { get; set; }
        public Boolean IsWall { get; set; }

        public bool IsWalkable(System.Object unused)
        {
            return !IsWall;
        }
    }
}