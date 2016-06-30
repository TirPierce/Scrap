using Microsoft.Xna.Framework;
using Scrap.GameElements.Building;
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
        Wheel backWheel;
        Wheel frontWheel;
        Crate torsoBack;
        Crate torsoMiddle;
        Crate torsoFront;
        Rocket rocket1;
        Nozzle face;

        public ScrapBadger(ScrapGame game, Vector2 pos):base(game)
        {
            torsoMiddle = new Crate(game, pos);
            KeyObject = torsoMiddle;
            torsoFront = new Crate(game, pos +  new Vector2(1.2f, 0));
            AddNewSegmentToConstruct(torsoMiddle, torsoFront, new Point(1, 0), Direction.Up);

            torsoBack = new Crate(game, pos +  new Vector2(-1.2f, 0));
            AddNewSegmentToConstruct(torsoMiddle, torsoBack, new Point(-1, 0), Direction.Up);

            backWheel = new Wheel(game, pos + new Vector2(-1.2f, 1.2f));
            AddNewSegmentToConstruct(torsoBack, backWheel, new Point(0, 1), Direction.Up);

            frontWheel = new Wheel(game, pos + new Vector2(1.2f, 1.2f));
            AddNewSegmentToConstruct(torsoFront, frontWheel, new Point(0, 1), Direction.Up);

            rocket1 = new Rocket(game, pos + new Vector2(0, -1.2f));
            AddNewSegmentToConstruct(torsoMiddle, rocket1, new Point(0, -1), Direction.Up);
            foreach(var constructElement in this.buildElements)
            {

                constructElement.Value.DisableSensors();
                constructElement.Value.EnableSensors();
            }

            game.camera.Follow(torsoMiddle, game.camera.Magnification);
            
        }
    }
}
