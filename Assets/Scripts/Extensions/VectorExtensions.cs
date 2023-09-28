using UnityEngine;

namespace Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 AddX(this Vector3 vector, float value)
        {
            vector.x += value;
            return vector;
        }       
        public static Vector3 AddY(this Vector3 vector, float value)
        {
            vector.y += value;
            return vector;
        }        
        public static Vector3 AddZ(this Vector3 vector, float value)
        {
            vector.z += value;
            return vector;
        }    
        public static Vector3 SetX(this Vector3 vector, float value)
        {
            vector.x = value;
            return vector;
        }      
        public static Vector3 SetY(this Vector3 vector, float value)
        {
            vector.y = value;
            return vector;
        }   
        public static Vector3 SetZ(this Vector3 vector, float value)
        {
            vector.z = value;
            return vector;
        }

        public static TypeDirection GetDirectionType(this Vector3 tile1Position, Vector3 tile2Position)
        {
            bool sameX = Mathf.Approximately(tile1Position.x, tile2Position.x);
            bool sameZ = Mathf.Approximately(tile1Position.z, tile2Position.z);

            if (sameX && sameZ)
            {
                return TypeDirection.Straight;
            }

            if ((sameX && !sameZ) || (!sameX && sameZ))
            {
                return TypeDirection.Straight;
            }

            return TypeDirection.Diagonal;
        }
        public static TypeDirection GetDirectionByAngleType(this Vector3 tile1Position, Vector3 tile2Position)
        {
            float angleThreshold = 20.0f;

            Vector3 direction = (tile2Position - tile1Position).normalized;
            float angleX = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
            float angleZ = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            if (Mathf.Abs(angleX) > 80)
            {
                angleX -= 90 * Mathf.Sign(angleX);
            }
            if (Mathf.Abs(angleZ) > 80)
            {
                angleZ -= 90 * Mathf.Sign(angleZ);
            }

            if (Mathf.Abs(angleX) <= angleThreshold || Mathf.Abs(angleZ) <= angleThreshold)
            {
                return TypeDirection.Straight;
            }

            return TypeDirection.Diagonal;
        }
    }
}