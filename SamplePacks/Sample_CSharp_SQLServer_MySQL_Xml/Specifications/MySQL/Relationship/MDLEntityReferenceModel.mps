<CONFIG>
	NAME MDLEntityReferenceModel
	CATEGORY EntityReference
	NODE Relationship
	TOPLEVEL False
</CONFIG>
<CONTENT>
</CONTENT><OUTPUT>
<%%:
var entityName
// entity  reference should not be a self reference
if (ReferencedEntityID != CurrentEntity.EntityID && ReferencedEntityID != CurrentEntity.BaseEntityID)
{
	with (Entity from Solution.Find(ReferencedEntityID))
	{
		// create entity reference and add basic information
		entityName = EntityName
		CurrentEntityReference = New()
		CurrentEntityReference.Tags = "DB"
		CurrentEntityReference.EntityReferenceName = ../MDLEntityReferenceName(baseName = entityName)
		CurrentEntityReference.EntityID = CurrentEntity.EntityID
		CurrentEntityReference.ReferencedEntityID = EntityID
		CurrentEntityReference.IsNullable = ../IsNullable
		CurrentEntityReference.Order = 600 + CurrentEntity.EntityReferenceCount
		
		add(CurrentEntityReference)		
			
		// add property relationship
		CurrentPropertyRelationship = New()
		CurrentPropertyRelationship.RelationshipID = ../RelationshipID
		CurrentPropertyRelationship.PropertyID = CurrentEntityReference.PropertyID
		CurrentPropertyRelationship.Order = 1
		add(CurrentPropertyRelationship)
	}
}
%%></OUTPUT>