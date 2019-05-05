using UnityEngine;

namespace SunCube.Controllers
{
    public class RotorController : MonoBehaviour
    {
        public enum Axis
        {
            X,
            Y,
            Z,
        }

        public Axis RotateAxis;
        public float StartRotarSpeed;
        private float _rotarSpeed;
        
        public float RotarSpeed
        {
            get { return _rotarSpeed; }
            set { _rotarSpeed = Mathf.Clamp(value, -3000, 3000); }
        }

        private float rotateDegree;
        private Vector3 OriginalRotate;

        private void Start()
        {
            OriginalRotate = transform.localEulerAngles;
            RotarSpeed = StartRotarSpeed;
        }

        private void Update()
        {
            rotateDegree += RotarSpeed*Time.deltaTime;
            rotateDegree = rotateDegree%360;

            switch (RotateAxis)
            {
                case Axis.Y:
                    transform.localRotation = Quaternion.Euler(OriginalRotate.x, rotateDegree, OriginalRotate.z);
                    break;
                case Axis.Z:
                    transform.localRotation = Quaternion.Euler(OriginalRotate.x, OriginalRotate.y, rotateDegree);
                    break;
                default:
                    transform.localRotation = Quaternion.Euler(rotateDegree, OriginalRotate.y, OriginalRotate.z);
                    break;
            }
        }
    }
}