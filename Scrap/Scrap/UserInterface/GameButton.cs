using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scrap.GameElements.Building;
using Scrap.GameElements.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.UserInterface
{

    public class GameButton
    {//should have base class button
        Segment segment;
        Direction offsetDirection;
        Action<Direction> callBack;
        public UIStatus status = UIStatus.Inactive;
        public GameButton(Segment seg, Direction dir, Action<Direction> callBack)
        {
            this.segment = seg;
            this.offsetDirection = dir;
            this.callBack = callBack;
            //register with UI controller to get mouse input 
            //ui buttons should have a state rather than being deleted and created
        }
        public void DoActions()
        {
            Debug.WriteLine("Button:" + offsetDirection.ToString());
            callBack.Invoke(offsetDirection);
        }
        public virtual void Draw(SpriteBatch batch)
        {
            if (status == UIStatus.Active)
            {//ToDo: + MathHelper.PiOver2 is magic
                batch.Draw(segment.sprite.Texture, Rotate(Orientation.DirectionToRadians(offsetDirection) + segment.body.Rotation + MathHelper.PiOver2 + MathHelper.Pi, 1.2f, segment.body.WorldCenter), null,
            Color.FromNonPremultiplied(150, 50, 150, 200), Orientation.DirectionToRadians(offsetDirection) + segment.body.Rotation,
            new Vector2(segment.sprite.FrameWidth / 2f, segment.sprite.FrameHeight / 2f), .01f
            * (100f / (float)segment.sprite.FrameWidth), SpriteEffects.None, 0);
                /*
                batch.Draw(segment.sprite.Texture, segment.body.WorldCenter, null,
                    Color.FromNonPremultiplied(150, 50, 150, 200), segment.body.Rotation,
                    new Vector2(segment.sprite.FrameWidth / 2f
                    + Sensor.GetRelativePositionOfADirection(offsetDirection).X * segment.sprite.FrameWidth, segment.sprite.FrameHeight / 2f + segment.sprite.FrameHeight
                    * Sensor.GetRelativePositionOfADirection(offsetDirection).Y), .01f
                    * (100f / (float)segment.sprite.FrameWidth), SpriteEffects.None, 0);*/
            }
            else 
            {

            }
        }
        public bool TestPoint(Vector2 point)
        {
            
            if (this.segment != null && status == UIStatus.Active)
            {//ToDo: + MathHelper.PiOver2 is magic
                Vector2 buttonWorldCenter = Rotate(Orientation.DirectionToRadians(offsetDirection) + segment.body.Rotation + MathHelper.PiOver2 + MathHelper.Pi, 1.2f, segment.body.WorldCenter);
                Rot rot = new Rot(Orientation.DirectionToRadians(offsetDirection) + segment.body.Rotation);
                Transform transform = new Transform(ref buttonWorldCenter, ref rot);
                
               
                Vector2 pLocal = MathUtils.MulT(transform.q, point - transform.p);
                if(pLocal.X >-.6f && pLocal.X < .6f && pLocal.Y >-.6f && pLocal.Y < .6f)
                    return true;

            }

            return false;
        }

        private Vector2 Rotate(float angle, float distance, Vector2 centre)
        {
            return new Vector2((float)(distance * Math.Cos(angle)), (float)(distance * Math.Sin(angle))) + centre;
        }

        Vector2[] buttonDimensions = { new Vector2(-.6f, -.6f), new Vector2(.6f, -.6f), new Vector2(.6f, .6f), new Vector2(-.6f, .6f) };


        private bool IsPointInPolygon(Vector2[] polygon, Vector2 point)
        {
            Vector2[] transformedPolygon = new Vector2[polygon.Length];
            //Quaternion transform = new Quaternion(new Vector3(0,0,0), )
            //Vector2.Transform(polygon, ref new Quaternion(), transformedPolygon);
            //Segment.DirectionToRadians(offsetDirection) + segment.body.Rotation
            bool isInside = false;
            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                if (((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y)) &&
                (point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                {
                    isInside = !isInside;
                }
            }
            return isInside;
        }
        //game button should be in the scope of the object that created it and drawn by that object
    }
}
