MetaObject Table 
Column	    :Type	            :Nullable	:Description
Id	        :bigint	            :No	        :Auto Increment Row Identifier
objUid	    :uuid v7         	:No	        :Global Object Identifier
objTypeUid	:uuid v7         	:No	        :References the Object Type (Class, Interface, Property, etc.)
name	    :nvarchar(200)	    :No	        :Object Name
displayName :nvarchar(200)      :No         :Display of Object Name
description	:nvarchar(max)	    :Yes	    :Object Description
createdOn	:datetimeoffset	    :No	        :Creation timestamp
modifiedOn	:datetimeoffset	    :Yes	    :Last modified timestamp
deletedOn	:datetimeoffset	    :Yes	    :Soft delete timestamp
createdBy	:nvarchar(100)	    :No	        :Creator
modifiedBy	:nvarchar(100)	    :Yes	    :Last modifier
deletedBy	:nvarchar(100)	    :Yes	    :Deleted by

----

MetaObjectRelationship Table
Column	    :Type	            :Nullable	:Description
Id	        :bigint	            :No	        :Auto Increment Row Identifier
relUid      :uuid v7           :No          :Global Relationship Identifier
end1Uid	    :uuid v7	        :No	        :First object
end2Uid	    :uuid v7	        :No	        :Second object
defUid	    :uuid v7	        :No	        :Relationship Definition
ordinal	    :int	            :Yes	    :Ordering when multiple relationships exist
displayName	:nvarchar(200)	    :Yes	    :Optional display label
description	:nvarchar(max)	    :Yes	    :Description
createdOn	:datetimeoffset	    :No	        :Created timestamp
modifiedOn	:datetimeoffset	    :Yes	    :Modified timestamp
deletedOn	:datetimeoffset	    :Yes	    :Deleted timestamp
createdBy	:nvarchar(100)	    :No	        :Creator
modifiedBy	:nvarchar(100)	    :Yes	    :Modifier
deletedBy	:nvarchar(100)	    :Yes	    :Deleted By