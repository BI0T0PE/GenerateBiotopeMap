using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    public class Program
    {
        static Pxy NowPxy = new Pxy();

        public char[][] Map = new char[][]
        {
            new char[5]{ '#','#','#','#','#' },
            new char[5]{ '#',' ',' ',' ','#' },
            new char[5]{ '#',' ','#',' ','#' },
            new char[5]{ '#',' ','#','G','#' },
            new char[5]{ '#','#','#','#','#' },

        };

        public static char[][] ViewMap = new char[5][]
        {
            new char[5]{' ',' ',' ', ' ', ' ' },
            new char[5]{' ',' ',' ', ' ', ' ' },
            new char[5]{' ',' ',' ', ' ', ' ' },
            new char[5]{' ',' ',' ', ' ', ' ' },
            new char[5]{' ',' ',' ', ' ', ' ' },
        };

        static void DrawMap()
        {
            for (int i = 0; i < ViewMap.Length; i++)
            {
                for (int j = 0; j < ViewMap[i].Length; j++)
                {
                    Console.Write(ViewMap[i][j]);
                }
                Console.Write(System.Environment.NewLine);
            }
        }

        static Pxy GetP()
        {
            Pxy pxy = new Pxy();
            for (int i = 0; i < ViewMap.Length; i++)
            {
                for (int j = 0; j < ViewMap[i].Length; j++)
                {
                    if (ViewMap[i][j] == 'P')
                    {
                        pxy.x = i;
                        pxy.y = j;
                    }
                }
            }
            return pxy;
        }

        public char[][] MoveP(string key)
        {
            switch (key)
            {
                case "w":
                    if (NowPxy.x - 1 >= 0 && Map[NowPxy.x - 1][NowPxy.y] != '#')
                    {
                        ViewMap[NowPxy.x][NowPxy.y] = Map[NowPxy.x][NowPxy.y];
                        ViewMap[NowPxy.x - 1][NowPxy.y] = 'P';
                    }
                    break;
                case "a":
                    if (NowPxy.y - 1 >= 0 && Map[NowPxy.x][NowPxy.y - 1] != '#')
                    {
                        ViewMap[NowPxy.x][NowPxy.y] = Map[NowPxy.x][NowPxy.y];
                        ViewMap[NowPxy.x][NowPxy.y - 1] = 'P';
                    }
                    break;
                case "s":
                    if (NowPxy.x + 1 < Map.Length && Map[NowPxy.x + 1][NowPxy.y] != '#')
                    {
                        ViewMap[NowPxy.x][NowPxy.y] = Map[NowPxy.x][NowPxy.y];
                        ViewMap[NowPxy.x + 1][NowPxy.y] = 'P';
                    }
                    break;
                case "d":
                    if (NowPxy.y + 1 < Map.Length && Map[NowPxy.x][NowPxy.y + 1] != '#')
                    {
                        ViewMap[NowPxy.x][NowPxy.y] = Map[NowPxy.x][NowPxy.y];
                        ViewMap[NowPxy.x][NowPxy.y + 1] = 'P';
                    }
                    break;
            }
            return ViewMap;
        }

        static void Main()
        {
            //gti.TerraNoise = 0.1;
            //MapParameter a = gti.GetRangeforNoise(gti.TerraMapPrameters, 0.1);
            //Console.WriteLine(a.Name);
            /*
            Program program = new Program();
            program.BakeMap();
            ViewMap[3][1] = 'P';
            while (true)
            {
            NowPxy.x = GetP().x;
            NowPxy.y = GetP().y;
                DrawMap();
                string key = Console.ReadLine();
                program.MoveP(key);
            }
        }
        public void BakeMap()
        {
            for(int i = 0; i < Map.Length; i++)
            {
                for(int j = 0; j < Map[i].Length;j++)
                {
                    ViewMap[i][j] = Map[i][j];
                }
            }
        }*/
        }
        class Pxy
        {
            public int x;
            public int y;
        }

    }
}