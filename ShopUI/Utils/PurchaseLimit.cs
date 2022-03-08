using System;
using System.Collections.Generic;
using System.Text;

namespace ItemShops.Utils
{
    public struct PurchaseLimit
    {
        public readonly int global;
        public readonly int perPlayer;

        /// <summary>
        /// Dictates how many times a purchasable can be purchased.
        /// 
        /// A value of 0 means there is no limit.
        /// </summary>
        /// <param name="global">The number of times the item can be bought globally.</param>
        /// <param name="perPlayer">The number of times the item can be bought on a per-player basis.</param>
        public PurchaseLimit(int global = 0, int perPlayer = 0)
        {
            this.global = global;
            this.perPlayer = perPlayer;
        }
    }
}
