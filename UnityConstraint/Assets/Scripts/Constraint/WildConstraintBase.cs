using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Constraint
{
    public enum Axis
    {
        X,
        Y,
        Z,
    }

    public class WildConstraintBase : MonoBehaviour
    {
        [SerializeField]
        protected bool isOneShot;

        [SerializeField]
        protected GameObject objTarget;

        protected Vector3 sclOrgTarget;      // Original target scale;

        protected Vector3 offset;            // Distance offset vector from myself to target.
        protected Quaternion rotOffset;        // Rotation between myself and target.

        virtual protected void Awake()
        {
            GetOffset();
            sclOrgTarget = objTarget.transform.lossyScale;
            rotOffset = GetRotDiff(transform.forward, objTarget.transform.forward);
        }

        protected void GetOffset()
        {
            Vector3 p1 = transform.position;
            Vector3 p2 = objTarget.transform.position;
            offset = p1 - p2;
        }

        protected Quaternion GetRotDiff(Vector3 v1, Vector3 v2)
        {
            Quaternion rotDiff;
            rotDiff = Quaternion.FromToRotation(v1,v2);
            return rotDiff;
        }

        protected Vector3 GetSclDiff()
        {
            Vector3 sclDiffTarget;
            Vector3 v = objTarget.transform.lossyScale;
            sclDiffTarget = CalculateScaleDiff(sclOrgTarget,v);
            return sclDiffTarget;
        }

        private Vector3 CalculateScaleDiff(Vector3 v1, Vector3 v2)
        {
            Vector3 result;
            float x1 = v1.x;
            float y1 = v1.y;
            float z1 = v1.z;
            x1 = x1 == 0 ? 0.1f : x1;
            y1 = y1 == 0 ? 0.1f : y1;
            z1 = z1 == 0 ? 0.1f : z1;
            v1 = new Vector3(x1, y1, z1);
            result = new Vector3(v2.x / v1.x, v2.y / v1.y, v2.z / v1.z);
            float xf = result.x;
            float yf = result.y;
            float zf = result.z;
            xf = xf == 0 ? 1.0f : xf;
            yf = yf == 0 ? 1.0f : yf;
            zf = zf == 0 ? 1.0f : zf;
            result = new Vector3(xf, yf, zf);
            return result;
        }

        protected void LookAt(Vector3 v1, Vector3 v2)
        {
            Quaternion rotL = Quaternion.LookRotation(v1, v2);
            transform.rotation = rotL;
        }

        public static Vector3 RotateVector(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            Vector3 result;
            Quaternion r = Quaternion.FromToRotation(v2, v3);
            result = r * v1;
            return result;
        }

        public static Vector3 RotateVectorRelatively(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
        {
            Vector3 result;
            Quaternion r1 = Quaternion.FromToRotation(v2, v3);
            Quaternion r2 = Quaternion.FromToRotation(Vector3.up, v4);
            result = r1 * r2 * v1;
            return result;
        }

        public static Vector3 ReverseDir(Vector3 v)
        {
            Vector3 result;
            result = new Vector3(-v.x,-v.y,-v.z);
            return result;
        }
    }
}

