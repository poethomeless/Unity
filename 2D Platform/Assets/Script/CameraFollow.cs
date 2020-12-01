using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //相机跟随对象target
    public Transform target;
    //平滑因子
    public float smoothing;

    public Vector2 minPostion;
    public Vector2 maxPostion;

    // Start is called before the first frame update
    void Start()
    {
        GameController.camShake = GameObject.FindGameObjectWithTag("CameraShake").GetComponent<CameraShake>();
    }
    private void LateUpdate()
    {
        if (target != null) 
        {
            if (transform.position != target.position) 
            {
                Vector3 targetPos = target.position;
                targetPos.x = Mathf.Clamp(targetPos.x, minPostion.x, maxPostion.x);
                targetPos.y = Mathf.Clamp(targetPos.y, minPostion.y, maxPostion.y);
                //线性差值函数（起点坐标，终点坐标，起点去终点坐标时间）
                transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
            }
        }
    }
    public void SetCamPosLimit(Vector2 minPos, Vector2 maxPos)
    {
        minPostion = minPos;
        maxPostion = maxPos;
    }
}
