using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Scrap.GameElements.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap
{
    public class PlayerController
    {

        InputManager inputManager;
        ScrapGame game;
        Texture2D pointer;
        Texture2D pointerClosed;
        Vector2 mouseWorld;
        public Segment selectedSegment = null;
        Scrap.GameElements.Entities.Segment.Direction validDirections;
        public void LoadContent()
        {
            pointer = game.Content.Load<Texture2D>("Pointer");
            pointerClosed = game.Content.Load<Texture2D>("PointerClosed");
        }
        public PlayerController(ScrapGame game)
        {
            inputManager = InputManager.GetManager();
            this.game = game;
        }
        public void Update()
        {
            inputManager.Update();

            if (inputManager.WasKeyReleased(Keys.Q))
            {
                game.camera.Rotate(-.005f);
            }
            if (inputManager.WasKeyReleased(Keys.E))
            {
                game.camera.Rotate(+.005f);
            }
            if (inputManager.WasKeyReleased(Keys.D))
            {
                game.camera.Position += new Vector2(1f, 0f);
            }
            if (inputManager.WasKeyReleased(Keys.A))
            {
                game.camera.Position += new Vector2(-1f, 0f);
            }
            if (inputManager.WasKeyReleased(Keys.W))
            {
                game.camera.Position += new Vector2(0f, -1f);
            }
            if (inputManager.WasKeyReleased(Keys.S))
            {
                game.camera.Position += new Vector2(0f, 1f);
            }
            if (inputManager.WasKeyReleased(Keys.Space))
            {
                game.camera.Position = new Vector2(0f, 0f);
            }
            game.camera.Zoom(inputManager.ScroleWheelDelta() * .01f);//ToDo: Camera Controls need to be changed
            mouseWorld = (game.camera.MousePick(inputManager.MouseState.Position));
            if (inputManager.MouseState.LeftButton == ButtonState.Pressed && inputManager.PrevMouseState.LeftButton == ButtonState.Released)
            {
                if (selectedSegment == null)
                {
                    foreach (var entity in this.game.entityList)
                    {
                        if (entity.TestEntity(ref mouseWorld))
                        {
                            if (entity.constructElement == null)
                            {
                                selectedSegment = entity;
                                break;
                            }
                            if ((entity.constructElement != null && entity.constructElement.Draggable()))
                            {
                                if(entity.constructElement.construct!= null && entity.constructElement.construct.KeyObject != entity)
                                    entity.constructElement.RemoveFromConstruct();
                                selectedSegment = entity;
                                break;
                            }
                        }
                    }
                }
            }
            else if (selectedSegment != null && inputManager.MouseState.LeftButton == ButtonState.Released && inputManager.PrevMouseState.LeftButton == ButtonState.Pressed)
            {
                ReleaseSegment();
            }

            if (selectedSegment != null)
            {
                MoveSegment();
            }
        }
        protected void MoveSegment()
        {
            float length = (mouseWorld - selectedSegment.body.Position).Length();
            Vector2 direction = (mouseWorld - selectedSegment.body.Position) / length;

            selectedSegment.body.ApplyLinearImpulse(direction);
            Mouse.SetPosition((int)game.camera.ProjectPoint(selectedSegment.body.Position).X, (int)game.camera.ProjectPoint(selectedSegment.body.Position).Y);

        }
        protected void ReleaseSegment()
        {
            //Todo:direction selection code
            selectedSegment = null;
        }
        public void PlaceSegment()
        {
            ReleaseSegment();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (selectedSegment != null)
            {
                spriteBatch.Draw(pointerClosed, new Rectangle((int)game.camera.ProjectPoint(selectedSegment.body.Position).X - 10, (int)game.camera.ProjectPoint(selectedSegment.body.Position).Y - 10, 20, 20), Color.BlanchedAlmond);
            }
            else
            {
                if (inputManager.MouseState.LeftButton == ButtonState.Released)
                    spriteBatch.Draw(pointer, new Rectangle((int)inputManager.MouseState.Position.X - 10, (int)inputManager.MouseState.Position.Y - 10, 20, 20), Color.BlanchedAlmond);
                else
                    spriteBatch.Draw(pointerClosed, new Rectangle((int)inputManager.MouseState.Position.X - 10, (int)inputManager.MouseState.Position.Y - 10, 20, 20), Color.BlanchedAlmond);
            }
        }
    }
}
