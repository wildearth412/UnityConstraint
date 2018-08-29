using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTriangleIntersectionTest : MonoBehaviour
{
    /// <summary>
    /// Determine whether a ray intersect with a triangle
    /// This is in reference to DirectX SDK,Pick.cpp
    /// </summary>
    /// <param name="orig"></param> Origin of the ray
    /// <param name="dir"></param> Direction of the ray
    /// <param name="v0"></param> vertex 0 of triangle
    /// <param name="v1"></param> vertex 1 of triangle
    /// <param name="v2"></param> vertex 2 of triangle
    /// <param name="t"></param> weight of the intersection for the ray
    /// <param name="u"></param> barycentric coordinate of intersection in triangle
    /// <param name="v"></param> barycentric coordinate of intersection in triangle
    /// <returns> bool </returns>
    public static bool TestRayTriangleIntersection
                (Vector3 orig, Vector3 dir,
                Vector3 v0, Vector3 v1, Vector3 v2,
                out float t, out float u, out float v)
    {
        t = u = v = 0;

        Vector3 e1 = v1 - v0;                  // E1
        Vector3 e2 = v2 - v0;                  // E2
        Vector3 p = Vector3.Cross(dir, e2);    // P
        float det = Vector3.Dot(e1, p);        // determinant

        Vector3 o;                             // Keep det > 0
        if (det > 0)
        {
            o = orig - v0;
        }
        else
        {
            o = v0 - orig;
            det = -det;
        }

        if (det < 0.0001f)                      // If determinant is near zero, ray lies in plane of triangle
        {
            return false;
        }

        u = Vector3.Dot(o, p);                  // Calculate u and make sure u <= 1
        if (u < 0.0f || u > det)
        {
            return false;
        }

        Vector3 q = Vector3.Cross(o, e1);         // Q

        v = Vector3.Dot(dir, q);                // Calculate v and make sure u + v <= 1
        if (v < 0.0f || u + v > det)
        {
            return false;
        }

        t = Vector3.Dot(e2,q);                  // Calculate t

        float iDet = 1.0f / det;
        t *= iDet;
        u *= iDet;
        v *= iDet;

        return true;
    }
}
