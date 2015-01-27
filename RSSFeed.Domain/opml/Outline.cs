using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RSSFeed.Domain.opml
{
 /// <summary>

       /// Outline class

       /// </summary>

       public class Outline
       {

           #region Constructors
           /// <summary>

           /// Constructor

           /// </summary>

           public Outline()

           {

               Outlines = new List<Outline>();

           }

    

           /// <summary>

           /// Constructors

           /// </summary>

           /// <param name="Element">Element containing outline information</param>

           public Outline(XmlElement Element)

           {

               if (Element.Name.Equals("outline", StringComparison.CurrentCultureIgnoreCase))

               {

                   if (Element.Attributes["text"] != null)

                   {

                       Text = Element.Attributes["text"].Value;

                   }

                   else if (Element.Attributes["description"] != null)

                   {

                       Description = Element.Attributes["description"].Value;

                   }

                   else if (Element.Attributes["htmlUrl"] != null)

                   {

                       HTMLUrl = Element.Attributes["htmlUrl"].Value;

                   }

                   else if (Element.Attributes["type"] != null)

                   {

                       Type = Element.Attributes["type"].Value;

                   }

                   else if (Element.Attributes["language"] != null)

                   {

                       Language = Element.Attributes["language"].Value;

                   }

                   else if (Element.Attributes["title"] != null)

                   {

                       Title = Element.Attributes["title"].Value;

                   }

                   else if (Element.Attributes["version"] != null)

                   {

                       Version = Element.Attributes["version"].Value;

                   }

                   else if (Element.Attributes["xmlUrl"] != null)

                   {

                       XMLUrl = Element.Attributes["xmlUrl"].Value;

                   }

                   foreach (XmlNode Child in Element.ChildNodes)

                   {

                       try

                       {

                           if (Child.Name.Equals("outline", StringComparison.CurrentCultureIgnoreCase))

                           {

                               Outlines.Add(new Outline((XmlElement)Child));

                           }

                       }

                       catch { }

                   }

               }

           }

    

          #endregion
          #region Properties
          /// <summary>

          /// Outline list

          /// </summary>

          public List<Outline> Outlines { get; set; }

   

          /// <summary>

          /// Url of the XML file

          /// </summary>

          public string XMLUrl { get; set; }

   

          /// <summary>

          /// Version number

          /// </summary>

          public string Version { get; set; }

   

          /// <summary>

          /// Title of the item

          /// </summary>

          public string Title { get; set; }

   

          /// <summary>

          /// Language used

          /// </summary>

          public string Language { get; set; }

   

          /// <summary>

          /// Type

          /// </summary>

          public string Type { get; set; }

   

          /// <summary>

          /// HTML Url

          /// </summary>

          public string HTMLUrl { get; set; }

   

          /// <summary>

          /// Text

          /// </summary>

          public string Text { get; set; }

   

          /// <summary>

          /// Description

          /// </summary>

          public string Description { get; set; }

   

          #endregion

   

          #region Overridden Functions

          public override string ToString()

          {

              StringBuilder OutlineString = new StringBuilder();

              OutlineString.Append("<outline text=\"" + Text + "\"");

              if (!string.IsNullOrEmpty(XMLUrl))

              {

                  OutlineString.Append(" xmlUrl=\"" + XMLUrl + "\"");

              }

              if (!string.IsNullOrEmpty(Version))

              {

                  OutlineString.Append(" version=\"" + Version + "\"");

              }

              if (!string.IsNullOrEmpty(Title))

              {

                  OutlineString.Append(" title=\"" + Title + "\"");

              }

              if (!string.IsNullOrEmpty(Language))

              {

                  OutlineString.Append(" language=\"" + Language + "\"");

              }

              if (!string.IsNullOrEmpty(Type))

              {

                  OutlineString.Append(" type=\"" + Type + "\"");

              }

              if (!string.IsNullOrEmpty(HTMLUrl))

              {

                  OutlineString.Append(" htmlUrl=\"" + HTMLUrl + "\"");

              }

              if (!string.IsNullOrEmpty(Text))

              {

                  OutlineString.Append(" text=\"" + Text + "\"");

              }

              if (!string.IsNullOrEmpty(Description))

              {

                  OutlineString.Append(" description=\"" + Description + "\"");

              }

              if (Outlines.Count > 0)

              {

                  OutlineString.Append(">\r\n");

                  foreach (Outline Outline in Outlines)

                  {

                      OutlineString.Append(Outline.ToString());

                  }

                  OutlineString.Append("</outline>\r\n");

              }

              else

              {

                  OutlineString.Append(" />\r\n");

              }

              return OutlineString.ToString();

          }

   

          #endregion

      }
}
