using Hl7.Cql.Model;
using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqlSquared.SqlSchema
{
    public class TranslatorOptions
    {
        public Func<string?,string?,string?>? ViewNamingPattern { get; set; }

        public Func<ClassInfo?,ResourceType?>? ResourceForClassInfo { get; set; }

        public static TranslatorOptions Default() => new()
        {
            ViewNamingPattern = DefaultViewName,
            ResourceForClassInfo = DefaultResourceForClassInfo,
        };

        public static string? DefaultViewName(string? @namespace, string? name)
        {
            if (!string.IsNullOrWhiteSpace(@namespace))
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    return $"{@namespace}_{name.Replace('.', '_')}";
                }
                else return name;
            }
            return null;
        }

        public static ResourceType? DefaultResourceForClassInfo(ClassInfo? @class)
        {
            const string uri = "http://hl7.org/fhir/StructureDefinition/";
            const int uriLength = 40;
            if (@class is null || string.IsNullOrWhiteSpace(@class.identifier))
                return null;
            else if (@class.identifier.StartsWith(uri))
            {
                var resourceName = @class.identifier.Substring(uriLength);
                if (Enum.TryParse<ResourceType>(resourceName, out var resourceType))
                    return resourceType;
            }
            return null;
        }

    }
}
