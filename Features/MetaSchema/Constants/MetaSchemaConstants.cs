namespace Forge.Features.MetaSchema.Constants;

public class MetaSchemaConstants
{
    public static class Applications
    {
        public static readonly Guid CoreForge = Guid.Parse("019f9b2e-a69a-7454-9d09-100000000001");
        public const string CurrentVersion = "0.0.1";
    }

    public static class Version
    {
        public const string InitialObjectVersion = "0.0.1.1";
    }

    public static class ObjectTypes
    {
        public static readonly Guid Class = Guid.Parse("019f9b59-11c2-7c05-a214-200000000001");
        public static readonly Guid Interface = Guid.Parse("019f9b59-11c2-7a9d-80eb-200000000002");
        public static readonly Guid Property = Guid.Parse("019f9b59-11c2-7366-aa16-200000000003");
        public static readonly Guid Relation = Guid.Parse("019f9b59-11c2-7908-863d-200000000004");
        public static readonly Guid RelationPath = Guid.Parse("019f9b59-11c2-7b66-b50b-200000000005");
        public static readonly Guid Graph = Guid.Parse("019f9b59-11c2-7f3a-8027-200000000006");
        public static readonly Guid View = Guid.Parse("019f9b59-11c2-7dea-bd06-200000000007");
        public static readonly Guid Action = Guid.Parse("019f9b59-11c2-7b2e-8dda-200000000008");
        public static readonly Guid Query = Guid.Parse("019f9b59-11c2-7d5d-bf3c-200000000009");
        public static readonly Guid Transform = Guid.Parse("019f9b59-11c2-7e20-9ce7-200000000010");
        public static readonly Guid Calculate = Guid.Parse("019f9b59-11c2-715b-a75f-200000000011");
        public static readonly Guid Event = Guid.Parse("019f9b59-11c2-726a-acce-200000000012");
        public static readonly Guid String = Guid.Parse("019f9b59-11c2-7c6c-a6e1-200000000013");
        public static readonly Guid EncryptedString = Guid.Parse("019f9b59-11c2-7a53-a349-200000000014");
        public static readonly Guid Integer = Guid.Parse("019f9b59-11c2-739e-a800-200000000015");
        public static readonly Guid Decimal = Guid.Parse("019f9b59-11c2-7475-af11-200000000016");
        public static readonly Guid Boolean = Guid.Parse("019f9b59-11c2-7d64-92c9-200000000017");
        public static readonly Guid Json = Guid.Parse("019f9b59-11c2-7dac-af2b-200000000018");
        public static readonly Guid Date = Guid.Parse("019f9b59-11c2-79d2-a9ea-200000000019");
        public static readonly Guid Time = Guid.Parse("019f9b59-11c2-7b35-bd45-200000000020");
        public static readonly Guid DateTime = Guid.Parse("019f9b59-11c2-711d-8d39-200000000021");
        public static readonly Guid List = Guid.Parse("019f9b59-11c2-71b4-bfee-200000000022");
        public static readonly Guid Map = Guid.Parse("019f9b59-11c2-7a5a-bfb5-200000000023");
        public static readonly Guid Api = Guid.Parse("019f9b59-11c2-76b0-a94d-200000000024");
        public static readonly Guid AccessGroups = Guid.Parse("019f9b59-11c2-7165-8f75-200000000025");
        public static readonly Guid Role = Guid.Parse("019f9b59-11c2-7d32-a096-200000000026");
        public static readonly Guid Policy = Guid.Parse("019f9b59-11c3-783f-a1fd-200000000027");
        public static readonly Guid Workflow = Guid.Parse("019f9b59-11c3-76f5-bf6b-200000000028");
    }

    public static class RelationshipTypes
    {
        public static readonly Guid Implements = Guid.Parse("019f9b6f-1442-7ad8-b5f8-200000000029");
        public static readonly Guid PrimaryInterface = Guid.Parse("019f9b6f-1442-7de7-b040-200000000030");
        public static readonly Guid Exposes = Guid.Parse("019f9b6f-1442-78e8-8494-200000000031");
        public static readonly Guid HasDirectedRel = Guid.Parse("019f9b6f-1442-7d38-8015-200000000032");
        public static readonly Guid HasDirectedRelPath = Guid.Parse("019f9b6f-1442-7eb2-9d5f-200000000033");
        public static readonly Guid HasGraph = Guid.Parse("019f9b6f-1442-7777-b5da-200000000034");
        public static readonly Guid HasView = Guid.Parse("019f9b6f-1442-797f-8e97-200000000035");
        public static readonly Guid DataTypeScope = Guid.Parse("019f9b6f-1442-7bd1-a1ef-200000000036");
        public static readonly Guid HasAction = Guid.Parse("019f9b6f-1442-71d0-a6c8-200000000037");
        public static readonly Guid HasApi = Guid.Parse("019f9b6f-1442-74f5-a1e5-200000000038");
        public static readonly Guid HasEvent = Guid.Parse("019f9b6f-1442-7b08-af59-200000000039");
        public static readonly Guid InvokesAction = Guid.Parse("019f9b6f-1442-7776-a532-200000000040");
        public static readonly Guid RequiresAccessGroup = Guid.Parse("019f9b6f-1442-735e-af47-200000000041");
        public static readonly Guid RequiresRole = Guid.Parse("019f9b6f-1442-7413-a012-200000000042");
        public static readonly Guid RequiresPolicy = Guid.Parse("019f9b6f-1442-73ac-9f81-200000000043");
    }

    public static class Relationship
    {
        public static readonly Guid Implements = Guid.Parse("019f9b6f-1442-7ad8-b5f8-300000000001");
        public static readonly Guid PrimaryInterface = Guid.Parse("019f9b6f-1442-7de7-b040-300000000002");
        public static readonly Guid Exposes = Guid.Parse("019f9b6f-1442-78e8-8494-300000000003");
        public static readonly Guid HasDirectedRel = Guid.Parse("019f9b6f-1442-7d38-8015-300000000004");
        public static readonly Guid HasDirectedRelPath = Guid.Parse("019f9b6f-1442-7eb2-9d5f-300000000005");
        public static readonly Guid HasGraph = Guid.Parse("019f9b6f-1442-7777-b5da-300000000006");
        public static readonly Guid HasView = Guid.Parse("019f9b6f-1442-797f-8e97-300000000007");
        public static readonly Guid DataTypeScope = Guid.Parse("019f9b6f-1442-7bd1-a1ef-300000000008");
        public static readonly Guid HasAction = Guid.Parse("019f9b6f-1442-71d0-a6c8-300000000009");
        public static readonly Guid HasApi = Guid.Parse("019f9b6f-1442-74f5-a1e5-300000000010");
        public static readonly Guid HasEvent = Guid.Parse("019f9b6f-1442-7b08-af59-300000000011");
        public static readonly Guid InvokesAction = Guid.Parse("019f9b6f-1442-7776-a532-300000000012");
        public static readonly Guid RequiresAccessGroup = Guid.Parse("019f9b6f-1442-735e-af47-300000000013");
        public static readonly Guid RequiresRole = Guid.Parse("019f9b6f-1442-7413-a012-300000000014");
        public static readonly Guid RequiresPolicy = Guid.Parse("019f9b6f-1442-73ac-9f81-300000000015");
    }
}