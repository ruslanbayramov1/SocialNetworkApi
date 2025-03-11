using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Zust.BL.DTOs.AiDatas;
using Zust.BL.Exceptions.Common;
using Zust.BL.Options;
using Zust.BL.Services.Interfaces;

namespace Zust.BL.Services.Implements;

public class AiAnalyzeService : IAiAnalyzeService
{
    private readonly HttpClient _client;
    private readonly string apiKey = "";
    private readonly string assistantId = "";
    private readonly string apiFileUrl = "https://api.openai.com/v1/files";
    private readonly string apiThreadMessageUrl = "https://api.openai.com/v1/threads/{thread_id}/messages";
    private readonly string apiThreadMessageRunUrl = "https://api.openai.com/v1/threads/{thread_id}/runs";
    private readonly string apiThreadUrl = "https://api.openai.com/v1/threads";
    private readonly string apiThreadMessageRetrieveGetUrl = "https://api.openai.com/v1/threads/{thread_id}/runs/{run_id}";
    private readonly string apiThreadDeleteUrl = "https://api.openai.com/v1/threads/{thread_id}";
    public AiAnalyzeService(HttpClient httpClient, IOptions<OpenAiOption> opt)
    {
        OpenAiOption _opt = opt.Value;
        _client = httpClient;
        apiKey = _opt.ApiKey;
        assistantId = _opt.AssistantId;
    }

    public async Task<FileResponse> UploadFileAsync(IFormFile file)
    {
        // Clear default headers and add Authorization header
        _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        _client.DefaultRequestHeaders.Add("OpenAI-Beta", $"assistants=v2");

        // Prepare the form content to upload the file
        using (var form = new MultipartFormDataContent())
        {
            // Add the 'purpose' field to the form
            form.Add(new StringContent("assistants"), "purpose");

            // Create a StreamContent from the file and set its content type to application/jsonl
            var fileContent = new StreamContent(file.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/jsonl");

            // Add the file content to the form, "file" is the key OpenAI expects
            form.Add(fileContent, "file", file.FileName);

            // Send the POST request with the form data
            var response = await _client.PostAsync(apiFileUrl, form);

            // Check the response
            if (response.IsSuccessStatusCode)
            {
                // Deserialize the JSON response into a FileResponse object
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var deData = JsonSerializer.Deserialize<FileResponse>(jsonResponse);
                return deData;
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new Exception("Not valid data");
            }
        }
    }

    public async Task<FileListResponse> GetAllFilesAsync()
    {
        _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var data = await _client.GetAsync(apiFileUrl);

        if (data.IsSuccessStatusCode)
        {
            var jsonResponse = await data.Content.ReadAsStringAsync();
            var fileListResponse = JsonSerializer.Deserialize<FileListResponse>(jsonResponse);

            var jsonDoc = JsonDocument.Parse(jsonResponse);
            fileListResponse.Object = jsonDoc.RootElement.GetProperty("object").ToString();

            return fileListResponse;
        }
        else
        {
            var errorResponse = await data.Content.ReadAsStringAsync();
            throw new Exception($"Error fetching files: {errorResponse}");
        }
    }

    public async Task<FileResponse> GetFileByIdAsync(string fileId)
    {
        _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var data = await _client.GetAsync($"{apiFileUrl}/{fileId}");

        var jsonResponse = await data.Content.ReadAsStringAsync();

        if (String.IsNullOrEmpty(jsonResponse))
            throw new NotFoundException("File");

        var deData = JsonSerializer.Deserialize<FileResponse>(jsonResponse);

        var jsonDoc = JsonDocument.Parse(jsonResponse);
        deData.Object = jsonDoc.RootElement.GetProperty("object").ToString();

        return deData;
    }

    public async Task<string> AnalyzeAsync(string fileId)
    {
        _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        _client.DefaultRequestHeaders.Add("OpenAI-Beta", $"assistants=v2");

        // -----CREATE THREAD-----
        var dataThread = await _client.PostAsync(apiThreadUrl, null);
        var dataThreadParsed = JsonDocument.Parse(await dataThread.Content.ReadAsStringAsync());
        string threadId = dataThreadParsed.RootElement.GetProperty("id").ToString();

        // -----CREATE MESSAGE BASED ON THREAD-----
        var reqData = new
        {
            role = "user",
            content = "check my file, is it violates terms or not.",
            attachments = new[]
            {
                new
                {
                    file_id = fileId,
                    tools = new[]
                    {
                        new { type = "file_search" } // tools olarak nesne kullanıyoruz
                    }
                }
            }
        };


        var reqDataStr = JsonSerializer.Serialize(reqData);
        var messageContent = new StringContent(reqDataStr, Encoding.UTF8, "application/json");
        var resMessage = await _client.PostAsync(apiThreadMessageUrl.Replace("{thread_id}", threadId), messageContent);

        if (!resMessage.IsSuccessStatusCode)
            throw new Exception(await resMessage.Content.ReadAsStringAsync());

        // -----RUN MESSAGE WITH ASSISTANT ID-----
        var reqRunData = new
        {
            assistant_id = assistantId
        };
        var reqRunDataStr = JsonSerializer.Serialize(reqRunData);
        var messageRunContent = new StringContent(reqRunDataStr, Encoding.UTF8, "application/json");
        var resMessageRun = await _client.PostAsync(apiThreadMessageRunUrl.Replace("{thread_id}", threadId), messageRunContent);

        // -----RETRIEVE MESSAGE WITH RUN ID AND THREAD ID-----
        var reqRunDataParsed = JsonDocument.Parse(await resMessageRun.Content.ReadAsStringAsync());
        string runId = reqRunDataParsed.RootElement.GetProperty("id").ToString(); // getting run id from parsed run post request data

        var resRetrieveRunData = await _client.GetAsync(apiThreadMessageRetrieveGetUrl.Replace("{thread_id}", threadId).Replace("{run_id}", runId));

        // -----RETRIEVE MESSAGE-----
        var finalData = await _client.GetAsync(apiThreadMessageUrl.Replace("{thread_id}", threadId));

        var finalDataStr = await finalData.Content.ReadAsStringAsync();
        var finalDataParse = JsonDocument.Parse(finalDataStr);
        var finalProperty = finalDataParse.RootElement.GetProperty("first_id");
        var sum = 0;
        Thread.Sleep(15000);
        finalData = await _client.GetAsync(apiThreadMessageUrl.Replace("{thread_id}", threadId));
        await _client.DeleteAsync(apiThreadDeleteUrl.Replace("{thread_id}", threadId));

        return await finalData.Content.ReadAsStringAsync();
    }
}