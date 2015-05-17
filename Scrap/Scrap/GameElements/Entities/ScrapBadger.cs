using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.GameElements.Entities
{
    class ScrapBadger:Construct
    {
        DriveWheel backWheel;
        Wheel frontWheel;
        Crate torsoBack;
        Crate torsoMiddle;
        Crate torsoFront;
        Nozzle face;

        public ScrapBadger(ScrapGame game):base(game)
        {
           
        }
    }
}
