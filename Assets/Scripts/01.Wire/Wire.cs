using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class Wire : MonoBehaviour
{
    private LineRenderer lr;                                                        //线渲染器.

    private Transform collidersParent;                                              //碰撞体的父物体.

    private Transform startPos;                                                     //线起点.
    private Transform endPos;                                                       //线终点.
    private Vector3 controlPos;                                                     //控制点.

    private Vector3 tempStartPos;
    private Vector3 tempEndPos;

    [SerializeField]
    private float offset = 2.0f;                                                    //控制点偏移量.
    private int segment = 20;                                                       //线的段数.
    private float lineWidth = 0.2f;                                                 //线的宽度.

    private List<GameObject> colliders = new List<GameObject>();                     //碰撞器物体集合.
    private void Awake()
    {
        lr = gameObject.GetComponent<LineRenderer>();
        if (lr == null)
        {
            lr = gameObject.AddComponent<LineRenderer>();
        }

        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;

        collidersParent = transform.Find("Colliders");

        startPos = transform.Find("Start");
        endPos = transform.Find("End");
    }

    private void Update()
    {
        //只要位置改变就重新绘制曲线.
        if(tempStartPos != startPos.position || tempEndPos != endPos.position)
        {

            //计算曲线点.
            this.controlPos = CalcControlPos(startPos.position, endPos.position, offset);

            //绘制曲线.
            Vector3[] poses = DrawWire(this.controlPos, this.segment);

            //添加碰撞器.
            AttackCollider(poses, collidersParent, lineWidth);

            tempStartPos = startPos.position;
            tempEndPos = endPos.position;
        }

    }

    private void OnDrawGizmos()
    {
        if(endPos == null)
        {
            Awake();
        }
        //方向(由起始点指向终点)
        Vector3 dir = endPos.position - startPos.position;
        //取另外一个方向. 这里取向上.
        Vector3 otherDir = Vector3.up;

        //求平面法线.  注意otherDir与dir不能调换位置,平面的法线是有方向的,(调换位置会导致法线方向相反)
        //ps: 左手坐标系使用左手定则 右手坐标系使用右手定则 (具体什么是左右手坐标系这里不细说请Google)
        //unity中世界坐标使用的是左手坐标系,所以法线的方向应该用左手定则判断.
        Vector3 planeNormal = Vector3.Cross(otherDir, dir);

        //再求startPos与endPos的垂线. 其实就是再求一次叉乘.
        Vector3 vertical = Vector3.Cross(dir, planeNormal).normalized;
        //中点.
        Vector3 centerPos = (startPos.position + endPos.position) / 2f;
        //控制点.
        Vector3 controlPos = centerPos + vertical * offset;

        Gizmos.color = Color.white;
        Gizmos.DrawLine(startPos.position, endPos.position);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(centerPos, controlPos + planeNormal.normalized * 5);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(startPos.position, startPos.position + otherDir.normalized * 5);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(centerPos, centerPos + vertical.normalized * 5);
    }

    /// <summary>
    /// 获取控制点.
    /// </summary>
    /// <param name="startPos">起点.</param>
    /// <param name="endPos">终点.</param>
    /// <param name="offset">偏移量.</param>
    private Vector3 CalcControlPos(Vector3 startPos, Vector3 endPos, float offset)
    {
        //方向(由起始点指向终点)
        Vector3 dir = endPos - startPos;
        //取另外一个方向. 这里取向上.
        Vector3 otherDir = Vector3.up;

        //求平面法线.  注意otherDir与dir不能调换位置,平面的法线是有方向的,(调换位置会导致法线方向相反)
        //ps: 左手坐标系使用左手定则 右手坐标系使用右手定则 (具体什么是左右手坐标系这里不细说请Google)
        //unity中世界坐标使用的是左手坐标系,所以法线的方向应该用左手定则判断.
        Vector3 planeNormal = Vector3.Cross(otherDir, dir);

        //再求startPos与endPos的垂线. 其实就是再求一次叉乘.
        Vector3 vertical = Vector3.Cross(dir, planeNormal).normalized;
        //中点.
        Vector3 centerPos = (startPos + endPos) / 2f;
        //控制点.
        Vector3 controlPos = centerPos + vertical * offset;

        return controlPos;
    }

    /// <summary>
    /// 绘制曲线.
    /// </summary>
    private Vector3[] DrawWire(Vector3 controlPos, int segments)
    {
        Vector3[] bezierPoses = BezierUtils.GetBeizerList(startPos.position, controlPos, endPos.position, segments);

        lr.positionCount = bezierPoses.Length;
        for (int i = 0; i <= bezierPoses.Length - 1; i++)
        {
            lr.SetPosition(i, bezierPoses[i]);
        }
        return bezierPoses;
    }

    /// <summary>
    /// 给线添加碰撞体.
    /// </summary>
    /// <param name="poses">点集合.</param>
    /// <param name="colls">碰撞器的父物体.</param>
    /// <param name="radius">半径.</param>
    private void AttackCollider(Vector3[] poses, Transform colls, float radius)
    {
        Vector3 lastPos = poses[0];
        for (int i = 1; i < poses.Length; i++)
        {
            Vector3 nextPos = poses[i];

            GameObject colliderObj = null;
            if (i <= colliders.Count-1)
            {
                colliderObj = colliders[i - 1];
            }
            else
            {
                colliderObj = new GameObject();
                colliders.Add(colliderObj);
            }
            
            colliderObj.name = (i - 1).ToString();
            colliderObj.transform.parent = colls;
            colliderObj.transform.forward = (nextPos - lastPos).normalized;

            CapsuleCollider coll = colliderObj.GetComponent<CapsuleCollider>();
            if(coll == null)
            {
                coll = colliderObj.AddComponent<CapsuleCollider>();
            }
            Vector3 center = (lastPos + nextPos) / 2f;

            //设置胶囊体参数.
            colliderObj.transform.position = center;
            coll.center = Vector3.zero;
            coll.radius = radius;
            coll.height = Vector3.Distance(lastPos, nextPos);
            coll.direction = 2;                         //0-X 1-Y 2-Z
            coll.tag = "Wire";

            lastPos = nextPos;
        }
    }
}
