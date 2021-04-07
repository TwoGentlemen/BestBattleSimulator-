using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleControlerAnim : MonoBehaviour
{
    private AllMobs mobs;
    void Start()
    {
        mobs = GetComponentInParent<AllMobs>();
    }

    public void Attack()
    {
        if(mobs == null) {Debug.LogError("No scripts AllMobs!!!"); return;}

        mobs.Attack();

    }
}
