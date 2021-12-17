using System.Text.Json;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;

public static class JsonParser {
	public static void ParseJson<T>(JsonElement jElement, object obj, IObjectSpace objectSpace)  {
		ITypeInfo typeInfo = objectSpace.TypesInfo.FindTypeInfo(typeof(T));
		var properties = jElement.EnumerateObject();
		foreach(JsonProperty property in properties) {
			string memberName = property.Name;
			IMemberInfo memberInfo = typeInfo.FindMember(memberName);
			if(memberInfo.IsAssociation) {
				ParseAssociationProperty(property.Value, obj, memberInfo, objectSpace);
			}
			else {
				ParseSimpleProperty(property.Value, obj, memberInfo);
			}
		}
		objectSpace.CommitChanges();
	}

	private static void ParseSimpleProperty(JsonElement jElement, object obj, IMemberInfo memberInfo) {
		object value = GetValue(jElement, memberInfo.MemberType);
		memberInfo.SetValue(obj, value);
	}

    private static object GetValue(JsonElement jsonElement, Type memberType) {
		if(memberType.IsArray) { 
			var JsonArray = jsonElement.EnumerateArray();
			List<object> result = new List<object>();
			foreach (JsonElement element in JsonArray) {
				result.Add(GetValue(element, memberType.GetElementType()));
            }
			return result;
		}
		if(memberType == typeof(Int16)) return jsonElement.GetInt16();
		if(memberType == typeof(UInt16)) return jsonElement.GetUInt16();
		if(memberType == typeof(Boolean)) return jsonElement.GetBoolean();
		if(memberType == typeof(Byte)) return jsonElement.GetByte();
		if(memberType == typeof(DateTime)) return jsonElement.GetDateTime();
		if(memberType == typeof(DateTimeOffset)) return jsonElement.GetDateTimeOffset();
		if(memberType == typeof(Decimal)) return jsonElement.GetDecimal();
		if(memberType == typeof(Double)) return jsonElement.GetDouble();
		if(memberType == typeof(Guid)) return jsonElement.GetGuid();
		if(memberType == typeof(HashCode)) return jsonElement.GetHashCode();
		if(memberType == typeof(Int32)) return jsonElement.GetInt32();
		if(memberType == typeof(Int64)) return jsonElement.GetInt64();
		if(memberType == typeof(SByte)) return jsonElement.GetSByte();
		if(memberType == typeof(Single)) return jsonElement.GetSingle();
		if(memberType == typeof(String)) return jsonElement.GetString();
		if(memberType == typeof(UInt32)) return jsonElement.GetUInt32();
		if(memberType == typeof(UInt64)) return jsonElement.GetUInt64();
		throw  new JsonException("Type mismatch during serialization");
	}

    private static void ParseAssociationProperty(JsonElement jElement, object obj, IMemberInfo memberInfo, IObjectSpace objectSpace) {
		string keyPropertyName = memberInfo.MemberTypeInfo.KeyMember.Name;
		var keyProperty = jElement.GetProperty(keyPropertyName);
		var keyValue = GetValue(keyProperty, memberInfo.MemberTypeInfo.KeyMember.MemberType);
		object value = objectSpace.GetObjectByKey(memberInfo.MemberType, keyValue);
		memberInfo.SetValue(obj, value);
	}
}

