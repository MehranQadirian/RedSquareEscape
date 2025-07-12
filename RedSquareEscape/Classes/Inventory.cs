using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSquareEscape.Classes
{
    public class Inventory
    {
        private List<Item> items = new List<Item>();
        public List<Item> Items { get; private set; } = new List<Item>();
        public int Capacity { get; set; } = 10;

        public bool AddItem(Item item)
        {
            if (items.Count < Capacity)
            {
                items.Add(item);
                return true;
            }
            return false;
        }

        public void RemoveItem(ItemType type)
        {
            var item = Items.FirstOrDefault(i => i.Type == type);
            if (item != null)
                Items.Remove(item);
        }

        public void UseItem(int index)
        {
            if (index >= 0 && index < items.Count)
            {
                // Item usage logic would be handled by Player class
                items.RemoveAt(index);
            }
        }
        public bool HasItem(ItemType type)
        {
            return Items.Any(item => item.Type == type);
        }
    }
}
