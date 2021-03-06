using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance = null;

    public bool isOn = false;
    [SerializeField] public string tagSpawnBlue {get;private set; } = "GroundBlue"; //??? ??????????? ?? ??????? ????? ??????? ?????
    [SerializeField] public string tagSpawnRed {get;private set; } = "GroundRed"; //??? ??????????? ?? ??????? ????? ??????? ?????
    [SerializeField] private GameObject objectNode;
    [SerializeField] private float shift = 0.01f;
    [SerializeField] private Units[] spawnMob;

    private Vector3 curPos = Vector3.zero;

    private NodeBuild node = null;

    private int indexCurrentMob = 0;
    private bool isSelectedMob = false;

    private void Awake()
    {
        if(instance != null) { Debug.LogError("Spawn manager completed is instance!!!");}
        instance = this;
    }

    private void Start()
    {
        objectNode = Instantiate(objectNode,curPos,Quaternion.identity);
        GameManager.instance.startGameFite+=StartFite;

        node = objectNode.GetComponent<NodeBuild>();
    }

    private void StartFite()
    {
        Destroy(objectNode);
        Destroy(this);
    }

    private void Update()
    {
        if (GameManager.instance.isPlay) { return;}
        if (EventSystem.current.IsPointerOverGameObject()) {objectNode.SetActive(false); return;}

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit))
        {
            
            if (hit.collider.CompareTag(tagSpawnBlue) || hit.collider.CompareTag(tagSpawnRed)) 
            {
                
                curPos = hit.point+hit.normal*shift;
            }
           
        }

        if (objectNode && isSelectedMob)
        {
            objectNode.SetActive(true);

            objectNode.transform.position = Vector3.Lerp(objectNode.transform.position,curPos,Time.deltaTime*10);
        }
        else
        {
           
            objectNode.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0) && isOn && isSelectedMob)
        {
            //????? ????
            
            if (hit.collider.CompareTag(tagSpawnRed))
            {
                if (PlayerStats.instance.Buy(spawnMob[indexCurrentMob].Price, false))
                {
                    var m = Instantiate(spawnMob[indexCurrentMob].mob, objectNode.transform.position, Quaternion.Euler(0, -90, 0));
                    var c = m.gameObject.GetComponent<AllMobs>();
                    if (c == null) { Debug.LogError("Error");}
                    c.SetMaterial(false);
                   
                }
            }
            else
            {
                if (PlayerStats.instance.Buy(spawnMob[indexCurrentMob].Price, true))
                {
                    var m = Instantiate(spawnMob[indexCurrentMob].mob, objectNode.transform.position, Quaternion.Euler(0,90,0));
                    var c = m.gameObject.GetComponent<AllMobs>();
                    if (c == null) { Debug.LogError("Error"); }
                    c.SetMaterial(true);
                    
                }
            }
           

        }

        if (Input.GetMouseButtonDown(1))
        {
            if(node == null) { return;}

            if (node.objectInNode)
            {
                var mob = node.objectInNode.GetComponent<AllMobs>();
                if (mob == null) { Debug.LogError("SpawnManager: Not completed mob!!!"); return;}

                if (hit.collider.CompareTag(tagSpawnRed))
                {
                    PlayerStats.instance.Sell(mob.unit.Price,false);
                }
                else
                {
                    PlayerStats.instance.Sell(mob.unit.Price, true);
                }
                
                node.objectInNode.transform.position = new Vector3(-1000,-1000,-1000);
                Destroy(node.objectInNode,0.1f);
                
            }
        }
    }

    
    public void SetObjects(int i)
    {
        if(spawnMob.Length <= 0 || i> spawnMob.Length || i<0) { isSelectedMob = false; return;}
        
        isSelectedMob = true;
        indexCurrentMob = i;
        
    }
}
