using System;
using UnityEngine;

[Flags]
public enum ObjectTags
{
    None = 0,

    Items = 1,
    Camera = 2
}

public class MultiTags : MonoBehaviour
{
    public ObjectTags tags;
}