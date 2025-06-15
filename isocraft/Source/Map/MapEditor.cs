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
        private List<SpriteEntity> MapSprite = new();
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


        private St_CurSelect curSelect;
        private EditObject edit = EditObject.None;

        public Texture2D bkg;



        private enum UIButtons
        {
            DeleteButton,
            SaveButton,
            LoadButton,
            ExitButton,
            MapSizeChangeButton,
            TileSetButton,
        };

        private string[] buttons_str = {
            "Clean",
            "Save",
            "Load",
            "Back",
            "MpCg",
            "Tile",
            "Erase",

        };
        private Save save;

        public void Init(int width, int height)
        {
            CurrentMap = new Map();
            CurrentMap.Init(width, height);
        }




        public MapEditor(int width, int height)
        {
            bkg = Game1._Instance.Content.Load<Texture2D>("UI\\solid");

            CurrentMap = new Map();
            CurrentMap.Init(width, height);
            save = new Save();
            AdditionalInput = new TextInputBox[RbInput_Cnt];
            Buttons = new Button[Enum.GetValues(typeof(UIButtons)).Length];
            buttons_action = new Action[] { DeleteButtonClicked, SaveButtonClicked, LoadButtonClicked, BackButtonClicked, MapSizeChangeButtonClicked, TileSetButtonClicked, EraseModeClicked };

            for (int i = 0; i < buttons_action.Length; i++)
            {
                Buttons[i] = new Button(Button.Button_path, UIScreenPos(Button_Dims, 1), Button_Dims, true, 0, 1, new Vector2(1, 1), 1, 1000, buttons_str[i]);
                Buttons[i].set_Action(buttons_action[i]);
            }

            MapNameInput = new TextInputBox(null, "MapName", UIScreenPos(Button_Dims, 3), Button_Dims * 3, true);
            MapWidthSizeInput = new TextInputBox(null, " MapWidth", UIScreenPos(Button_Dims, 1), Button_Dims, true);
            MapHeightSizeInput = new TextInputBox(null, "MapHeight", UIScreenPos(Button_Dims, 1), Button_Dims, true);
        }

        public string[] Read_ContentFile(string Folder)
        {
            string folderPath = Path.Combine(Game1._Instance.Content.RootDirectory, Folder);

            string[] files = Directory.GetFiles(folderPath, "*.png");
            return files;
        }

        private void Select_Reset()
        {
            curSelect.tileEntity = null;
            curSelect.spriteEntity = null;

        }

        private Vector2 UIScreenPos(Vector2 Dims, int size)
        {
            Vector2 tmp = new Vector2(CurPos.X + Dims.X * size, CurPos.Y);

            if (tmp.X >= Game1.screen_width)
            {
                tmp = new Vector2(100, CurPos.Y + Button_Dims.Y);
            }

            return CurPos = tmp;
        }

        public void DeleteButtonClicked()
        {
            CurrentMap.Clear();
        }
        public void BackButtonClicked()
        {
            Game1._game_Status = Game1.Game_Status.Game_Menu;
        }

        public void LoadButtonClicked()
        {

            sdfds
            if (MapNameInput.GetInput() == null || MapNameInput.GetInput().Equals(""))
            {
                CurrentMap = save.LoadMapData("Test");
            }

            else
            {
                CurrentMap = save.LoadMapData(MapNameInput.GetInput());
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

            MapSprite.Clear();

            string[] Url = Read_ContentFile("Tiles");

            for (int i = 0; i < Url.Length; i++)
            {
                drawUIList.Add(new Button(Url[i], UIScreenPos(Button_Dims, 2), Button_Dims * 2, true, 0, 1, new Vector2(1, 1), 1, 1));
                drawUIList[i].set_Action(UISelected);
            }
        }

        public void EraseModeClicked()
        {
            Select_Reset();
            edit = EditObject.Erase;
            MapSprite.Clear();

        }

        public void UISelected(Button selected)
        {

            if (edit == EditObject.Tile)
            {
                curSelect.tileEntity = new Tile(selected.GetCurrentAnimationModelPath(), new Vector2(int.MaxValue / 2, int.MaxValue / 2), new Vector2(1, 1), 0, 1);

            }

        }




        public void MapSizeChangeButtonClicked()
        {
            CurrentMap.Init(int.Parse(MapWidthSizeInput.GetInput()), int.Parse(MapHeightSizeInput.GetInput()));
        }
        public void SaveButtonClicked()
        {
            sadasd
            if (MapNameInput.GetInput() == null || MapNameInput.GetInput().Equals(""))
            {
                save.SaveMapData(CurrentMap, "Test");
                return;
            }

            save.SaveMapData(CurrentMap, MapNameInput.GetInput());
        }

        public void Update()
        {
            Vector2 CursorPos = Game1.MouseScreenPos;

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

            for (int i = 0; i < drawUIList.Count; i++)
            {
                MapSprite[i].Update();
            }



            if (FlatMouse.Instance.IsLeftMouseButtonDown())
            {
                Rectangle rect = new Rectangle(0, 0, Game1.screen_width, (int)(CurPos.Y + Button_Dims.Y));

                if (!rect.Contains(CursorPos))
                {
                    Point moustPoint = Coordinate.ToTile((int)CursorPos.X, (int)CursorPos.Y);

                    if (edit == EditObject.Erase)
                    {


                        //찾아서 삭제
                    }

                    else if (edit == EditObject.Tile)
                    {
                        curSelect.tileEntity.pos = moustPoint.ToVector2();

                        MapTiles.Add(curSelect.tileEntity);

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
                            curSelect.tileEntity.Change_Dir(-2);
                        
                        }
                    }
                
                }

                if (FlatKeyboard.Instance.IsKeyDown(Keys.E))
                {
                    if (edit == EditObject.Tile)
                    {
                        if (curSelect.tileEntity != null)
                        {
                            curSelect.tileEntity.Change_Dir(2);

                        }
                    }

                }

                if (FlatKeyboard.Instance.IsKeyDown(Keys.R))
                {
                    if (edit == EditObject.Tile)
                    {
                        if (curSelect.tileEntity != null)
                        {
                            curSelect.tileEntity.Change_Dims(new Point(1, 1));
                        }
                    }

                }

                if (FlatKeyboard.Instance.IsKeyDown(Keys.T))
                {
                    if (edit == EditObject.Tile)
                    {
                        if (curSelect.tileEntity != null)
                        {
                            curSelect.tileEntity.Change_Dims(new Point(-1, -1));

                        }
                    }

                }


            }

        }

        public void Draw(Sprites sprite)
        {

            sprite.Draw(bkg, new Rectangle(0, 0, Game1.screen_width, Game1.screen_height), Color.White);

            sprite.Draw(bkg, new Rectangle(0, 0, Game1.screen_width, (int)(CurPos.Y + Button_Dims.Y)), Color.Aqua);

            for (int i = 0; i < drawUIList.Count; i++)
            {
                drawUIList[i].Draw(sprite);
            }

            for (int i = 0; i < MapSprite.Count; i++)
            {
                MapSprite[i].Draw(sprite);
            }

            for (int i = 0; i < MapTiles.Count; i++)
            {
                MapTiles[i].Draw(sprite);
            }


            for (int i = 0; i < Enum.GetValues(typeof(UIButtons)).Length; i++)
            {
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
