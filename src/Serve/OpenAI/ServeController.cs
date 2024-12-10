// using System.Text.Json.Serialization;
// using OpenAI;
//
// namespace LangChain.Serve.OpenAI;
//
// public class ServeController(
//     ServeOptions options)
// {
//     public ModelsList GetModel()
//     {
//         return new ModelsList
//         {
//             Models = options.ListModels().Select(static x => new Model12
//             {
//                 Object = ModelObject.Model,
//                 Created = DateTimeOffset.UtcNow,
//                 Id = x,
//                 OwnedBy = "OpenAI",
//             }).ToList()
//         };
//     }
//
//     public ModelsList ListModels()
//     {
//         return new ModelsList
//         {
//             Models = options.ListModels().Select(static x => new Model12
//             {
//                 Object = ModelObject.Model,
//                 Created = DateTimeOffset.UtcNow,
//                 Id = x,
//                 OwnedBy = "OpenAI",
//             }).ToList()
//         };
//     }
// }
//
// public sealed class ModelsList
// {
//     [JsonInclude]
//     [JsonPropertyName("data")]
//     public IReadOnlyCollection<Model15> Models { get; set; } = [];
// }