using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MyHP
{
    [SerializeField] private float forceImpulse = 2;

    private List<Rigidbody> rigidbodies = new List<Rigidbody>();
    private void Start()
    {
        AddRbOnTowerParts();
    }

    private void SearchChildren(Transform child)
    {
        if(child.childCount != 0)
        {
            for (int i = 0; i < child.childCount; i++)
            {
                SearchChildren(child.GetChild(i));
            }
        }
        else
        {
            var collider = child.gameObject.AddComponent<MeshCollider>();
            collider.convex = true;
            var rb = child.gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rigidbodies.Add(rb);
        }
    }
    private void AddRbOnTowerParts()
    {
        SearchChildren(transform);
    }

    public override void Death()
    {
        
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = false;

           
        }
        foreach (var rb in rigidbodies)
        {
            rb.AddForce(Vector3.up*forceImpulse,ForceMode.Impulse);

        }

        if (transform.childCount > 0) { 
        transform.GetChild(0).parent = null;
        Destroy(gameObject);
            }
    }
}
