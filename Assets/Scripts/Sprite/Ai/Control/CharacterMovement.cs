using Node.AI;
using Path.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 角色移动
public class CharacterMovement
{
    /* 当改变方向时调用了回调*/
    public Action<MeshPool.Direction> onChangeDirection;

    /// <summary>
    /// 当前地块
    /// </summary>
    public Vector2Int position { get; private set; }

    /// <summary>
    /// 观看方向
    /// </summary>
    public MeshPool.Direction lookAt { get; private set; }

    /// <summary>
    /// 最终目的地
    /// </summary>
    public Vector2Int destination { get; private set; }
    private MapManage Map;

    /// <summary>
    /// 目前的位置
    /// </summary>
    public Vector3 visualPosition
    {
        get
        {
            return new Vector3(
                Mathf.Lerp(position.x, nextPosition.x, movementPercent),
                Mathf.Lerp(position.y, nextPosition.y, movementPercent),
                0
            );
        }
    }

    /// <summary>
    /// 当前位置和下一位置之间的移动百分比
    /// </summary>
    private float movementPercent;

    /// <summary>
    /// 下一个地块
    /// </summary>
    private Vector2Int nextPosition;

    /// <summary>
    /// 是否有目的地
    /// </summary>
    private bool hasDestination;

    /// <summary>
    /// 当前路径队列，平铺位置列表
    /// </summary>
    public Queue<Vector2Int> path { get; private set; }

    private float speed = .1f;

    private BaseCharacter character;

    public CharacterMovement(Vector2Int position, BaseCharacter character)
    {
        this.position = position;
        this.character = character;
        Map.characters.Add(this.character);
        this.ResetMovement();
    }

    // 检查视角方向是否相同如果不相同，调用onChangedDirection。
    private void UpdateLookAt(Vector2Int nextPos)
    {
        MeshPool.Direction original = this.lookAt;
        Vector2Int t = nextPos - this.position;

        if (t.x > 0)
        {
            lookAt = MeshPool.Direction.E;
        }
        else if (t.x < 0)
        {
            lookAt = MeshPool.Direction.W;
        }
        else if (t.y > 0)
        {
           lookAt = MeshPool.Direction.N;
        }
        else
        {
            lookAt = MeshPool.Direction.S;
        }

        if (lookAt != original && onChangeDirection != null)
        {
            onChangeDirection(lookAt);
        }
    }

    /// <summary>
    /// 检查是否有一条路径，如果没有，尝试获取一条路径。然后一个一个地向目的地移动。
    /// </summary>
    /// <param name="task"></param>
    public void Move(Task task)
    {
        if (hasDestination == false)
        {
            PathResult pathResult = PathFinder.GetPath(GameObject.FindObjectOfType<MapManage>(), this.position, task.targets.currentPosition);

            if (pathResult.success == false)
            {
                task.taskStatus = TaskStatus.Failed; 
                this.ResetMovement();
                return;
            }

            hasDestination = true;
            path = new Queue<Vector2Int>(pathResult.path);
            destination = task.targets.currentPosition;
        }
        //判断到最后目的地了吗
        if (destination == position)
        {
            ResetMovement();
            return;
        }

        if (position == nextPosition)
        {
            nextPosition = path.Dequeue();
            UpdateLookAt(nextPosition);
        }

        float distance = Distance(position,nextPosition);
       // float distanceThisFrame = _speed * map.f;路径成本
        movementPercent +=  distance/10;

        if (movementPercent >= 1f)
        {
            position = nextPosition;
            movementPercent = 0f;
        }
    }

    // 重置有关移动的所有数据（在路径末尾使用）
    private void ResetMovement()
    {
        destination = position;
        hasDestination = false;
        nextPosition = position;
        movementPercent = 0f;
        path = new Queue<Vector2Int>();
    }

    public  float Distance(Vector2Int a, Vector2Int b)
    {
        if (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) == 1)
        {
            return 1f;
        }

        if (Mathf.Abs(a.x - b.x) == 1 && Mathf.Abs(a.y - b.y) == 1)
        {
            return 1.41121356237f;
        }

        return Mathf.Sqrt(Mathf.Pow((float)a.x - (float)b.x, 2) +Mathf.Pow((float)a.y - (float)b.y, 2));
    }
}