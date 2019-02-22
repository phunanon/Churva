using System;

namespace Churva.Interpreter.BluePrints.Attributes
{
    public class KeywordAttribute: Attribute
    {
        /// <summary>
        /// Gets or sets the keyword used by the managing class.
        /// </summary>
        public String Word { get; set; }
    }
}