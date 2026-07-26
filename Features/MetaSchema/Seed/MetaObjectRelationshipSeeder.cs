using Forge.Features.MetaSchema.Constants;
using Forge.Features.MetaSchema.Entities;

namespace Forge.Features.MetaSchema.Seed;

public class MetaObjectRelationshipSeeder
{
    public static IEnumerable<MetaObjectRelationship> GetMetaObjectRelationships()
    {
        var seeds = GetSeeds();

        return seeds.Select(seed => new MetaObjectRelationship
        {
            End1Uid = seed.End1Uid,
            End2Uid = seed.End2Uid,
            Ordinal = seed.Ordinal,
            Uuid = seed.Uuid,
            Name = seed.Name,
            DisplayName = seed.DisplayName,
            Description = null,
            RelTypeUid = seed.RelTypeUid,
            IsActive = true
        });
    }

    private static IReadOnlyCollection<MetaObjectRelationshipSeed> GetSeeds()
    {
        return
        [
           new(MetaSchemaConstants.ObjectTypes.Class, MetaSchemaConstants.ObjectTypes.Interface, 0,
               MetaSchemaConstants.Relationship.Implements, "Implements", "Implements",
               MetaSchemaConstants.RelationshipTypes.Implements),
           new(MetaSchemaConstants.ObjectTypes.Class, MetaSchemaConstants.ObjectTypes.Interface, 0,
               MetaSchemaConstants.Relationship.PrimaryInterface, "PrimaryInterface", "PrimaryInterface",
               MetaSchemaConstants.RelationshipTypes.PrimaryInterface),
           new(MetaSchemaConstants.ObjectTypes.Interface, MetaSchemaConstants.ObjectTypes.Property, 0,
               MetaSchemaConstants.Relationship.Exposes, "Exposes", "Exposes",
               MetaSchemaConstants.RelationshipTypes.Exposes),
           new(MetaSchemaConstants.ObjectTypes.Interface, MetaSchemaConstants.ObjectTypes.Relation, 0,
               MetaSchemaConstants.Relationship.HasDirectedRel, "HasDirectedRel", "HasDirectedRel",
               MetaSchemaConstants.RelationshipTypes.HasDirectedRel),
           new(MetaSchemaConstants.ObjectTypes.Interface, MetaSchemaConstants.ObjectTypes.RelationPath, 0,
               MetaSchemaConstants.Relationship.HasDirectedRelPath, "HasDirectedRelPath", "HasDirectedRelPath",
               MetaSchemaConstants.RelationshipTypes.HasDirectedRelPath),
           new(MetaSchemaConstants.ObjectTypes.Interface, MetaSchemaConstants.ObjectTypes.Graph, 0,
               MetaSchemaConstants.Relationship.HasGraph, "HasGraph", "HasGraph",
               MetaSchemaConstants.RelationshipTypes.HasGraph),
           new(MetaSchemaConstants.ObjectTypes.Graph, MetaSchemaConstants.ObjectTypes.View, 0,
               MetaSchemaConstants.Relationship.HasView, "HasView", "HasView",
               MetaSchemaConstants.RelationshipTypes.HasView),
        //    new(MetaSchemaConstants.ObjectTypes.Property, MetaSchemaConstants.ObjectTypes.String, …, Map, 0,
        //    MetaSchemaConstants.Relationship.DataTypeScope, "DataTypeScope", "DataTypeScope",
        //    MetaSchemaConstants.RelationshipTypes.DataTypeScope),
           new(MetaSchemaConstants.ObjectTypes.Interface, MetaSchemaConstants.ObjectTypes.Action, 0,
               MetaSchemaConstants.Relationship.HasAction, "HasAction", "HasAction",
               MetaSchemaConstants.RelationshipTypes.HasAction),
           new(MetaSchemaConstants.ObjectTypes.Action, MetaSchemaConstants.ObjectTypes.Api, 0,
               MetaSchemaConstants.Relationship.HasApi, "HasApi", "HasApi",
               MetaSchemaConstants.RelationshipTypes.HasApi),
           new(MetaSchemaConstants.ObjectTypes.Interface, MetaSchemaConstants.ObjectTypes.Event, 0,
               MetaSchemaConstants.Relationship.HasEvent, "HasEvent", "HasEvent",
               MetaSchemaConstants.RelationshipTypes.HasEvent),
           new(MetaSchemaConstants.ObjectTypes.Event, MetaSchemaConstants.ObjectTypes.Action, 0,
               MetaSchemaConstants.Relationship.InvokesAction, "InvokesAction", "InvokesAction",
               MetaSchemaConstants.RelationshipTypes.InvokesAction)
        //    new(MetaSchemaConstants.ObjectTypes.Class, Action, Query, Transform, Calculate,
        //        MetaSchemaConstants.ObjectTypes.AccessGroup, 0, MetaSchemaConstants.Relationship.RequiresAccessGroup,
        //        "RequiresAccessGroup", "RequiresAccessGroup",
        //        MetaSchemaConstants.RelationshipTypes.RequiresAccessGroup),
        //    new(MetaSchemaConstants.ObjectTypes.Class, Action, Query, Transform, Calculate,
        //        MetaSchemaConstants.ObjectTypes.Role, 0, MetaSchemaConstants.Relationship.RequiresRole, "RequiresRole",
        //        "RequiresRole", MetaSchemaConstants.RelationshipTypes.RequiresRole),
        //    new(MetaSchemaConstants.ObjectTypes.Class, Action, Query, Transform, Calculate,
        //        MetaSchemaConstants.ObjectTypes.Policy, 0, MetaSchemaConstants.Relationship.RequiresPolicy,
        //        "RequiresPolicy", "RequiresPolicy", MetaSchemaConstants.RelationshipTypes.RequiresPolicy),
        ];
    }
}