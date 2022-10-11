using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PuppeteerSharp;

namespace CookiesSaver
{
    public class PuppeteerWorker
    {
        private Browser _browser;
        private Page _page;
        private LaunchOptions _options;
        private readonly string _browserPath;
        private readonly string _url;

        public PuppeteerWorker(string browserPath, string url)
        {
            _browserPath = browserPath;
            _url = url;
            _options = new LaunchOptions
            {
                Headless = false,
                Args = new[]
                {
                    "--disable-gpu",
                    "--disable-dev-shm-usage",
                    "--disable-setuid-sandbox",
                    "--no-first-run",
                    "--no-sandbox",
                    "--no-zygote",
                    "--deterministic-fetch",
                    "--disable-features=IsolateOrigins",
                    "--disable-site-isolation-trials",
                    "--lang=en-GB",
                    "--window-size=1280,900"
                },
                IgnoredDefaultArgs = new string[]
                {
                    "--enable-automation"
                },
                ExecutablePath = browserPath
            };
        }

        public async Task Start()
        {
            Console.WriteLine(DateTime.Now + " : Started");
            await BrowserLoader(_url, 5);
            Console.WriteLine("Press any key to continiue...");
            Console.ReadLine();
            var result = await _page.GetCookiesAsync();
            var cookiesJson = JsonConvert.SerializeObject(result);
            await File.WriteAllTextAsync("cookies.json", cookiesJson);
            //await UpdateCookies(JsonConvert.DeserializeObject<ResolveCaptchaResponse>(text));
        }
        private async Task BrowserLoader(string url, int count)
        {
            try
            {
                _browser = await Puppeteer.LaunchAsync(_options);
                //await _browser.DefaultContext.ClearPermissionOverridesAsync();
                _page = await _browser.NewPageAsync();
                await _page.SetViewportAsync(new ViewPortOptions { Width = 1280, Height = 900 });
                await _page.GoToAsync(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now + " : ERROR " + ex.Message + " at " + _page.Url);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
