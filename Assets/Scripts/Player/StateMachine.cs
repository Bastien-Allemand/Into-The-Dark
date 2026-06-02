using System;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    enum PlayerState
    {
        walk,
        crouch,
        run,
        dead,   //really dead       this corpse died of stopping living.
        usePhone,
        animationLock
    };
    //  const expr void(*)(void*) de C#
    static readonly Action<object>[] handlers =
    {
        Walk,
        Crouch,
        Run,
        Dead,
        UsePhone,
        animationLock
    };

    static void Walk(object data)
    {

    }
    static void Crouch(object data)
    {

    }
    static void Run(object data)
    {

    }
    static void Dead(object data)
    {

    }
    static void UsePhone(object data)
    {

    }
    static void animationLock(object data)
    {

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
