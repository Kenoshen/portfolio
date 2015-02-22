using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace Winger.UI.CSS
{
    public class CSSElement
    {
        private List<string> searchNames = new List<string>();
        private Dictionary<CSSProperty, string> properties = new Dictionary<CSSProperty, string>();
        public string ParentDirectory = null;

        public CSSElement(string searchName)
        {
            AddSearchName(searchName);
        }


        public CSSElement(List<string> searchNameList)
        {
            if (searchNameList != null)
            {
                foreach (string sn in searchNameList)
                {
                    AddSearchName(sn);
                }
            }
        }


        public void AddSearchName(string searchName)
        {
            if (searchName != null && !searchName.Equals(""))
            {
                string sntrim = searchName.Trim();
                if (!sntrim.StartsWith(".") && !sntrim.StartsWith("#") && !sntrim.StartsWith("*"))
                {
                    sntrim = sntrim.ToLower();
                }
                searchNames.Add(sntrim);
            }
        }


        public void RemoveSearchName(string searchName)
        {
            if (searchName != null && !searchName.Equals(""))
            {
                string sntrim = searchName.Trim();
                if (!sntrim.StartsWith(".") && !sntrim.StartsWith("#") && !sntrim.StartsWith("*"))
                {
                    sntrim = sntrim.ToLower();
                }
                searchNames.Remove(sntrim);
            }
        }


        public List<string> GetSearchNames()
        {
            return searchNames;
        }


        public void AddProperty(CSSProperty property, string value)
        {
            if (property != CSSProperty.NONE && value != null && !value.Equals(""))
            {
                properties[property] = value;
            }
        }


        public void RemoveProperty(CSSProperty property)
        {
            if (property != CSSProperty.NONE)
            {
                properties.Remove(property);
            }
        }


        public string GetProperty(CSSProperty property)
        {
            if (property != CSSProperty.NONE)
            {
                try
                {
                    return properties[property];
                }
                catch (Exception) { }
            }
            return null;
        }


        public bool HasProperty(CSSProperty property)
        {
            return (GetProperty(property) != null);
        }


        public List<CSSProperty> GetProperties()
        {
            return properties.Keys.ToList<CSSProperty>();
        }


        public override string ToString()
        {
            if (searchNames.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(searchNames[0]);
                for (int i = 1; i < searchNames.Count; i++)
                {
                    sb.Append(", " + searchNames[i]);
                }
                sb.Append("\n{\n");
                foreach (CSSProperty property in properties.Keys)
                {
                    sb.Append("\t" + property.AsString() + ": " + properties[property] + ";\n");
                }
                sb.Append("}\n");
                return sb.ToString();
            }
            else
            {
                return "[CSS Element has no SearchNames]";
            }
        }
    }
}
