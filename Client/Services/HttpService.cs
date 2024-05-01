using System.IO.Pipelines;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Snaptwit.Shared.DTOs;

namespace SnapTwit.Client.Services;

public class HttpService<T>
{
    private readonly HttpClient _http;
    private readonly AuthenticationStateProvider _auth;

    public HttpService(HttpClient http, AuthenticationStateProvider auth)
    {
        _http = http;
        _auth = auth;
    }

    public async Task<(T?, string?)> GetAsync(string url)
    {
        var response = await _http.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await (_auth as AuthStateProvider)!.Logout();
            return (default, null);
        }

        if (response.IsSuccessStatusCode)
        {
            T? results = await response.Content.ReadFromJsonAsync<T>();
            return (results, null);
        }
        else
        {
            string error = await response.Content.ReadAsStringAsync();
            return (default, error);
        }
    }

    public async Task<string?> PostAsJsonAsync(string url, T item)
    {
        var response = await _http.PostAsJsonAsync(url, item);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await (_auth as AuthStateProvider)!.Logout();
            return null;
        }

        if (response.IsSuccessStatusCode)
        {
            return null;
        }
        else
        {
            string error = await response.Content.ReadAsStringAsync();
            return error;
        }
    }

    public async Task<UploadResult?> UploadFile(IBrowserFile file, long maxSize, string text, string url)
    {
        using var content = new MultipartFormDataContent();
        using var fileContent = new StreamContent(file.OpenReadStream(maxSize));
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

        content.Add(
            content: fileContent,
            name: "\"files\"",
            fileName: (text.Length == 0) ? "file" : text
        );

        var response = await _http.PostAsync(url, content);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            await (_auth as AuthStateProvider)!.Logout();

        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<UploadResult>();
            
        return null;
    }
}