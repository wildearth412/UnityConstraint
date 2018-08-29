using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constraint;

public class DistanceConstraint : ConstraintBase
{    
    [SerializeField]
    private GameObject objTarget2;
    private Vector3 offset2;
    [SerializeField]
    private bool useCustomOffset;
    [SerializeField]
    private float customOffset;
    [SerializeField]
    private float customOffset2;
    [SerializeField]
    private Vector3 poleVector = Vector3.up;
    [SerializeField]
    private bool poleConstraint;
    public GameObject poleTarget;
    private Vector3 posA;
    private Vector3 posB;
    private Vector3 vecAB;
    private float disAB;

    override protected void Awake()
    {
        Vector3 p1 = transform.position;
        Vector3 p2 = objTarget.transform.position;
        offset = p1 - p2;
        Vector3 p3 = objTarget2.transform.position;
        offset2 = p1 - p3;
        //Debug.Log(offset.magnitude + "  " + offset2.magnitude);
    }

    private void Start()
    {
        ApplyDistanceConstraint();
    }

    private void Update()
    {
        if ((objTarget != null && objTarget2 != null) && !isOneShot)
        {
            ApplyDistanceConstraint();
        }
    }

    public void ApplyDistanceConstraint()
    {
        posA = objTarget.transform.position;
        posB = objTarget2.transform.position;
        vecAB = posB - posA;
        disAB = vecAB.magnitude;

        int status = (useCustomOffset ? 2 : 3) * (poleConstraint ? 5 : 7);
        switch(status)
        {
            case 10:
                bool s1 = CheckLinear(customOffset, customOffset2);
                if (s1) { return; }
                else
                {
                    ApplyPoleConstraint(customOffset, customOffset2, true);
                }
                break;
            case 14:
                bool s2 = CheckLinear(customOffset, customOffset2);
                if (s2) { return; }
                else
                {
                    ApplyPoleConstraint(customOffset, customOffset2, false);
                }
                break;
            case 15:
                bool s3 = CheckLinear(offset.magnitude, offset2.magnitude);
                if (s3) { return; }
                else
                {
                    ApplyPoleConstraint(offset.magnitude, offset2.magnitude, true);
                }
                break;
            case 21:
                bool s4 = CheckLinear(offset.magnitude, offset2.magnitude);
                if (s4) { return; }
                else
                {
                    ApplyPoleConstraint(offset.magnitude, offset2.magnitude, false);
                }
                break;
        }
    }

    private bool CheckLinear(float d1,float d2)
    {
        bool result = false;

        float dis1, dis2;
        dis1 = d1 < 0 ? 0 : d1;
        dis2 = d2 < 0 ? 0 : d2;
        // Avoid too smal value for d1 , d2.
        if((dis1 + dis2) <= 0.01f) { dis1 = dis2 = 0.5f; }

        if ((dis1 + dis2) <= disAB)
        {
            Vector3 pos = vecAB.normalized;
            pos = pos * (disAB * dis1 / (dis1 + dis2)) + objTarget.transform.position;
            transform.position = pos;
            result = true;
            return result;
        }
        else if (Mathf.Abs(dis1 - dis2) >= disAB)
        {
            float dir = dis1 > dis2 ? 1.0f : -1.0f;
            Vector3 pos2 = vecAB.normalized;
            pos2 = pos2 * dir * dis1 + objTarget.transform.position;
            transform.position = pos2;
            result = true;
            return result;
        }
        else
        {
            return result;
        }
    }

    private void ApplyPoleConstraint(float d1, float d2, bool s)
    {
        Vector3 vp;
        if(s)                          // using dynamic pole vector.
        {
            Vector3 posP = poleTarget.transform.position;
            Vector3 vecAP = posP - posA;
            float lineF = Mathf.Abs(Vector3.Dot(vecAB, vecAP)/(vecAB.magnitude * vecAP.magnitude));
            if (lineF > 0.99f)         // if pole point is too close to line AB.
            {
                vecAP = Vector3.up;
            }
            vp = vecAP;
        }
        else                           // using constant pole vector.
        {
            vp = poleVector;
            if (poleVector.magnitude <= 0.01f)
            {
                vp = Vector3.up;
            }            
        }

        Vector3 vecNmr = Vector3.Cross(vecAB,vp);
        Vector3 vecOC = Vector3.Cross(vecNmr,vecAB);
        float cosABC = (d2 * d2 + disAB * disAB - d1 * d1) / (d2 * disAB * 2.0f);
        float disOB = d2 * Mathf.Abs(cosABC);
        float disOC = Mathf.Sqrt(d2 * d2 - disOB * disOB);
        Vector3 vecBC;
        //            C
        //           /.`
        //          / . `
        //         d1 .  d2
        //        /   .    `
        //       /    .     `
        //      A-----O-------B
        if (cosABC >= 0)
        {
            vecBC = -vecAB.normalized * disOB + vecOC.normalized * disOC;
        }
        //                    C
        //                  //.
        //                / / .
        //             d1  d2 .
        //            /   /   .
        //         /     /    .
        //        A-----B-----O
        else
        {
            vecBC = vecAB.normalized * disOB + vecOC.normalized * disOC;            
        }
        Vector3 posC = posB + vecBC;
        transform.position = posC;
    }
}
