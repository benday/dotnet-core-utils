using System;
using System.Linq;
using System.Xml.Linq;

namespace Benday.XmlUtilities
{
    public static class XmlUtility
    {
        public static double GetAttributeValueAsDouble(XElement element, string attributeName)
        {
            var value = GetAttributeValue(element, attributeName);

            if (String.IsNullOrEmpty(value) == true)
            {
                return (double)0;
            }
            else
            {
                double temp = default(double);

                if (Double.TryParse(value, out temp) == true)
                {
                    return temp;
                }
                else
                {
                    return temp;
                }
            }
        }

        public static string GetAttributeValue(XElement element, string attributeName)
        {
            if (element == null)
                throw new ArgumentNullException("fromValue", "fromValue is null.");
            if (String.IsNullOrEmpty(attributeName))
                throw new ArgumentException("attributeName is null or empty.", "attributeName");


            if (element.Attribute(attributeName) != null)
            {
                return element.Attribute(attributeName).Value;
            }
            else
            {
                return String.Empty;
            }
        }

        public static void SetAttributeValue(XElement toValue, string name, double value)
        {
            SetAttributeValue(toValue, name, value.ToString());
        }

        public static void SetAttributeValue(XElement toValue, string name, string value)
        {
            toValue.SetAttributeValue(name, value);
        }

        public static string GetAttributeValue(XElement element, string attributeName, string defaultReturnValue)
        {
            if (element == null)
                return defaultReturnValue;
            else
            {
                var value = element.Attribute(attributeName);
                if (value == null)
                    return defaultReturnValue;
                else
                    return value.Value;
            }
        }

        public static void SetChildElement(XElement parentElement,
            string childElementName,
            double childElementValue)
        {
            SetChildElement(parentElement, childElementName,
                childElementValue.ToString());
        }

        public static void SetChildElement(XElement parentElement,
            string childElementName,
            DateTime childElementValue)
        {
            SetChildElement(parentElement, childElementName,
                childElementValue.ToString());
        }

        public static void SetChildElement(XElement parentElement, string childElementName, string childElementValue)
        {
            if (parentElement == null)
                throw new ArgumentNullException("parentElement", "parentElement is null.");
            if (String.IsNullOrEmpty(childElementName))
                throw new ArgumentException("childElementName is null or empty.", "childElementName");
            if (String.IsNullOrEmpty(childElementValue))
            {
                childElementValue = String.Empty;
            }

            parentElement.SetElementValue(childElementName, childElementValue);
        }

        public static XElement GetChildElement(XElement parentElement, string childElementName)
        {
            if (parentElement == null)
                throw new ArgumentNullException("parentElement", "parentElement is null.");
            if (String.IsNullOrEmpty(childElementName))
                throw new ArgumentException("childElementName is null or empty.", "childElementName");

            var childElement = parentElement.Descendants(childElementName).FirstOrDefault();
            return childElement;
        }

        public static string GetChildElementValue(XElement parentElement,
            string childElementName,
            string defaultReturnValue)
        {
            if (parentElement == null)
                throw new ArgumentNullException("parentElement", "parentElement is null.");
            if (String.IsNullOrEmpty(childElementName))
                throw new ArgumentException("childElementName is null or empty.", "childElementName");

            XElement childElement = GetChildElement(parentElement, childElementName);

            if (childElement == null)
                return defaultReturnValue;
            else
                return childElement.Value;
        }

        public static DateTime GetChildElementValue(XElement parentElement,
            string childElementName,
            DateTime defaultReturnValue)
        {
            if (parentElement == null)
                throw new ArgumentNullException("parentElement", "parentElement is null.");
            if (String.IsNullOrEmpty(childElementName))
                throw new ArgumentException("childElementName is null or empty.", "childElementName");

            XElement childElement = GetChildElement(parentElement, childElementName);

            if (childElement == null)
                return defaultReturnValue;
            else
            {
                DateTime returnValue;

                if (DateTime.TryParse(childElement.Value, out returnValue) == true)
                {
                    return returnValue;
                }
                else
                {
                    return defaultReturnValue;
                }
            }
        }

        public static int GetChildElementValue(XElement parentElement,
            string childElementName,
            int defaultReturnValue)
        {
            if (parentElement == null)
                throw new ArgumentNullException("parentElement", "parentElement is null.");
            if (String.IsNullOrEmpty(childElementName))
                throw new ArgumentException("childElementName is null or empty.", "childElementName");

            XElement childElement = GetChildElement(parentElement, childElementName);

            if (childElement == null)
                return defaultReturnValue;
            else
            {
                int returnValue;

                if (Int32.TryParse(childElement.Value, out returnValue) == true)
                {
                    return returnValue;
                }
                else
                {
                    return defaultReturnValue;
                }
            }
        }

        public static string GetChildElementValue(XElement parentElement,
            string childElementName,
            string childAttributeName,
            string childAttributeValue,
            string defaultReturnValue)
        {
            if (parentElement == null)
                throw new ArgumentNullException("parentElement", "parentElement is null.");
            if (String.IsNullOrEmpty(childElementName))
                throw new ArgumentException("childElementName is null or empty.", "childElementName");

            var childElement = (
                from temp in parentElement.Descendants(childElementName)
                where temp.HasAttributes == true &&
                temp.Attribute(childAttributeName) != null &&
                temp.Attribute(childAttributeName).Value == childAttributeValue
                select temp).FirstOrDefault();

            if (childElement == null)
                return defaultReturnValue;
            else
                return childElement.Value;
        }

        public static XDocument StringToXDocument(string fromValue)
        {
            return XDocument.Parse(fromValue);
        }
    }
}
