using Forge.Features.MetaSchema.Constants;
using Forge.Features.MetaSchema.Entities;

namespace Forge.Features.MetaSchema.Seed;

public class MetaObjectSeeder
{
    public static IEnumerable<MetaObject> GetMetaObjects()
    {
        var seeds = GetSeeds();

        return seeds.Select(seed => new MetaObject
        {
            Uuid = seed.Uuid,
            Name = seed.Name,
            DisplayName = seed.DisplayName,
            Description = null,
            ApplicationUid = MetaSchemaConstants.Applications.CoreForge,
            ObjTypeUid = null,
            Version = MetaSchemaConstants.Version.InitialObjectVersion,
            IsActive = true
        });
    }

    private static IReadOnlyCollection<MetaObjectSeed> GetSeeds()
    {
        return
        [
            new(MetaSchemaConstants.ObjectTypes.Class, "Class", "Class"),
            new(MetaSchemaConstants.ObjectTypes.Interface, "Interface", "Interface"),
            new(MetaSchemaConstants.ObjectTypes.Property, "Property", "Property"),
            new(MetaSchemaConstants.ObjectTypes.Relation, "Relation", "Relation"),
            new(MetaSchemaConstants.ObjectTypes.RelationPath, "RelationPath", "RelationPath"),
            new(MetaSchemaConstants.ObjectTypes.Graph, "Graph", "Graph"),
            new(MetaSchemaConstants.ObjectTypes.View, "View", "View"),
            new(MetaSchemaConstants.ObjectTypes.Action, "Action", "Action"),
            new(MetaSchemaConstants.ObjectTypes.Query, "Query", "Query"),
            new(MetaSchemaConstants.ObjectTypes.Transform, "Transform", "Transform"),
            new(MetaSchemaConstants.ObjectTypes.Calculate, "Calculate", "Calculate"),
            new(MetaSchemaConstants.ObjectTypes.Event, "Event", "Event"),
            new(MetaSchemaConstants.ObjectTypes.String, "String", "String"),
            new(MetaSchemaConstants.ObjectTypes.EncryptedString, "EncryptedString", "EncryptedString"),
            new(MetaSchemaConstants.ObjectTypes.Integer, "Integer", "Integer"),
            new(MetaSchemaConstants.ObjectTypes.Decimal, "Decimal", "Decimal"),
            new(MetaSchemaConstants.ObjectTypes.Boolean, "Boolean", "Boolean"),
            new(MetaSchemaConstants.ObjectTypes.Json, "Json", "Json"),
            new(MetaSchemaConstants.ObjectTypes.Date, "Date", "Date"),
            new(MetaSchemaConstants.ObjectTypes.Time, "Time", "Time"),
            new(MetaSchemaConstants.ObjectTypes.DateTime, "DateTime", "DateTime"),
            new(MetaSchemaConstants.ObjectTypes.List, "List", "List"),
            new(MetaSchemaConstants.ObjectTypes.Map, "Map", "Map"),
            new(MetaSchemaConstants.ObjectTypes.Api, "Api", "Api"),
            new(MetaSchemaConstants.ObjectTypes.AccessGroups, "AccessGroups", "AccessGroups"),
            new(MetaSchemaConstants.ObjectTypes.Role, "Role", "Role"),
            new(MetaSchemaConstants.ObjectTypes.Policy, "Policy", "Policy"),
            new(MetaSchemaConstants.ObjectTypes.Workflow, "Workflow", "Workflow"),


            new(MetaSchemaConstants.RelationshipTypes.Implements, "Implements", "Implements"),
            new(MetaSchemaConstants.RelationshipTypes.PrimaryInterface, "PrimaryInterface", "PrimaryInterface"),
            new(MetaSchemaConstants.RelationshipTypes.Exposes, "Exposes", "Exposes"),
            new(MetaSchemaConstants.RelationshipTypes.HasDirectedRel, "HasDirectedRel", "HasDirectedRel"),
            new(MetaSchemaConstants.RelationshipTypes.HasDirectedRelPath, "HasDirectedRelPath", "HasDirectedRelPath"),
            new(MetaSchemaConstants.RelationshipTypes.HasGraph, "HasGraph", "HasGraph"),
            new(MetaSchemaConstants.RelationshipTypes.HasView, "HasView", "HasView"),
            new(MetaSchemaConstants.RelationshipTypes.DataTypeScope, "DataTypeScope", "DataTypeScope"),
            new(MetaSchemaConstants.RelationshipTypes.HasAction, "HasAction", "HasAction"),
            new(MetaSchemaConstants.RelationshipTypes.HasApi, "HasApi", "HasApi"),
            new(MetaSchemaConstants.RelationshipTypes.HasEvent, "HasEvent", "HasEvent"),
            new(MetaSchemaConstants.RelationshipTypes.InvokesAction, "InvokesAction", "InvokesAction"),
            new(MetaSchemaConstants.RelationshipTypes.RequiresAccessGroup, "RequiresAccessGroup",
                "RequiresAccessGroup"),
            new(MetaSchemaConstants.RelationshipTypes.RequiresRole, "RequiresRole", "RequiresRole"),
            new(MetaSchemaConstants.RelationshipTypes.RequiresPolicy, "RequiresPolicy", "RequiresPolicy"),


            // Continue adding Runtime, Primitive,
            // API, IAM and Workflow objects...
        ];
    }
}