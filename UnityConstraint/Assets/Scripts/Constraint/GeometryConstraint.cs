using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constraint;

public class GeometryConstraint : ConstraintBase
{
    [SerializeField]
    private bool normalConstraint;
    [SerializeField]
    private Axis normalAxis = Axis.Y;
    //[SerializeField]
    //private Axis tangentAxis = Axis.Z;
    [SerializeField]
    private int layerIndex = 0;

    //private Mesh mesh;
    private Bounds bounds;
    private MeshCollider meshCol;

    void Start ()
    {
        layerIndex = 1 << layerIndex;
        //layerIndex = ~layerIndex;

        //mesh = objTarget.GetComponent<MeshFilter>().mesh;      
        meshCol = objTarget.GetComponent<MeshCollider>();      
        if (meshCol == null)
        {
            objTarget.AddComponent<MeshCollider>();
            meshCol = objTarget.GetComponent<MeshCollider>();
        }
        bounds = meshCol.bounds;
        ApplyGeometryConstraint();
    }
	
	void Update ()
    {
        if ((objTarget != null) && !isOneShot)
        {
            ApplyGeometryConstraint();
        }
    }

    public void ApplyGeometryConstraint()
    {
        //mesh.RecalculateBounds();
        //bounds = mesh.bounds;
        //boundCenter = mesh.bounds.center;
        Vector3 boundCenter = bounds.center;

        Vector3 pos = transform.position;
        Vector3 posA = Vector3.zero;
        Vector3 posB = Vector3.zero;
        Vector3 hitPos = pos;
        Vector3 nmr = Vector3.up;
        Vector3 dir = boundCenter - pos;
        Vector3 dirBack = -dir.normalized * (bounds.size.magnitude + 9.0f);
        posA = pos + dirBack;
        posB = pos - dirBack;
        //if (bounds.Contains(pos)) { }

        RaycastHit hit;
        Ray ray = new Ray(posA, dir* 999.9f);       
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerIndex))
        {
            //Debug.DrawRay(transform.position, dir, Color.yellow, 1.0f, false);
            hitPos = hit.point;
            nmr = hit.normal; 
        }
        ray = new Ray(posB, -dir * 999.9f);    
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerIndex))
        {
            //Debug.DrawRay(transform.position, -dir, Color.black, 1.0f, false);
            float dis1 = Vector3.Distance(pos,hitPos);
            float dis2 = Vector3.Distance(hit.point,pos);
            hitPos = dis2 < dis1 ? hit.point : hitPos;
            nmr = dis2 < dis1 ? hit.normal : nmr;
        }

        // Apply position. 
        transform.position = hitPos;

        // Apply rotation.
        //Debug.DrawRay(transform.position, nmr * 10.0f, Color.cyan, 1.0f, false);
        if (normalConstraint)
        {
            ApplyNormalConstraint(nmr);
        }
    }

    public void ApplyNormalConstraint(Vector3 v)
    {
        Vector3 vf = Vector3.forward;
        Vector3 vu = v;
        switch (normalAxis)
        {
            case Axis.Z:
                vf = v;
                vu = RotateVector(vf, transform.forward, transform.up);
                LookAt(vf, vu);
                break;
            case Axis.X:
                vf = RotateVector(vu, transform.right, transform.forward);
                LookAt(vf, vu);
                vu = RotateVector(vu, transform.right, transform.up);
                LookAt(vf, vu);
                break;
            case Axis.Y:
                vf = RotateVector(vu, transform.up, transform.forward);
                LookAt(vf, vu);
                break;
        }
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = new Color(0, 1, 0, 0.2f);
    //    Gizmos.DrawCube(objTarget.GetComponent<MeshCollider>().bounds.center, objTarget.GetComponent<MeshCollider>().bounds.size);
    //    //Debug.Log(objTarget.GetComponent<MeshCollider>().bounds.size);
    //}
}
