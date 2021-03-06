﻿using System;
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

        //存放各格子資訊
        MinesweeperConfig _MinesweeperConfig;

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
                    _Program.CheckPlaid(int.Parse(x), int.Parse(y));
                    _Program.ShowCanvas();
                }
            }    
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void GameInit()
        {
            Array.Clear(BoomsCanvas,0, BoomsCanvas.Length);
            _MinesweeperConfig = new MinesweeperConfig();

            this.SetBooms();
            this.SetPointNumber();
            this.SetSaveArea();
        }

        /// <summary>
        /// 確認選擇的地方是炸彈或數字,來決定下一步該如何執行
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void CheckPlaid(int x, int y)
        {
            var BoomsQuery = _MinesweeperConfig._BoomsPosition.Where(o => o.Booms_x == x && o.Booms_y == y);
            if (BoomsQuery.Any())
            {
                Console.WriteLine("Game Over \r\n 遊戲刷新");
                GameInit();
                return;
            }
            var NumberPointQuery = _MinesweeperConfig._NumberPosition.Where(o => o.Number_x == x && o.Number_y == y);
            if (NumberPointQuery.Any())
            {
                if (_MinesweeperConfig._NumberPosition.Count == 1)
                {
                    Console.WriteLine("You Win \r\n 遊戲刷新");
                    GameInit();
                    return;
                }
                else
                {
                    BoomsCanvas[x, y] = "0";
                    _MinesweeperConfig._NumberPosition.Remove(NumberPointQuery.FirstOrDefault());
                }
                
                return;
            }
            
            SeedFill(x,y);
        }
                /// <summary>
        /// 利用遞迴尋找以使用者選擇的座標為中心,利用Fill找出其SaveArea區域
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SeedFill(int x, int y)
        {
            var Query = _MinesweeperConfig._SaveAreaPosition.Where(o => o.SaveArea_x == x && o.SaveArea_y == y).FirstOrDefault();
            bool AnyQuery = _MinesweeperConfig._SaveAreaPosition.Where(o => o.SaveArea_x == x && o.SaveArea_y == y).Any();
            _MinesweeperConfig._SaveAreaPosition.Remove(Query);

            //Open         
            if (AnyQuery)
            {
                CheckAround(x, y);

                //利用遞迴檢查SaveArea自身及上下左右
                SeedFill(x, y);
                SeedFill(x - 1, y);
                SeedFill(x + 1, y);
                SeedFill(x, y - 1);
                SeedFill(x, y + 1);
            }
        }

        /// <summary>
        /// 以SaveArea座標為中心,尋找其九宮格是否有數字
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void CheckAround( int x, int y)
        {
            //上下左右
            var top = _MinesweeperConfig._NumberPosition.Where(o => o.Number_x == x - 1 && o.Number_y == y);
            var down = _MinesweeperConfig._NumberPosition.Where(o => o.Number_x == x + 1 && o.Number_y == y);
            var left = _MinesweeperConfig._NumberPosition.Where(o => o.Number_x == x && o.Number_y == y - 1);
            var right = _MinesweeperConfig._NumberPosition.Where(o => o.Number_x == x && o.Number_y == y + 1);

            //數字左上 左下 右上 右下
            var tlNum = _MinesweeperConfig._NumberPosition.Where(o => o.Number_x == x - 1 && o.Number_y == y - 1);
            var trNum = _MinesweeperConfig._NumberPosition.Where(o => o.Number_x == x + 1 && o.Number_y == y - 1);
            var dlNum = _MinesweeperConfig._NumberPosition.Where(o => o.Number_x == x - 1 && o.Number_y == y + 1);
            var drNum = _MinesweeperConfig._NumberPosition.Where(o => o.Number_x == x + 1 && o.Number_y == y + 1);

            //炸彈
            var tlSaveArea = _MinesweeperConfig._SaveAreaPosition.Where(o => o.SaveArea_x == x - 1 && o.SaveArea_y == y - 1);
            var trSaveArea = _MinesweeperConfig._SaveAreaPosition.Where(o => o.SaveArea_x == x + 1 && o.SaveArea_y == y - 1);
            var dlSaveArea = _MinesweeperConfig._SaveAreaPosition.Where(o => o.SaveArea_x == x - 1 && o.SaveArea_y == y + 1);
            var drSaveArea = _MinesweeperConfig._SaveAreaPosition.Where(o => o.SaveArea_x == x + 1 && o.SaveArea_y == y + 1);

            //上下左右
            if (top.Any())
            {
                BoomsCanvas[x - 1, y] = "x";
                _MinesweeperConfig._NumberPosition.Remove(top.FirstOrDefault());
            }
            if (down.Any())
            {
                BoomsCanvas[x + 1, y] = "x";
                _MinesweeperConfig._NumberPosition.Remove(down.FirstOrDefault());
            }
            if (left.Any())
            {
                BoomsCanvas[x, y - 1] = "x";
                _MinesweeperConfig._NumberPosition.Remove(left.FirstOrDefault());
            }
            if (right.Any())
            {
                BoomsCanvas[x, y + 1] = "x";
                _MinesweeperConfig._NumberPosition.Remove(right.FirstOrDefault());
            }

            //數字左上 左下 右上 右下
            if (tlNum.Any())
            {
                BoomsCanvas[x - 1, y - 1] = "x";
                _MinesweeperConfig._NumberPosition.Remove(tlNum.FirstOrDefault());
            }      

            if (trNum.Any())
            {
                BoomsCanvas[x + 1, y - 1] = "x";
                _MinesweeperConfig._NumberPosition.Remove(trNum.FirstOrDefault());
            }

            if (dlNum.Any())
            {
                BoomsCanvas[x - 1, y + 1] = "x";
                _MinesweeperConfig._NumberPosition.Remove(dlNum.FirstOrDefault());
            }

            if (drNum.Any())
            {
                BoomsCanvas[x + 1, y + 1] = "x";
                _MinesweeperConfig._NumberPosition.Remove(drNum.FirstOrDefault());
            }

            //安全區左上 左下 右上 右下
            if (tlSaveArea.Any())
            {
                SeedFill(x - 1, y - 1);
            }

            if (trSaveArea.Any())
            {
                SeedFill(x + 1, y - 1);
            }

            if (dlSaveArea.Any())
            {
                SeedFill(x - 1, y + 1);
            }

            if (drSaveArea.Any())
            {
                SeedFill(x + 1, y + 1);
            }
        }

        /// <summary>
        /// 設置炸彈
        /// </summary>
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
                        _MinesweeperConfig._BoomsPosition.Add(new BoomsPosition()
                        {
                            Booms_x = BoomsPosition_x,
                            Booms_y = BoomsPosition_y
                        });
                        flag = false;
                    }
                }
            }
        }

        /// <summary>
        /// 設置提示數字
        /// </summary>
        public void SetPointNumber()
        {
            #region 以炸彈為中心九宮格
            foreach (var _p in _MinesweeperConfig._BoomsPosition)
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

        /// <summary>
        /// 設置安全區域
        /// </summary>
        public void SetSaveArea()
        {
            for (int x = 0; x < Position_x; x++)
            {
                for (int y = 0; y < Position_y; y++)
                {
                    if (string.IsNullOrEmpty(BoomsCanvas[x,y]))
                    {
                        BoomsCanvas[x, y] = string.Format("-");
                        _MinesweeperConfig._SaveAreaPosition.Add(new SaveAreaPosition()
                        {
                            SaveArea_x = x,
                            SaveArea_y = y
                        });
                    }
                }
            }
        }

        /// <summary>
        /// 提示數字計算
        /// </summary>
        /// <param name="BoomX"></param>
        /// <param name="BoomY"></param>
        /// <returns></returns>
        public string BoomsAmount(int BoomX, int BoomY)
        {
            string BoomsPosition = BoomsCanvas[BoomX, BoomY];
            if (string.IsNullOrEmpty(BoomsPosition))
            {
                _MinesweeperConfig._NumberPosition.Add(new NumberPosition()
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

        /// <summary>
        /// 畫面顯示
        /// </summary>
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
    }
}
