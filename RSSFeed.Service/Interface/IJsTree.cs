using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSSFeed.Domain;
using RSSFeed.Domain.Poco;

namespace RSSFeed.Service.Interface
{
    public interface IJsTree
    {
        IList<JsTreeNode> JsTreeNodesList { get; set; }
        void Remove();
        void Add(IList<Group> feedEntity);
        void Add(IList<Site> sites);
        IList<JsTreeNode> Insert(IList<Site> sites);
    }
}
