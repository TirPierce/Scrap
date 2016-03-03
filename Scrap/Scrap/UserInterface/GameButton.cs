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
        public GameButton(Segment seg, Direction dir, Action callBack)
        {
            this.segment = seg;
            this.offsetDirection = dir;

            //register with UI controller to get mouse input 
            //ui buttons should have a state rather than being deleted and created
        }
        
        public virtual void Draw(SpriteBatch batch)
        {
            if (segment.Status == SegmentStatus.Locked)
            {
                batch.Draw(segment.texture, segment.body.WorldCenter, null, Color.FromNonPremultiplied(150, 50, 150, 200), segment.body.Rotation, new Vector2(segment.texture.Width / 2f + Segment.GetSensorOffset(offsetDirection).X * segment.texture.Width, segment.texture.Height / 2f + segment.texture.Height * Segment.GetSensorOffset(offsetDirection).Y), .01f * (100f / (float)segment.texture.Width), SpriteEffects.None, 0);
            }
        }

        //game button should be in the scope of the object that created it and drawn by that object
    }
}
