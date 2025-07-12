using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSquareEscape.Classes
{
    public class Inventory
    {
        public List<Item> Items { get; set; } = new List<Item>();
        public int Capacity { get; set; } = 10;

        public bool AddItem(Item item)
        {
            if (Items.Count < Capacity)
            {
                Items.Add(item);
                return true;
            }
            return false;
        }

        public void RemoveItem(Item item)
        {
            Items.Remove(item);
        }

        public void UseItem(int index)
        {
            if (index >= 0 && index < Items.Count)
            {
                // Item usage logic would be handled by Player class
                Items.RemoveAt(index);
            }
        }
    }
}
