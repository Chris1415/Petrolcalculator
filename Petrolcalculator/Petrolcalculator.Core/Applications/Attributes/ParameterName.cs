using System;

namespace Petrolcalculator.Core.Applications.Attributes
{
    /// <summary>
    /// Enum Attribute for Description Usage
    /// </summary>
    /// <author>
    /// Christian Hahn, Jun-2016
    /// </author>
    public class ParameterName : Attribute
    {
        /// <summary>
        /// The Description Property
        /// </summary>
        public string Name;

        /// <summary>
        /// c'tor
        /// </summary>
        /// <param name="name">the given Description</param>
        public ParameterName(string name)
        {
            Name = name;
        }
    }
}
