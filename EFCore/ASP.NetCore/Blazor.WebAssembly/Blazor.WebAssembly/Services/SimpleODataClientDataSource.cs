using System.Collections;
using Blazor.WebAssembly.Models;
using DevExpress.Blazor;
using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using Simple.OData.Client;

namespace Blazor.WebAssembly.Services;

public class SimpleODataClientDataSource : GridCustomDataSource {
    private readonly ODataClient _client;
    public SimpleODataClientDataSource(IHttpClientFactory httpClientFactory)
        => _client = new ODataClient(new ODataClientSettings(httpClientFactory.CreateClient("API"), new Uri("odata/", UriKind.Relative)));

    public override async Task<int> GetItemCountAsync(GridCustomDataSourceCountOptions options, CancellationToken cancellationToken)
        => await ApplyFiltering(options.FilterCriteria, _client.For<Post>()).Count()
            .FindScalarAsync<int>(cancellationToken);

    public override async Task<IList> GetItemsAsync(GridCustomDataSourceItemsOptions options, CancellationToken cancellationToken) {
        var filteredClient = ApplyFiltering(options.FilterCriteria, _client.For<Post>().Top(options.Count).Skip(options.StartIndex));
        return (await ApplySorting(options, filteredClient).FindEntriesAsync(cancellationToken)).ToList();
    }

    private static IBoundClient<Post> ApplyFiltering(CriteriaOperator criteria, IBoundClient<Post> boundClient)
        => !criteria.ReferenceEqualsNull() ? boundClient.Filter(ToSimpleClientCriteria(criteria)) : boundClient;

    private static string ToSimpleClientCriteria(CriteriaOperator criteria)
        => $"{criteria}".Replace("[", "").Replace("]", "");

    private static IBoundClient<Post> ApplySorting(GridCustomDataSourceItemsOptions options, IBoundClient<Post> boundClient) {
        return options.SortInfo != null && options.SortInfo.Any() ? boundClient.OrderBy(options.SortInfo
                .Where(info => !info.DescendingSortOrder).Select(info => info.FieldName).ToArray())
            .OrderByDescending(options.SortInfo
                .Where(info => info.DescendingSortOrder).Select(info => info.FieldName).ToArray()) : boundClient;

    public async Task DeleteAsync(Post instance)
        => await _client.For<Post>().Key(instance.ID).DeleteEntryAsync();

    public async Task AddOrUpdateAsync(Post instance, bool update = false) {
        if (!update) {
            await _client.For<Post>().Set(new { instance.Title, instance.Content }).InsertEntryAsync();
        }
        else {

            await _client.For<Post>().Key(instance.ID).Set(instance).UpdateEntryAsync();
        }
    }
}