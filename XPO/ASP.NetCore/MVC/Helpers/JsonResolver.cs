using System.Reflection;
using DevExpress.Xpo.Metadata;
using BusinessObjectsLibrary.BusinessObjects;

public class JsonResolver : Newtonsoft.Json.Serialization.DefaultContractResolver {
	public bool SerializeCollections { get; set; } = false;
	public bool SerializeReferences { get; set; } = true;
	public bool SerializeByteArrays { get; set; } = true;
	readonly XPDictionary dictionary;
	public JsonResolver() {
		dictionary = new ReflectionDictionary();
		dictionary.GetDataStoreSchema(typeof(Employee), typeof(Department));
	}
	protected override List<MemberInfo> GetSerializableMembers(Type objectType) {
		XPClassInfo classInfo = dictionary.QueryClassInfo(objectType);
		if(classInfo != null && classInfo.IsPersistent) {
			var allSerializableMembers = base.GetSerializableMembers(objectType);
			var serializableMembers = new List<MemberInfo>(allSerializableMembers.Count);
			foreach(MemberInfo member in allSerializableMembers) {
				XPMemberInfo mi = classInfo.FindMember(member.Name);
				if(!(mi.IsPersistent || mi.IsAliased || mi.IsCollection || mi.IsManyToManyAlias)
					|| ((mi.IsCollection || mi.IsManyToManyAlias) && !SerializeCollections)
					|| (mi.ReferenceType != null && !SerializeReferences)
					|| (mi.MemberType == typeof(byte[]) && !SerializeByteArrays)) {
					continue;
				}
				serializableMembers.Add(member);
			}
			return serializableMembers;
		}
		return base.GetSerializableMembers(objectType);
	}
}