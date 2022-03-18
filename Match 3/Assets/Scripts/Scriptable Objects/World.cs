using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject 是一个可独立于类实例来保存大量数据的数据容器。
/// ScriptableObjects 的一个主要用例是通过避免重复值来减少项目的内存使用量。
/// 如果项目有一个预制件在附加的 MonoBehaviour 脚本中存储不变的数据，这将非常有用。每次实例化预制件时，都会产生单独的数据副本。
/// 这种情况下可以不使用该方法并且不存储重复数据，而是使用 ScriptableObject 来存储数据，然后通过所有预制件的引用访问数据。
/// 这意味着内存中只有一个数据副本。
/// </summary>
[CreateAssetMenu(fileName = "World", menuName = "World")]
public class World : ScriptableObject {

    public Level[] levels;
}
