using System.Collections;
using System.Linq;
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
            if(parameters.ContainsKey("keys") && parameters.ContainsKey("typeName")) {
                string typeName = parameters["typeName"].ToString();

                ITypeInfo typeInfo = typesInfo.PersistentTypes.FirstOrDefault(t => t.Name == typeName);
                if(typeInfo != null) {
                    Type type = typeInfo.Type;
                    using IObjectSpace objectSpace = objectSpaceFactory.CreateObjectSpace(type);
                    IEnumerable<int> keys = ((IEnumerable<string>)parameters["keys"]).Select(k => int.Parse(k));
                    IEnumerable<ObjectPermission> objectPermissions = objectSpace
                        .GetObjects(type, new InOperator(typeInfo.KeyMember.Name, keys))
                        .Cast<object>()
                        .Select(entity => CreateObjectPermission(typeInfo, entity))
                        .ToList();

                    return Ok(objectPermissions);
                }
            }
            return NoContent();
        }

        [HttpGet("/GetTypePermissions")]
        public ActionResult GetTypePermissions(string typeName) {
            ITypeInfo typeInfo = typesInfo.PersistentTypes.FirstOrDefault(t => t.Name == typeName);
            if(typeInfo != null) {
                Type type = typeInfo.Type;
                using IObjectSpace objectSpace = objectSpaceFactory.CreateObjectSpace(type);

                var result = new TypePermission {
                    Key = type.Name,
                    Create = security.CanCreate(type)
                };
                foreach(IMemberInfo member in GetPersistentMembers(typeInfo)) {
                    result.Data.Add(member.Name, security.CanWrite(type, member.Name));
                }
                return Ok(result);
            }
            return NoContent();
        }

        private ObjectPermission CreateObjectPermission(ITypeInfo typeInfo, object entity) {
            var objectPermission = new ObjectPermission {
                Key = typeInfo.KeyMember.GetValue(entity).ToString(),
                Write = security.CanWrite(entity),
                Delete = security.CanDelete(entity)
            };
            foreach(IMemberInfo member in GetPersistentMembers(typeInfo)) {
                objectPermission.Data.Add(member.Name, new MemberPermission {
                    Read = security.CanRead(entity, member.Name),
                    Write = security.CanWrite(entity, member.Name)
                });
            }
            return objectPermission;
        }

        private static IEnumerable<IMemberInfo> GetPersistentMembers(ITypeInfo typeInfo) {
            return typeInfo.Members.Where(p => p.IsVisible && p.IsProperty && (p.IsPersistent || p.IsList));
        }
    }
}
