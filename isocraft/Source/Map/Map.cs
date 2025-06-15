using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace isocraft
{
    public class Map
    {
        public int width;
        public int height;

        public List<List<Tuple<int, int>>> SpriteList;
        public List<List<Object>> SpriteInfo;

        public List<List<Tuple<int, int,string>>> TileList;
        public string name;



        //[JsonConstructor]
        //public Map(int map_width, int map_height)
        //{
        //    Change_MapSize(map_width, map_height);
        //}


        public void Map_Update(int x, int y, Tuple<int, int> val)
        {
            if (y >= height || y < 0 || x < 0 || x >= width) return;

            SpriteList[y][x] = val;
        }

        public void Clear()
        {
            TileList.Clear();
            SpriteList.Clear();
            SpriteInfo.Clear();
        }



        public void Set_MapSize(int width, int height)
        {
            this.width = width;
            this.height = height;

        }
      
        public void Init(int width, int height)
        {
            Set_MapSize(width, height);
            Change_MapSize();
        }




        private void Change_MapSize()
        {
            SpriteList = new();
            SpriteInfo = new();
            TileList = new();

            // Map_width와 Map_height에 맞게 (0, 0)으로 초기화
            for (int i = 0; i < height; i++)  // 높이만큼 반복
            {
                var row = new List<Tuple<int, int>>();
                var row2 = new List<Object>();
                var row3 = new List<Tuple<int, int, string>>();

                for (int j = 0; j < width; j++)  // 너비만큼 반복
                {
                    row.Add(Tuple.Create(0, 0));  // 각 (x, y)를 (0, 0)으로 설정
                    row2.Add(null);
                    row3.Add(Tuple.Create(0, 0, ""));
                }
                SpriteList.Add(row);  // 한 줄을 Deploy에 추가
                SpriteInfo.Add(row2);
                TileList.Add(row3);
            }
        }





    }
}
