using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveHole : MonoBehaviour
{
    private Touch touch;
    private float dragHoleSpeed;
    public bool isControllerActive;
    private float camSpeed;
    private float holePosZ;

    Vector3 clampPosition;

    private void Start()
    {
        isControllerActive = true;
        Game.firstStage = true;
        dragHoleSpeed = 0.0075f;
    }

    private void Update()
    {
        
        // Movement Restrictions 
        if (Game.firstStage)
        {
            clampPosition = transform.position;
            clampPosition.x = Mathf.Clamp(clampPosition.x, -3.5f, 3.5f);
            clampPosition.z = Mathf.Clamp(clampPosition.z, -5.5f, 5.5f);
            transform.position = clampPosition;

        }
        else if (Game.secondStage){

            clampPosition = transform.position;
            clampPosition.x = Mathf.Clamp(clampPosition.x, -3.5f, 3.5f);
            clampPosition.z = Mathf.Clamp(clampPosition.z, 18.5f, 29.5f);
            transform.position = clampPosition;

        }
        

        // Hole's Move With Touch
        if (Input.touchCount > 0 && isControllerActive)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                transform.position = new Vector3
                                        (
                                        transform.position.x + touch.deltaPosition.x * dragHoleSpeed,
                                        transform.position.y,
                                        transform.position.z + touch.deltaPosition.y * dragHoleSpeed
                                        );
            }
        }

        // Stage-One to Stage-Two Movement (Hole and Camera)
        if (GameManager.gM.isMovingToCenter)
        {
            transform.position = Vector3.MoveTowards(
                                        transform.position,
                                        new Vector3(0,transform.position.y, transform.position.z),
                                        3f * Time.deltaTime);
        }
        if (GameManager.gM.isMovingToTheSecondStage)
        {
            transform.position = Vector3.MoveTowards(
                            transform.position,
                            new Vector3(transform.position.x, transform.position.y, 19f),
                            3f * Time.deltaTime);

            Camera.main.transform.position = Vector3.MoveTowards(
                            Camera.main.transform.position,
                            new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 14f),
                            GameManager.gM.flexibleCamSpeed * Time.deltaTime);
        }
    }
}
