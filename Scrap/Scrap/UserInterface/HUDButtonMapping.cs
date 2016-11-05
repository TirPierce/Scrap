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
            actions = new List<BehaviourTile>();
        }
        List<BehaviourTile> actions;

        public bool AddAction(HUDDraggingTile draggedTile)
        {
            if (actions.Count < 4)
            {
                draggedTile.area.Location = placementArea.Location + new Point((actions.Count / 2)*25, (actions.Count % 2)*25);
                actions.Add(draggedTile.behaviour);
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
        Segment placedSegment;
        ScrapGame game;
        InputManager inputManager;
        HUDDraggingTile draggedTile;

        Point draggingOldPosition;

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
        public void AddSegment(Segment segment)
        {
            placedSegment = segment;
            int tileVerticalOffset = 0;
            foreach (BehaviourTile tile in placedSegment.behaviourList)
            {

                Point position = new Point(game.GraphicsDevice.Viewport.Width - 160, 20 + tileVerticalOffset);
                draggingTiles.Add(new HUDDraggingTile(new Rectangle(position.X, position.Y, 20, 20),segment, tile));
                tileVerticalOffset += 30;
            }

        }
        public void Update()
        {

            if (draggedTile != null)
            {
                if (inputManager.MouseState.LeftButton == ButtonState.Pressed)
                {
                    draggedTile.area.Location = inputManager.MouseState.Position;
                }
                else
                {
                    bool placed = false;
                    foreach (HUDRecievingTile recievingTile in recievingTiles.Values)
                    {
                        if (recievingTile.placementArea.Contains(draggedTile.area.Location))
                        {
                            if (recievingTile.AddAction(draggedTile)) 
                            { 
                                placed = true;
                            }
                        }
                    }
                    if(!placed)
                    {
                        draggedTile.area.Location = draggingOldPosition;
                    }
                    
                    draggedTile = null;
                }
            }
            else
            {
                if (inputManager.MouseState.LeftButton == ButtonState.Pressed && inputManager.PrevMouseState.LeftButton == ButtonState.Released)
                {
                    foreach (HUDDraggingTile tile in draggingTiles)
                    {
                        if (tile.area.Contains(inputManager.MouseState.Position))
                        {
                            draggingOldPosition = tile.area.Location;
                            draggedTile = tile;
                            break;
                        }
                    }
                }

            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (HUDDraggingTile tile in draggingTiles)
            {
                spriteBatch.Draw(tile.behaviour.TileTexture,tile.area,null, Color.White,0, Vector2.Zero, SpriteEffects.None, .5f);

                
            }
            foreach (HUDRecievingTile tile in recievingTiles.Values)
            {
                spriteBatch.Draw(this.backgroundTexture,tile.placementArea, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
                spriteBatch.Draw(tile.buttonFace, new Rectangle(tile.placementArea.X-20, tile.placementArea.Y-20, 20, 20), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, .9f);
                //spriteBatch.Draw(tile.buttonFace, tile.placementArea., null, Color.White, 0, tile.placementArea.Center.ToVector2(), SpriteEffects.None, .9f);

            }

        }
    }
}
