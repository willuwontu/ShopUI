using System;
using System.Collections.Generic;
using System.Text;

namespace ItemShops.Utils
{
    public struct Tag
    {
        public readonly string name;

        public Tag(string name)
        {
            this.name = name.ToUpper();
        }
    }
}
