using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RSSFeed.Domain.opml
{


    /// <summary>

    /// Body class

    /// </summary>

    public class Body
    {

        #region Constructors

        /// <summary>

        /// Constructor

        /// </summary>

        public Body()
        {

            Outlines = new List<Outline>();

        }



        /// <summary>

        /// Constructor

        /// </summary>

        /// <param name="Element">XmlElement containing the body information</param>

        public Body(XmlElement Element)
        {

            Outlines = new List<Outline>();

            if (Element.Name.Equals("body", StringComparison.CurrentCultureIgnoreCase))
            {

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

        /// List of outlines

        /// </summary>

        public List<Outline> Outlines { get; set; }



        #endregion



        #region Overridden Functions



        public override string ToString()
        {

            StringBuilder BodyString = new StringBuilder();

            BodyString.Append("<body>");

            foreach (Outline Outline in Outlines)
            {

                BodyString.Append(Outline.ToString());

            }

            BodyString.Append("</body>");

            return BodyString.ToString();

        }


        #endregion

    }


}
