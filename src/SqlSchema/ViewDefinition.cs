using Hl7.Fhir.Introspection;
using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CqlSquared.SqlSchema
{
    [Serializable]
    [DataContract]
    [FhirType("ViewDefinition", "http://hl7.org/fhir/uv/sql-on-fhir/StructureDefinition/ViewDefinition", IsResource = false)]
    public partial class ViewDefinition : Hl7.Fhir.Model.Base, IIdentifiable<Identifier>
    {
        public Identifier Identifier { get; set; } = new Identifier();

        public string Uri { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public ResourceType ResourceType { get; set; } = ResourceType.Basic;

        public List<FHIRVersion> ResourceVersion { get; set; } = new List<FHIRVersion>();
        public List<ConstantComponent> Constant { get; set; } = new List<ConstantComponent>();

        public List<SelectComponent> Select { get; set; } = new List<SelectComponent>();

        public List<WhereComponent> Where { get; set; } = new List<WhereComponent>();

        public override IDeepCopyable DeepCopy()
        {
            var copy = new ViewDefinition();
            base.CopyTo(copy);
            return copy;
        }

        public partial class ConstantComponent
        {
            public string Name { get; set; } = string.Empty;
            public Hl7.Fhir.Model.DataType? Value { get; set; }
        }

        public partial class SelectComponent
        {
            public List<ColumnComponent> Column { get; set; } = new List<ColumnComponent>();

            public List<SelectComponent> Select { get; set; } = new List<SelectComponent>();

            public string ForEach { get; set; } = string.Empty;

            public string ForEachOrNull {  get; set; } = string.Empty;

            public List<SelectComponent> UnionAll { get; set; } = new List<SelectComponent>();


            public partial class ColumnComponent
            {
                public string Path { get; set;} = string.Empty;
                public string Name { get; set;} = string.Empty;
            }
        }

        public partial class WhereComponent
        {
            public string Path { get; set; } = string.Empty; 
            public string Description { get; set; } = string.Empty;

        }
    }
}
