using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Benday.XmlUtilities
{
    public static class XmlUtilityExtensionMethods
    {
        public static IEnumerable<XElement> ElementsByLocalName(this XElement parent, string name)
        {
            if (parent == null)
            {
                return null;
            }
            else
            {
                var result = (from temp in parent.Elements()
                              where temp.Name.LocalName == name
                              select temp);

                return result;
            }
        }

        public static IEnumerable<XElement> ElementsByLocalNameAndAttributeValue(this XElement parent,
            string elementName,
            string attributeName,
            string attributeValue)
        {
            var matchingElementsByName = parent.ElementsByLocalName(elementName);

            var result = (from temp in matchingElementsByName
                          where
                          temp.HasAttributes == true &&
                          temp.AttributeValue(attributeName) == attributeValue
                          select temp);

            return result;
        }

        public static XElement ElementByLocalName(this XElement parent, string name)
        {
            if (parent == null)
            {
                return null;
            }
            else
            {
                var result = (from temp in parent.Elements()
                              where temp.Name.LocalName == name
                              select temp).FirstOrDefault();

                return result;
            }
        }

        public static XElement ElementByLocalNameAndAttributeValue(this XElement parent,
            string elementName,
            string attributeName,
            string attributeValue)
        {
            var matchingElementsByName = parent.ElementsByLocalName(elementName);

            var match = (from temp in matchingElementsByName
                         where
                         temp.HasAttributes == true &&
                         temp.AttributeValue(attributeName) == attributeValue
                         select temp).FirstOrDefault();

            return match;
        }

        public static XElement ElementByLocalNameAndAttributeValue(this XElement parent,
            string elementName,
            string attributeName1,
            string attributeValue1,
            string attributeName2,
            string attributeValue2)
        {
            var matchingElementsByName = parent.ElementsByLocalName(elementName);

            var match = (from temp in matchingElementsByName
                         where
                         temp.HasAttributes == true &&
                         temp.AttributeValue(attributeName1) == attributeValue1 &&
                         temp.AttributeValue(attributeName2) == attributeValue2
                         select temp).FirstOrDefault();

            return match;
        }

        /// <summary>
        /// Finds a child element starting from parent and returns the inner text value.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="childElement"></param>
        /// <returns></returns>
        public static string ElementValue(this XElement parent, string childElement)
        {
            var child = parent.ElementByLocalName(childElement);

            if (child == null)
            {
                return null;
            }
            else
            {
                return child.Value;
            }
        }

        public static void SetElementValueByLocalName(this XElement parent, string childElement, string value)
        {
            var child = parent.ElementByLocalName(childElement);

            if (child == null)
            {
                throw new InvalidOperationException("Could not locate target element.");
            }
            else
            {
                child.Value = value;
            }
        }

        public static string ElementValueByChildNameAndAttributeValue(
            this XElement parent, string childElement,
            string attributeName, string attributeValue)
        {
            var child = parent.ElementByLocalNameAndAttributeValue(childElement, attributeName, attributeValue);

            if (child == null)
            {
                return null;
            }
            else
            {
                return child.Value;
            }
        }

        public static string AttributeValue(this XElement parent, string attributeName)
        {
            if (parent == null)
            {
                return String.Empty;
            }
            else if (parent.HasAttributes == false)
            {
                return String.Empty;
            }
            else if (parent.Attribute(attributeName) == null)
            {
                return String.Empty;
            }
            else
            {
                return parent.Attribute(attributeName).Value;
            }
        }
    }
}
