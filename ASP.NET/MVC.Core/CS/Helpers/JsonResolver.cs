using System;
using DevExpress.Xpo.Metadata;
using System.Collections.Generic;
using System.Reflection;
using XafSolution.Module.BusinessObjects;

namespace AspNetCoreMvcApplication {
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
}

namespace Microsoft.Extensions.DependencyInjection {
	using AspNetCoreMvcApplication;
	public static class SampleJsonMvcBuilderExtensions {
		public static IMvcBuilder AddDxSampleModelJsonOptions(this IMvcBuilder builder, Action<JsonResolver> setupAction = null) {
			return builder.AddJsonOptions(opt => {
				var resolver = new JsonResolver();
				opt.SerializerSettings.ContractResolver = resolver;
				opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
				setupAction?.Invoke(resolver);
			});
		}
	}
}
