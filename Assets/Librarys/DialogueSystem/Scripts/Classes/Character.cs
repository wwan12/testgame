using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueManager.Models;

/// <summary>
/// 可以在Unity编辑器中创建可编写字符脚本的对象
/// </summary>
[CreateAssetMenu( fileName = "New Character", menuName = "对话角色" )]
public class Character : ScriptableObject
{
    /// <summary> Name of the <see cref="Character"/> </summary>
    public string Name;

    /// <summary> List of <see cref="Expression"/> of the <see cref="Character"/>. </summary>
    public List<Expression> Expressions;

    /// <summary> Sound that will be played each time a letter or character is added to the dialogue display </summary>
    public AudioClip Voice;
}