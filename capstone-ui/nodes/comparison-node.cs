using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace capstone_ui.nodes
{
    public class Comparison
    {
        public string name { get; set; }
        public Image image1 { get; set; }
        public Image image2 { get; set; }
        
        public Comparison()
        {

        }

        ~Comparison()
        {
                
        }
    }

    public class Image
    {
        public string name { get; set; }
        public string path { get; set; }

        public Image()
        {

        }

    }
}
