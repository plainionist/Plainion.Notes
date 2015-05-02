using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Plainion;
using System.ComponentModel.DataAnnotations;

namespace Plainion.Wiki.Rendering
{
    public class AttributeRenderingSettings
    {
        public AttributeRenderingSettings()
        {
            Attributes = new List<AttributeRenderingStyle>();
        }

        public IList<AttributeRenderingStyle> Attributes
        {
            get;
            set;
        }
    }

    public class AttributeRenderingStyle
    {
        /// <summary>
        /// Qualified name of the attribute this config belongs to.
        /// </summary>
        [Required]
        public string QualifiedName
        {
            get;
            set;
        }

        public bool IsRenderValueOnDefinition
        {
            get;
            set;
        }

        public string RenderValueOnDefinitionPrefix
        {
            get;
            set;
        }
    }
}
