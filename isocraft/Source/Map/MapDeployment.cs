using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isocraft
{

    // MapDeployment -> EntityManager & TileSet -> Coordinate -> BFS


    public class MapDeployment
    {

        private static MapDeployment _instance;
        public Map StageMap;

        private MapDeployment()
        {

        }

        public static MapDeployment Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MapDeployment();
                return _instance;
            }
        }

        public void LoadAndUpdate(string filename)
        {
            if (filename == null)
            {
                throw new Exception("ERR");
            }
            EntityManager.Clear();

        ;

            StageMap = Save.Instance.LoadMapData(filename);

            //            public List<List<Tuple<int, int>>> SpriteList;
            //public List<List<Object>> SpriteInfo;

            //public List<List<Tuple<int, int, string>>> TileList;
            //public string name;

         //   coordinate에 tilemapping이랑 info저장해야됨


            for (int i = 0; i < StageMap.TileList.Count; i++)
            {
                for (int j = 0; j < StageMap.TileList[i].Count; j++)
                {

                    Tuple<int, int, string> tuple = StageMap.TileList[i][j];

                    if (tuple.Item3 == null || tuple.Item3.Equals("")) continue;

                    Tile tile = new Tile(StageMap.TileList[i][j].Item3, new Vector2(j, i), new Vector2(StageMap.TileList[i][j].Item1, StageMap.TileList[i][j].Item1), StageMap.TileList[i][j].Item2, 1);
                    TileMap.Add_Tile(tile, 1);

                }
            }

          

            for (int i = 0; i < StageMap.SpriteList.Count; i++)
            {
                for (int j = 0; j < StageMap.SpriteList[i].Count; j++)
                {
                    Tuple<int, int> tuple = StageMap.SpriteList[i][j];

                    if (tuple.Item1 != (int)GameEnums.Type.None)
                    {
                        switch ((int)tuple.Item1)
                        {
                            case (int)GameEnums.Type.Wall:

                                break;
                            case (int)GameEnums.Type.Buliding:

                                Building building = Building.ParseToInstance(StageMap.SpriteInfo[i][j], j, i);
                                EntityManager.AddBuildings(building);
                                break;
                            case (int)GameEnums.Type.Hero:

                                switch ((int)tuple.Item2) {
                                    case (int)GameEnums.Hero.male:

                                        male Male = new male("Character\\Animation\\Male\\male_default", new Vector2(j, i), 1);
                                        EntityManager.AddHero(Male);

                                      
                                        break;
                                }


                                break;
                            case (int)GameEnums.Type.Enemy:

                                Villain villain = Villain.ParseToInstance(StageMap.SpriteInfo[i][j], j, i, (int)tuple.Item2);
                                EntityManager.AddEnemys(villain);



                                break;

                        }
                    
                    }

                
                }
            
            }

            Coordinate.Instance.Init(StageMap.width, StageMap.height);


            //Enemy,
            //Hero


        }

   


    }
}
