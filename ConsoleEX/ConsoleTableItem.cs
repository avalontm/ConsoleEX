using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEX
{
    public class ConsoleTableItem
    {
        public string Title { set; get; }
        public List<object> Items { set; get; }

        public ConsoleTableItem() {
            Items = new List<object>();
        }
    }
}
