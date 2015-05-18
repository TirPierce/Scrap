using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;
using Scrap.GameElements;
using Scrap.GameElements.Entities;
using Microsoft.Xna.Framework;
using Scrap.GameElements.GameWorld;

namespace Scrap
{
    public class XmlLoader
    {
       
        //a uri could be used here to get levels from a web server  //the final build should either point to the installation folder or LocalAppData/Scrap/Levels/
        string gesturefile = Path.Combine(Environment.CurrentDirectory, @"..\..\..\Content\Levels\Level 1.xml");

        public XmlLoader(){}

        //public XmlLoader(ScrapGame game)
        //{
        //    Level current = new Level();

        //    Crate crate1 = new Crate();
        //    Crate crate2 = new Crate();
        //    //Wheel wheel1 = new Wheel(game, new Vector2(21.55f, -10));
        //    //Wheel wheel2 = new Wheel(game, new Vector2(22.55f, -10));

        //    //current.EntityList.Add(crate1);
        //    //current.EntityList.Add(crate2);
        //    //current.EntityList.Add(wheel1);
        //    //current.EntityList.Add(wheel2);
        //    current.CrateList.Add(crate1);
        //    current.CrateList.Add(crate2);
        //    //current.WheelList.Add(wheel1);
        //    //current.WheelList.Add(wheel2);

        //    WriteXMl(current);

        //    Level newTask = GetTask();
        //}

        public void SaveLevel(List<Entity> itemList)
        {
            Level currentLevel = new Level();
            //foreach(Entity current in itemList)
            //{
            //    currentLevel.EntityList.Add(current);
            //}
            currentLevel.EntityList = itemList;

            WriteXMl(currentLevel);
        }

        public void LoadLevel(ref List<Entity> itemList, ScrapGame game)
        {
            Level currentLevel = GetTask();
            //foreach (Crate current in currentLevel.CrateList)
            //{
            //    itemList.Add(current);
            //}

            foreach (Entity current in currentLevel.EntityList)
            {
                current.Init(game);
            }
            itemList.AddRange(currentLevel.EntityList);
        }

        public void WriteXMl(Level level)
        {
            XmlSerializer serializer;
            serializer = new XmlSerializer(level.GetType());

            MemoryStream stream = new MemoryStream();

            StreamWriter writer = new StreamWriter(stream, Encoding.Unicode);
            serializer.Serialize(writer, level);

            int count = (int)stream.Length;

            byte[] arr = new byte[count];
            stream.Seek(0, SeekOrigin.Begin);

            stream.Read(arr, 0, count);

            using (BinaryWriter binWriter = new BinaryWriter(File.Open(gesturefile, FileMode.Create)))
            {
                binWriter.Write(arr);
            }

        }

        public Level GetTask()
        {
            XmlSerializer serializer;
            serializer = new XmlSerializer(typeof(Level));

            StreamReader stream = new StreamReader(gesturefile, Encoding.Unicode);
            return (Level)serializer.Deserialize(stream);
        }
    }
}
