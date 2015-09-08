using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;
using Scrap.GameElements.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace Scrap
{

    [Serializable]
    //[System.Xml.Serialization.XmlRootAttribute("Level", Namespace = "", IsNullable = false)]
    public class Level
    {
        //[System.Xml.Serialization.XmlElementAttribute("LevelItems")]
        public List<Segment> EntityList { get; set; }
        public List<Crate> CrateList { get; set; }
        public List<Wheel> WheelList { get; set; }

        public Level()
        {
            //EntityList = new List<Entity>();
            //CrateList = new List<Crate>();
            //WheelList = new List<Wheel>();
        }
    }
}
