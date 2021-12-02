﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;
using System.Linq;

namespace EksamensProjekt2021
{
    enum Dir
    {
        Right,
        Left,
        Up,
        Down
    }
    public class GameWorld : Game
    {
        public static bool HCDebug = false;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static List<GameObject> gameObjects;
        private static List<GameObject> deleteObjects;
        private static List<Enemy> enemies;
        private static List<Projectile> projectiles;

        public static Player player;
        public static Enemy enemy;
        public static RoomManager roomManager;
        public static Door door;
        public static GameObject target;

        private Texture2D cursor;

        private Texture2D collisionTexture;



        private Texture2D trumpWalkRight;
        private Texture2D trumpWalkLeft;
        private Texture2D trumpWalkUp;
        private Texture2D trumpWalkDown;

        private Song music;

        


        public static Vector2 screenSize;
        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            player = new Player();
            roomManager = new RoomManager();
            screenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

        }



        public void RemoveObject(GameObject go)
        {

        }

        private void AddEnemy()
        {
            Enemy enemy = new Enemy(player);
            gameObjects.Add(enemy);
        }

        protected override void Initialize()
        {
            // _graphics.IsFullScreen = true;
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
            screenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            player = new Player();
            player.Position = new Vector2(500, 500);

            gameObjects = new List<GameObject>();
            projectiles = new List<Projectile>();
            enemies = new List<Enemy>();
            deleteObjects = new List<GameObject>();
            //AddGameObject(new Enemy());
            AddEnemy();
            gameObjects.Add(player);

            for (byte i = 0; i < 4; i++) // Create the 4 doors. GameObject will handle LoadContent() and Update().
            {
                door = new Door(i);
                gameObjects.Add(door);
            }

            roomManager.CreateMap(9);



            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            collisionTexture = Content.Load<Texture2D>("CollisionTexture ");

            foreach (GameObject go in gameObjects)
            {
                go.LoadContent(this.Content);
            }

            foreach (Projectile go in projectiles)
            {
                go.LoadContent(this.Content);
            }


            trumpWalkRight = Content.Load<Texture2D>("trumpWalkRight");
            trumpWalkLeft = Content.Load<Texture2D>("trumpWalkLeft");
            trumpWalkUp = Content.Load<Texture2D>("trumpWalkUp");
            trumpWalkDown = Content.Load<Texture2D>("trumpWalkDown");
          
            player.animations[0] = new SpriteAnimation(trumpWalkRight, 6, 3); // SpriteAnimation(texture2D texture, int frames, int fps) forklaret hvad de gør i SpriteAnimation.cs
            player.animations[1] = new SpriteAnimation(trumpWalkLeft, 6, 3);
            player.animations[2] = new SpriteAnimation(trumpWalkUp, 6, 3);
            player.animations[3] = new SpriteAnimation(trumpWalkDown, 6, 3);
            //enum kan castes til int, så derfor kan vi bruge et array til at skife imellem dem. forklaret i player og hvor det relevant

            player.anim = player.animations[0]; //ændre sig afhængig af direction i player

            

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            UpdateGameObjects(gameTime);
            
            player.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            foreach (GameObject go in gameObjects)
            {
                go.Draw(_spriteBatch);
                
               // DrawCollisionBox(go);
               //den kan ikke finde ud af at tegne player rectangle lige nu så den er disabled
            }
            foreach (Projectile go in projectiles)
            {
                go.Draw(_spriteBatch);
            }

           
            
            
             //   player.anim.Draw(_spriteBatch);
            
          //  player.anim.Draw(_spriteBatch); //vi bruger Draw metoden i den SpriteAnimation "anim" som vi lavede på playeren. det ser fucking nice ud fordi det er så simpelt
          //jeg efterlader dem lige her indtil videre men jeg har overrided draw i player for at gøre det samme


            _spriteBatch.End();


            base.Draw(gameTime);



        }

        private void AddGameObject(GameObject gameObject)
        {

            if (gameObject is null)
                throw new System.ArgumentNullException($"{nameof(gameObject)} cannot be null.");

            gameObject.LoadContent(this.Content);
            gameObjects.Add(gameObject);


        }
        private void AddPlayer(GameObject gameObject)
        {
            //måske ikke nødvendig
            if (gameObject is null)
                throw new System.ArgumentNullException($"{nameof(gameObject)} cannot be null.");

            gameObject.LoadContent(this.Content);
            


        }

        public static void Instantiate(Projectile go)
        {
            projectiles.Add(go);
        }

        public static void Despawn(Projectile go)
        {
            deleteObjects.Add(go);
        }

        public void UpdateGameObjects(GameTime gameTime)
        {
            gameObjects.AddRange(projectiles);
            projectiles.Clear();

            foreach (GameObject go in gameObjects)
            {
                go.Update(gameTime);

            }
            foreach (GameObject go in deleteObjects)
            {
                gameObjects.Remove(go);
            }

        }

        private void DrawCollisionBox(GameObject go)
        {

            Rectangle topLine = new Rectangle(go.Collision.X, go.Collision.Y, go.Collision.Width, 1);
            Rectangle bottomLine = new Rectangle(go.Collision.X, go.Collision.Y + go.Collision.Height, go.Collision.Width, 1);
            Rectangle rightLine = new Rectangle(go.Collision.X + go.Collision.Width, go.Collision.Y, 1, go.Collision.Height);
            Rectangle leftLine = new Rectangle(go.Collision.X, go.Collision.Y, 1, go.Collision.Height);

            _spriteBatch.Draw(collisionTexture, topLine, Color.Red);
            _spriteBatch.Draw(collisionTexture, bottomLine, Color.Red);
            _spriteBatch.Draw(collisionTexture, rightLine, Color.Red);
            _spriteBatch.Draw(collisionTexture, leftLine, Color.Red);
        }

        
    }
}
