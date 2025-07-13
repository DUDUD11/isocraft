
using Flat.Graphics;
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
        public static List<UIEntity> UIEntities = new();

        public static Circle male_circle;

       


        //public static List<HeroEntity> Heroes = new();
        //public static List<DoorEntity> Doors = new();
        //public static List<TileEntity> Tiles = new();

        public static void Init()
        {



            male_circle = new Circle(new Vector2(Game1._Instance.getZoom() * Game1.screen_width/2, Game1._Instance.getZoom() * Game1.screen_height/2), new Vector2((int)Game1._Instance.getZoom() * Game1.screen_width/2, (int)Game1._Instance.getZoom() * Game1.screen_height/2), false);

            Button button = new Button("UI\\IconOutLine", "UI\\Attack", new Vector2(male_circle.pos.X, male_circle.pos.Y + male_circle.dims.Y / 2 - Game1.screen_height / 32), new Vector2(Game1.screen_width / 16, Game1.screen_height / 16), true, 0, 1,
                new Vector2(1, 1), 1, 100);

            Button button2 = new Button("UI\\IconOutLine2", "UI\\DeadEye", new Vector2(male_circle.pos.X-male_circle.dims.X/2+Game1.screen_width/32, male_circle.pos.Y ), new Vector2(Game1.screen_width / 16, Game1.screen_height / 16), true, 0, 1,
       new Vector2(1, 1), 1, 100);

            Button button3 = new Button("UI\\IconOutLine2", "UI\\doubleAttack", new Vector2(male_circle.pos.X+male_circle.dims.X/2-Game1.screen_width/32, male_circle.pos.Y), new Vector2(Game1.screen_width / 16, Game1.screen_height / 16), true, 0, 1,
       new Vector2(1, 1), 1, 100);

            male_circle.AddButton(button);
            male_circle.AddButton(button2);
            male_circle.AddButton(button3);

            //  circle.AddButton(new Button());
            UIEntities.Add(male_circle);

   


        }

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

        public static void AddUI(UIEntity uIEntity)
        {
            UIEntities.Add(uIEntity);

           
            //   Coordinate.Instance.Place_Unit(hero.pos.ToPoint(), new Point((int)GameEnums.Type.Hero, (int)GameEnums.Hero.male));
        }

        public static void DeleteUI(UIEntity uIEntity)
        {
            UIEntities.Remove(uIEntity);
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
