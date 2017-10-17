using UnityEngine;
class MathUtil{

   public static Vector3 ComputeCartesian(float radius, float thau, float phi)
    {

       float radElevation = (-thau+90) * Mathf.Deg2Rad ;
        float radPolar = (phi+90) * Mathf.Deg2Rad;

        float a = radius * Mathf.Cos(radElevation);

        return new Vector3(a * Mathf.Cos(radPolar), radius * Mathf.Sin(radElevation), a * Mathf.Sin(radPolar));


    }

    public static Vector3 ComputeSpherical(float x,float y, float z)
    {
        if (x == 0f) x = Mathf.Epsilon;
        Vector3 pos = new Vector3(x, y, z);

        float radius = pos.magnitude;
        float elevation = Mathf.Acos(pos.y/radius);
        float polar = Mathf.Atan2(z, x);
 
        polar += 1.5f*Mathf.PI;
        polar = polar % (2 * Mathf.PI);
        

        return new Vector3(radius, elevation* Mathf.Rad2Deg, polar* Mathf.Rad2Deg);
    }

}