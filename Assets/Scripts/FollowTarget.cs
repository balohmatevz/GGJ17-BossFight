using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform Target;

    public float DesiredDistance = 15f;

    float RotationAdjustment = 0f;

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Target != null)
        {
            float newZ = Target.position.z - DesiredDistance;
            Vector3 targetPos = new Vector3(Target.position.x, transform.position.y, newZ);
            transform.position = Vector3.Lerp(this.transform.position, targetPos, 2 * Time.deltaTime);

            if (GameController.obj.GameStage != GameController.GameStages.INTRODUCTION)
            {
                RotationAdjustment += 0.25f * Time.deltaTime;
                RotationAdjustment = Mathf.Min(RotationAdjustment, 1);
            }


            if (GameController.obj.GameStage == GameController.GameStages.STAGE_1 || GameController.obj.GameStage == GameController.GameStages.STAGE_2)
            {
                Quaternion thisRot = this.transform.rotation;
                Quaternion targetRot = Quaternion.Euler(40, 0, 0);
                transform.rotation = Quaternion.Lerp(thisRot, targetRot, RotationAdjustment * Time.deltaTime);
            }
            else if (GameController.obj.GameStage == GameController.GameStages.TRANSITION_TO_STAGE_2)
            {
                Quaternion thisRot = this.transform.rotation;
                this.transform.LookAt(Vector3.zero);
                Quaternion targetRot = this.transform.rotation;
                transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(-52, 53, -2), 5 * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(thisRot, targetRot, Time.deltaTime);
            }
        }
    }
}
