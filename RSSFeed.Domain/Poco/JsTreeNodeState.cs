using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSSFeed.Domain.Poco
{
    public class JsTreeNodeState
    {
        public bool opened { set; get; }
        public bool disabled { set; get; }
        public bool selected { set; get; }

        public JsTreeNodeState()
        {
            opened = true;
        }
    }
}
