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
				List<PermissionContainer> permissionContainerList = new List<PermissionContainer>();
				ITypeInfo typeInfo = ObjectSpace.TypesInfo.PersistentTypes.FirstOrDefault(t => t.Name == typeName);
				if(typeInfo != null) {
					Type type = typeInfo.Type;
					IList entityList = ObjectSpace.GetObjects(type, new InOperator(typeInfo.KeyMember.Name, keys));
					foreach(var entity in entityList) {
						PermissionContainer permissionContainer = new PermissionContainer();
						permissionContainer.Key = typeInfo.KeyMember.GetValue(entity).ToString();
						IEnumerable<IMemberInfo> memberList = GetPersistentMembers(typeInfo);
						foreach(IMemberInfo member in memberList) {
							bool permission = Security.IsGranted(new PermissionRequest(ObjectSpace, type, SecurityOperations.Read, entity, member.Name));
							permissionContainer.Data.Add(member.Name, permission);
						}
						permissionContainerList.Add(permissionContainer);
					}
					result = Ok(permissionContainerList.AsQueryable());
				}
			}
			return result;
		}
		private static IEnumerable<IMemberInfo> GetPersistentMembers(ITypeInfo typeInfo) {
			return typeInfo.Members.Where(p => p.IsVisible && p.IsProperty && (p.IsPersistent || p.IsList));
		}
	}
}
