using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewProjectileData", menuName = "ScriptableObjects/ProjectileData", order = 1)]
public class ProjectileData : ScriptableObject
{
    public float speed = 5f;
}
