class Program
{
    static void Main(string[] args)
    {
        var driver = Crawler.InitializeDriver();
        Console.WriteLine("Please log in to the browser, then press ENTER to continue...");
        driver.Navigate().GoToUrl("https://www.bilibili.com");
        Console.ReadLine();

        string targetUrl = "https://space.bilibili.com/388282085/favlist?fid=290867585&ftype=create";

        Crawler.FeFa(driver, targetUrl);
        driver.Quit();
    }
}
