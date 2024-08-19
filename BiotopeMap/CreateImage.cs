using BiotopeMap.GetNoise;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiotopeMap
{
    namespace GetNoise
    {
        public class CreateMapImg
        {

            public CreateMapImg()
            {
            }

            public void CreateImag(NoiseArray Noisearray, int h = 180, String SavePath = "..\\test.png")
            {
                var array = Noisearray.array;
                //空の画像を生成
                var img = new Image<Rgba32>(array.Count, array[0].Count);

                if (h > 255)
                {
                    h = 255;
                }
                for (int i = 0; i < img.Height; i++)
                {
                    for (int j = 0; j < img.Width; j++)
                    {
                        double dnc = array[i][j];
                        switch (Noisearray.mode)
                        {
                            case NoiseValueMode.gradation256:
                                if (dnc < h)
                                {
                                    img[i, j] = new Rgba32(30, 50, (byte)dnc);
                                }
                                else if (dnc >= h)
                                {
                                    var d = 0;
                                    if (dnc > 255) { d = 255; } else { d = (int)dnc; }
                                    img[i, j] = new Rgba32(90, (byte)d, 95);
                                }
                                else
                                {
                                    img[i, j] = new Rgba32(1, 1, 100);
                                }
                                break;
                            case NoiseValueMode.binary:
                                if (dnc == 1)
                                {
                                    img[i, j] = new Rgba32(255, 0, 0);
                                }
                                else
                                {
                                    img[i, j] = new Rgba32(0, 0, 0, 0);
                                }
                                break;
                            default:
                                img[i, j] = new Rgba32((byte)(255 - dnc), 255, (byte)dnc);
                                break;
                        }


                    }

                }
                try
                {
                    img.Save(SavePath);
                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine(e.ToString());
                }
                catch (NotSupportedException ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }
            public void CreateImag(List<List<TerraInfo>> terraInfos, int h = 180, String SavePath = "..\\test.png")
            {
                var array = terraInfos;
                //空の画像を生成
                var img = new Image<Rgba32>(array.Count, array[0].Count);

                if (h > 255)
                {
                    h = 255;
                }
                for (int i = 0; i < img.Height; i++)
                {
                    for (int j = 0; j < img.Width; j++)
                    {
                        int dnc = (int)array[i][j].height;
                        if (array[i][j].blocks == TerraBlocks.Water)
                        {
                            img[i, j] = new Rgba32(40, 50, (byte)(dnc*0.8));
                        }
                        else if (array[i][j].blocks == TerraBlocks.Sea)
                        {
                            img[i, j] = new Rgba32(30, 50, (byte)(dnc*0.98));
                        }
                        else if (array[i][j].blocks == TerraBlocks.Gland)
                        {
                            var d = 0;
                            if (dnc > 255) { d = 255; } else { d = (int)dnc; }
                            img[i, j] = new Rgba32(90, (byte)d, 95);
                        }
                    }


                }
                try
                {
                    img.Save(SavePath);
                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine(e.ToString());
                }
                catch (NotSupportedException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
