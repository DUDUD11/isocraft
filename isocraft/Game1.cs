using Flat;
using Flat.Graphics;
using Flat.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;

namespace isocraft;

public class Game1 : Game
{
    public static Game1 _Instance;

    private Screen screen;
    private Sprites sprites;
    private Shapes shapes;
    private Camera camera;

    private GraphicsDeviceManager graphics;


    private static float left, right, bottom, top;
    public static Vector2 MouseScreenPos;
    public static int screen_width;
    public static int screen_height;
    public static double GameTime;


    public static Point offset = Point.Zero;
    public static Point shape_offset;
    public static bool Player_turn = true;

    public static SpriteSelected selected;

    public static Point Display_size;

    private Texture2D cursor;

    // queue spriteentity
    public static Queue<SpriteEntity> WorkQueue;
 
    //2층 높이 여부느 나중에

    public male Male;
    public Villain villain;

    public static Game_Status _game_Status = Game_Status.Game_Menu;
    public static Playing_Status _playing_Status = Playing_Status.Idle;
    

    public enum Game_Status
    { 
        Game_Menu,
        Game_Option,
        Game_Pause,
        Game_MapEditor,
        Game_Playing,
    }

    public enum Playing_Status
    {
        Idle,
        Updating,
        
    }

    public struct SpriteSelected
    {
        public SpriteEntity sprite;
        public Point init_pos;

    
     
    }

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
        _Instance = this;

    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        IsFixedTimeStep = true;
        FlatUtil.SetAbsoulteBackBufferSize(graphics, 1920, 1080);
        screen = new Screen(this, 1920, 1080);
        sprites = new Sprites(this);
        shapes = new Shapes(this);
        camera = new Camera(screen);
        camera.IncZoom();
  
        camera.GetExtents(out left, out right, out bottom, out top);
        screen_width = (int)right*2;
        screen_height = (int)top*2;

        Display_size.X = screen.Width;
        Display_size.Y = screen.Height;
        shape_offset = new Point(screen_width / 2, screen_height / 2);


        cursor = Content.Load<Texture2D>("Cursor\\CursorArrow");


        Coordinate.Instance.Init(100, 100);

        WorkQueue = new Queue<SpriteEntity>();


        base.Initialize();
    }

    protected override void LoadContent()
    {
        //  Shader.init();

        // TODO: use this.Content to load your game content here


        for (int i = 3; i <=3; i++)
        {
            for (int j = 3; j <= 3; j++)
            {
                Male = new male("Character\\Animation\\Male\\male_default", new Vector2(i, j),1);
              
                EntityManager.AddHero(Male);
            }
        }

        for (int i = 8; i <= 8; i++)
        {
            for (int j = 8; j <= 8; j++)
            {
                villain = new Zombie("Character\\Animation\\Zombie\\zombie_default", new Vector2(i, j), 1);

                EntityManager.AddEnemys(villain);
            }
        }



    }

    public void Mouse_KeyboardUpdate()
    {

        FlatKeyboard keyboard = FlatKeyboard.Instance;
        FlatMouse mouse = FlatMouse.Instance;

        keyboard.Update();
        mouse.Update();
        Vector2 tmp = mouse.GetMouseWorldPosition(this, this.screen, this.camera);

        MouseScreenPos = new Vector2(screen_width * 0.5f + tmp.X - offset.X, screen_height * 0.5f + tmp.Y - offset.Y) * camera.Zoom;


     

        // 좌표변환필요

    }



    protected override void Update(GameTime gameTime)
    {
        GameTime = Flat.FlatUtil.GetElapsedTimeInSeconds(gameTime);
       

        WorldTimer.Instance.Add_Time(GameTime);

        Mouse_KeyboardUpdate();


        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // click 처리
        {
            if (FlatMouse.Instance.IsLeftMouseButtonPressed())
            {

                Point point = Coordinate.ToTile((int)MouseScreenPos.X, (int)MouseScreenPos.Y);

   

                if (Coordinate.Instance.Vaild(point))
                {
                    Heros hero = EntityManager.FindHero(point);

                    if (hero != null)
                    {
                      

                        hero.Selected();
                        Selected(hero);


                      //  _playing_Status = Playing_Status.Idle;
                    }

                }
            }

            if (FlatMouse.Instance.IsRightMouseButtonPressed())
            {
                Point point = Coordinate.ToTile((int)MouseScreenPos.X, (int)MouseScreenPos.Y);

                if (selected.sprite is Heros hero)
                {
                    hero.RightClick(point);
                }

            }






        }



        { 
            // UI UPDATE
        
        
        }

        { 
        // QUEUE 확인
        
        }

        {
            //  Queue 없으면
            // Left Clicked  RightClicked확인
        }

        if (FlatKeyboard.Instance.IsKeyDown(Keys.X))
        {
            Toggle_Turn_Hero();
        }

        if (FlatKeyboard.Instance.IsKeyDown(Keys.Z))
        {
            Toggle_Turn_Enemy();
        }

        if (FlatKeyboard.Instance.IsKeyDown(Keys.E))
        {
            //Toggle_Turn_Enemy();
        }


        if (Player_turn)
        {
            for (int i = 0; i < EntityManager.Heroes.Count; i++)
            {
                EntityManager.Heroes[i].Update();
            }
        }

        else
        {
            for (int i = 0; i < EntityManager.Enemys.Count; i++)
            {
                EntityManager.Enemys[i].Update();
            }

        }

        //{
        //    for (int i = 0; i < EntityManager.AllEntities.Count; i++)
        //    {
        //        EntityManager.AllEntities[i].Update();
        //    }
        //}


     

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
    

        screen.Set();
        GraphicsDevice.Clear(new Color(118, 147, 179));
        shapes.RemoveAll();


        MoveScreen();


        #region cursor

        sprites.Begin();
        //sprites.Draw(cursor, new Rectangle((int)(100 - offset.X), (int)(100 + offset.Y), 28, 28), Color.White, new Vector2(0, cursor.Bounds.Height));
        sprites.Draw(cursor, new Rectangle((int)(MouseScreenPos.X), (int)(MouseScreenPos.Y), 28, 28), Color.White, new Vector2(0, cursor.Bounds.Height));
        sprites.End();

        #endregion

        sprites.Begin(camera);

        //AntiAliasingShader(cursor, new Vector2(28, 28));


       // sprites.Draw(cursor, new Rectangle((int)(100 - offset.X), (int)(100 + offset.Y), 28, 28), Color.White, new Vector2(0, cursor.Bounds.Height));

        for (int i = 0; i < EntityManager.AllEntities.Count; i++)
        {
            EntityManager.AllEntities[i].Draw(sprites);
        }

        sprites.End();

        shapes.Begin(camera);

       
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
          
                Point point = Coordinate.Instance.Map[i, j];
                point = Draw_Offset(point);


                shapes.DrawBox(point.ToVector2() , TileMap.Tile_Size * FlatMath.InvSqrt2, TileMap.Tile_Size * FlatMath.InvSqrt2, MathHelper.PiOver4, Color.White);
                
            }
        }

        if (_playing_Status == Playing_Status.Selected && selected.sprite is Heros hero)
        {
            for (int i = -hero.range; i <= hero.range; i++)
            {
                for (int j = -hero.range; j <= hero.range; j++)
                {
                    if (i + hero.range < 0 || j + hero.range < 0) continue;

                    if (!Coordinate.Instance.boundary_check(new Point(i + hero.range, j + hero.range)))
                    {
                        continue;
                    }


                    if (BFS.Instance.bfsMap[i + hero.range, j + hero.range] <= hero.range)
                    {
                        Point point = Coordinate.Instance.Map[selected.init_pos.Y+i, selected.init_pos.X+j];
                        point = Draw_Offset(point);

                        shapes.DrawBox(point.ToVector2(), TileMap.Tile_Size * FlatMath.InvSqrt2, TileMap.Tile_Size * FlatMath.InvSqrt2, MathHelper.PiOver4, Color.Blue);

                    }
                
                }
            }
        }



        shapes.End();
        
        
        screen.Unset();
        screen.Present(sprites);

        base.Draw(gameTime);

    }

    private void MoveScreen()
    {

        bool update = false;
        int _width = screen_width * camera.Zoom;
        int _height = screen_height * camera.Zoom;
        int remain = 0;

        if (MouseScreenPos.X  < _width * 0.15f )
        {
            remain = (int)(MouseScreenPos.X - (_width * 0.15f));
            remain = Math.Max(-15, remain);

            camera.Move(new Vector2(remain, 0));
            offset.X += remain;
            update = true;

        }

        if (MouseScreenPos.X > _width * 0.85f )
        {
            remain = (int)(MouseScreenPos.X - (_width * 0.85f));
            remain = Math.Min(15, remain);

            camera.Move(new Vector2(remain, 0));
            offset.X += remain;
            update = true;
        }

        if (MouseScreenPos.Y < _height * 0.15f)
        {

            remain = (int)(MouseScreenPos.Y - (_height * 0.15f));
            remain = Math.Max(-15, remain);

            camera.Move(new Vector2(0, remain));
            offset.Y += remain;
            update = true;
        }

        if (MouseScreenPos.Y > _height * 0.85f)
        {

            remain = (int)(MouseScreenPos.Y - (_height * 0.85f));
            remain = Math.Min(15, remain);

            camera.Move(new Vector2(0, remain));
            offset.Y += remain;
            update = true;
        }

        if (update)
        {
            camera.GetExtents(out left, out right, out bottom, out top);
        }

        //out of position 위치 정보는 화면 밖인지 정확하게 판단하지 않지만 현재는 충분함
        //if (!update && Out_of_position(out int dir))
        //{
        //    //맵을 바꾸든가 죽게 처리
        //    {
        //        string next_map = mapDeployment.NextMap(dir);

        //        if (next_map != null)
        //        {


        //            mapDeployment.Load_Map(next_map, hero);
        //            this.Camera_Reset();

        //        }
        //        else
        //        {

        //            // kill instantly
        //            hero.Get_Hit(-1);
        //        }
        //    }
        //    // WALKAWAY 판별하는곳
        //    if (gamePlay_Status == GamePlay_status.Cinematic_WalkAway)
        //    {
        //        CinamaFlag = false;
        //        gamePlay_Status = GamePlay_status.Normal;
        //    }


        //}

    }

    private Point Draw_Offset(Point point)
    {
        point -= shape_offset;
        return new Point(point.X, -point.Y);
    
    }

    private void Selected(SpriteEntity spriteEntity)
    {
        selected.sprite = spriteEntity;
        selected.init_pos = spriteEntity.pos.ToPoint();
    }

    private void Toggle_Turn_Hero()
    {
        if (Player_turn) return;

        Player_turn = !Player_turn;

      
            foreach (Heros heros in EntityManager.Heroes)
            {
                heros.Reset_act();
                heros.updateRequired = true;
            }

      

    
    }

    private void Toggle_Turn_Enemy()
    {
        if (!Player_turn) return;

        Player_turn = !Player_turn;

  

    
            foreach (Villain villain in EntityManager.Enemys)
            {
                villain.Reset_act();
                villain.updateRequired = true;
            }

      


    }

}
