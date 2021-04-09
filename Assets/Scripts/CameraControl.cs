using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public bool isMoveMouse = true;
    [Header("Parametors")]
    public float moveSpeed = 8f;
    public float board = 10f;
    public float scrollSpeed = 10f;

    [Space(4)]
    [Header("Limitions")]
    [Space(2)]
    [Header("Zoom")]
    public float maxY = 80f;
    public float minY = 10f;
    [Space(2)]
    [Header("Move")]
    public float maxMoveX = 0;
    public float minMoveX = -80f;
    public float maxMoveZ = 80;
    public float minMoveZ = -10;

    private bool isPause = true;

    void Update()
    {
        if(gameObject.transform.parent != null) { return;}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
            if (isPause) { 
            Cursor.lockState = CursorLockMode.Locked; 
            Cursor.visible = false;

            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            
        }



        if (Input.GetKey(KeyCode.W) || (Input.mousePosition.y >= Screen.height - board && isMoveMouse))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(KeyCode.S) || (Input.mousePosition.y <= board && isMoveMouse))
        {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.A) || (Input.mousePosition.x <= board && isMoveMouse))
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(KeyCode.D) || (Input.mousePosition.x >= Screen.width - board && isMoveMouse))
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime, Space.World);
        }

        //Zoom
        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        transform.Translate(Vector3.forward * scroll * scrollSpeed * Time.deltaTime * 100);

        //Limits
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.x = Mathf.Clamp(pos.x, minMoveX, maxMoveX);
        pos.z = Mathf.Clamp(pos.z, minMoveZ, maxMoveZ);
        transform.position = pos;

    }
}
