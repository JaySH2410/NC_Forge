using Forge.Features.MetaSchema.Constants;
using Forge.Features.MetaSchema.Entities;

namespace Forge.Features.MetaSchema.Seed;

public static class MetaInterfaceSeeder
{
    public static IEnumerable<MetaInterface> GetMetaInterfaces()
    {
        yield return new MetaInterface
        {
            Uuid = MetaSchemaConstants.InterfaceImplementations.ClassImplementsInterface,
            ObjUid = MetaSchemaConstants.ObjectTypes.Class,
            InterfaceUid = MetaSchemaConstants.ObjectTypes.Interface,
            IsPrimary = true,
            Ordinal = 0,
            IsActive = true
        };
    }
}