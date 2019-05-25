using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BanVotCauLong.Models.Entities
{
    public class Item
    {
        public int id { get; set; }
        public string name { get; set; }
        public long price { get; set; }
        public int amount { get; set; }
        [StringLength(50)]
        public string ImageLink { get; set; }
        public Item() { }
        public Item(int i, string n, long p, int a,string img)
        {
            id = i; name = n; price = p; amount = a; ImageLink = img;
        }
    }
}