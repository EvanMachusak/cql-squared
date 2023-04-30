
using CqlSquared.Model.Schema;
using System.Xml.Serialization;

namespace CqlSquared.Model
{
    public static class Models
    {
        private static readonly XmlSerializer serializer = new(typeof(ModelInfo));

        public static ModelInfo Fhir401() => LoadModel(typeof(Models).Assembly.GetManifestResourceStream("fhir401")!);
        public static ModelInfo LoadModel(Stream model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var modelInfo = serializer.Deserialize(model) as ModelInfo;
            if(modelInfo == null)
            {
                throw new InvalidOperationException($"Could not deserialize the provided stream as a {nameof(ModelInfo)}");
            }    
            else return modelInfo;
        }

    }
}