using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scrap.GameElements.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.UserInterface
{
    public class GameButton
    {//should have base class button
        Segment segment;
        Direction offsetDirection;
        Action callBack;
        public GameButton(Segment seg, Direction dir, Action callBack)
        {
            this.segment = seg;
            this.offsetDirection = dir;
            this.callBack = callBack;
            //register with UI controller to get mouse input 
            //ui buttons should have a state rather than being deleted and created
        }
        public void DoActions()
        {
            callBack.Invoke();
        }
        public virtual void Draw(SpriteBatch batch)
        {
            if (segment.Status == SegmentStatus.Locked)
            {
                batch.Draw(segment.sprite.Texture, segment.body.WorldCenter, null,
                    Color.FromNonPremultiplied(150, 50, 150, 200), segment.body.Rotation,
                    new Vector2(segment.sprite.FrameWidth / 2f
                    + Segment.GetSensorOffset(offsetDirection).X * segment.sprite.FrameWidth, segment.sprite.FrameHeight / 2f + segment.sprite.FrameHeight
                    * Segment.GetSensorOffset(offsetDirection).Y), .01f
                    * (100f / (float)segment.sprite.FrameWidth), SpriteEffects.None, 0);
            }
            else 
            {

            }
        }
        public bool TestPoint(Vector2 point)
        {
            if (this.segment != null && segment.status == SegmentStatus.Locked)
            {
                Transform transform;
                segment.body.GetTransform(out transform);
                transform.p = transform.p - Segment.GetSensorOffset(offsetDirection);
                Vector2 pLocal = MathUtils.MulT(transform.q, point - transform.p);
                if(pLocal.X >-.6f && pLocal.X < .6f && pLocal.Y >-.6f && pLocal.Y < .6f)
                    return true;

            }

            return false;
        }
        //game button should be in the scope of the object that created it and drawn by that object
    }
}
