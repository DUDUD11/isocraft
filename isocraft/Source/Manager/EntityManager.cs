using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isocraft
{
    public static class EntityManager
    {
        
  
        public static List<SpriteEntity> AllEntities = new();
        public static List<Heros> Heroes = new();
        public static List<Villain> Enemys = new();
        public static List<CoverableObject> Objects = new();
  

        //public static List<HeroEntity> Heroes = new();
        //public static List<DoorEntity> Doors = new();
        //public static List<TileEntity> Tiles = new();

        public static Heros FindHero(Point point)
        {
            for (int i = 0; i < Heroes.Count; i++)
            {
                if (Heroes[i].pos.ToPoint().Equals(point))
                {
                    return Heroes[i];

                }
            
            }
            return null;
        
        }

        public static Villain FindEnemy(Point point)
        {
            for (int i = 0; i < Enemys.Count; i++)
            {
                if (Enemys[i].pos.ToPoint().Equals(point))
                {
                    return Enemys[i];

                }

            }
            return null;

        }

        public static CoverableObject FindObject(Point point)
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                if (Objects[i].Mapping(point))
                {
                    return Objects[i];

                }

            }
            throw new Exception("err");

        }

        public static void AddHero(Heros hero)
        {
            AllEntities.Add(hero);
            Heroes.Add(hero);
         //   Coordinate.Instance.Place_Unit(hero.pos.ToPoint(), new Point((int)GameEnums.Type.Hero, (int)GameEnums.Hero.male));
        }

        public static void AddEnemys(Villain villain)
        {
            AllEntities.Add(villain);
            Enemys.Add(villain);
        //    Coordinate.Instance.Place_Unit(villain.pos.ToPoint(), new Point((int)GameEnums.Type.Enemy, (int)GameEnums.Enemy.zombie));
        }

        public static void AddBuildings(CoverableObject coverableObject)
        {
            AllEntities.Add(coverableObject);
            Objects.Add(coverableObject);
      //      Coordinate.Instance.Place_Unit(coverableObject.pos.ToPoint(), new Point((int)GameEnums.Type.Buliding, (int)GameEnums.Objects.building),coverableObject.dims.ToPoint());
        }

        public static void Clear()
        {
            AllEntities.Clear();
            Heroes.Clear();
            Enemys.Clear();
            Objects.Clear();


        }



    }
}
