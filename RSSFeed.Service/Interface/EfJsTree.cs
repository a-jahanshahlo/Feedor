using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using RSSFeed.Domain;
using RSSFeed.Domain.Poco;
using Site = RSSFeed.Domain.Site;

namespace RSSFeed.Service.Interface
{
    public class EfJsTree : IJsTree
    {
        public IList<JsTreeNode> JsTreeNodesList { get; set; }

        public EfJsTree()
        {
            JsTreeNodesList = new List<JsTreeNode>();
        }


        public void Remove()
        {
            throw new System.NotImplementedException();
        }

        public void Add(IList<Group> feedEntity)
        {

            foreach (var group in feedEntity)
            {
                var gp = new JsTreeNode()
                {
                    id = group.Id.ToString(),
                    a_attr = new JsTreeNodeAAttributes() { href = group.Id.ToString() },
                    text = group.GroupName,
                    icon = ""
                };

                foreach (var site in group.Sites)
                {
                    var st = new JsTreeNode()
                    {
                        id = site.Id.ToString() + group.Id.ToString(),
                        a_attr = new JsTreeNodeAAttributes() { href = site.SiteUrl },
                        text = site.SiteName,
                        icon = ""
                    };
                    ;
                    foreach (var channel in site.Channels.ToList())
                    {
                        var ch = new JsTreeNode()
                        {
                            id = channel.Id.ToString() + site.Id.ToString() + group.Id.ToString(),
                            a_attr = new JsTreeNodeAAttributes() { href = channel.Link },
                            text = channel.Title,
                            icon = ""
                        };
                        st.children.Add(ch);
                    }
                    gp.children.Add(st);
                }
                JsTreeNodesList.Add(gp);
            }

        }



        public void Add(IList<Site> sites)
        {
            foreach (var site in sites)
            {
                var st = new JsTreeNode()
                {
                    id = site.Id.ToString(),
                    a_attr = new JsTreeNodeAAttributes() { href = site.SiteUrl },
                    text = site.SiteName,
                    icon = site.SiteUrl + "/favicon.ico"
                };

                foreach (var channel in site.Channels.ToList())
                {
                    var text = channel.Title + (channel.Items == null ? "" : "(" + channel.Items.Count + ")");
                    var ch = new JsTreeNode()
                    {
                        id = channel.Id.ToString() + "c",
                        a_attr = new JsTreeNodeAAttributes() { href = channel.Id.ToString() },
                        text = text,
                        icon = ""
                    };
                    st.children.Add(ch);
                }
                JsTreeNodesList.Add(st);
            }

        }

        public IList<JsTreeNode> Insert(IList<Site> sites)
        {
            var newSites = new JsTreeNode[JsTreeNodesList.Count];

            JsTreeNodesList.CopyTo(newSites, JsTreeNodesList.Count);
            Add(sites);
            IEnumerable<JsTreeNode> jsTreeNodes = JsTreeNodesList.Except(newSites);
         
            return jsTreeNodes.ToList();
        }
    }

}