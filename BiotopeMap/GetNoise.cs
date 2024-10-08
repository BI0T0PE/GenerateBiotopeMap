﻿using System.Drawing;
using Microsoft.Extensions.ObjectPool;
using SixLabors.ImageSharp;
using XorShiftAddSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;
using System.Reflection.Metadata;
using System.Security.Principal;
using static BiotopeMap.GetNoise.GetNoiseArray;
using System.Threading.Tasks;

namespace BiotopeMap
{
    namespace GetNoise
    {
        
        public class GetNoiseArray
        {
            private XorShiftAddPool XorRand;

            public GetNoiseArray(XorShiftAddPool xorRand)
            {
                XorRand = xorRand;
            }

            /// <summary>
            /// 0~1のノイズを0~256に正規化する
            /// </summary>
            /// <param name="pram"></param>
            /// <returns></returns>
            public NoiseArray GetContourArray(NoiseArrayPram pram)
            {
                GetNoise getNoise = new GetNoise(XorRand);
                var h = pram.h;
                var w = pram.w;
                var StartX = pram.startX;
                var StartY = pram.startY;
                var noisePram = pram.noisePrams;
                List<List<double>> CountorArray = new();

                ParallelOptions option = new ParallelOptions();
                option.MaxDegreeOfParallelism = 4;
                object lockObject = new object();
                for (int i = 0 + StartX; i < h + StartX; i++)
                {
                    double[] data = new double[w];
                    Parallel.For(0 + StartY, w + StartY, j =>
                    //(int j = 0 + StartY; j < w + StartY; j++)
                    {

                        double denc = getNoise.OctavesNoise(noisePram, (double)i, (double)j);
                        double height = 0;
                        switch (pram.mode)
                        {
                            case NoiseValueMode.gradation256:
                                height = denc * 256;
                                break;
                            case NoiseValueMode.binary:
                                height = denc > pram.threshold ? 1 : 0;
                                break;
                            default:
                                height = denc * 256;
                                break;
                        }
                        lock (lockObject)
                        {
                            data[j - StartY] = height;
                        }
                    });
                    CountorArray.Add(new List<double>(data));
                }
                NoiseArray array = new NoiseArray
                {
                    array = CountorArray,
                    mode = pram.mode
                };
                return array;
            }
        }

        /// <summary>
        /// ノイズ生成クラス
        /// </summary>
        public class GetNoise
        {

            private XorShiftAddPool XorRand;
            private int[] p = new int[512];

            /// <summary>
            /// 0~1のノイズを生成
            /// </summary>
            /// <param name="xorRand">生成に使用する乱数器</param>
            public GetNoise(XorShiftAddPool xorRand)
            {
                XorRand = xorRand;
                CreateHashTable();
            }
            internal void CreateHashTable()
            {
                var rnd = XorRand.Get();
                for (int i = 0; i < 256; i++)
                {

                    int a = (int)Math.Round(rnd.NextDouble() * 256);
                    p[i] = a;
                    p[i + 256] = a;
                }
                XorRand.Return(rnd);
            }

            /// <summary>
            /// オクターブノイズを生成する
            /// </summary>
            /// <param name="noisePram">ノイズパラメータリスト</param>
            /// <param name="x">x座標</param>
            /// <param name="y">y座標</param>
            /// <param name="z">z座標</param>
            /// <returns>0~1のノイズ</returns>
            public double OctavesNoise(List<NoisePram> noisePram, double x, double y, double z = 0)
            {
                double density = 0;
                for (int i = 0; i < noisePram.Count; i++)
                {
                    double total = 0;
                    double amplitude = 10;
                    double maxValue = 0;

                    double frequency = noisePram[i].Frequency;
                    for (int j = 0; j < noisePram[i].Octaves; j++)
                    {
                        double a = CreateNoise(
                            ((x + noisePram[i].OffsetX) / noisePram[i].Scale) * frequency,
                            ((y + noisePram[i].OffsetY) / noisePram[i].Scale) * frequency) * amplitude;
                        total += a;

                        maxValue += amplitude;

                        amplitude *= noisePram[i].Persistence;
                        frequency *= 2;

                    }
                    switch (noisePram[i].Mode)
                    {
                        case NoiseCompoundMode.plus:
                            density += total / maxValue;
                            break;
                        case NoiseCompoundMode.multi:
                            density *= total / maxValue;
                            break;
                        case NoiseCompoundMode.minus:
                            density -= total / maxValue;
                            break;
                        case NoiseCompoundMode.divid:
                            density /= total / maxValue;
                            break;
                        default:
                            density += total / maxValue;
                            break;
                    }
                }
                return density;
            }
            /// <summary>
            /// 座標からパーリンノイズを生成する
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="z"></param>
            /// <returns>0~1の範囲のノイズ</returns>
            internal double CreateNoise(double x, double y, double z = 0)
            {
                //与えられたxyzから格子点を求める
                //xyzを囲む整数座標の正方形
                //0~255までの範囲かつ負の範囲をとらない
                int xi = (int)x & 255;
                int yi = (int)y & 255;
                int zi = (int)z & 255;

                double xf = x - Math.Floor(x);
                double yf = y - Math.Floor(y);
                double zf = z - Math.Floor(z);

                int xa = (int)Math.Floor(x);
                int ya = (int)Math.Floor(y);
                int za = (int)Math.Floor(z);

                //取り出した小数部を滑らかにする
                double u = smootherStep(xf);
                double v = smootherStep(yf);
                double w = smootherStep(zf);

                //0~255の乱数を取得
                int p000 = p[p[p[xi] + yi] + zi];
                int p010 = p[p[p[xi] + inc(yi)] + zi];
                int p001 = p[p[p[xi] + yi] + inc(zi)];
                int p011 = p[p[p[xi] + inc(yi)] + inc(zi)];
                int p100 = p[p[p[inc(xi)] + yi] + zi];
                int p110 = p[p[p[inc(xi)] + inc(yi)] + zi];
                int p101 = p[p[p[inc(xi)] + yi] + inc(zi)];
                int p111 = p[p[p[inc(xi)] + inc(yi)] + inc(zi)];

                //この辺よくわからない
                double g000 = GetGrad(p000, xf, yf, zf);
                double g100 = GetGrad(p100, xf - 1, yf, zf);
                double g010 = GetGrad(p010, xf, yf - 1, zf);
                double g001 = GetGrad(p001, xf, yf, zf - 1);
                double g110 = GetGrad(p110, xf - 1, yf - 1, zf);
                double g011 = GetGrad(p011, xf, yf - 1, zf - 1);
                double g101 = GetGrad(p101, xf - 1, yf, zf - 1);
                double g111 = GetGrad(p111, xf - 1, yf - 1, zf - 1);

                //この辺もよくわからない
                double x0 = lerp(g000, g100, u);
                double x1 = lerp(g010, g110, u);
                double x2 = lerp(g001, g101, u);
                double x3 = lerp(g011, g111, u);
                double y0 = lerp(x0, x1, v);
                double y1 = lerp(x2, x3, v);
                double z0 = lerp(y0, y1, w);

                return (z0 + 1) / 2;
            }
            private int inc(int num) //numに+1した場合に256を超えたら0に戻す
            {
                return (num + 1) % 256;
            }

            /// <summary>
            /// 格子点の固有ベクトルを求める
            /// </summary>
            /// <param name="index">0~255までのどれか</param>
            /// <param name="x">x座標の小数部</param>
            /// <param name="y">y座標の小数部</param>
            /// <param name="z">z座標の小数部 3Dでない場合は0</param>
            /// <returns></returns>
            private double GetGrad(int index, double x, double y, double z = 0)
            {
                //格子点8つ分の固有ベクトルを求める
                //indexから最初の4ビットを取り出す
                int h = index & 15;

                //indexの最上位ビット(MSB)が0であれば u=xとする そうでなければy
                double u = h < 8 ? x : y;

                double v;

                // 第1および第2ビットが0の場合、v = yとする
                if (h < 4)
                {
                    v = y;
                }

                //最初と2番目の上位ビットが1の場合、v = xとする
                else if (h == 12 || h == 14)
                {
                    v = x;
                }

                // 第1および第2上位ビットが等しくない場合（0/1、1/0）、v = zとする
                else
                {
                    v = z;
                }
                // 最後の2ビットを使って、uとvが正か負かを判断する そしてそれらの加算を返す
                return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
            }

            /// <summary>
            /// 0~1の乱数を0~2048に変換する
            /// </summary>
            /// <param name="seed">0~1の値</param>
            /// <returns></returns>
            private double GetIndex(double rnd)
            {
                double index = Math.Round(rnd * 256);
                return index;

            }

            /// <summary>
            /// 補完関数 滑らかにするっぽい
            /// </summary>
            /// <param name="x">小数部</param>
            /// <returns></returns>
            private double smootherStep(double x)
            {
                return x * x * x * (x * (x * 6 - 15) + 10);
            }

            double lerp(double a, double b, double t)
            {
                return a + (b - a) * t;
            }
        }
        public enum NoiseCompoundMode : int
        {
            plus = 0,
            multi = 1,
            minus = 2,
            divid = 3,
        }
        public enum NoiseValueMode : int
        {
            /// <summary>
            /// gradation mode 0~256
            /// </summary>
            gradation256 = 0,
            /// <summary>
            /// binary mode 0or1
            /// </summary>
            binary = 1
        }
        public record NoiseArray
        {
            public List<List<double>> array = new();
            public NoiseValueMode mode = NoiseValueMode.gradation256;
        }
        /// <summary>
        /// ノイズ配列のパラメータ
        /// </summary>
        public record NoiseArrayPram
        {
            /// <summary>
            /// noise parameter list
            /// </summary>
            public List<NoisePram> noisePrams { get; set; } = new();
            /// <summary>
            /// array height
            /// </summary>
            public int h { get; set; } = 500;
            /// <summary>
            /// array width
            /// </summary>
            public int w { get; set; } = 500;
            /// <summary>
            /// generate start x
            /// </summary>
            public int startX { get; set; } = 0;
            /// <summary>
            /// generate start y
            /// </summary>
            public int startY { get; set; } = 0;
            /// <summary>
            /// generate mode <br></br>
            /// </summary>
            public NoiseValueMode mode { get; set; } = NoiseValueMode.binary;
            public double threshold { get; set; } = 0;
        }

        /// <summary>
        /// オクターブノイズ用パラメータクラス
        /// </summary>
        public record NoisePram
        {
            /// <summary>
            /// オクターブ
            /// </summary>
            public int Octaves { get; set; }

            /// <summary>
            /// よくわからない
            /// </summary>
            public double Persistence { get; set; }
            public double Frequency { get; set; }

            /// <summary>
            /// 拡大率
            /// </summary>
            public int Scale { get; set; }

            /// <summary>
            /// 合成モード 0=加算 1=乗算 2=減算 3=除算
            /// </summary>
            public NoiseCompoundMode Mode { get; set; } = NoiseCompoundMode.plus;

            public double OffsetX { get; set; } = 0;
            public double OffsetY { get; set; } = 0;
        }
    }

}