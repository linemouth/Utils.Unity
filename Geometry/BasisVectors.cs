using Unity.VisualScripting;
using UnityEngine;

namespace Utils.Unity
{
    public struct BasisVectors
    {
        public static BasisVectors Right => new BasisVectors(Direction.Right);
        public static BasisVectors Up => new BasisVectors(Direction.Up);
        public static BasisVectors Forward => new BasisVectors(Direction.Forward);
        public static BasisVectors Left => new BasisVectors(Direction.Left);
        public static BasisVectors Down => new BasisVectors(Direction.Down);
        public static BasisVectors Back => new BasisVectors(Direction.Back);
        public Vector3 normal;
        public Vector3 x;
        public Vector3 y;

        public BasisVectors(Direction direction)
        {
            switch(direction)
            {
                case Direction.Right:
                    normal = Vector3.right;
                    x = Vector3.back;
                    y = Vector3.up;
                    break;
                case Direction.Up:
                default:
                    normal = Vector3.up;
                    x = Vector3.right;
                    y = Vector3.forward;
                    break;
                case Direction.Forward:
                    normal = Vector3.forward;
                    x = Vector3.right;
                    y = Vector3.up;
                    break;
                case Direction.Left:
                    normal = Vector3.left;
                    x = Vector3.forward;
                    y = Vector3.up;
                    break;
                case Direction.Down:
                    normal = Vector3.down;
                    x = Vector3.right;
                    y = Vector3.forward;
                    break;
                case Direction.Back:
                    normal = Vector3.back;
                    x = Vector3.left;
                    y = Vector3.up;
                    break;
            }
        }
        public BasisVectors(Direction direction, Vector2 scale) : this(direction, scale.x, scale.y) { }
        public BasisVectors(Direction direction, float scaleX, float scaleY) : this(direction)
        {
            x *= scaleX;
            y *= scaleY;
        }
        public BasisVectors(Vector3 normal)
        {
            this.normal = Geometry.Normalized(normal);
            y = Vector3.Cross(Vector3.right, this.normal);
            if(y.sqrMagnitude < 0.1f)
            {
                y = Vector3.Cross(this.normal, Vector3.back);
            }
            x = Vector3.Cross(y, this.normal);
            x.Normalize();
            y = Vector3.Cross(x, this.normal);
            y.Normalize();
        }
        public BasisVectors(Vector3 normal, Vector2 scale) : this(normal, scale.x, scale.y) { }
        public BasisVectors(Vector3 normal, float scaleX, float scaleY) : this(normal)
        {
            x *= scaleX;
            y *= scaleY;
        }
        public BasisVectors(Vector3 x, Vector3 y) : this(Vector3.Cross(x, y), x, y) { }
        public BasisVectors(Vector3 normal, Vector3 x, Vector3 y)
        {
            this.normal = Geometry.Normalized(normal);
            this.x = x;
            this.y = y;
        }
        public static BasisVectors operator *(BasisVectors basisVectors, float scale)
        {
            basisVectors.x *= scale;
            basisVectors.y *= scale;
            return basisVectors;
        }
        public static BasisVectors operator *(BasisVectors basisVectors, Vector2 scale)
        {
            basisVectors.x *= scale.x;
            basisVectors.y *= scale.y;
            return basisVectors;
        }
    }
}
