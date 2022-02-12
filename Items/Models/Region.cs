using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Items.Models
{
    public class Region
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Id { set; get; }
        public string CategoryName { set; get; }
        public string CategoryValue { set; get; }
        public int? ParentId { set; get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Parents { set; get; }
    }
}
