using Node.AI;
using Path.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 角色移动
public class CharacterMovement
{
    /* 当改变方向时调用了回调*/
    public System.Action<MeshPool.Direction> onChangeDirection;

    /* Current tile */
    public Vector2Int position { get; protected set; }

    /* Direction we are looking at */
    public MeshPool.Direction lookingAt { get; protected set; }

    /* Final destination */
    public Vector2Int destination { get; protected set; }

    /* Current path queue, list of tile positions */
    public Queue<Vector2Int> path { get { return this._path; } }

    private MapManage Map;

    /* Character position on screen */
    public Vector3 visualPosition
    {
        get
        {
            return new Vector3(
                Mathf.Lerp(this.position.x, this._nextPosition.x, this._movementPercent),
                Mathf.Lerp(this.position.y, this._nextPosition.y, this._movementPercent),
                0
            );
        }
    }

    /* 当前位置和下一位置之间的移动百分比 */
    private float _movementPercent;

    /* Next tile */
    private Vector2Int _nextPosition;

    /* Do we have a destionation ? */
    private bool _hasDestination;

    /* 当前路径队列，平铺位置列表 */
    private Queue<Vector2Int> _path;

    /* Character speed. TODO: definine this using character.stats */
    private float _speed = .1f;

    /* Character */
    private BaseCharacter _character;

    public CharacterMovement(Vector2Int position, BaseCharacter character)
    {
        this.position = position;
        this._character = character;
        Map.characters.Add(this._character);
        this.ResetMovement();
    }

    // 检查lookingat是否相同如果不相同，调用onChangedDirection。
    private void UpdateLookingAt(Vector2Int nextPos)
    {
        MeshPool.Direction original = this.lookingAt;
        Vector2Int t = nextPos - this.position;

        if (t.x > 0)
        {
            this.lookingAt = MeshPool.Direction.E;
        }
        else if (t.x < 0)
        {
            this.lookingAt = MeshPool.Direction.W;
        }
        else if (t.y > 0)
        {
            this.lookingAt = MeshPool.Direction.N;
        }
        else
        {
            this.lookingAt = MeshPool.Direction.S;
        }

        if (this.lookingAt != original && this.onChangeDirection != null)
        {
            this.onChangeDirection(this.lookingAt);
        }
    }

    /// 检查我们是否有一条路径，如果没有，请尝试获取一条路径。然后一个一个地向目的地移动。
    public void Move(Task task)
    {
        if (this._hasDestination == false)
        {
            PathResult pathResult = PathFinder.GetPath(GameObject.FindObjectOfType<MapManage>(), this.position, task.targets.currentPosition);

            if (pathResult.success == false)
            {
                task.taskStatus = TaskStatus.Failed; // Maybe a special failed condition;
                this.ResetMovement();
                return;
            }

            this._hasDestination = true;
            this._path = new Queue<Vector2Int>(pathResult.path);
            this.destination = task.targets.currentPosition;
        }
        // Are we on our final destination 
        if (this.destination == this.position)
        {
            this.ResetMovement();
            return;
        }

        if (this.position == this._nextPosition)
        {
            this._nextPosition = this._path.Dequeue();
            this.UpdateLookingAt(this._nextPosition);
        }

        float distance = Distance(this.position, this._nextPosition);
       // float distanceThisFrame = this._speed * Loki.map[this.position].pathCost;路径成本
        this._movementPercent +=  distance/10;

        if (this._movementPercent >= 1f)
        {
          //  map.characters.Remove(this._character);
          //  Loki.map[this._nextPosition].characters.Add(this._character);
            this.position = this._nextPosition;
            this._movementPercent = 0f;
        }
    }

    // Reset all data about the movement (used at the end of a path)
    private void ResetMovement()
    {
        this.destination = this.position;
        this._hasDestination = false;
        this._nextPosition = this.position;
        this._movementPercent = 0f;
        this._path = new Queue<Vector2Int>();
    }

    public  float Distance(Vector2Int a, Vector2Int b)
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