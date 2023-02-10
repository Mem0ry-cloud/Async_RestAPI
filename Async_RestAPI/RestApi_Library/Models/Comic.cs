using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestApi_Library.Models
{
    public class Comic
    {
        public int Num { get; set; }
        public string Img { get; set; }
        public string Title { get; set; }
        public int Day { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
