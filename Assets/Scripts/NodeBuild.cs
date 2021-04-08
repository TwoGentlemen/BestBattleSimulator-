using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBuild : MonoBehaviour
{
    [SerializeField] private Color NoBuild = Color.red;
    [SerializeField] private Color YesBuild = Color.red;

    [HideInInspector] public GameObject objectInNode = null;

    private Renderer redMaterial = null;
    

    private void Start()
    {
        redMaterial = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(SpawnManager.instance.tagSpawn))
        {
            SpawnManager.instance.isOn = false;
            redMaterial.material.color = NoBuild;
        }

        if (other.CompareTag(GameManager.instance.currentTeam))
        {
            objectInNode = other.gameObject;
        }
        else
        {
            objectInNode = null;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(SpawnManager.instance.tagSpawn))
        {
            SpawnManager.instance.isOn = true;
            redMaterial.material.color = YesBuild;
        }
        objectInNode = null;
    }

}
