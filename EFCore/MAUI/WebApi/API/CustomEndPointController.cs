using System.Text.Json;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.BusinessObjects;

namespace WebAPI.API {
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomEndPointController : ControllerBase {
        private readonly ISecurityProvider _securityProvider;
        private readonly IObjectSpaceFactory _securedObjectSpaceFactory;


        public CustomEndPointController(ISecurityProvider securityProvider, IObjectSpaceFactory securedObjectSpaceFactory) {
            _securityProvider = securityProvider;
            _securedObjectSpaceFactory = securedObjectSpaceFactory;
        }

        [HttpGet(nameof(CanCreate))]
        public IActionResult CanCreate(string typeName) {
            var strategy = (SecurityStrategy)_securityProvider.GetSecurity();
            var objectType = strategy.TypesInfo.PersistentTypes.First(info => info.Name == typeName).Type;
            return Ok(strategy.CanCreate(objectType));
        }

        [HttpGet("AuthorPhoto/{postId}")]
        public FileStreamResult AuthorPhoto(int postId) {
            using var objectSpace = _securedObjectSpaceFactory.CreateObjectSpace(typeof(Post));
            var post = objectSpace.GetObjectByKey<Post>(postId);
            var photoBytes = post.Author.Photo.MediaData;
            return File(new MemoryStream(photoBytes), "application/octet-stream");
        }

        [HttpPost(nameof(Archive))]
        public async Task<IActionResult> Archive([FromBody] Post post) {
            using var objectSpace = _securedObjectSpaceFactory.CreateObjectSpace<Post>();
            post = objectSpace.GetObject(post);
            var photo = post.Author.Photo.MediaResource.MediaData;
            await System.IO.File.WriteAllTextAsync($"{post.PostId}",
                JsonSerializer.Serialize(new { photo, post.Title, post.Content, post.Author.UserName }));
            return Ok();
        }

        [HttpGet(nameof(GetReport))]
        public RedirectResult GetReport() 
	        => Redirect("~/api/report/DownloadByName(Post Report)");

	       
    }
}
