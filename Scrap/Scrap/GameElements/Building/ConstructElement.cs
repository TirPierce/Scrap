using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Scrap.GameElements.GameWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.GameElements.Entities
{

    public class ConstructElement
    {
        public const float OFFSET = 1.2f;

        public Segment segment;
        public Construct construct;
        Body body;
        ScrapGame game;
        public Point offSet;
        private Joint rootJoint;
        public List<Joint> branchJoints;

        public ConstructElement(ScrapGame game, Segment entity, Construct construct, Point offSet, Joint rootJoint)
        {
            branchJoints = new List<Joint>(); 
            this.rootJoint = rootJoint;
            this.segment = entity;
            this.game = game;
            this.construct = construct;
            this.offSet = offSet;
        }

        public void RemoveFromConstruct()
        {
            if (construct != null)
            {
                BreakRoot();
                BreakBranch();
                if (construct.buildElements.ContainsKey(offSet))
                    construct.buildElements.Remove(offSet);
                segment.constructElement.construct = null;
                
            }
        }
        public void BreakBranch()
        {
            foreach (Joint item in branchJoints)
            {
                game.world.RemoveJoint(item);
            }
        }

        public void BreakRoot()
        {
            if (rootJoint != null)
                game.world.RemoveJoint(rootJoint);

        }

        public bool Draggable() 
        {
            //Todo:check grid to see if draggable
            if (construct != null && construct.KeyObject.constructElement == this)
                return false;
            return true;
        }

        public void Update()
        {

        }
    }

}