using CookiesSaver;

var url = args[0].Substring(args[0].IndexOf('=') + 1);
var browserPath = args[1].Substring(args[1].IndexOf('=') + 1);
if (args.Length <= 1)
{
    Console.WriteLine("Arg must be!");
    return;
}
Console.WriteLine( "URL: " + url);
Console.WriteLine( "Browser Path: " + browserPath);
PuppeteerWorker puppeteerWorker = new PuppeteerWorker(url, browserPath);
await puppeteerWorker.Start();