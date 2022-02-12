using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Items.Models
{
    public class Order
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Id { set; get; }
        public int RegionId { set; get; }
        public DateTime OrderDate { set;get; }
        public int ItemId { set; get; }
        public int Amount { set; get; }
    }
}
