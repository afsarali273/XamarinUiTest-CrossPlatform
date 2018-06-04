using System;
using System.Reflection;
using NUnit.Framework;
using Xamarin.UITest;
using Mobile;
using System.Threading;

[TestFixture(Platform.Android)]
[TestFixture(Platform.iOS)]
public class Coverage
{
    IApp app;
    Platform platform;

    public Coverage(Platform platform)
    {
        this.platform = platform;
    }

    [SetUp]
    public void SetUp()
    {
        app = Core.StartApp(platform);

    }

    [Test]
    public void CoverageTest()
    {
        String testname = MethodBase.GetCurrentMethod().Name;
        
        Core.EnvPicker(app, testname);
        Core.SoftwareUpdate(app, testname);
        Core.OTPBypass(app, testname);
        try
        {
            Console.WriteLine(testname);
            Core.performLogin(app, Core.dictProfiles["nxtlvlusername"], Core.dictProfiles["GlobalPassword"], testname);
            Core.WaitForLoadingScreen(app);
            Core.Menu(app, testname);

            Thread.Sleep(900);

            app.WaitForElement(x => x.Marked("Help"), timeout: TimeSpan.FromSeconds(5));
            app.Tap(x => x.Marked("Help"));

            try
            {
                app.WaitForElement(x => x.Marked("Coverage Maps"), timeout: TimeSpan.FromSeconds(5));
                app.Tap(x => x.Marked("Coverage Maps"));

                var i = app.Query(c => c.WebView());

                Console.WriteLine(i);
                //interacting with Webview
                app.Tap(x => x.XPath("//*[@id=\"txtSearch1\"]"));
                app.ClearText();
                app.EnterText("Midrand");
                Thread.Sleep(5000);
                Core.TakeScreenShot(app, "Searched midrand", testname);

                //tap on an empty spot
                app.TapCoordinates(365, 874);

                Thread.Sleep(500);

                //tap search button          
                app.TapCoordinates(304, 454);

                //tap midrand from search results
                Core.TakeScreenShot(app, "Midrand Pin", testname);
                try
                {
                    app.Tap(x => x.XPath("//*[@id=\"searchResultsDiv\"]/div[1]"));
                }
                catch { }
                //Logout
                Core.Logout(app, testname);
            }
            catch
            {

                //For IOS no  coverage map
                app.Tap(x => x.Text(Core.OR["help"]));
                Core.TakeScreenShot(app, "Help logout", testname);

                app.Tap(x => x.Text(Core.OR["Logout"]));
            }


           
        }
        finally
        {
            Core.TakeScreenShot(app, "Last Screenshot", testname);
        }
    }
}