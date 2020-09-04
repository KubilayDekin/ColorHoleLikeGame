using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerCollider : MonoBehaviour
{
    // This class is responsible for destroying objects and deliver the destructed object's information to the 
    // "GameManager" class that arranges game
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Unobtainable")
        {
            Time.timeScale = 0;
            GameManager.gM.RestartLevel();
        }
        else if(other.tag=="Obtainable")
        {

            Magnet.mgnt.ReleaseMagnetForce(other.attachedRigidbody);

            if(Game.firstStage || Game.secondStage)
            {
                GameManager.gM.CalculateFillAmount();
            }

            if (Game.firstStage)
            {
                GameManager.gM.stageOneObjs.Remove(other.gameObject);
                Destroy(other.gameObject);

                if (GameManager.gM.stageOneObjs.Count == 0)
                {
                    GameManager.gM.StageOneToStageTwo();
                }

            } else if (Game.secondStage)
            {
                GameManager.gM.stageTwoObj.Remove(other.gameObject);
                Destroy(other.gameObject);

                if (GameManager.gM.stageTwoObj.Count == 0)
                {
                    GameManager.gM.LoadNextLevel();
                }

            }
        }
    }
}
