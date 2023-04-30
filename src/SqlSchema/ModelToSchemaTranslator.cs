using CqlSquared.Model.Schema;
using System.Xml.Linq;

namespace CqlSquared.SqlSchema
{
    public class ModelToSchemaTranslator
    {
        public IDictionary<string, Schema> Translate(IEnumerable<ClassInfo> classes, TranslatorOptions? options = null)
        {
            options ??= TranslatorOptions.Default();
            var classesByName = classes.ToDictionary(ci => $"{ci.@namespace}.{ci.name}");
            string ViewName(string @namespace, string name)
            {
                var viewNaming = options.ViewNamingPattern ?? TranslatorOptions.DefaultViewName;
                return viewNaming(@namespace, name)
                    ?? throw new InvalidOperationException("View name is null.");
            }

            var views = new Dictionary<string, Schema>();
            foreach (var classInfo in classes)
            {
                var schema = new Schema();
                schema.View = ViewName(classInfo.@namespace, classInfo.name);
                if (options.FHIRVersions is not null)
                {
                    schema.FhirVersion = options.FHIRVersions
                        .Where(v => !string.IsNullOrWhiteSpace(v))
                        .ToArray();
                }
                var resourceIdentifier = options.ResourceForClassInfo ?? TranslatorOptions.DefaultResourceForClassInfo;
                schema.Resource = resourceIdentifier(classInfo);
                views.Add(schema.View, schema);

            }
            foreach (var classInfo in classes)
            {
                if (classInfo.element is not null)
                {
                    var viewName = ViewName(classInfo.@namespace, classInfo.name);
                    if (views.TryGetValue(viewName, out var schema))
                    {
                        var columns = new Dictionary<string, string>();
                        foreach (var element in classInfo.element)
                        {
                            if (element.name == "address")
                            {
                            }
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
                }
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
