using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 说明：视野对象基类
/// 
/// @by wsh 2017-05-20
/// </summary>

public abstract class ClassObjPoolBase
{
    private static List<ClassObjPoolBase> instanceList = new List<ClassObjPoolBase>();

    protected ClassObjPoolBase()
    {
    }

    public abstract void Release(FOWRevealer obj);
    protected abstract void OnDispose();

    protected static void AddInstance(ClassObjPoolBase instance)
    {
        instanceList.Add(instance);
    }

    public static void Dispose()
    {
        for (int i = 0; i < instanceList.Count; i++)
        {
            instanceList[i].OnDispose();
        }
        instanceList.Clear();
    }
}

public class FOWRevealer :IFOWRevealer
{
    // 共享数据
    protected bool m_isValid;
    protected Vector3 m_position;
    protected float m_radius;

    public bool bChkReset = true;
    public ClassObjPoolBase holder;
    public uint usingSeq;

    public virtual void OnRelease() {
        m_isValid = false;
    }

    public virtual void OnInit() {
        m_position = Vector3.zero;
        m_radius = 0f;
        m_isValid = false;
    }

    public virtual void Dispose() { }

    public virtual void Release()
    {
        this.OnRelease();
        this.holder.Release(this);
    }

    //static public FOWRevealer Get()
    //{
    //    return ClassObjPool<FOWRevealer>.Get();
    //}

    public virtual Vector3 GetPosition()
    {
        return m_position;
    }

    public virtual float GetRadius()
    {
        return m_radius;
    }

    public bool IsValid()
    {
        return m_isValid;
    }

    public virtual void Update(int deltaMS)
    {
        // 更新所有共享数据，m_isValid最后更新
    }

   
}
