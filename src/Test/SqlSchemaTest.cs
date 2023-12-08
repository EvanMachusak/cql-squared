using CqlSquared.SqlSchema;
using Hl7.Cql.Model;
using System.Xml.Serialization;

namespace CqlSquared.Test
{
    [TestClass]
    public class SqlSchemaTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            XmlSerializer serializer = new(typeof(ModelInfo));
            var stream = typeof(SqlSchemaTest).Assembly.GetManifestResourceStream("fhir401")
               ?? throw new ArgumentException($"Manifest resource stream fhir401 is not included in this assembly.");
            Fhir401 = (ModelInfo)serializer.Deserialize(stream)!;
        }

        public static ModelInfo? Fhir401 { get; set; }

        [TestMethod]
        public void Translate()
        {
            var translator = new ModelToViewDefinitionTranslator();
            var options = TranslatorOptions.Default();
            var schemas = translator.Translate(Fhir401!, model =>
                model.typeInfo
                .OfType<ClassInfo>()
                .Where(ci => ci.name == "Patient")
                .ToArray(),
            options);

        }
    }
}