using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.Xpo.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApiService.Controllers {
    [ApiController]
    [Route("[controller]/[action]")]
    public class XpoController : ControllerBase {
        readonly WebApiDataStoreService DataStoreService;
        public XpoController(WebApiDataStoreService dataStoreService) {
            this.DataStoreService = dataStoreService;
        }
        [HttpPost]
        public Task<OperationResult<UpdateSchemaResult>> UpdateSchema([FromQuery] bool doNotCreateIfFirstTableNotExist, [FromBody] DBTable[] tables) {
            return DataStoreService.UpdateSchemaAsync(doNotCreateIfFirstTableNotExist, tables);
        }
        [HttpPost]
        public Task<OperationResult<SelectedData>> SelectData([FromBody] SelectStatement[] selects) {
            return DataStoreService.SelectDataAsync(selects);
        }
        [HttpPost]
        public Task<OperationResult<ModificationResult>> ModifyData([FromBody] ModificationStatement[] dmlStatements) {
            return DataStoreService.ModifyDataAsync(dmlStatements);
        }
    }
}
