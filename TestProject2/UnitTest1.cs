using BiotopeMap.GenerateTerra;
using BiotopeMap.GetNoise;
using XorShiftAddSharp;
using static System.Net.Mime.MediaTypeNames;

namespace TestProject2
{
    [TestClass]
    public class UnitTest1
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        public void TestMethod1()
        {
            XorShiftAddPool xorShiftAddPool = new(23);
            BiotopeMap.GetNoise.GetNoise grad = new(xorShiftAddPool);
            NoisePram noisePram = new();
            noisePram.Frequency = 2;
            noisePram.Persistence = (double)1;
            noisePram.Octaves = 3;
            noisePram.Scale = 1000;
            List<NoisePram> noisePrams = new();
            noisePrams.Add(noisePram);

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    double g = grad.OctavesNoise(noisePrams, (double)i, (double)j, 0);
                    Console.WriteLine(g.ToString());
                }
            }
        }
        [TestMethod]
        public void TestMethod2()
        {
            XorShiftAddSharp.XorShiftAdd xor = new(213123);
            Console.WriteLine(xor.NextDouble());
            Console.WriteLine(xor.NextDouble());
        }
        [TestMethod]
        public void BinaryTest()
        {
            XorShiftAddPool xorShiftAddPool = new(23);
            BiotopeMap.GetNoise.GetNoiseArray grad = new(xorShiftAddPool);
            List<NoisePram> noisePrams = new();
            List<NoisePram> riverPrams = new();
            int scale = 1;
            NoisePram riverPram = new()
            {
                Frequency = 1,
                Persistence = 2,
                Octaves = 3,
                Scale = 50 * scale,
                Mode = NoiseCompoundMode.plus,
            };
            NoisePram riverPram2 = new()
            {
                Frequency = 1,
                Persistence = 1,
                Octaves = 3,
                Scale = 1000 * scale,
                OffsetX = -150,
                OffsetY = 100,
                Mode = NoiseCompoundMode.multi,
            };
            riverPrams.Add(riverPram);
            riverPrams.Add(riverPram2);
            NoiseArrayPram riverarrayPram = new NoiseArrayPram
            {
                noisePrams = riverPrams,
                h = 400 * scale,
                w = 400 * scale,
                mode = NoiseValueMode.binary,
                startX = 400,
                startY = 400,
                threshold = 0.39


            };
            NoisePram noisePram1 = new()
            {
                Frequency = 2,
                Persistence = 1,
                Octaves = 3,
                Scale = 15 * scale,
                Mode = NoiseCompoundMode.plus,
            };

            NoisePram noisePram2 = new()
            {
                Frequency = 0.5,
                Persistence = 20,
                Octaves = 2,
                Scale = 15 * scale,
                Mode = NoiseCompoundMode.multi,
            };

            NoisePram noisePram3 = new()
            {
                Frequency = 2,
                Persistence = 1,
                Octaves = 1,
                Scale = 30 * scale,
                Mode = NoiseCompoundMode.multi,
                OffsetX = 256,
                OffsetY = 256,
            };


            NoisePram noisePram4 = new()
            {
                Frequency = 2,
                Persistence = 1,
                Octaves = 1,
                Scale = 150 * scale,
                Mode = NoiseCompoundMode.plus,
                OffsetX = 1000,
                OffsetY = 1000,
            };
            NoisePram noisePram5 = new()
            {
                Frequency = 2,
                Persistence = 1,
                Octaves = 1,
                Scale = 1000 * scale,
                Mode = NoiseCompoundMode.minus,
                OffsetX = 1000,
                OffsetY = 1000,
            };
            NoisePram noisePram6 = new()
            {
                Frequency = 2,
                Persistence = 1,
                Octaves = 1,
                Scale = 800 * scale,
                Mode = NoiseCompoundMode.plus,
                OffsetX = 2000,
                OffsetY = 2000,
            };

            noisePrams.Add(noisePram1);
            noisePrams.Add(noisePram2);
            noisePrams.Add(noisePram3);
            noisePrams.Add(noisePram4);
            noisePrams.Add(noisePram5);
            noisePrams.Add(noisePram6);
            NoiseArrayPram arrayPram = new ()
            {
                noisePrams = noisePrams,
                h = 400 * scale,
                w = 400 * scale,
                mode = NoiseValueMode.gradation256,
                startX = 100,
                startY = 100,
                threshold = 0.7

            };
            CreateMapImg img = new();
            string path = "..\\test.png";
            var riverarry = grad.GetContourArray(riverarrayPram);
            var arry = grad.GetContourArray(arrayPram);
            TerraArrayInfo terra= new TerraArrayInfo();
            terra.BaseLand=terra.ConvertTerraInfo(arry,140);
            terra.River = riverarry;
            GenerateTerra generateTerra = new GenerateTerra(terra);
            var river=generateTerra.GenerateRiver();
            img.CreateImag(river, 140,SavePath:"..\\testmap.png");
            img.CreateImag(riverarry, 140,SavePath:"..\\riverdot.png");
            img.CreateImag(arry, SavePath: path, h: 140);
            //var arry =grad.GetContourArray(arrayPram);
            /*for (int i = 0; i < 30; i++)
            {
                int h = i * 5 + 100;
                string path = "..\\test" + i + ".png";
                img.CreateImag(arry, h: h, SavePath: path);

            }*/
        }
        [TestMethod]
        public void CreateMapImgTest1()
        {
            XorShiftAddPool xorShiftAddPool = new(23);
            BiotopeMap.GetNoise.GetNoiseArray grad = new(xorShiftAddPool);
            List<NoisePram> noisePrams = new();
            int scale = 1;
            NoisePram noisePram1 = new()
            {
                Frequency = 2,
                Persistence = 1,
                Octaves = 3,
                Scale = 15 * scale,
                Mode = NoiseCompoundMode.plus,
            };

            NoisePram noisePram2 = new()
            {
                Frequency = 0.5,
                Persistence = 20,
                Octaves = 2,
                Scale = 15 * scale,
                Mode = NoiseCompoundMode.multi,
            };

            NoisePram noisePram3 = new()
            {
                Frequency = 2,
                Persistence = 1,
                Octaves = 1,
                Scale = 30 * scale,
                Mode = NoiseCompoundMode.multi,
                OffsetX = 256,
                OffsetY = 256,
            };


            NoisePram noisePram4 = new()
            {
                Frequency = 2,
                Persistence = 1,
                Octaves = 1,
                Scale = 150 * scale,
                Mode = NoiseCompoundMode.plus,
                OffsetX = 1000,
                OffsetY = 1000,
            };
            NoisePram noisePram5 = new()
            {
                Frequency = 2,
                Persistence = 1,
                Octaves = 1,
                Scale = 1000 * scale,
                Mode = NoiseCompoundMode.minus,
                OffsetX = 1000,
                OffsetY = 1000,
            };
            NoisePram noisePram6 = new()
            {
                Frequency = 2,
                Persistence = 1,
                Octaves = 1,
                Scale = 800 * scale,
                Mode = NoiseCompoundMode.plus,
                OffsetX = 2000,
                OffsetY = 2000,
            };

            noisePrams.Add(noisePram1);
            noisePrams.Add(noisePram2);
            noisePrams.Add(noisePram3);
            noisePrams.Add(noisePram4);
            noisePrams.Add(noisePram5);
            noisePrams.Add(noisePram6);
            NoiseArrayPram arrayPram = new NoiseArrayPram
            {
                noisePrams = noisePrams,
                h = 200 * scale,
                w = 200 * scale,
                mode = NoiseValueMode.gradation256,
                startX = 400,
                startY = 400,
                threshold = 0.7


            };
            CreateMapImg img = new();
            string path = "..\\test.png";
            img.CreateImag(grad.GetContourArray(arrayPram), SavePath: path, h: 140);
            //var arry =grad.GetContourArray(arrayPram);
            /*for (int i = 0; i < 30; i++)
            {
                int h = i * 5 + 100;
                string path = "..\\test" + i + ".png";
                img.CreateImag(arry, h: h, SavePath: path);

            }*/
        }
    }
}