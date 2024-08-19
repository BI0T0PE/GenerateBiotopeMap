using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiotopeMap
{
    public record TerraInfo
    {
        public Blocks blocks { get; set; }
        public double height { get; set; }
    }

    public record TerraBlocks
    {
        public static Blocks Gland = new Blocks()
        {
            Name = "grand"
        };
        public static Blocks Water = new Blocks()
        {
            Name = "water"
        };
        public static Blocks Sea = new Blocks()
        {
            Name = "sea"
        };
    }
    public record Blocks
    {
        public string Name { get; set; } = "";
    }
}
