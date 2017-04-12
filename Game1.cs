using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Another2DShooter;

namespace Another2DShooter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player player;

        Vector2 vRight = new Vector2(1,0);
        Vector2 vLeft = new Vector2(-1, 0);
        Vector2 vUp = new Vector2(0, 1);
        Vector2 vDown = new Vector2(0, -1);

        KeyboardState currentKeyboardState;
        KeyboardState preciousKeyboardState;

        GamePadState currentGamePadState;
        GamePadState previousGamePadState;

        MouseState currentMouseState;
        MouseState previousMouseState;

        float playerMoveSpeed;

        Texture2D mainBackground;
        Rectangle rectBackground;
        float scale = 1f;

        ParallaxingBackground bgLayer1;
        ParallaxingBackground bgLayer2;

        Texture2D enemyTexture;
        List<Enemy> enemies;

        TimeSpan enemySpawnTime;
        TimeSpan prevSpawnTime;

        Random random;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new Player();

            bgLayer1 = new ParallaxingBackground();
            bgLayer2 = new ParallaxingBackground();

            enemies = new List<Enemy>();
            prevSpawnTime = TimeSpan.Zero;
            enemySpawnTime = TimeSpan.FromSeconds(2.0f);
            random = new Random();

            playerMoveSpeed = 10.0f;
            base.Initialize();
        }

        

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load the players resources
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(Content.Load<Texture2D>("Graphics\\player"), playerPosition);

            // Load the parallaxing background

            bgLayer1.Initialize(Content, "Graphics\\bgLayer1", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, -1);
            bgLayer2.Initialize(Content, "Graphics\\bgLayer2", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, -2);

            mainBackground = Content.Load<Texture2D>("Graphics\\mainbackground");
            // TODO: use this.Content to load your game content here

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            previousGamePadState = currentGamePadState;
            preciousKeyboardState = currentKeyboardState;

            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            UpdatePlayer(gameTime);

            bgLayer1.Update(gameTime);
            bgLayer2.Update(gameTime);

            UpadateEnemies(gameTime);

            UpdateCollision(gameTime);
            base.Update(gameTime); 
        }
        /// <summary>
        /// This is a separated method for the control of the player
        /// It controls the player and keeps it in the boundaries(The view of the game)
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdatePlayer(GameTime gameTime)
        {
            player.position.X += currentGamePadState.ThumbSticks.Left.X * playerMoveSpeed;
            player.position.Y -= currentGamePadState.ThumbSticks.Left.Y * playerMoveSpeed;

            if (currentKeyboardState.IsKeyDown(Keys.Right) || currentGamePadState.DPad.Right == ButtonState.Pressed) player.position.X += playerMoveSpeed;

            if (currentKeyboardState.IsKeyDown(Keys.Left) || currentGamePadState.DPad.Left == ButtonState.Pressed) player.position.X -= playerMoveSpeed;

            if (currentKeyboardState.IsKeyDown(Keys.Up) || currentGamePadState.DPad.Up == ButtonState.Pressed) player.position.Y -= playerMoveSpeed;

            if (currentKeyboardState.IsKeyDown(Keys.Down) || currentGamePadState.DPad.Down == ButtonState.Pressed) player.position.Y += playerMoveSpeed;

            player.position.X = MathHelper.Clamp(player.position.X, 0, GraphicsDevice.Viewport.Width - player.width);
            player.position.Y = MathHelper.Clamp(player.position.Y, 0, GraphicsDevice.Viewport.Height - player.height);

            Vector2 mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);

            Vector2 playerToMouse = new Vector2(mousePosition.X - player.position.X, mousePosition.Y - player.position.Y);
            


        }

        private void AddEnemy() {
            enemyTexture = Content.Load<Texture2D>("Graphics\\enemy");
            Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + enemyTexture.Width /2,random.Next(100,GraphicsDevice.Viewport.Height -200));
            Enemy enemy = new Enemy();
            enemy.Initialize(enemyTexture, position);
            enemies.Add(enemy);
        }

        private void UpadateEnemies(GameTime gameTime){
            if (gameTime.TotalGameTime - prevSpawnTime > enemySpawnTime)
            {
                prevSpawnTime = gameTime.TotalGameTime;
                AddEnemy();
            }

            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies[i].Update(gameTime);
                if (!enemies[i].active)
                {
                    enemies.RemoveAt(i);
                }
            }
        }

        private void UpdateCollision(GameTime gameTime)
        {
            Rectangle rect1;
            Rectangle rect2;
            //Rectangle(x coordination, y coordination, width, length)
            //Due the the relative x,y coordinations, rect1 is the "box" of the player
            rect1 = new Rectangle((int)player.position.X, (int)player.position.Y,player.width,player.height);

            for (int i = 0; i < enemies.Count; i++)
            {
                rect2 = new Rectangle((int)enemies[i].position.X, (int)enemies[i].position.Y, enemies[i].width, enemies[i].height);
                //If the player is intersected by an enemy, the player's health will subtracted by the the damage of the enemy
                if (rect1.Intersects(rect2))
                {
                    player.hp -= enemies[i].damage;
                    if (player.hp <=0)
                    {
                        player.active = false;
                    }
                }
            }

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            //start drawing
            spriteBatch.Begin();

            //draw the background
            spriteBatch.Draw(mainBackground, rectBackground, Color.White);

            bgLayer1.Draw(spriteBatch);
            bgLayer2.Draw(spriteBatch);

            //draw the player
            player.Draw(spriteBatch);
            //draw the enemies
            for (int i = 0; i <enemies.Count;  i++)
            {
                enemies[i].Draw(spriteBatch);
            }
            //End the drawing
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
