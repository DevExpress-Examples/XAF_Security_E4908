using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCoreMvcApplication {
	public class PermissionHelper {
		IObjectSpace ObjectSpace { get; set; }
		SecurityStrategyComplex Security { get; set; }
		public PermissionHelper(IObjectSpace objectSpace, SecurityStrategyComplex security) {
			ObjectSpace = objectSpace;
			Security = security;
		}
		public ObjectPermission CreateObjectPermission(ITypeInfo typeInfo, object entity) {
			Type type = typeInfo.Type;
			ObjectPermission objectPermission = new ObjectPermission();
			objectPermission.Key = typeInfo.KeyMember.GetValue(entity).ToString();
			objectPermission.Write = Security.IsGranted(new PermissionRequest(ObjectSpace, type, SecurityOperations.Write, entity));
			objectPermission.Delete = Security.IsGranted(new PermissionRequest(ObjectSpace, type, SecurityOperations.Delete, entity));
			IEnumerable<IMemberInfo> members = GetPersistentMembers(typeInfo);
			foreach(IMemberInfo member in members) {
				MemberPermission memberPermission = CreateMemberPermission(entity, type, member);
				objectPermission.Data.Add(member.Name, memberPermission);
			}
			return objectPermission;
		}
		public MemberPermission CreateMemberPermission(object entity, Type type, IMemberInfo member) {
			return new MemberPermission {
				Read = Security.IsGranted(new PermissionRequest(ObjectSpace, type, SecurityOperations.Read, entity, member.Name)),
				Write = Security.IsGranted(new PermissionRequest(ObjectSpace, type, SecurityOperations.Write, entity, member.Name))
			};
		}
		private static IEnumerable<IMemberInfo> GetPersistentMembers(ITypeInfo typeInfo) {
			return typeInfo.Members.Where(p => p.IsVisible && p.IsProperty && (p.IsPersistent || p.IsList));
		}
		public TypePermission CreateTypePermission(ITypeInfo typeInfo) {
			Type type = typeInfo.Type;
			TypePermission typePermission = new TypePermission();
			typePermission.Create = Security.IsGranted(new PermissionRequest(ObjectSpace, type, SecurityOperations.Create));
			IEnumerable<IMemberInfo> members = GetPersistentMembers(typeInfo);
			foreach(IMemberInfo member in members) {
				bool writePermission = Security.IsGranted(new PermissionRequest(ObjectSpace, type, SecurityOperations.Write, null, member.Name));
				typePermission.Data.Add(member.Name, writePermission);
			}
			return typePermission;
		}
	}
}
