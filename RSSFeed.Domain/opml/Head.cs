using System;
using System.Text;
using System.Xml;

namespace RSSFeed.Domain.opml
{
    /// <summary>

    /// Head class
    /// </summary>
    public class Head
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public Head()
        {
            Docs = "http://www.opml.org/spec2";
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }


        /// <summary>

        /// Constructor

        /// </summary>

        /// <param name="Element">XmlElement containing the header information</param>

        public Head(XmlElement Element)

        {

            if (Element.Name.Equals("head", StringComparison.CurrentCultureIgnoreCase))
            {

                foreach (XmlNode Child in Element.ChildNodes)

                {

                    try
                    {

                        if (Child.Name.Equals("title", StringComparison.CurrentCultureIgnoreCase))

                        {

                            Title = Child.InnerText;

                        }
                        else if (Child.Name.Equals("ownerName", StringComparison.CurrentCultureIgnoreCase))

                        {

                            OwnerName = Child.InnerText;

                        }

                        else if (Child.Name.Equals("ownerEmail", StringComparison.CurrentCultureIgnoreCase))

                        {

                            OwnerEmail = Child.InnerText;

                        }

                        else if (Child.Name.Equals("dateCreated", StringComparison.CurrentCultureIgnoreCase))

                        {

                            DateCreated = DateTime.Parse(Child.InnerText);

                        }

                        else if (Child.Name.Equals("dateModified", StringComparison.CurrentCultureIgnoreCase))

                        {

                            DateModified = DateTime.Parse(Child.InnerText);

                        }

                        else if (Child.Name.Equals("docs", StringComparison.CurrentCultureIgnoreCase))

                        {

                            Docs = Child.InnerText;

                        }

                    }

                    catch { }

                }

            }

        }


        #endregion


        #region Properties

        /// <summary>

        /// Title of the OPML document

        /// </summary>

        public string Title { get; set; }

    

        /// <summary>

        /// Date it was created

        /// </summary>

        public DateTime DateCreated { get; set; }



        /// <summary>

        /// Date it was last modified

        /// </summary>

        public DateTime DateModified { get; set; }


        /// <summary>

        /// Owner of the file

        /// </summary>

        public string OwnerName { get; set; }

 

        /// <summary>

        /// Owner's email address

        /// </summary>

        public string OwnerEmail { get; set; }

 

        /// <summary>

        /// Location of the OPML spec

        /// </summary>

        public string Docs { get; set; }

        #endregion

        #region Overridden Functions


        public override string ToString()

        {

            StringBuilder HeadString = new StringBuilder();

            HeadString.Append("<head>");

            HeadString.Append("<title>" + Title + "</title>\r\n");

            HeadString.Append("<dateCreated>" + DateCreated.ToString("R") + "</dateCreated>\r\n");

            HeadString.Append("<dateModified>" + DateModified.ToString("R") + "</dateModified>\r\n");

            HeadString.Append("<ownerName>" + OwnerName + "</ownerName>\r\n");

            HeadString.Append("<ownerEmail>" + OwnerEmail + "</ownerEmail>\r\n");

            HeadString.Append("<docs>" + Docs + "</docs>\r\n");

            HeadString.Append("</head>\r\n");

            return HeadString.ToString();

        }



        #endregion

    }


}