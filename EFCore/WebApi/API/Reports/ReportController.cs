#nullable enable
using DevExpress.ExpressApp.ReportsV2.Services;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.WebApi.Reports;

[Authorize]
[Route("api/[controller]")]
// This is a WebApi Reports controller sample.
public class ReportController : ControllerBase {
    private readonly IReportExportService service;

    public ReportController(IReportExportService reportExportService) {
        service = reportExportService;
    }

    private void ApplyParametersFromQuery(XtraReport report) {
        foreach(var parameter in report.Parameters) {
            var queryParam = Request.Query[parameter.Description];
            if(queryParam.Count > 0) {
                parameter.Value = queryParam.First();
            }
        }
    }
    private SortProperty[]? LoadSortPropertiesFromQuery() {
        if(Request.Query.Keys.Contains("sortProperty")) {
            var queryParam = Request.Query["sortProperty"];
            SortProperty[] result = new SortProperty[queryParam.Count];
            for(int i = 0; i < queryParam.Count; i++) {
                string[] paramData = queryParam[i].Split(",");
                result[i] = new SortProperty(paramData[0], (SortingDirection)Enum.Parse(typeof(SortingDirection), paramData[1]));
            }
            return result;
        }
        return null;
    }

    private async Task<object> GetReportContentAsync(XtraReport report, ExportTarget fileType) {
        Stream ms = await service.ExportReportAsync(report, fileType);
        HttpContext.Response.RegisterForDispose(ms);
        return File(ms, service.GetContentType(fileType), $"{report.DisplayName}.{service.GetFileExtension(fileType)}");
    }

    [HttpGet("DownloadByKey({key})")]
    public async Task<object> DownloadByKey(string key,
        [FromQuery] ExportTarget fileType = ExportTarget.Pdf,
        [FromQuery] string? criteria = null) {
        using var report = service.LoadReport<ReportDataV2>(key);
        ApplyParametersFromQuery(report);
        SortProperty[]? sortProperties = LoadSortPropertiesFromQuery();
        service.SetupReport(report, criteria, sortProperties);
        return await GetReportContentAsync(report, fileType);
    }

    [HttpGet("DownloadByName({displayName})")]
    public async Task<object> DownloadByName(string displayName,
        [FromQuery] ExportTarget fileType = ExportTarget.Pdf,
        [FromQuery] string? criteria = null) {
        if(!string.IsNullOrEmpty(displayName)) {
            using var report = service.LoadReport<ReportDataV2>(data => data.DisplayName == displayName);
            ApplyParametersFromQuery(report);
            SortProperty[]? sortProperties = LoadSortPropertiesFromQuery();
             service.SetupReport(report, criteria, sortProperties);
            return await GetReportContentAsync(report, fileType);
        }
        return NotFound();
    }
}
#nullable restore