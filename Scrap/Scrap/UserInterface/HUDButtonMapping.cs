using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Scrap.GameElements.Building;
using Scrap.GameElements.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.UserInterface
{
    class HUDRecievingTile
    {
        public HUDRecievingTile(Rectangle placementArea, Texture2D buttonFace)
        {
            this.placementArea = placementArea;
            this.buttonFace = buttonFace;
            actions = new List<HUDDraggingTile>();
        }
        public List<HUDDraggingTile> actions;
        
        public bool AddAction(HUDDraggingTile draggedTile)
        {
            if (actions.Count < 4)
            {
                if (draggedTile.recievingTile != null)
                {
                    draggedTile.recievingTile.actions.Remove(draggedTile);
                }

                draggedTile.recievingTile = this;
                draggedTile.area.Location = placementArea.Location + new Point((actions.Count / 2)*25, (actions.Count % 2)*25);
                actions.Add(draggedTile);
                return true;
            }
            return false;
        }

        public Rectangle placementArea;
        public Texture2D buttonFace;
    }
    class HUDDraggingTile
    {
        public HUDDraggingTile(Rectangle area, Segment segment, BehaviourTile behaviour)
        {
            this.area= area;
            this.segment= segment;
            this.behaviour= behaviour;
        }
        public HUDRecievingTile recievingTile;
        public Rectangle area;
        public Segment segment;
        public BehaviourTile behaviour;
    }
    public class HUDButtonMapping
    {
        Dictionary<string, HUDRecievingTile> recievingTiles = new Dictionary<string, HUDRecievingTile>();
        List<HUDDraggingTile> draggingTiles = new List<HUDDraggingTile>();

        Texture2D backgroundTexture;
        Construct construct;
        private SpriteFont font;
        ScrapGame game;
        InputManager inputManager;
        HUDDraggingTile draggedTile;
        bool isActive = false;
        Point draggingOldPosition;

        Segment currentSegment;

        public HUDButtonMapping(Construct construct, ScrapGame game)
        {
            this.game = game;
            this.construct = construct;
            backgroundTexture = game.Content.Load<Texture2D>("HUDBackground");

            inputManager = InputManager.GetManager();


            Texture2D texture = game.Content.Load<Texture2D>("Buttons/ButtonA");
            recievingTiles.Add("ButtonA", new HUDRecievingTile(new Rectangle(game.GraphicsDevice.Viewport.Width - 110, 50, 50, 50), texture));

            texture = game.Content.Load<Texture2D>("Buttons/ButtonB");
            recievingTiles.Add("ButtonB", new HUDRecievingTile(new Rectangle(game.GraphicsDevice.Viewport.Width - 50, 50, 50, 50), texture));

            texture = game.Content.Load<Texture2D>("Buttons/ButtonX");
            recievingTiles.Add("ButtonX", new HUDRecievingTile(new Rectangle(game.GraphicsDevice.Viewport.Width - 110, 110, 50, 50), texture));

            texture = game.Content.Load<Texture2D>("Buttons/ButtonY");
            recievingTiles.Add("ButtonY", new HUDRecievingTile(new Rectangle(game.GraphicsDevice.Viewport.Width - 50, 110, 50, 50), texture));

            texture = game.Content.Load<Texture2D>("Buttons/StickLeft");
            recievingTiles.Add("StickLeft", new HUDRecievingTile(new Rectangle(game.GraphicsDevice.Viewport.Width - 110, 170, 50, 50), texture));

            texture = game.Content.Load<Texture2D>("Buttons/StickRight");
            recievingTiles.Add("StickRight", new HUDRecievingTile(new Rectangle(game.GraphicsDevice.Viewport.Width - 50, 170, 50, 50), texture));

            texture = game.Content.Load<Texture2D>("Buttons/TriggerLeft");
            recievingTiles.Add("TriggerLeft", new HUDRecievingTile(new Rectangle(game.GraphicsDevice.Viewport.Width - 110, 230, 50, 50), texture));

            texture = game.Content.Load<Texture2D>("Buttons/TriggerRight");
            recievingTiles.Add("TriggerRight", new HUDRecievingTile(new Rectangle(game.GraphicsDevice.Viewport.Width - 50, 230, 50, 50), texture));
        }

        public void RemoveSegment(Segment segment)
        {
            foreach (HUDDraggingTile tile in draggingTiles.Reverse<HUDDraggingTile>())
            {
                if (tile.segment == segment)
                {
                    draggingTiles.Remove(tile);
                    if(tile.recievingTile != null)
                        tile.recievingTile.actions.Remove(tile);
                }
            }
        }

        public void EnableForSegment(Segment segment)
        {
            foreach (HUDDraggingTile tile in draggingTiles.Reverse<HUDDraggingTile>())
            {
                if (tile.segment == currentSegment)
                {
                    draggingTiles.Remove(tile);
                    if (tile.recievingTile == null)
                        draggingTiles.Remove(tile);
                }
            }
            var previousSegment = currentSegment;
            currentSegment = segment;
            int tileVerticalOffset = -30;

            var behaviours = segment.behaviourList;
            foreach(HUDDraggingTile tile in draggingTiles)
            {
                if(behaviours.Contains(tile.behaviour))
                {
                    behaviours.Remove(tile.behaviour);
                }

            }

            foreach (BehaviourTile tile in behaviours)
            {
                tileVerticalOffset += 30;
                Point position = new Point(game.GraphicsDevice.Viewport.Width - 160, 20 + tileVerticalOffset);
                draggingTiles.Add(new HUDDraggingTile(new Rectangle(position.X, position.Y, 20, 20), segment, tile));
            }
        }
        public bool BindInput(Segment segment, bool left)
        {

           // HUDRecievingTile 
            foreach (HUDDraggingTile tile in draggingTiles)
            {
                if(tile.recievingTile == null)
                {
                    if (!left)
                    {
                        recievingTiles["TriggerRight"].AddAction(tile);
                    }
                    else
                    {
                        recievingTiles["TriggerLeft"].AddAction(tile);
                    }
                    return true;
                }
            }

            return false;
        }
        public void TriggerInput(String input, float value)
        {
            foreach(HUDDraggingTile tile in recievingTiles[input].actions)
            {
                ((AnalogueTile)tile.behaviour).action.Invoke(value);
            }

        }
        public void TriggerInput(String input, bool value)
        {
            recievingTiles[input].actions.Cast<BoolTile>().ToList().ForEach(o => o.action(value));
        }
        public void Update()
        {

            
        }
        public void Draw(SpriteBatch spriteBatch)
        {

            foreach (HUDRecievingTile tile in recievingTiles.Values)
            {
                spriteBatch.Draw(this.backgroundTexture, tile.placementArea, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
                spriteBatch.Draw(tile.buttonFace, new Rectangle(tile.placementArea.X - 20, tile.placementArea.Y - 20, 20, 20), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, .9f);
                //spriteBatch.Draw(tile.buttonFace, tile.placementArea., null, Color.White, 0, tile.placementArea.Center.ToVector2(), SpriteEffects.None, .9f);
                foreach (HUDDraggingTile tileAction in tile.actions)
                {
                    spriteBatch.Draw(tileAction.behaviour.TileTexture, tileAction.area, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, .5f);
                }
            }

            foreach (HUDDraggingTile tile in draggingTiles)
            {
                spriteBatch.Draw(tile.behaviour.TileTexture, tile.area, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, .5f);


            }



        }
    }
}
