using System;
using System.Reflection;
using NUnit.Framework;
using Xamarin.UITest;
using Mobile;
using System.Threading;

[TestFixture(Platform.Android)]
[TestFixture(Platform.iOS)]
public class ActivateNXTLVL
{
    IApp app;
    Platform platform;

    public ActivateNXTLVL(Platform platform)
    {
        this.platform = platform;
    }
    [SetUp]
    public void SetUp()
    {
        app = Core.StartApp(platform);

    }

    [Test]
    public void ActivateNXTLVLTest()
    {
        String testname = MethodBase.GetCurrentMethod().Name;

        Core.EnvPicker(app,testname);
        Core.SoftwareUpdate(app, testname);
        Core.OTPBypass(app, testname);
        try
        {
            Console.WriteLine(testname);
            Core.performLogin(app, Core.dictProfiles["nxtlvlusername"], Core.dictProfiles["GlobalPassword"], testname);
            Core.WaitForLoadingScreen(app);
            Core.Menu(app, testname);

            // Go to Slider Menu & select Acivate to NXT LVL
            app.Tap(x => x.Text(Core.OR["Competitionsandpromotions"]));
            Core.TakeScreenShot(app, "Competitions and promo", testname);

            try
            {
                app.WaitForElement(x => x.Text(Core.OR["ActivateNXTLVL"]), timeout: TimeSpan.FromSeconds(5));
                app.Tap(x => x.Text(Core.OR["ActivateNXTLVL"]));

                app.WaitForElement(x => x.Text(Core.OR["NxtLvlRegistration"]), timeout: TimeSpan.FromSeconds(5));
                app.WaitForElement(x => x.Text(Core.OR["SouthAfricanID"]), timeout: TimeSpan.FromSeconds(5));
                Core.TakeScreenShot(app, "Activate NXT LVL", testname);

                app.Tap(x => x.Marked(Core.OR["IdTextBoxfld"]));

                app.EnterText(x => x.Marked(Core.OR["IdTextBoxfld"]), "7901145878189");
                app.WaitForElement(x => x.Text(Core.OR["TandC"]), timeout: TimeSpan.FromSeconds(15));

                app.Tap(x => x.Text(Core.OR["TandC"]));
                try
                {
                    app.WaitForElement(x => x.Text(Core.OR["T&C"]));
                }
                catch
                {
                    app.WaitForElement(x => x.Text("Terms and Conditions"));
                }
                app.ScrollDown();
                Core.TakeScreenShot(app, Core.OR["TandC"] + " swiped up", testname);

                app.ScrollUp();
                Core.TakeScreenShot(app, Core.OR["TandC"] + " swiped down", testname);

                app.WaitForElement(x => x.Marked(Core.OR["BackBtnIcon"]), timeout: TimeSpan.FromSeconds(15));
                app.Tap(x => x.Marked(Core.OR["BackBtnIcon"]));

                app.WaitForElement(x => x.Text(Core.OR["Activatebtn"]), timeout: TimeSpan.FromSeconds(5));
                Core.TakeScreenShot(app, "Back button", testname);

                app.Tap(x => x.Text(Core.OR["Activatebtn"]));
                app.WaitForElement(x => x.Text(Core.OR["WereSorryMsg"]), timeout: TimeSpan.FromSeconds(5));

                try
                {
                    app.WaitForElement(x => x.Text("The ID number entered is incorrect. Finger trouble maybe? Please try again and check that there are no white spaces between the numbers"), timeout: TimeSpan.FromSeconds(5));
                }
                catch
                {
                    app.WaitForElement(x => x.Text("Unfortunately you need to be under 25 in order to qualify for NXT LVL."), timeout: TimeSpan.FromSeconds(5));
                }

                app.WaitForElement(x => x.Text("Ok"), timeout: TimeSpan.FromSeconds(5));
                Core.TakeScreenShot(app, "Incorrect ID", testname);

                app.Tap(x => x.Text(Core.OR["Ok"]));

                app.WaitForElement(x => x.Marked(Core.OR["BackBtnIcon"]), timeout: TimeSpan.FromSeconds(15));
                app.Tap(x => x.Marked(Core.OR["BackBtnIcon"]));
                app.WaitForElement(x => x.Marked(Core.OR["BackBtnIcon"]), timeout: TimeSpan.FromSeconds(15));
                app.Tap(x => x.Marked(Core.OR["BackBtnIcon"]));
            }
            catch
            {

            }
            //Logout
            Core.Logout(app, testname);
            
        }
        finally
        {
            Core.TakeScreenShot(app, "Last Screenshot", testname);
        }
    }
}