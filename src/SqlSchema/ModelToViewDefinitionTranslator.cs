using Hl7.Cql.Model;
using Hl7.Fhir.Model;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace CqlSquared.SqlSchema
{
    public class ModelToViewDefinitionTranslator
    {
        public static readonly Regex SqlName = new Regex("^[A-Za-z][A-Za-z0-9_]*$", RegexOptions.Compiled);

        public IDictionary<string, ViewDefinition> Translate(Hl7.Cql.Model.ModelInfo model,
            Func<Hl7.Cql.Model.ModelInfo, IEnumerable<ClassInfo>> whichClasses, TranslatorOptions? options = null)
        {
            options ??= TranslatorOptions.Default();
            var classes = whichClasses(model);
            var classesByName = classes.ToDictionary(ci => $"{ci.@namespace}.{ci.name}");
            string ViewName(string @namespace, string name)
            {
                var viewNaming = options.ViewNamingPattern ?? TranslatorOptions.DefaultViewName;
                return viewNaming(@namespace, name)
                    ?? throw new InvalidOperationException("View name is null.");
            }
            var resourceVersion = new List<FHIRVersion>();
            if (string.Equals(model.url, "http://hl7.org/fhir", StringComparison.OrdinalIgnoreCase))
            {
                if (Enum.TryParse<FHIRVersion>($"N{model.version.Replace('.', '_')}", out var v))
                    resourceVersion.Add(v);
            }

            var views = new Dictionary<string, ViewDefinition>();
            foreach (var classInfo in classes)
            {
                var viewDefinition = new ViewDefinition();
                viewDefinition.Name = ViewName(classInfo.@namespace, classInfo.name);
                var resourceIdentifier = options.ResourceForClassInfo ?? TranslatorOptions.DefaultResourceForClassInfo;
                viewDefinition.ResourceType = resourceIdentifier(classInfo) ?? ResourceType.Basic;
                viewDefinition.ResourceVersion = new(resourceVersion);
                if (classInfo.element is not null)
                {
                    var columns = new Dictionary<string, string>();
                    foreach (var element in classInfo.element)
                    {
                        switch (element.elementTypeSpecifier)
                        {
                            case ListTypeSpecifier list:
                                break;
                            case NamedTypeSpecifier named:
                                break;
                            case TupleTypeSpecifier tuple:
                                break;
                            case ChoiceTypeSpecifier choice:
                                break;
                            default:
                                if (!string.IsNullOrWhiteSpace(element.elementType))
                                {

                                    if (classesByName.TryGetValue(element.elementType, out var elementClass))
                                    {
                                        if (IsPrimitive(elementClass))
                                        {
                                        }
                                        else if (AllElementsArePrimitives(elementClass, classesByName))
                                        {
                                        }
                                    }
                                }
                                else
                                {
                                }
                                break;
                        }
                    }
                }
                views.Add(viewDefinition.Name, viewDefinition);
            }
            return views;
        }

        private bool AllElementsArePrimitives(ClassInfo classInfo, Dictionary<string, ClassInfo> classes)
        {
            if (classInfo.element is not null && classInfo.element.Length > 0)
            {
                foreach (var element in classInfo.element)
                {
                    if (element.elementType is not null && classes.TryGetValue(element.elementType, out var elementClass))
                    {
                        if (!IsPrimitive(elementClass))
                            return false;
                    }
                    else return false;
                }
                return true;
            }
            return false;
        }


        private bool IsPrimitive(ClassInfo classInfo)
        {
            if (classInfo.element is not null && classInfo.element.Length == 1)
            {
                var element = classInfo.element[0];
                if (element.name == "value" && element.elementType != null && element.elementType.StartsWith("System."))
                    return true;
            }
            return false;
        }

    }
}
