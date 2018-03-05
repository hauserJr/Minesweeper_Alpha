using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper_Alpha
{
    class Program
    {
        //畫面配置X,Y
        static int Position_x = 9;
        static int Position_y = 9;

        //畫面呈現總格數
        static int PlaidAmount = Position_x * Position_y;

        //畫面
        static string[,] BoomsCanvas = new string[Position_x, Position_y];

        //炸彈位置
        List<BoomsPosition> _BoomsPosition = new List<BoomsPosition>();
        List<NumberPosition> _NumberPosition = new List<NumberPosition>();
        List<SaveAreaPosition> _SaveAreaPosition = new List<SaveAreaPosition>();

        List<CheckArea> _CheckArea = new List<CheckArea>();
        static void Main(string[] args)
        {
            Program _Program = new Program();
            _Program.GameInit();

            //Show
            _Program.ShowCanvas();
            string CheckArea = Console.ReadLine();
            if (CheckArea.Equals("T"))
            {
                while (true)
                {
                    string CheckXY = Console.ReadLine();
                    var x = CheckXY.Split(',')[0];
                    var y = CheckXY.Split(',')[1];
                    _Program.SeedFill(int.Parse(x), int.Parse(y));
                    _Program.ShowCanvas();
                    Console.ReadLine();
                }
            }
            
        }

        public void ShowCanvas()
        {
            //Show
            for (int x = 0; x < Position_x; x++)
            {
                for (int y = 0; y < Position_y; y++)
                {
                    Console.Write("  " + BoomsCanvas[x, y]);
                }
                Console.Write("\n\r");
            }
        }
        public void SeedFill(int x, int y)
        {
            var Query = _SaveAreaPosition.Where(o => o.SaveArea_x == x && o.SaveArea_y == y).FirstOrDefault();
            bool AnyQuery = _SaveAreaPosition.Where(o => o.SaveArea_x == x && o.SaveArea_y == y).Any();
            _SaveAreaPosition.Remove(Query);

            //Open         
            if (AnyQuery)
            {
                AroundOpen(x, y);
                SeedFill(x, y);
                SeedFill(x - 1, y);
                SeedFill(x + 1, y);
                SeedFill(x, y - 1);
                SeedFill(x, y + 1);
            }
        }

        public void AroundOpen( int x, int y)
        {
            //上下左右
            var top = _NumberPosition.Where(o => o.Number_x == x - 1 && o.Number_y == y);
            var down = _NumberPosition.Where(o => o.Number_x == x + 1 && o.Number_y == y);
            var left = _NumberPosition.Where(o => o.Number_x == x && o.Number_y == y - 1);
            var right = _NumberPosition.Where(o => o.Number_x == x && o.Number_y == y + 1);

            //左上 左下 右上 右下
            var tl = _NumberPosition.Where(o => o.Number_x == x - 1 && o.Number_y == y - 1);
            var tr = _NumberPosition.Where(o => o.Number_x == x + 1 && o.Number_y == y - 1);
            var dl = _NumberPosition.Where(o => o.Number_x == x - 1 && o.Number_y == y + 1);
            var dr = _NumberPosition.Where(o => o.Number_x == x + 1 && o.Number_y == y + 1);

            //上下左右
            if (top.Any())
            {
                BoomsCanvas[x - 1, y] = "x";
                _NumberPosition.Remove(top.FirstOrDefault());
            }
            if (down.Any())
            {
                BoomsCanvas[x + 1, y] = "x";
                _NumberPosition.Remove(down.FirstOrDefault());
            }
            if (left.Any())
            {
                BoomsCanvas[x, y - 1] = "x";
                _NumberPosition.Remove(left.FirstOrDefault());
            }
            if (right.Any())
            {
                BoomsCanvas[x, y + 1] = "x";
                _NumberPosition.Remove(right.FirstOrDefault());
            }

            if (tl.Any())
            {
                BoomsCanvas[x - 1, y - 1] = "x";
                _NumberPosition.Remove(tl.FirstOrDefault());
            }
            if (tr.Any())
            {
                BoomsCanvas[x + 1, y - 1] = "x";
                _NumberPosition.Remove(tr.FirstOrDefault());
            }
            if (dl.Any())
            {
                BoomsCanvas[x - 1, y + 1] = "x";
                _NumberPosition.Remove(dl.FirstOrDefault());
            }
            if (dr.Any())
            {
                BoomsCanvas[x + 1, y + 1] = "x";
                _NumberPosition.Remove(dr.FirstOrDefault());
            }
        }

        public void GameInit()
        {
            this.SetBooms();
            this.SetPointNumber();
            this.SetSaveArea();
        }

        public void SetBooms()
        {     
            Random _Random = new Random(Guid.NewGuid().GetHashCode());
            int _RandomMin = Position_x + 1;
            int _RandomMax = Position_y + 3;
            int Booms = _Random.Next(_RandomMin, _RandomMax);
            for (int i = 0; i < Booms; i++)
            {
                bool flag = true;
                while (flag)
                { 
                    _Random = new Random(Guid.NewGuid().GetHashCode());
                    int BoomsPosition_x = _Random.Next(0, Position_x);
                    int BoomsPosition_y = _Random.Next(0, Position_y);
                    if (string.IsNullOrEmpty(BoomsCanvas[BoomsPosition_x, BoomsPosition_y]))
                    {
                        BoomsCanvas[BoomsPosition_x, BoomsPosition_y] = string.Format("*");
                        _BoomsPosition.Add(new BoomsPosition()
                        {
                            Booms_x = BoomsPosition_x,
                            Booms_y = BoomsPosition_y
                        });
                        flag = false;
                    }
                }
            }
        }

        public void SetPointNumber()
        {
            #region 以炸彈為九宮格
            foreach (var _p in _BoomsPosition)
            {
                //1,1
                //00 10 20
                if ((_p.Booms_x - 1) >= 0 && (_p.Booms_y - 1) >= 0)
                {
                    BoomsCanvas[_p.Booms_x - 1, _p.Booms_y - 1] = BoomsAmount(_p.Booms_x - 1, _p.Booms_y - 1);
                }
                if ((_p.Booms_y - 1) >= 0)
                {
                    BoomsCanvas[_p.Booms_x, _p.Booms_y - 1] = BoomsAmount(_p.Booms_x, _p.Booms_y - 1);
                }
                if ((_p.Booms_x + 1) < Position_x && (_p.Booms_y - 1) >= 0)
                {
                    BoomsCanvas[_p.Booms_x + 1, _p.Booms_y - 1] = BoomsAmount(_p.Booms_x + 1, _p.Booms_y - 1);
                }

                //1,1
                //01 21
                if ((_p.Booms_x - 1) >= 0)
                {
                    BoomsCanvas[_p.Booms_x - 1, _p.Booms_y] = BoomsAmount(_p.Booms_x - 1, _p.Booms_y);
                }
                if ((_p.Booms_x + 1) < Position_x)
                {
                    BoomsCanvas[_p.Booms_x + 1, _p.Booms_y] = BoomsAmount(_p.Booms_x +1, _p.Booms_y);
                }

                //1,1
                //02 12 22
                if ((_p.Booms_x - 1) >= 0 && (_p.Booms_y + 1) < Position_y)
                {
                    BoomsCanvas[_p.Booms_x - 1, _p.Booms_y + 1] = BoomsAmount(_p.Booms_x - 1, _p.Booms_y + 1);
                }
                if ((_p.Booms_y + 1) < Position_y)
                {
                    BoomsCanvas[_p.Booms_x, _p.Booms_y + 1] = BoomsAmount(_p.Booms_x, _p.Booms_y + 1);
                }
                if ((_p.Booms_x + 1) < Position_x && (_p.Booms_y + 1) < Position_y)
                {
                    BoomsCanvas[_p.Booms_x + 1, _p.Booms_y + 1] = BoomsAmount(_p.Booms_x + 1, _p.Booms_y + 1);
                }
            }
            #endregion
        }
        public void SetSaveArea()
        {
            for (int x = 0; x < Position_x; x++)
            {
                for (int y = 0; y < Position_y; y++)
                {
                    if (string.IsNullOrEmpty(BoomsCanvas[x,y]))
                    {
                        BoomsCanvas[x, y] = string.Format("-");
                        _SaveAreaPosition.Add(new SaveAreaPosition()
                        {
                            SaveArea_x = x,
                            SaveArea_y = y
                        });
                    }
                }
            }
        }

        //提示炸彈數量確認
        public string BoomsAmount(int BoomX, int BoomY)
        {
            string BoomsPosition = BoomsCanvas[BoomX, BoomY];
            if (string.IsNullOrEmpty(BoomsPosition))
            {
                _NumberPosition.Add(new NumberPosition()
                {
                    Number_x = BoomX,
                    Number_y = BoomY
                });
                return "1";
            }
            if (BoomsPosition.Equals("*"))
            {
                return "*";
            }
            return (int.Parse(BoomsPosition) + 1).ToString();
        }

        public class BoomsPosition
        {
            public int Booms_x { get; set; }

            public int Booms_y { get; set; }
        }

        public class NumberPosition
        {
            public int Number_x { get; set; }

            public int Number_y { get; set; }
        }

        public class SaveAreaPosition
        {
            public int SaveArea_x { get; set; }

            public int SaveArea_y { get; set; }
        }
        public class CheckArea
        {
            public int CheckArea_x { get; set; }

            public int CheckArea_y { get; set; }
        }
    }
}
