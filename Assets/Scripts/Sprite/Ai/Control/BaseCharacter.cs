using Node.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 代表游戏中的任何活生生的角色
public abstract class BaseCharacter
{

    /* 运动 */
    public CharacterMovement movement { get; protected set; }

    /* Action AI (BTree) */
    public CharacterBrain brain { get; protected set; }

    /* Shortcut/getter for the position (from this.movement) */
    public Vector2Int position { get { return this.movement.position; } }

    public Material material { get; protected set; }

    /*简单字符的基础图形，可以覆盖或根本不使用 */
    // public GraphicInstance graphics { get; set; }

    /* Character name */
    public string name { get; protected set; }

    /* Base mesh for a simple character, , can be overwritten or not used at all */
    private Mesh _mesh;
    private Vector2 size;

    public BaseCharacter(Vector2Int position,Vector2 size)
    {
        this.size = size;
        this.movement = new CharacterMovement(position, this);
        this.brain = new CharacterBrain(this, this.GetBrainNode());
        this.name = this.SetName();

      //  if (this.def.graphics != null && this.def.graphics.textureName != string.Empty)
      //  { // 生成纹理
          //  this.graphics = GraphicInstance.GetNew(this.def.graphics);

      //  }

        QueueAiControl.tick.toAdd.Enqueue(this.Update);
    }

    /// Set name (this should be overwritten by childrens)
    public virtual string SetName()
    {
        return "Undefined " + Random.Range(1000, 9999);
    }

    /// Get the root BrainNode for the Action AI (this should be overwritten by childrens)
    public abstract BrainNodePriority GetBrainNode();

    /// Uddate stats, movement, AI
    public virtual void Update()
    {
        this.brain.Update();
    }

    /// Character default draw method
    public virtual void UpdateDraw()
    {
        if (material == null)
        {
            return;
        }

        if (_mesh == null)
        {
            _mesh = MeshPool.GetPlaneMesh(size );
        }

        Graphics.DrawMesh(
            _mesh,
            movement.visualPosition,
            Quaternion.identity,
            material,
            0
        );
    }
}