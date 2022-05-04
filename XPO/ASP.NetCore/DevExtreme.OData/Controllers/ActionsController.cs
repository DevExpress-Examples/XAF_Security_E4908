using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Core;

namespace DevExtreme.OData.Controllers {
	public class ActionsController : ODataController {
		readonly IObjectSpaceFactory objectSpaceFactory;
        readonly SecurityStrategy security;
        readonly ITypesInfo typesInfo;
        public ActionsController(ISecurityProvider securityProvider, IObjectSpaceFactory objectSpaceFactory, ITypesInfo typesInfo) {
            this.typesInfo = typesInfo;
            this.objectSpaceFactory = objectSpaceFactory;
            this.security = (SecurityStrategy)securityProvider.GetSecurity();
        }
		[HttpPost("/GetPermissions")]
		public ActionResult GetPermissions(ODataActionParameters parameters) {
			ActionResult result = NoContent();
			if(parameters.ContainsKey("keys") && parameters.ContainsKey("typeName")) {
				IEnumerable<Guid> keys = ((IEnumerable<string>)parameters["keys"]).Select(k => Guid.Parse(k));
				string typeName = parameters["typeName"].ToString();
				ITypeInfo typeInfo = typesInfo.PersistentTypes.FirstOrDefault(t => t.Name == typeName);
				using(IObjectSpace objectSpace = objectSpaceFactory.CreateObjectSpace(typeInfo.Type)) {
					if(typeInfo != null) {
						IList entityList = objectSpace.GetObjects(typeInfo.Type, new InOperator(typeInfo.KeyMember.Name, keys));
						List<ObjectPermission> objectPermissions = new List<ObjectPermission>();
						foreach(object entity in entityList) {
							ObjectPermission objectPermission = CreateObjectPermission(typeInfo, entity);
							objectPermissions.Add(objectPermission);
						}
						result = Ok(objectPermissions);
					}
				}
			}
			return result;
		}
		[HttpGet("/GetTypePermissions")]
		public ActionResult GetTypePermissions(string typeName) {
			ActionResult result = NoContent();
            ITypeInfo typeInfo = typesInfo.PersistentTypes.FirstOrDefault(t => t.Name == typeName);
            using(IObjectSpace objectSpace = objectSpaceFactory.CreateObjectSpace(typeInfo.Type)) {
                if(typeInfo != null) {
					TypePermission typePermission = CreateTypePermission(typeInfo);
					result = Ok(typePermission);
				}
			}
            return result;
		}
        private ObjectPermission CreateObjectPermission(ITypeInfo typeInfo, object entity) {
            Type type = typeInfo.Type;
            ObjectPermission objectPermission = new ObjectPermission();
            objectPermission.Key = typeInfo.KeyMember.GetValue(entity).ToString();
            objectPermission.Write = security.CanWrite(entity);
            objectPermission.Delete = security.CanDelete(entity);
            IEnumerable<IMemberInfo> members = GetPersistentMembers(typeInfo);
            foreach(IMemberInfo member in members) {
                MemberPermission memberPermission = CreateMemberPermission(entity, type, member);
                objectPermission.Data.Add(member.Name, memberPermission);
            }
            return objectPermission;
        }
        private MemberPermission CreateMemberPermission(object entity, Type type, IMemberInfo member) {
            return new MemberPermission {
                Read = security.CanRead(entity, member.Name),
                Write = security.CanWrite(entity, member.Name)
            };
        }
        private static IEnumerable<IMemberInfo> GetPersistentMembers(ITypeInfo typeInfo) {
			return typeInfo.Members.Where(p => p.IsVisible && p.IsProperty && (p.IsPersistent || p.IsList));
		}
        private TypePermission CreateTypePermission(ITypeInfo typeInfo) {
            Type type = typeInfo.Type;
            TypePermission typePermission = new TypePermission();
            typePermission.Key = type.Name;
            typePermission.Create = security.CanCreate(type);
            IEnumerable<IMemberInfo> members = GetPersistentMembers(typeInfo);
            foreach(IMemberInfo member in members) {
                bool writePermission = security.CanWrite(type, member.Name);
                typePermission.Data.Add(member.Name, writePermission);
            }
            return typePermission;
        }
	}
}
