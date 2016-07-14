using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.GameElements.Building
{
    public enum Direction { Up = 0, Right = 90, Down = 180, Left = 270};
    public class Orientation
    {

        
        /*
        public static int UP = 0;
        public static int Right = 0;
        public static int Down = 0;
        public static int Left = 0;
        */
        private Direction direction;

        public Direction Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        private float rotationRadians;
        private Point unitDirectionPoint;
        public Direction AddDirectionsAsClockwiseAngles(Direction direction)
        {
            int newDirection = ((int)direction + (int)this.direction) % 360;
            return (Direction)newDirection;
        }
        public Orientation(Direction direction)
        {
            this.direction = direction;
            switch (direction)
            {
                case Direction.Up:
                    rotationRadians =  MathHelper.Pi;
                    break;
                case Direction.Right:
                    rotationRadians = 3 * MathHelper.PiOver2;
                    break;
                case Direction.Down:
                    rotationRadians = 0;
                    break;
                case Direction.Left:
                    rotationRadians = MathHelper.PiOver2;
                    break;
                default:
                    rotationRadians = 0;
                    break;
            }
        }
        public float ToRadians()
        {
            return DirectionToRadians(direction);
        }
        public static float DirectionToRadians(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return 0;
                //return MathHelper.Pi;
                case Direction.Right:
                    return 3 * MathHelper.PiOver2;
                case Direction.Down:
                    return MathHelper.Pi;
                //return 0;
                case Direction.Left:
                    return MathHelper.PiOver2;
                default:
                    return 0;
            }
        }
        public const float OFFSET = 1.2f;
        public static Vector2 GetRelativePositionOfADirection(Direction direction)
        {
            Vector2 offset = Vector2.Zero;
            if (direction == Direction.Right)
            {
                offset = Vector2.UnitX * OFFSET;
            }
            if (direction == Direction.Left)
            {
                offset = Vector2.UnitX * -OFFSET;
            }
            if (direction == Direction.Down)
            {
                offset = Vector2.UnitY * OFFSET;
            }
            if (direction == Direction.Up)
            {
                offset = Vector2.UnitY * -OFFSET;
            }
            return offset;
        }
        //public static Point DirectonToPoint(Direction direction)
        //{
        //    Point offset = Point.Zero;
        //    if (direction == Direction.Right)
        //    {
        //        offset = new Point(1, 0);
        //    }
        //    if (direction == Direction.Left)
        //    {
        //        offset = new Point(-1, 0);
        //    }
        //    if (direction == Direction.Down)
        //    {
        //        offset = new Point(0, 1);
        //    }
        //    if (direction == Direction.Up)
        //    {
        //        offset = new Point(0, -1);
        //    }
        //    return offset;
        //}
        public static Point DirectionToPoint(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return new Point(-1, 0);
                case Direction.Right:
                    return new Point(1, 0);
                case Direction.Up:
                    return new Point(0, -1);
                case Direction.Down:
                    return new Point(0, 1);
            }
            return new Point(0, 0);
        }
        public static Direction PointToDirection(Point point)
        {
            if (point == new Point(-1, 0))
                return Direction.Left;

            if (point == new Point(1, 0))
                return Direction.Right;

            if (point == new Point(0, -1))
                return Direction.Up;

            return Direction.Down;
        }
    }

}
