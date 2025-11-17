using System.Net.Http.Headers;

namespace Bugtracker.Main.Statics;

public static class WebHelper
{
    public static async Task<string> UploadImage(FileResult file)
    {
        using (var fileStream = await file.OpenReadAsync())
        {
            using (var memStream = new MemoryStream())
            {
                await fileStream.CopyToAsync(memStream);
                var form = new MultipartFormDataContent();
                var byteContent = new ByteArrayContent(memStream.ToArray());
                form.Add(byteContent, "file", file.FileName);
                using (var client = new HttpClient())
                {
                    HttpRequestMessage message = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri($"{AuthData.BackendUrl}/upload"),
                        Content = form
                    };
                    using (var result = await client.SendAsync(message))
                    {
                        result.EnsureSuccessStatusCode();
                        return await result.Content.ReadAsStringAsync();
                    }
                }
            }
        }
    }
}