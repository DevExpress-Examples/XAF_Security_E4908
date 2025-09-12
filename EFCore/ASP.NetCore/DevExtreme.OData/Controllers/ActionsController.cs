using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace DevExtreme.OData.Controllers {
    [Authorize]
    [ValidateAntiForgeryToken]
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
                    IEnumerable<Guid> keys = ((IEnumerable<string>)parameters["keys"]).Select(k => Guid.Parse(k));
                    IEnumerable<ObjectPermission> objectPermissions = objectSpace
                        .GetObjects(type, new InOperator(typeInfo.KeyMember.Name, keys))
                        .Cast<object>()
                        .Select(entity => CreateObjectPermission(typeInfo, entity, objectSpace))
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
                    Create = security.CanCreate(type, objectSpace)
                };
                foreach(IMemberInfo member in GetPersistentMembers(typeInfo)) {
                    result.Data.Add(member.Name, security.CanWrite(type, objectSpace, member.Name));
                }
                return Ok(result);
            }
            return NoContent();
        }

        private ObjectPermission CreateObjectPermission(ITypeInfo typeInfo, object entity, IObjectSpace objectSpace) {
            var objectPermission = new ObjectPermission {
                Key = typeInfo.KeyMember.GetValue(entity).ToString(),
                Write = security.CanWrite(objectSpace, entity),
                Delete = security.CanDelete(objectSpace, entity)
            };
            foreach(IMemberInfo member in GetPersistentMembers(typeInfo)) {
                objectPermission.Data.Add(member.Name, new MemberPermission {
                    Read = security.CanRead(objectSpace, entity, member.Name),
                    Write = security.CanWrite(objectSpace, entity, member.Name)
                });
            }
            return objectPermission;
        }

        private static IEnumerable<IMemberInfo> GetPersistentMembers(ITypeInfo typeInfo) {
            return typeInfo.Members.Where(p => p.IsVisible && p.IsProperty && (p.IsPersistent || p.IsList));
        }
    }
}
