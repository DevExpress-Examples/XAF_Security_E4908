using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ASPNETCoreODataService.Controllers {
	[Route("api/[controller]")]
	public class ActionsController : ODataController, IDisposable {
		SecurityProvider securityProvider;
		public ActionsController(SecurityProvider securityProvider) {
			this.securityProvider = securityProvider;
		}
		[HttpPost]
		[ODataRoute("GetPermissions")]
		public ActionResult GetPermissions(ODataActionParameters parameters) {
			ActionResult result = NoContent();
			if(parameters.ContainsKey("keys") && parameters.ContainsKey("typeName")) {
				IEnumerable<Guid> keys = ((IEnumerable<string>)parameters["keys"]).Select(k => Guid.Parse(k));
				string typeName = parameters["typeName"].ToString();
				using(IObjectSpace objectSpace = securityProvider.ObjectSpaceProvider.CreateObjectSpace()) {
					ITypeInfo typeInfo = objectSpace.TypesInfo.PersistentTypes.FirstOrDefault(t => t.Name == typeName);
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
		[HttpGet]
		[ODataRoute("GetTypePermissions")]
		public ActionResult GetTypePermissions(string typeName) {
			ActionResult result = NoContent();
			using(IObjectSpace objectSpace = securityProvider.ObjectSpaceProvider.CreateObjectSpace()) {
				ITypeInfo typeInfo = objectSpace.TypesInfo.PersistentTypes.FirstOrDefault(t => t.Name == typeName);
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
            objectPermission.Write = securityProvider.Security.CanWrite(entity);
            objectPermission.Delete = securityProvider.Security.CanDelete(entity);
            IEnumerable<IMemberInfo> members = GetPersistentMembers(typeInfo);
            foreach(IMemberInfo member in members) {
                MemberPermission memberPermission = CreateMemberPermission(entity, type, member);
                objectPermission.Data.Add(member.Name, memberPermission);
            }
            return objectPermission;
        }
        private MemberPermission CreateMemberPermission(object entity, Type type, IMemberInfo member) {
            return new MemberPermission {
                Read = securityProvider.Security.CanRead(entity, member.Name),
                Write = securityProvider.Security.CanWrite(entity, member.Name)
            };
        }
        private static IEnumerable<IMemberInfo> GetPersistentMembers(ITypeInfo typeInfo) {
			return typeInfo.Members.Where(p => p.IsVisible && p.IsProperty && (p.IsPersistent || p.IsList));
		}
        private TypePermission CreateTypePermission(ITypeInfo typeInfo) {
            Type type = typeInfo.Type;
            TypePermission typePermission = new TypePermission();
            typePermission.Key = type.Name;
            typePermission.Create = securityProvider.Security.CanCreate(type);
            IEnumerable<IMemberInfo> members = GetPersistentMembers(typeInfo);
            foreach(IMemberInfo member in members) {
                bool writePermission = securityProvider.Security.CanWrite(type, member.Name);
                typePermission.Data.Add(member.Name, writePermission);
            }
            return typePermission;
        }
		public void Dispose() {
			securityProvider?.Dispose();
		}
	}
}
