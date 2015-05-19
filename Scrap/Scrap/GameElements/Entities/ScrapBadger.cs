using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.GameElements.Entities
{
    [Serializable]
    class ScrapBadger:Construct
    {
        DriveWheel backWheel;
        DriveWheel frontWheel;
        Crate torsoBack;
        Crate torsoMiddle;
        Crate torsoFront;
        Nozzle face;

        public ScrapBadger(ScrapGame game, Vector2 pos):base(game)
        {
            torsoBack = new Crate(game, pos + new Vector2(-1,0));
            torsoMiddle = new Crate(game, pos);
            torsoFront = new Crate(game, pos + new Vector2(1, 0));
            backWheel = new DriveWheel(game, pos + new Vector2(-1, 1));
            frontWheel = new DriveWheel(game, pos + new Vector2(1, 1));
            
            JoinEntities(torsoBack, torsoMiddle, Entity.Direction.Right);
            JoinEntities(torsoMiddle, torsoFront, Entity.Direction.Right);
            JoinEntities(torsoBack, backWheel, Entity.Direction.Down);
            JoinEntities(torsoFront, frontWheel, Entity.Direction.Down);

            game.camera.Follow(torsoMiddle, game.camera.Magnification);
            KeyObject = torsoMiddle;
        }
    }
}
