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
            Console.WriteLine(tables.Length.ToString()+ " UpdateSchema");
            return DataStoreService.UpdateSchemaAsync(doNotCreateIfFirstTableNotExist, tables);
        }
        [HttpPost]
        public Task<OperationResult<SelectedData>> SelectData([FromBody] SelectStatement[] selects) {
            Console.WriteLine(selects.Length.ToString() + " SelectData");
            var resp = DataStoreService.SelectDataAsync(selects);
            return resp;
        }
        [HttpPost]
        public Task<OperationResult<ModificationResult>> ModifyData([FromBody] ModificationStatement[] dmlStatements) {
            Console.WriteLine(dmlStatements.Length.ToString() + " ModifyData");
            return DataStoreService.ModifyDataAsync(dmlStatements);
        }
        [HttpGet]
        public OperationResult<SelectedData> GetData() {
            SelectStatement[] selects = null;
            return DataStoreService.SelectData(selects);
        }
    }
}
