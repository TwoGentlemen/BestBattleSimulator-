using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gigant : AllMobs
{
    [Space(5)]
    [SerializeField] private Transform cameraPoint;
    private bool isPlayerControl = false;
    private float bufTime = 0;
    private Transform mainCamera;
    private Vector3 startPosCamera; 
    private Quaternion startRotCamera; 

    public override void StartGame()
    {
        base.StartGame();
        gameObject.layer = 0;
        mainCamera = Camera.main.transform;
    }
    public override void Move()
    {
        if (!isPlayerControl) 
        {
            agent.isStopped = false;
            base.Move();
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                isPlayerControl = false;
                gameObject.layer = 0;

                mainCamera.transform.parent = null;

                mainCamera.position = startPosCamera;
                mainCamera.rotation = startRotCamera;
                return;
            }

            agent.isStopped = true;

            MoveRotation();

            var moveX = Input.GetAxisRaw("Horizontal");
            var moveZ = Input.GetAxisRaw("Vertical");

            Vector3 dir = new Vector3(moveX,0,moveZ);
            if(dir != Vector3.zero) { 
            animator.SetBool("walk",true);
            transform.Translate(dir.normalized*Time.deltaTime*8);
            }
            else
            {
                animator.SetBool("walk", false);
            }
           
            bufTime+=Time.deltaTime;

            if (Vector3.Distance(target.position,transform.position) <= rangeAttack && bufTime>= timeReload)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    animator.Play("BomjAttack");
                    bufTime=0;
                }
                
            }else
            {
                animator.SetBool("attack", false);
            }

            
            
        }
    }
    private void MoveRotation()
    {
        if(cameraPoint == null) { return;}

        var moveY = Input.GetAxis("Mouse Y");
        var moveX = Input.GetAxis("Mouse X");

        mainCamera.transform.parent = cameraPoint.transform;

        transform.Rotate(0,moveX*Time.deltaTime*100,0);
        cameraPoint.Rotate(moveY * Time.deltaTime * 100,0,0);
        mainCamera.transform.rotation = cameraPoint.rotation;
        mainCamera.transform.position = cameraPoint.transform.position-cameraPoint.forward*30;


    }

    private void OnMouseUp()
    {
        startPosCamera = mainCamera.position;
        startRotCamera = mainCamera.rotation;

        isPlayerControl =true;
        gameObject.layer = 2;

       
    }

    public override void Death()
    {
        mainCamera.transform.parent = null;

        mainCamera.position = startPosCamera;
        mainCamera.rotation = startRotCamera;

        base.Death();
    }
}
