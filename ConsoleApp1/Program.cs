using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ConsoleApp1;

class Program
{
    class VietQRResponse
    {
        public string code { get; set; } = null!;
        public string desc { get; set; } = null!;
        public VietQRResponseData? data { get; set; }
    }
    class VietQRResponseData
    {
        public string? qrDataURL { get; set; }
    }
    class VietQRRequestBody
    {
        public string accountNo { get; set; } = null!;
        public string accountName { get; set; } = null!;
        public int acqId { get; set; }
        public int amount { get; set; }
        public string addInfo { get; set; } = null!;
        public string template { get; set; } = null!;
    }

    public static void Main()
    {
        //string? error = null;
        //string? result = null;
        //HttpClient httpRequest = new HttpClient(new HttpClientHandler
        //{
        //    UseProxy = false
        //});
        //httpRequest.DefaultRequestHeaders.Add("x-client-id", "24c3d1a3-4db6-4e00-8ba5-4a5fe12e4288");
        //httpRequest.DefaultRequestHeaders.Add("x-api-key", "99ab2b19-0be1-4328-8097-a66c03b494ba");
        //try
        //{
        //    for (int i = 0; i<2; i++)
        //    {
        //        var body = new VietQRRequestBody()
        //        {
        //            accountNo = "0942142790",
        //            accountName = "HOANG HUY HOC",
        //            acqId = 970422,
        //            amount = 10000,
        //            addInfo = $"{DateTime.Now.ToString("dd")} {DateTime.Now.ToString("MM")} {DateTime.Now.ToString("yyyy")}  {DateTime.Now.ToString("HH")} {DateTime.Now.ToString("mm")}   Tai khoan hhh nap tien",
        //            template = "print"
        //        };
        //        string json = JsonSerializer.Serialize(body);
        //        StringContent content = new(json, Encoding.UTF8, "application/json");
        //        HttpResponseMessage response = await httpRequest.PostAsync("https://api.vietqr.io/v2/generate", content);
        //        response.EnsureSuccessStatusCode();
        //        VietQRResponse? responseBody = await response.Content.ReadFromJsonAsync<VietQRResponse>(new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        //        if (responseBody!.code != "00")
        //        {
        //            error = responseBody!.desc!;
        //        }
        //        else
        //        {
        //            string qrDataString = responseBody.data!.qrDataURL!;
        //            result = Regex.Match(qrDataString, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
        //        }
        //    }

        //}
        //catch (Exception e)
        //{
        //    error = e.Message;
        //}
        //httpRequest.Dispose();
        //Console.WriteLine(result??error);
        /*TcpClient a = new();
        try
        {
            a.ConnectAsync(IPAddress.Parse("192.168.252.1"), 8888, ).;
        }
        catch
        {

        }
        

        Console.WriteLine(a.Connected);*/
    }
}



