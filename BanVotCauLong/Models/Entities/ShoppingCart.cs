using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanVotCauLong.Models.Entities
{
    public class ShoppingCart
    {
        public List<Item> lst = new List<Item>();

        public void InsertItem(string name, int id, long price,string img)
        {
            bool check = false;
            foreach (var item in lst)
            {
                if (item.id == id)
                {
                    check = true;
                    item.amount++;
                    break;
                }
            }
            if (!check) lst.Add(new Item(id, name, price, 1, img));
        }
        public void UpdateAmount(int id, int amount)
        {
            foreach (var item in lst)
            {
                if (item.id == id)
                {
                    if (amount != 0)
                    {
                        item.amount++;
                        break;
                    }
                    else
                    {
                        lst.Remove(item);
                    }
                }
            }
        }
        public void RemoveItem(int id)
        {
            foreach (Item item in lst)
            {
                if (item.id == id)
                {
                    lst.Remove(item);
                    break;
                }
            }
        }
        public void SubtractItem(int id)
        {
            foreach (Item item in lst)
            {
                if (item.id == id)
                {
                    if (item.amount > 1) { item.amount--; break; }
                    else
                    {
                        lst.Remove(item);   break;
                    }
                }
            }
        }
        public int GetTotal()
        {
            int total = 0;
            foreach (Item item in lst)
            {
                total += item.amount;
            }
            return total;
        }
        public double GetTotalMoney()
        {
            double total = 0;
            foreach (Item item in lst)
            {
                total += item.amount * item.price;
            }
            return total;
        }
    }
}