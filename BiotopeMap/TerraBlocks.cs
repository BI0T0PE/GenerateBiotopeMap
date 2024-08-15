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
        public int height { get; set; }
    }

    public record TerraBlocks
    {
        public static Blocks Gland { get;private set; } = new Blocks()
        {
            Name = "grand"
        };
        public static Blocks Water { get; private set; } = new Blocks()
        {
            Name = "water"
        };
    }
    public record Blocks
    {
        public string Name { get; set; } = "";
    }
}
