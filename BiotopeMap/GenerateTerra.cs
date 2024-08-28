using BiotopeMap.GetNoise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BiotopeMap
{
    namespace GenerateTerra
    {
        public class GenerateTerra
        {
            private List<List<TerraInfo>> land;
            private NoiseArray river;
            private double[][] RiverArray;
            public GenerateTerra(TerraArrayList terra)
            {
                land = terra.baseLand;
                river = terra.river;
                RiverArray = new double[river.array.Count][];

            }

            public List<List<TerraInfo>> GenerateRiver()
            {
                List<List<int>> list = new List<List<int>>();
                if (river.mode == NoiseValueMode.binary)
                {
                    for (var i = 0; i < river.array.Count; i++)
                    {
                        RiverArray[i] = new double[river.array[i].Count];
                    }
                    for (var i = 0; i < river.array.Count; i++)
                    {
                        //RiverArray[i] = new double[river.array[i].Count];
                        for (var j = 0; j < river.array[i].Count; j++)
                        {
                            if (river.array[i][j] == 1)
                            {
                                generate(i, j);
                            }
                        }
                    }
                }
                List<List<double>> ans = new();
                Console.WriteLine(RiverArray.Length);
                for (var i = 0; i < RiverArray.Length; i++)
                {
                    ans.Add(new List<double>(RiverArray[i]));
                }

                NoiseArray noise = new NoiseArray()
                {
                    array = ans,
                    mode = NoiseValueMode.gradation256,
                };
                return land;

            }
            public void generate(int x, int y, int vx = 0, int vy = 0,float rate=0)
            {
                while (true)
                {
                    bool flag = false;
                    bool[] landflag = new bool[9];
                    Array.Fill(landflag, false);
                    (double maxh, int a, int b,bool flag) maxs = new(0, 0, 0,false);
                    for (var a = -1; a <= 1; a++)
                    {
                        for (var b = -1; b <= 1; b++)
                        {
                            int vec = vx * a + vy * b;
                            if (a == b && a == 0)
                            {
                                //flag = false;
                                continue;
                            }
                            else if (x + a < 0 || y + b < 0)
                            {
                                continue;
                                //flag = false;
                            }
                            else if ((x + a) >= river.array.Count || (y + b) >= river.array[x].Count)
                            {
                                continue;
                                //flag = false;
                            }
                            else if (land[x + a][y + b].blocks == TerraBlocks.Water||land[x + a][y + b].blocks == TerraBlocks.Sea)
                            {
                                //flag = false;
                                continue;
                            }
                            else if (land[x][y].height + vec*1.2 >= land[x + a][y + b].height)
                            {
                                //RiverArray[x + a][y + b] = 1;
                                land[x + a][y + b].height = land[x + a][y + b].height - 0.04;
                                land[x + a][y + b].blocks = TerraBlocks.Water;
                                generate(x + a, y + b, a, b);
                                //x = x + a;
                                //y = y + b;
                                flag = true;
                                maxs.flag = true;
                                //break;
                            }
                            else
                            {
                                if (maxs.maxh < land[x+a][y+b].height-vec*1.2)
                                {
                                    maxs.maxh = land[x + a][y+b].height;
                                    maxs.a = a;
                                    maxs.b = b;
                                }
                                //RiverArray[x + a][y + b] = 0;
                                //flag = false;
                                continue;
                            }
                        }
                    }
                    if (!maxs.flag)
                    {
                        Task.Run(()=>generate(x+maxs.a,y+maxs.b,maxs.a,maxs.b));
                    }
                    if (flag) break;
                    else return;
                }


            }
        }
        public class TerraArrayList
        {
            public List<List<TerraInfo>> baseLand { get; set; } = new();
            public NoiseArray river { get; set; } = new NoiseArray();
            public List<List<TerraInfo>> ConvertTerraInfo(NoiseArray array, int h)
            {

                List<List<TerraInfo>> land = new();
                for (int x = 0; x < array.array.Count; x++)
                {
                    TerraInfo[] data = new TerraInfo[array.array[x].Count];
                    for (int y = 0; y < array.array[x].Count; y++)
                    {
                        TerraInfo terra = new();
                        if (array.array[x][y] > h)
                        {
                            terra.blocks = TerraBlocks.Gland;
                        }
                        else
                        {
                            terra.blocks = TerraBlocks.Sea;
                        }
                        terra.height = (int)array.array[x][y];
                        data[y] = terra;
                    }
                    land.Add(new List<TerraInfo>(data));
                }
                return land;
            }
        }
    }
}
