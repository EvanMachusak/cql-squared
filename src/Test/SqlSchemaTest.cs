using CqlSquared.Model;
using CqlSquared.Model.Schema;
using CqlSquared.SqlSchema;

namespace CqlSquared.Test
{
    [TestClass]
    public class SqlSchemaTest
    {
        [TestMethod]
        public void Translate()
        {
            var model = Models.Fhir401();
            var translator = new ModelToSchemaTranslator();
            var options = TranslatorOptions.Default();
            options.FHIRVersions = new[] { TranslatorOptions.Version401 };
            var classes = model.typeInfo
                .OfType<ClassInfo>()
                .Where(ci => ci.name == "Patient")
                .ToArray();
            var schemas = translator.Translate(classes, options);

        }
    }
}