using DevExpress.Data.Filtering;
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
	public class ActionsController : SecuredController {
		public ActionsController(XpoDataStoreProviderService xpoDataStoreProviderService, IConfiguration config, SecurityProvider securityHelper, IHttpContextAccessor contextAccessor)
			: base(xpoDataStoreProviderService, config, securityHelper, contextAccessor) { }
		[HttpPost]
		[ODataRoute("GetPermissions")]
		public ActionResult GetPermissions(ODataActionParameters parameters) {
			ActionResult result = NoContent();
			if(parameters.ContainsKey("keys") && parameters.ContainsKey("typeName")) {
				List<string> keys = new List<string>(parameters["keys"] as IEnumerable<string>);
				string typeName = parameters["typeName"].ToString();
				ITypeInfo typeInfo = ObjectSpace.TypesInfo.PersistentTypes.FirstOrDefault(t => t.Name == typeName);
				if(typeInfo != null) {
					IList entityList = ObjectSpace.GetObjects(typeInfo.Type, new InOperator(typeInfo.KeyMember.Name, keys));
				    List<ObjectPermission> objectPermissions = new List<ObjectPermission>();
					foreach(object entity in entityList) {
                        ObjectPermission objectPermission = CreateObjectPermission(typeInfo, entity);
                        objectPermissions.Add(objectPermission);
                    }
                    result = Ok(objectPermissions);
				}
			}
			return result;
		}
		[HttpGet]
		[ODataRoute("GetTypePermissions")]
		public ActionResult GetTypePermissions(string typeName) {
			ActionResult result = NoContent();
			ITypeInfo typeInfo = ObjectSpace.TypesInfo.PersistentTypes.FirstOrDefault(t => t.Name == typeName);
			if(typeInfo != null) {
                TypePermission typePermission = CreateTypePermission(typeInfo);
                result = Ok(typePermission);
            }
            return result;
		}
        private ObjectPermission CreateObjectPermission(ITypeInfo typeInfo, object entity) {
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
        private MemberPermission CreateMemberPermission(object entity, Type type, IMemberInfo member) {
            return new MemberPermission {
                Read = Security.IsGranted(new PermissionRequest(ObjectSpace, type, SecurityOperations.Read, entity, member.Name)),
                Write = Security.IsGranted(new PermissionRequest(ObjectSpace, type, SecurityOperations.Write, entity, member.Name))
            };
        }
        private static IEnumerable<IMemberInfo> GetPersistentMembers(ITypeInfo typeInfo) {
			return typeInfo.Members.Where(p => p.IsVisible && p.IsProperty && (p.IsPersistent || p.IsList));
		}
        private TypePermission CreateTypePermission(ITypeInfo typeInfo) {
            Type type = typeInfo.Type;
            TypePermission typePermission = new TypePermission();
            typePermission.Key = type.Name;
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
