using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scrap.GameState;
using Scrap.Entities;
using Microsoft.Xna.Framework;

namespace Scrap.UserInterface
{
    public interface IGUI
    {
        void InventionCreator(Entity entity,Invention invention);
        void SetObjectiveLocation(Vector2 location);

    }
}
