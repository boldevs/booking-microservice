using Newtonsoft.Json.Serialization;

namespace BuldingBlock.EventStoreDB.Serialization;

public class NonDefaultConstructorContractResolver: DefaultContractResolver
{
    protected override JsonObjectContract CreateObjectContract(Type objectType)
    {
        return JsonObjectContractProvider.UsingNonDefaultConstructor(
            base.CreateObjectContract(objectType),
            objectType,
            base.CreateConstructorParameters
        );
    }
}
