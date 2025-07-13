using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Reflection.Metadata;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using Flat.Graphics;
using Flat.Input;
using static System.Net.WebRequestMethods;
using System.Linq.Expressions;
using static isocraft.GameEnums;
using System.Runtime.CompilerServices;
using static isocraft.Villain;


namespace isocraft
{
    public class MapEditor
    {
        private struct St_CurSelect
        {
            public SpriteEntity spriteEntity;
            public Tile tileEntity;
     

        }

        private enum EditObject
        {
            None = 0,
            Erase = 1,
            Tile = 2,
            Building =3,
            Hero=4,
            Enemy=5,
        }



        #region singleton
        private static MapEditor _instance;


        private MapEditor()
        {

        }

        public static MapEditor Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MapEditor();
                return _instance;
            }
        }

        #endregion
        private List<Building> MapBuilding = new();
        private List<Villain> MapVillain = new();
        private List<Heros> MapHeros = new();
     //   private List<Props> MapProps = new();

        private List<Tile> MapTiles = new();
     
        private List<Button> drawUIList = new();
        public Button[] Buttons;
        public Action[] buttons_action;


        private const int RbInput_Cnt = 10;
        private TextInputBox[] AdditionalInput;

        public TextInputBox MapNameInput;
        public TextInputBox MapWidthSizeInput;
        public TextInputBox MapHeightSizeInput;

        private Vector2 Button_Dims = TileMap.Tile_Dims;
        private Vector2 CurPos = Vector2.Zero;

        private Map CurrentMap;
        private Vector2 resetUIpos;

        private St_CurSelect curSelect;
        private EditObject edit = EditObject.None;




        private enum UIButtons
        {
            DeleteButton,
            SaveButton,
            LoadButton,
            ExitButton,
            MapSizeChangeButton,
            TileSetButton,
            BuildingSetButton,
            EnemySetButton,
            EraseButton,
        };

        private string[] buttons_str = {
            "Clean",
            "Save",
            "Load",
            "Back",
            "MpCg",
            "Tile",
            "Building",
            "Enemy",
            "Erase",
          

        };
       
        public void Init(int width, int height)
        {
            CurrentMap = new Map();
            CurrentMap.Init(width, height);
        }




        public MapEditor(int width, int height)
        {
         

            CurrentMap = new Map();
            CurrentMap.Init(width, height);
       
            AdditionalInput = new TextInputBox[RbInput_Cnt];
            Buttons = new Button[Enum.GetValues(typeof(UIButtons)).Length];
            buttons_action = new Action[] { DeleteButtonClicked, SaveButtonClicked, LoadButtonClicked, BackButtonClicked, MapSizeChangeButtonClicked, TileSetButtonClicked, BuildingButtonClicked, VillainButtonClicked,  EraseModeClicked  };

            for (int i = 0; i < buttons_action.Length; i++)
            {
                Buttons[i] = new Button(Button.Button_path, UIScreenPos(Button_Dims, 1), Button_Dims, true, 0, 1, new Vector2(1, 1), 1, 1000, buttons_str[i]);
                Buttons[i].set_Action(buttons_action[i]);
            }

            MapNameInput = new TextInputBox(null, "MapName", UIScreenPos(Button_Dims, 3), Button_Dims * 3, true);
            MapWidthSizeInput = new TextInputBox(null, " MapWidth", UIScreenPos(Button_Dims, 3), Button_Dims*3, true);
            MapHeightSizeInput = new TextInputBox(null, "MapHeight", UIScreenPos(Button_Dims, 3), Button_Dims*3, true);

            resetUIpos = UIScreenPos(Button_Dims, 0);
        }

        public string[] Read_ContentFile(string Folder)
        {
            string folderPath = Path.Combine(Game1._Instance.Content.RootDirectory, Folder);

            string[] files = Directory.GetFiles(folderPath, "*.xnb");

            for (int i = 0; i < files.Length; i++)
            {


                files[i] = files[i].Substring(8, files[i].Length -12);


            }



            return files;
        }

        private void Select_Reset()
        {
            curSelect.tileEntity = null;
            curSelect.spriteEntity = null;
    
        }

        private void Clear(EditObject editObject)
        {

            drawUIList.Clear();
            reset_UIScreenPos();

            //MapProps.Celar();


        }

        private void Clear()
        {
            MapBuilding.Clear();
            MapVillain.Clear();
            MapHeros.Clear();
            MapTiles.Clear();
            //MapProps.Celar();

        }


        private void reset_UIScreenPos()
        {
            CurPos = resetUIpos;
        }



        private Vector2 UIScreenPos(Vector2 Dims, int size)
        {
            Vector2 tmp = new Vector2(CurPos.X + Dims.X * size, CurPos.Y);

            if (tmp.X >= Game1.screen_width+3500)
            {
                tmp = new Vector2(-3500, CurPos.Y + Button_Dims.Y + 50);
            }

            return CurPos = tmp;
        }

        public void DeleteButtonClicked()
        {
            CurrentMap.Clear();
            Clear();
            

        }
        public void BackButtonClicked()
        {
            Game1._game_Status = Game1.Game_Status.Game_Playing;
        }

        public void LoadButtonClicked()
        {
            if (MapNameInput.GetInput() == null || MapNameInput.GetInput().Equals(""))
            {
                MapNameInput.SetInput("MapTest");
            }

          
            else
            {
                CurrentMap = Save.Instance.LoadMapData(MapNameInput.GetInput());

                //            public List<List<Tuple<int, int>>> SpriteList;
                //public List<List<Object>> SpriteInfo;

                //public List<List<Tuple<int, int, string>>> TileList;
                //public string name;

                for (int i = 0; i < CurrentMap.height; i++)
                {
                    for (int j = 0; j < CurrentMap.width; j++)
                    {
                        Tuple<int, int, string> tuple = CurrentMap.TileList[i][j];

                        if (tuple.Item3 != null && !tuple.Item3.Equals(""))
                        {
                            MapTiles.Add(new Tile(CurrentMap.TileList[i][j].Item3, new Vector2(j, i), new Vector2(CurrentMap.TileList[i][j].Item1, CurrentMap.TileList[i][j].Item1), CurrentMap.TileList[i][j].Item2, 1));
                        }

                        Tuple<int, int> Object = CurrentMap.SpriteList[i][j];

                        switch ((int)Object.Item1)
                        {
                            case (int)GameEnums.Type.None:
                                break;
                            case ((int)GameEnums.Type.Buliding):

                                Building building = Building.ParseToInstance(CurrentMap.SpriteInfo[i][j], j, i);
                                MapBuilding.Add(building);
                                break;
                            case ((int)GameEnums.Type.Enemy):

                                Villain villain = Villain.ParseToInstance(CurrentMap.SpriteInfo[i][j], j, i,Object.Item2);
                                MapVillain.Add(villain);
                                break;

                            case ((int)GameEnums.Type.Hero):

                                male Male = new male("Character\\Animation\\Male\\male_default", new Vector2(j, i), 1);
                             

                         
                                MapHeros.Add(Male);
                                break;
                            //hero추가

                            default:
                                throw new Exception("err");
                        }


         

                    }  
                }
    }

            if (CurrentMap == null)
            {
                throw new Exception("file not exist!");
            }

            MapWidthSizeInput.SetInput(CurrentMap.width.ToString());
            MapHeightSizeInput.SetInput(CurrentMap.height.ToString());
        }

        public void TileSetButtonClicked()
        {
            Select_Reset();
            edit = EditObject.Tile;

            Clear(edit);

            string[] Url = Read_ContentFile("Tiles");

            for (int i = 0; i < Url.Length; i++)
            {
                drawUIList.Add(new Button(Url[i], UIScreenPos(Button_Dims, 1), Button_Dims * 1, true, 0, 1, new Vector2(1, 1), 1, 1));
                drawUIList[i].set_Action(UISelected);
            }
        }

        public void BuildingButtonClicked()
        {
            Select_Reset();
            edit = EditObject.Building;

            Clear(edit);

            string[] Url = Read_ContentFile("Object\\Building");

            for (int i = 0; i < Url.Length; i++)
            {
                int[,] mat = GameEnums.BuildingDictionary[Url[i]];

                int m = Math.Max(mat.GetLength(0), mat.GetLength(1));


                drawUIList.Add(new Button(Url[i], UIScreenPos(Button_Dims, (int)1.5*m), Button_Dims* m, true, 0, 1, new Vector2(1, 1), 1, 1));
                drawUIList[i].set_Action(UISelected);
            }
        }


        public void VillainButtonClicked()
        {
            Select_Reset();
            edit = EditObject.Enemy;

            Clear(edit);

            int tmp = 0;   
          
            foreach(string str in VillianDictionary.Keys)
                {
                    drawUIList.Add(new Button(str, UIScreenPos(Button_Dims, 1), Button_Dims, true, 0, 1, new Vector2(1, 1), 1, 1));
                    drawUIList[tmp++].set_Action(UISelected);

                }



           
        }





        public void EraseModeClicked()
        {
            Select_Reset();
            edit = EditObject.Erase;
            Clear(edit);

        }

        public void UISelected(Button selected)
        {

            if (edit == EditObject.Tile)
            {
                curSelect.tileEntity = new Tile(selected.GetCurrentAnimationModelPath(), new Vector2(int.MaxValue / 2, int.MaxValue / 2), new Vector2(1, 1), 0, 1);
               // curSelect.tileEntity.pos = Coordinate.ToTile((int)Game1.MouseScreenPos.X, (int)Game1.MouseScreenPos.Y).ToVector2();

          //      MapTiles.Add(curSelect.tileEntity);

            }

            if (edit == EditObject.Building)
            {
                int[,] mat = GameEnums.BuildingDictionary[selected.GetCurrentAnimationModelPath()];

                curSelect.spriteEntity = Building.ParseToInstance(selected.GetCurrentAnimationModelPath(), int.MaxValue / 2, int.MaxValue / 2);
            
            
            }

            if (edit == EditObject.Enemy)
            {
                string url = GameEnums.VillianDictionary[selected.GetCurrentAnimationModelPath()];

                Villain_st villain_St;
                villain_St.path = url;
                villain_St.dir = 0;
                villain_St.Patrol = null; // -> 우클릭으로 해야됨

                curSelect.spriteEntity = Villain.ParseToInstance(villain_St, int.MaxValue / 2, int.MaxValue / 2,GameEnums.Villian_type(selected.GetCurrentAnimationModelPath()));


            }


        }

        public void MapSizeChangeButtonClicked()
        {
            CurrentMap.Init(int.Parse(MapWidthSizeInput.GetInput()), int.Parse(MapHeightSizeInput.GetInput()));
        }
        public void SaveButtonClicked()
        {
        //      public List<List<Tuple<int, int>>> SpriteList;
        //public List<List<Object>> SpriteInfo;

        //public List<List<Tuple<int, int, string>>> TileList;
        //public string name;


        //CurrentMap.SpriteInfo = 

            
            if (MapNameInput.GetInput() == null || MapNameInput.GetInput().Equals(""))
            {
                throw new Exception("ERR");
            }

            CurrentMap.name = MapNameInput.GetInput();


            for (int i = 0; i < MapTiles.Count; i++)
            {
                Tile tile = MapTiles[i];
                Point pos = tile.pos.ToPoint();



                Tuple<int, int, string> tuple = new Tuple<int,int,string>((int)tile.dims.X, (int)((0.01f+ tile.angle) / MathHelper.PiOver4), tile.url);

                CurrentMap.TileList[pos.Y][pos.X] = tuple;
            
            }

            for (int i = 0; i < MapBuilding.Count; i++)
            {
                Building building = MapBuilding[i];
                Point pos = building.pos.ToPoint();

                //mapbuild에서는 base에서 2번째는 안씀

                Tuple<int, int> tuple_base = new Tuple<int, int>((int)GameEnums.Type.Buliding,0);
                Tuple<Object> tuple_info = new Tuple<Object>(Building.ParseToObj(building));
                CurrentMap.SpriteList[pos.Y][pos.X] = tuple_base;
                CurrentMap.SpriteInfo[pos.Y][pos.X] = tuple_info;

            }

            for (int i = 0; i < MapHeros.Count; i++)
            {
                Heros hero = MapHeros[i];
                Point pos = hero.pos.ToPoint();

                //mapbuild에서는 base에서 2번째는 안씀
                
                Tuple<int, int> tuple_base = new Tuple<int, int>(GameEnums.Type_ret(hero).X, GameEnums.Type_ret(hero).Y);
             //   Tuple<Object> tuple_info = new Tuple<Object>(Hero.ParseToObj(building));
                CurrentMap.SpriteList[pos.Y][pos.X] = tuple_base;
              //  CurrentMap.SpriteInfo[pos.Y][pos.X] = tuple_info;

            }


            for (int i = 0; i < MapVillain.Count; i++)
            {
                Villain villain = MapVillain[i];
                Point pos = villain.pos.ToPoint();

                //mapbuild에서는 base에서 2번째는 안씀

                Tuple<int, int> tuple_base = new Tuple<int, int>(GameEnums.Type_ret(villain).X, GameEnums.Type_ret(villain).Y);
                Tuple<Object> tuple_info = new Tuple<Object>(Villain.ParseToObj(villain));
                CurrentMap.SpriteList[pos.Y][pos.X] = tuple_base;
                CurrentMap.SpriteInfo[pos.Y][pos.X] = tuple_info;

            }






            Save.Instance.SaveMapData(CurrentMap, MapNameInput.GetInput());




        }

        private bool AlreadySpriteExist(Point val)
        {
       

            for (int i = 0; i < MapBuilding.Count; i++)
            {
                if (val.X == (int)MapBuilding[i].pos.X && val.Y == (int)MapBuilding[i].pos.Y)
                {
                    return true;
                }
            }

            for (int i = 0; i < MapVillain.Count; i++)
            {
                if (val.X == (int)MapVillain[i].pos.X && val.Y == (int)MapVillain[i].pos.Y)
                {
                    return true;
                }
            }

            for (int i = 0; i < MapHeros.Count; i++)
            {
                if (val.X == (int)MapHeros[i].pos.X && val.Y == (int)MapHeros[i].pos.Y)
                {
                    return true;
                }
            }


            return false;
        }

        public void Update()
        {
            Vector2 CursorPos = Coordinate.ToOffset(Game1.MouseScreenPos);

         

            for (int i = 0; i < Enum.GetValues(typeof(UIButtons)).Length; i++)
            {
                Buttons[i].Update();
            }

            MapNameInput.Update();
            MapWidthSizeInput.Update();
            MapHeightSizeInput.Update();

            //if (RbClicked)
            //{
            //    RbOkButton.ForceUpdate(CursorPos);
            //    RbCancleButton.ForceUpdate(CursorPos);

            //    for (int i = 0; i < RbInput_Cnt; i++)
            //    {
            //        if (RbInput[i] == null) break;
            //        RbInput[i].ForceUpdate(CursorPos);
            //    }
            //}

            //if (FlatMouse.Instance.IsRightMouseButtonPressed() && !RbClicked)
            //{
            //    RightButtonClicked();
            //}

            for (int i = 0; i < drawUIList.Count; i++)
            {
                drawUIList[i].Update();
            }

            //for (int i = 0; i < drawUIList.Count; i++)
            //{
            //    MapSprite[i].Update();
            //}

            



            if (FlatMouse.Instance.IsLeftMouseButtonDown())
            {
                Rectangle rect = new Rectangle(0, 0, Game1.screen_width, (int)(CurPos.Y + Button_Dims.Y));

                if (!rect.Contains(CursorPos))
                {
                    Point moustPoint = Coordinate.ToTile((int)Game1.MouseScreenPos.X, (int)Game1.MouseScreenPos.Y);

                 

                    if (Coordinate.Instance.boundary_check(moustPoint))
                    {

                        if (edit == EditObject.Erase)
                        {


                            //찾아서 삭제
                        }

                        else if (edit == EditObject.Tile)
                        {
                            Tile tile = curSelect.tileEntity.Clone();
                            tile.pos = moustPoint.ToVector2();


                            bool flag = false;

                            if (!AlreadySpriteExist(moustPoint)) 
                            {


                                for (int i = 0; i < MapTiles.Count; i++)
                                {
                                    if ((int)tile.pos.X == (int)MapTiles[i].pos.X && (int)tile.pos.Y == (int)MapTiles[i].pos.Y)
                                    {
                                        flag = true;
                                        break;
                                    }
                                }

                                if (!flag)
                                    MapTiles.Add(tile);
                            }

                        }

                        else if (edit == EditObject.Building)
                        {
                            if (curSelect.spriteEntity is not Building _build)
                            {
                                throw new Exception("err");
                            }

                            Building building = Building.Clone(_build);

                            building.pos = moustPoint.ToVector2();

                            if (!AlreadySpriteExist(moustPoint))
                            {
                                MapBuilding.Add(building);
                            
                            }


                        }

                        else if (edit == EditObject.Enemy)
                        {
                            if (curSelect.spriteEntity is not Villain _enemy)
                            {
                                throw new Exception("err");
                            }

                            int type = -1;

                            type = _enemy is Zombie ? (int)GameEnums.Enemy.zombie : type;
                            type = _enemy is Solidier ? (int)GameEnums.Enemy.solidier : type;

                            Villain villain = Villain.Clone(_enemy,type);

                            villain.pos = moustPoint.ToVector2();

                            if (!AlreadySpriteExist(moustPoint))
                            {
                                MapVillain.Add(villain);

                            }



                        }



                        else
                        {
                            male Male = new male("Character\\Animation\\Male\\male_default", new Vector2(moustPoint.X, moustPoint.Y), 1);
                            MapHeros.Add(Male);
                        }
                    }


                 
                }

            }

            if (FlatKeyboard.Instance.IsKeyAvailable)
            {        // update에서 
                     // Q 왼쪽 회전 E 오른쪽 회전 
                     // R 사이즈증가
                     // T 사이즈 감소 (최소 1)
                if (FlatKeyboard.Instance.IsKeyDown(Keys.Q))
                { 
                    if(edit == EditObject.Tile)
                    {
                        if (curSelect.tileEntity != null)
                        {
                            curSelect.tileEntity.angle_add(-2);

                        
                        }
                    }
                
                }

                if (FlatKeyboard.Instance.IsKeyDown(Keys.E))
                {
                    if (edit == EditObject.Tile)
                    {
                        if (curSelect.tileEntity != null)
                        {
                            curSelect.tileEntity.angle_add(2);

                        }
                    }

                }

                if (FlatKeyboard.Instance.IsKeyDown(Keys.R))
                {
                    if (edit == EditObject.Tile)
                    {
                        if (curSelect.tileEntity != null)
                        {
                            curSelect.tileEntity.dims_add(1);
                        }
                    }

                }

                if (FlatKeyboard.Instance.IsKeyDown(Keys.T))
                {
                    if (edit == EditObject.Tile)
                    {
                        if (curSelect.tileEntity != null)
                        {
                            curSelect.tileEntity.dims_add(-1);


                        }
                    }

                }


            }

        }

        public void Draw(Sprites sprite)
        {

            for (int i = 0; i < MapTiles.Count; i++)
            {
                MapTiles[i].Draw(sprite);
               
            }

            
            for (int i = 0; i < MapHeros.Count; i++)
            {
                MapHeros[i].Draw(sprite);
               
            }

          

            for (int i = 0; i < MapBuilding.Count; i++)
            {
                MapBuilding[i].Draw(sprite);
               
            }

            for (int i = 0; i < MapVillain.Count; i++)
            {

                MapVillain[i].Draw(sprite);

            }

            for (int i = 0; i < drawUIList.Count; i++)
            {
                drawUIList[i].offsetfixed = true;
                drawUIList[i].cameraused = true;
                drawUIList[i].Draw(sprite);
            }


            for (int i = 0; i < Enum.GetValues(typeof(UIButtons)).Length; i++)
            {
                Buttons[i].offsetfixed = true;
                Buttons[i].cameraused = true;
                Buttons[i].Draw(sprite);
            }

            MapNameInput.Draw(sprite);
            MapWidthSizeInput.Draw(sprite);
            MapHeightSizeInput.Draw(sprite);

            //if (RbClicked)
            //{
            //    RbOkButton.Draw(sprite);
            //    RbCancleButton.Draw(sprite);

            //    for (int i = 0; i < RbInput_Cnt; i++)
            //    {
            //        if (RbInput[i] == null) break;
            //        RbInput[i].Draw(sprite);
            //    }

            //}




        }
    }
}
