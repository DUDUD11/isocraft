using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace isocraft
{
    public class UpgradeData
    {
        public int remain_perk;
        public int hero_num;
        public int upgrade_objects;
        public int upgrade_steps;

        public List<List<List<bool>>> UpgradeList;

        public void Clear()
        {
            UpgradeList.Clear();
    
        }

        public void Set_MapSize(int hero_num, int upgradeable_obejcts, int upgrade_steps)
        {
            this.upgrade_objects = upgradeable_obejcts;
            this.hero_num = hero_num;
            this.upgrade_steps = upgrade_steps;
        }

        public void Init(int hero_num, int upgradeable_obejcts, int upgrade_steps)
        {
            Set_MapSize(hero_num,upgradeable_obejcts, upgrade_steps);
            Change_MapSize();
        }

        public void Map_Update(int hero_num, int upgradeable_obejcts, int upgrade_steps,bool flag)
        {
            UpgradeList[hero_num][upgradeable_obejcts][upgrade_steps] = flag;


        }

        private void Change_MapSize()
        {
            UpgradeList = new();
     

            // Map_width와 Map_height에 맞게 (0, 0)으로 초기화
            for (int i = 0; i < hero_num; i++)  // 높이만큼 반복
            {
                var row = new List<List<bool>>();
             

                for (int j = 0; j < upgrade_objects; j++)  // 너비만큼 반복
                {

                    var row2 = new List<bool>();

                    for (int h = 0; h < upgrade_steps; h++)
                    {
                        row2.Add(false);
                    }

                    row.Add(row2);
                }

                UpgradeList.Add(row);


            }
        }






    }
}

