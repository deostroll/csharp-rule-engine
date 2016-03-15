using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace caRuleEngineSample
{
    class LineItem
    {
        public int Sno { get; set; }
        public string ItemName { get; set; }
        public float Amount { get; set; }
        public string Category { get; set; }
        public string DiscountOption { get; set; }
        public string Remarks { get; set; }
    }
}
