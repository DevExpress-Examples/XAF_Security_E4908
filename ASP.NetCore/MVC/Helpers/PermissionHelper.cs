using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcApplication {
	public class PermissionHelper {
		SecurityStrategyComplex Security { get; set; }
		public PermissionHelper(SecurityStrategyComplex security) {
			Security = security;
		}
		public ObjectPermission CreateObjectPermission(ITypeInfo typeInfo, object entity) {
			Type type = typeInfo.Type;
			ObjectPermission objectPermission = new ObjectPermission();
			objectPermission.Key = typeInfo.KeyMember.GetValue(entity).ToString();
			objectPermission.Write = Security.CanWrite(entity);
			objectPermission.Delete = Security.CanDelete(entity);
			IEnumerable<IMemberInfo> members = GetPersistentMembers(typeInfo);
			foreach(IMemberInfo member in members) {
				MemberPermission memberPermission = CreateMemberPermission(entity, member);
				objectPermission.Data.Add(member.Name, memberPermission);
			}
			return objectPermission;
		}
		public MemberPermission CreateMemberPermission(object entity, IMemberInfo member) {
			return new MemberPermission {
				Read = Security.CanRead(entity, member.Name),
				Write = Security.CanWrite(entity, member.Name)
			};
		}
		private static IEnumerable<IMemberInfo> GetPersistentMembers(ITypeInfo typeInfo) {
			return typeInfo.Members.Where(p => p.IsVisible && p.IsProperty && (p.IsPersistent || p.IsList));
		}
		public TypePermission CreateTypePermission(ITypeInfo typeInfo) {
			Type type = typeInfo.Type;
			TypePermission typePermission = new TypePermission();
			typePermission.Create = Security.CanCreate(type);
			IEnumerable<IMemberInfo> members = GetPersistentMembers(typeInfo);
			foreach(IMemberInfo member in members) {
				bool writePermission = Security.CanWrite(type, member.Name);
				typePermission.Data.Add(member.Name, writePermission);
			}
			return typePermission;
		}
	}
}
