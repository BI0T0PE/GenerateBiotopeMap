using BiotopeMap.GetNoise;
using System;
using System.Collections.Generic;
using System.Linq;
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

            public NoiseArray GenerateRiver()
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
                    mode = NoiseValueMode.binary,
                };
                return noise;

            }
            public void generate(int x, int y)
            {
                while (true)
                {
                    bool flag = false;
                    int nextX = 0;
                    int nextY = 0;
                    for (var a = -1; a <= 1; a++)
                    {
                        for (var b = -1; b <= 1; b++)
                        {
                            if (a == b && a == 0)
                            {
                                flag=false;
                            }
                            else if (x + a < 0 || y + b < 0)
                            {
                                flag = false;
                            }
                            else if ((x + a) >= river.array.Count || (y + b) >= river.array[x].Count)
                            {
                                flag = false;
                            }
                            else if (land[x + a][y + b].blocks == TerraBlocks.Water)
                            {
                                flag = false;
                            }
                            else
                            {
                                if (land[x][y].height >= land[x + a][y + b].height)
                                {
                                    RiverArray[x + a][y + b] = 1;
                                    //generate(x + a, y + b);
                                    x=x + a;
                                    y=y + b;
                                    flag = true;
                                }
                                else
                                {
                                    RiverArray[x + a][y + b] = 0;
                                    flag= false;
                                }
                            }
                        }
                    }
                    if (!flag)
                    {
                        return;
                    }
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
                            terra.blocks = TerraBlocks.Water;
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
