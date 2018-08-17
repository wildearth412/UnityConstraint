using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constraint;

public class AimConstraint : RotationConstraint
{
    [SerializeField]
    private RotationType aimType = RotationType.All;
    [SerializeField]
    private Axis aimAxis = Axis.Z;
    [SerializeField]
    private Axis upAxis = Axis.Y;

    void Start ()
    {
        ApplyAimConstraint();
    }
	
	void Update ()
    {
        if ((objTarget != null) && !isOneShot)
        {
            ApplyAimConstraint();
        }
    }

    public void ApplyAimConstraint()
    {
        // Apply aim direction.
        Vector3 vf = CalculateAimDir();
        Vector3 vu = Vector3.up;
        switch (aimType)
        {
            case RotationType.All:
                switch(aimAxis)
                {
                    case Axis.Z:
                        switch(upAxis)
                        {
                            case Axis.Y:
                            case Axis.Z:
                                LookAt(vf, vu);        // Reset Direction: LookAt(CalculateAimDir(), Vector3.up);
                                break;
                            case Axis.X:
                                LookAt(vf, vu);
                                LookAt(vf, -transform.right);
                                break;
                        }
                        break;
                    case Axis.X:
                        switch (upAxis)
                        {
                            case Axis.Y:
                            case Axis.X:
                                LookAt(vf, vu);
                                LookAt(-transform.right, transform.up);
                                break;
                            case Axis.Z:
                                LookAt(vf, vu);
                                LookAt(transform.up, transform.right);
                                break;
                        }
                        break;
                    case Axis.Y:
                        switch(upAxis)
                        {
                            case Axis.Z:
                            case Axis.Y:
                                LookAt(vf, vu);
                                LookAt(transform.up, transform.forward);
                                break;
                            case Axis.X:
                                LookAt(vf, vu);
                                LookAt(transform.right, transform.forward);
                                break;
                        }
                        break;
                }
                break;
            case RotationType.Yaw:
                switch(aimAxis)
                {
                    case Axis.Y:
                        switch(upAxis)
                        {
                            case Axis.Y:
                            case Axis.X:
                                vu = new Vector3(vf.x, vf.y, 0);
                                LookAt(Vector3.forward, vu);
                                break;
                            case Axis.Z:
                                vu = new Vector3(vf.x, vf.y, 0);
                                LookAt(Vector3.forward, vu);
                                vf = RotateVector(vu, Vector3.up, Vector3.right);
                                LookAt(vf, vu);
                                break;
                        }
                        break;
                    case Axis.Z:
                        switch (upAxis)
                        {
                            case Axis.Z:
                            case Axis.Y:
                                vf = new Vector3(vf.x, vf.y, 0);
                                vu = RotateVector(vf, -Vector3.right, Vector3.up);
                                LookAt(vf, vu);
                                break;
                            case Axis.X:
                                vf = new Vector3(vf.x, vf.y, 0);
                                //vu = RotateVector(vf, Vector3.right, Vector3.up);
                                LookAt(vf, Vector3.forward);
                                //vu = RotateVector(vf, transform.forward, -transform.right);
                                //LookAt(vf, vu);
                                break;
                        }
                        break;
                    case Axis.X:
                        switch (upAxis)
                        {
                            case Axis.X:
                            case Axis.Y:
                                vf = new Vector3(vf.x, vf.y, 0);
                                vu = RotateVector(vf, -Vector3.right, Vector3.up);
                                LookAt(-Vector3.forward, vu);
                                break;
                            case Axis.Z:
                                vf = new Vector3(vf.x, vf.y, 0);
                                vf = RotateVector(vf, -Vector3.right, Vector3.up);
                                LookAt(vf, Vector3.forward);
                                break;
                        }
                        break;
                }
                break;
            case RotationType.Pitch:
                switch (aimAxis)
                {
                    case Axis.Z:
                        switch (upAxis)
                        {
                            case Axis.Z:
                            case Axis.X:
                                vf = new Vector3(vf.x, 0, vf.z);
                                LookAt(vf, Vector3.up);
                                break;
                            case Axis.Y:
                                vf = new Vector3(vf.x, 0, vf.z);
                                vu = RotateVector(vf, Vector3.forward, -Vector3.right);
                                LookAt(vf, vu);
                                break;
                        }
                        break;
                    case Axis.Y:
                        switch (upAxis)
                        {
                            case Axis.Y:
                            case Axis.Z:
                                vu = new Vector3(vf.x, 0, vf.z);
                                vf = RotateVector(vu, -Vector3.right, Vector3.forward);
                                LookAt(vf, vu);
                                break;
                            case Axis.X:                               
                                vu = new Vector3(vf.x, 0, vf.z);
                                LookAt(Vector3.up, vu);
                                break;
                        }
                        break;
                    case Axis.X:
                        switch (upAxis)
                        {
                            case Axis.X:
                            case Axis.Y:
                                vu = new Vector3(vf.x, 0, vf.z);
                                vu = RotateVector(vu, Vector3.right, -Vector3.forward);
                                LookAt(Vector3.up,vu);
                                break;
                            case Axis.Z:
                                vf = new Vector3(vf.x, 0, vf.z);
                                vf = RotateVector(vf, Vector3.right, Vector3.forward);
                                LookAt(vf,Vector3.up);
                                break;
                        }
                        break;
                }
                break;
            case RotationType.Roll:
                switch (aimAxis)
                {
                    case Axis.Y:
                        switch (upAxis)
                        {
                            case Axis.Y:
                            case Axis.Z:
                                vu = new Vector3(0, vf.y, vf.z);
                                vf = RotateVector(vu, Vector3.up, Vector3.forward);
                                LookAt(vf,vu);
                                break;
                            case Axis.X:
                                vu = new Vector3(0, vf.y, vf.z);
                                LookAt(-Vector3.right, vu);
                                break;
                        }
                        break;
                    case Axis.X:
                        switch (upAxis)
                        {
                            case Axis.X:
                            case Axis.Y:
                                vu = new Vector3(0, vf.y, vf.z);
                                vu = RotateVector(vu, -Vector3.forward,Vector3.up);
                                LookAt(Vector3.right, vu);
                                break;
                            case Axis.Z:
                                vf = new Vector3(0, vf.y, vf.z);
                                vf = RotateVector(vf, -Vector3.up, Vector3.forward);
                                LookAt(vf,Vector3.right);
                                break;
                        }
                        break;
                    case Axis.Z:
                        switch (upAxis)
                        {
                            case Axis.Z:
                            case Axis.X:
                                vf = new Vector3(0, vf.y, vf.z);
                                LookAt(vf, Vector3.right);
                                break;
                            case Axis.Y:
                                vf = new Vector3(0, vf.y, vf.z);
                                vu = RotateVector(vf, Vector3.forward, Vector3.up);
                                LookAt(vf, vu);
                                break;
                        }
                        break;
                }
                break;
        }
    }

    private Vector3 CalculateAimDir()
    {
        Vector3 result = objTarget.transform.position - transform.position;
        return result;
    }
}
