using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace RSSFeed.Domain.opml
{
    /// <summary>

     /// OPML class

     /// </summary>

     public class Opml

     {

         public static List<Outline> ParseOpml(string url)
         {
             XDocument opmlDoc = XDocument.Load(url);

             return ParseOpml(opmlDoc);


         }

         public static List<Outline> ParseOpml(Stream stream)
         {

             StreamReader reader = new StreamReader(stream);

             XDocument doc = XDocument.Load(reader);

             return ParseOpml(doc);

         }

         public static List<Outline> ParseOpml(XDocument opmlDoc)
         {
             List<Outline> outlines = new List<Outline>();

             foreach (var item in opmlDoc.Descendants("outline"))
             {
                 outlines.AddRange(ParseOutline(item));
             }


             return outlines.ToList();
         }

         private static IEnumerable<Outline> ParseOutline(XElement item)
         {
             List<Outline> output = new List<Outline>();

             foreach (var subItem in item.Descendants("outline"))
             {
                 output.AddRange(ParseOutline(subItem));
             }

             if (item.Attribute("xmlUrl") != null)
             {
                 Outline newOutline = new Outline()
                 {
                     Title = GetAttributeValue(item.Attribute("title")),
                     Text = GetAttributeValue(item.Attribute("text")),
                     Type = GetAttributeValue(item.Attribute("type"))
                 };

                 try
                 {
                     newOutline.XMLUrl = GetAttributeValue(item.Attribute("xmlUrl"));
                 }
                 catch (UriFormatException)
                 {
                 }

                 try
                 {
                     newOutline.HTMLUrl = GetAttributeValue(item.Attribute("htmlUrl"));
                 }
                 catch (UriFormatException)
                 {
                     newOutline.HTMLUrl = newOutline.XMLUrl;
                 }

                 if ((newOutline.XMLUrl != null && newOutline.XMLUrl.ToString().Length > 0) || (newOutline.HTMLUrl != null && newOutline.HTMLUrl.ToString().Length > 0))
                 {
                     output.Add(newOutline);
                 }
                 else
                 {
                     // Didn't get a good html or xml value, don't both adding
                 }
             }

             return output;

         }

         public static string GetAttributeValue(XAttribute attribute)
         {
             if (attribute == null)
             {
                 return "";
             }
             else
             {
                 return attribute.Value;
             }
         }

         #region Constructors

         /// <summary>

        /// Constructor

         /// </summary>

        public Opml()

         {

  

         }

 

       /// <summary>

        /// Constructor

        /// </summary>

         /// <param name="Location">Location of the OPML file</param>

         public Opml(string Location)

        {

             XmlDocument Document = new XmlDocument();

             Document.Load(Location);

            foreach (XmlNode Children in Document.ChildNodes)

          {

                if (Children.Name.Equals("opml", StringComparison.CurrentCultureIgnoreCase))

                {
                    foreach (XmlNode Child in Children.ChildNodes)

                    {

                       if (Child.Name.Equals("body", StringComparison.CurrentCultureIgnoreCase))

                        {

                            Body = new Body((XmlElement)Child);

                        }

                        else if (Child.Name.Equals("head", StringComparison.CurrentCultureIgnoreCase))

                         {

                           Head = new Head((XmlElement)Child);

                        }

                     }

                }

            }

        }

 

       /// <summary>

         /// Constructor

        /// </summary>

        /// <param name="Document">XmlDocument containing the OPML file</param>

         public Opml(XmlDocument Document)

        {

             foreach (XmlNode Children in Document.ChildNodes)

            {

                if (Children.Name.Equals("opml", StringComparison.CurrentCultureIgnoreCase))

               {

                   foreach (XmlNode Child in Children.ChildNodes)

                     {

                        if (Child.Name.Equals("body", StringComparison.CurrentCultureIgnoreCase))

                        {
                           Body = new Body((XmlElement)Child);

                       }

                        else if (Child.Name.Equals("head", StringComparison.CurrentCultureIgnoreCase))

                        {

                             Head = new Head((XmlElement)Child);

                      }

                  }

                }

            }

         }

        #endregion



        #region Properties

 

        /// <summary>
        /// Body of the file

        /// </summary>

        public Body Body { get; set; }

 

         /// <summary>

        /// Header information

       /// </summary>

         public Head Head { get; set; }

 

        #endregion



        #region Overridden Functions

       public override string ToString()

       {

            StringBuilder OPMLString = new StringBuilder();

           OPMLString.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?><opml version=\"2.0\">");

           OPMLString.Append(Head.ToString());

            OPMLString.Append(Body.ToString());

            OPMLString.Append("</opml>");

            return OPMLString.ToString();

       }

        #endregion

    }
}
