using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

public static class Crawler
{
    public static IWebDriver InitializeDriver()
    {
        ChromeOptions options = new ChromeOptions();
        options.AddArgument("--disable-gpu");
        options.AddArgument("--window-size=1920,1080");
        return new ChromeDriver(options);
    }

    public static void FeFa(IWebDriver driver, string targetUrl)
    {
        try
        {
            driver.Navigate().GoToUrl(targetUrl);

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            wait.Until(d => d.FindElement(By.TagName("body")));

            var js = (IJavaScriptExecutor)driver;
            for (int i = 0; i < 5; i++)
            {
                js.ExecuteScript("window.scrollBy(0, 1000);");
                System.Threading.Thread.Sleep(2000);
            }

            var favVideoList = wait.Until(d => d.FindElement(By.XPath("/html/body/div[2]/div[4]/div/div[1]/div[2]/div[3]/ul[1]")));
            var listItems = favVideoList.FindElements(By.XPath("./li"));
            Console.WriteLine($"Found {listItems.Count} items in the fav-video-list.");

            if (listItems.Count == 0)
            {
                throw new Exception("No <li> elements found. Ensure the page has fully loaded.");
            }

            int count = Math.Min(5, listItems.Count);

            Console.WriteLine($"Extracting details for the first {count} items:");
            for (int i = 1; i <= count; i++)
            {
                var listItem = driver.FindElement(By.XPath($"/html/body/div[2]/div[4]/div/div[1]/div[2]/div[3]/ul[1]/li[{i}]"));
                string dataAid = listItem.GetAttribute("data-aid");

                var anchor = listItem.FindElement(By.XPath("./a[@class='title']"));
                string title = anchor.GetAttribute("title");
                string href = anchor.GetAttribute("href");

                Console.WriteLine($"Item {i}:");
                Console.WriteLine($"  Title: {title}");
                Console.WriteLine($"  Href: {href}");
                Console.WriteLine($"  Data-Aid: {dataAid}");
            }
        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine("Timeout: fav-video-list was not found.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
