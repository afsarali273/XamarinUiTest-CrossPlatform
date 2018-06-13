using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xamarin.UITest;
using System.IO;
using Xamarin.UITest.Queries;
using NUnit.Framework;

namespace VodaiOS
{
    public class Core 
    {
     

        public static Dictionary<string, string> dictProfiles = new Dictionary<string, string>();
        public static Dictionary<string, string> OR = new Dictionary<string, string>();
        public static double nbr = 1;

        public static IApp StartApp(Platform platform)
        {

            if (platform == Platform.iOS && (Properties.Resources.MechineOS.Equals("Windows")))
            {
                //if you are using windows it will ignore all the IOS test scripts since windows can not run IOS applications/simulators
                Assert.Ignore();
            }
            else { }
                
            if (platform == Platform.Android)
                {
                    return ConfigureApp.Android.EnableLocalScreenshots().ApkFile(VodaiOS.Properties.Resources.apkPath).StartApp();
                }
                else
                    return ConfigureApp.iOS.EnableLocalScreenshots().AppBundle(Properties.Resources.apkPath).StartApp();
            
        }



        public static void WaitForLoadingScreen(IApp app)
        {
            Func<AppQuery, AppQuery> loadingScreenQuery = e => e.Marked("Spinner");
            app.WaitForNoElement(loadingScreenQuery, "Timed out waiting for Loading screen to disappear.", timeout: TimeSpan.FromSeconds(90));
        }

        public static void EnvPicker(IApp app,string testname)
        {

            Console.WriteLine(app);

            try { 
                app.WaitForElement(x => x.Text(Properties.Resources.env), timeout: TimeSpan.FromSeconds(90));

                app.Tap(x => x.Text(VodaiOS.Properties.Resources.env));
            }
            catch            {
                //skip for IOS
            }

            if (VodaiOS.Properties.Resources.env.Equals("LIVE PROD"))
                {
                    PopulateDictionary(@"Envtestdata/LIVE PROD.txt");
                    PopulateOR(@"EnvtestOR/LIVE PROD OR.txt");
                    //PopulateOR(@"Envtestdata/OR.txt");
                }
                else if (VodaiOS.Properties.Resources.env.Equals("PRODA External"))
                {
                    PopulateDictionary(@"Envtestdata/PRODA External.txt");
                    // PopulateOR(@"EnvtestOR/LIVE PROD OR.txt");
                    PopulateOR(@"EnvtestOR/PRODA External OR.txt");
                }
                else
                {
                    PopulateDictionary(@"Envtestdata/LIVE PROD.txt");
                    PopulateOR(@"EnvtestOR/LIVE PROD OR.txt");
                    //  PopulateOR(@"Envtestdata/OR.txt");
                }
            
        }
        public static void PopulateDictionary(string path)
        {
            string[] txtFileLines = File.ReadAllLines(path);
            foreach (var line in txtFileLines)
            {
                string[] str = line.Split(',');
                if (txtFileLines.Contains(Core.dictProfiles[str[0]] = str[1]))
                {
                    dictProfiles.Add(str[0], str[1]);

                }
                else
                {
                    Core.dictProfiles[str[0]] = str[1];
                    continue;
                }
            }


        }
        public static void PopulateOR(string path)
        {
            string[] txtFileLines = File.ReadAllLines(path);
            foreach (var line in txtFileLines)
            {
                string[] str = line.Split(',');
                if (txtFileLines.Contains(Core.OR[str[0]] = str[1]))
                {
                    OR.Add(str[0], str[1]);

                }
                else
                {
                    Core.OR[str[0]] = str[1];
                    continue;
                }
            }

        }

        public static void performLogin(IApp app, String Username, String Password, string testname)
        {
            app.ClearText(x => x.Marked(Core.OR["UserNameTextBox"]));
            app.Tap(x => x.Marked(Core.OR["UserNameTextBox"]));
            app.EnterText(x => x.Marked(Core.OR["UserNameTextBox"]), Username);
      
            app.WaitForElement(x => x.Text("Log in to My Vodacom"), timeout: TimeSpan.FromSeconds(30));
            
            app.WaitForElement(x => x.Text(Core.OR["Username"]));
            app.WaitForElement(x => x.Text(Core.OR["Password"]));
            app.Tap(x => x.Marked(Core.OR["PasswordTextBox"]).Index(1));
           // app.DoubleTapCoordinates(274, 930);
           // app.EnterText(Password);
           app.EnterText(x => x.Marked(Core.OR["PasswordTextBox"]).Index(1), Password);
            app.DismissKeyboard();

            Core.TakeScreenShot(app, "Login Details", testname);
            try
            {
                app.Tap(x => x.Marked(Core.OR["LoginButton"]));
            }
            catch
            {
                app.Tap(x => x.Text("Log in"));

            }
            try
            {
                app.ScrollDown();
                app.WaitForElement(x => x.Marked(Core.OR["Proceed"]), timeout: TimeSpan.FromSeconds(30));
                app.Tap(x => x.Marked(Core.OR["Proceed"]));


                app.WaitForElement(x => x.Text("Proceed to My Vodacom"), timeout: TimeSpan.FromSeconds(30));
                app.Tap(x => x.Text("Proceed to My Vodacom"));
            }
            catch
            {

            }
            app.WaitForElement(x => x.Marked("Quick buy Button"), timeout: TimeSpan.FromSeconds(30));
            
        }

        public static void Menu(IApp app, string testname)
        {
            try
            {
                Thread.Sleep(5000);
                app.WaitForElement(x => x.Marked(Core.OR["Backbtn"]), timeout: TimeSpan.FromSeconds(20));
                app.Tap(x => x.Marked(Core.OR["Backbtn"]));
                Thread.Sleep(3000);

            }
            catch
            {
                //Do nothing
            }
            Thread.Sleep(5000);
            try
            {
                app.WaitForElement(x => x.Marked(Core.OR["Menu"]), timeout: TimeSpan.FromSeconds(90));
                Core.TakeScreenShot(app, "Account Overview", testname);
                app.Tap(x => x.Marked(Core.OR["Menu"]));
                Thread.Sleep(3000);
                Core.TakeScreenShot(app, "Menu", testname);

            }
            catch
            {
                app.WaitForElement(x => x.Marked("menu"), timeout: TimeSpan.FromSeconds(90));
                Core.TakeScreenShot(app, "Account Overview", testname);
                app.Tap(x => x.Marked("menu"));
                Thread.Sleep(3000);
            }

        }

        public static void SurveyPopUp(IApp app, string testname)
        {
            try
            {
                app.WaitForElement(x => x.Text(Core.OR["NoThanks"]), timeout: TimeSpan.FromSeconds(30));
                Core.TakeScreenShot(app, "Survey pop-up", testname);
                app.Tap(x => x.Text(Core.OR["NoThanks"]));
            }
            catch
            {
                Thread.Sleep(5000);
            }
        }

        public static void Logout(IApp app, string testname)
        {
            // deleteDirectory();
            try
            {
                Thread.Sleep(5000);
                app.WaitForElement(x => x.Marked(Core.OR["Menu"]), timeout: TimeSpan.FromSeconds(30));
                Core.TakeScreenShot(app, "Account Overview", testname);
                app.Tap(x => x.Class("android.widget.ImageButton").Marked(Core.OR["Menu"]));
                Thread.Sleep(3000);
                Core.TakeScreenShot(app, "Menu - Logout", testname);
            }
            catch
            {
                Thread.Sleep(5000);
                app.WaitForElement(x => x.Marked(Core.OR["BackBtnIcon"]), timeout: TimeSpan.FromSeconds(30));
                app.Tap(x => x.Marked(Core.OR["BackBtnIcon"]));
                Thread.Sleep(3000);


                Menu(app, testname);


            }
            app.Tap(x => x.Text(Core.OR["help"]));
            app.WaitForElement(x => x.Text(Core.OR["Logout"]), timeout: TimeSpan.FromSeconds(30));
            app.Tap(x => x.Text(Core.OR["Logout"]));

       
             //   app.WaitForElement(x => x.Text("Welcome to My Vodacom"), timeout: TimeSpan.FromSeconds(30));
            
        }


        public static void PurchaseSummary(IApp app,string testname)
        {
            app.WaitForElement(x => x.Text(Core.OR["PaymentOptions"]), timeout: TimeSpan.FromSeconds(11));
            app.WaitForElement(x => x.Text(Core.OR["Purchasesummary"]), timeout: TimeSpan.FromSeconds(10));
            TakeScreenShot(app,"Purchase summary", testname);

            app.ScrollDown();
            app.Tap(x => x.Text("Buy"));
            WaitForLoadingScreen(app);
            try
            {
                app.WaitForElement(x => x.Text(Core.OR["Success"]), timeout: TimeSpan.FromSeconds(25));
                TakeScreenShot(app,"Success", testname);
                app.WaitForElement(x => x.Text(Core.OR["Ok"]), timeout: TimeSpan.FromSeconds(10));
                app.Tap(x => x.Text(Core.OR["Ok"]));
            }
            catch
            {
                try
                {
                    //When user is not authorised to perform purchase msg pops up
                    var message = app.WaitForElement(x => x.Marked("Message")).First().Text;
                    app.WaitForElement(x => x.Text(message));
                    Core.TakeScreenShot(app, "User_Not_Authorised", testname);
                    app.Tap(x => x.Text(Core.OR["Ok"]));
                }
                catch
                {
                    app.WaitForElement(x => x.Text("You do not have enough airtime. Please select a cheaper bundle or buy airtime first."), timeout: TimeSpan.FromSeconds(25));
                    Core.TakeScreenShot(app, "Out Of Airtime", testname);
                    app.Tap(x => x.Text(Core.OR["Ok"]));
                }
            }

            SurveyPopUp(app,testname);
        }

        public static void ProceedBtn(IApp app, string testname)
        {
           
                app.WaitForElement(x => x.Marked(Core.OR["Proceed"]), timeout: TimeSpan.FromSeconds(30));
                app.Tap(x => x.Marked(Core.OR["Proceed"]));
           
        }

        public static void QuickButton(IApp app, String testname)
        {
            app.WaitForElement(x => x.Marked(Core.OR["QuickbuyButton"]), timeout: TimeSpan.FromSeconds(30));

            app.Tap(x => x.Marked(Core.OR["QuickbuyButton"]));
            Core.TakeScreenShot(app, "Tap - Quick buy Button", testname);
        }

        public static void ScreenMove(IApp app, String testname, String str)
        {
            String appScreen = "Screenshot " + Properties.Resources.env + Properties.Resources.apkVersion + " ";
            String machineName = System.Environment.MachineName;
            String networkShareLocation = @"\\10.100.6.111\d$\Screen\";

            if (Properties.Resources.Exectype == "Local")
            {
                try
                {
                    MoveFiles(".", networkShareLocation + appScreen + machineName, testname, str);
                }

                catch
                {
                    MoveFiles(".", ".\\" + appScreen, testname, str);
                }

            }
            else
            {
                app.Screenshot("Last Screenshot");
                //app.Screenshot(testname + " Execution environment is : " + " " + VodaiOS.Properties.Resources.env + " And APK version is :" + VodaiOS.Properties.Resources.apkVersion);
            }
        }

        #region MenuItems
        public static void OTPBypass(IApp app, string testname)
        {
            nbr = 1;
            Thread.Sleep(1000);
          
                app.WaitForElement(x => x.Text("Welcome to My Vodacom"), timeout: TimeSpan.FromSeconds(30));

            
            app.WaitForElement(x => x.Marked(Core.OR["PleaseEnterNumberLabel"]));

            app.WaitForElement(x => x.Text(Core.OR["Cellphonenumber"]));
            app.Tap(x => x.Marked(Core.OR["CellphoneNumberTextBox"]));

            app.DismissKeyboard();

            // Bypass OTP authentication step
            app.EnterText(x => x.Marked(Core.OR["CellphoneNumberTextBox"]), dictProfiles["otpbypassnum"]);
            app.WaitForElement(x => x.Marked(Core.OR["ProceedButton"]));
            app.Tap(x => x.Marked(Core.OR["ProceedButton"]));
        }

        public static void SoftwareUpdate(IApp app, string testname)
        {
            try
            {
                app.WaitForElement(x => x.Text(Core.OR["Updateavailable"]), timeout: TimeSpan.FromSeconds(25));
                Core.TakeScreenShot(app, "Remind me later", testname);
                app.Tap(x => x.Text(Core.OR["Remindmelater"]));
            }
            catch
            {
                Thread.Sleep(5000);
            }
        }

        public static void StandardRates(IApp app, string testname)
        {
            try
            {
                app.WaitForElement(x => x.Text(Core.OR[Core.OR["Pleasenote"]]), timeout: TimeSpan.FromSeconds(20));

                app.Tap(x => x.Text(Core.OR["Cancel"]));
                Core.TakeScreenShot(app, "Tap - Cancel", testname);
            }
            catch
            {
                Thread.Sleep(5000);
                Core.TakeScreenShot(app, "StandardRates pop up not appearing", testname);
            }
        }

        public static void deleteDirectory()
        {
            if (Properties.Resources.env.Equals("LIVE PROD"))
            {
                // dictProfiles.Remove(@".\Envtestdata\LIVE PROD.txt");

                File.Delete(@".\Envtestdata\LIVE PROD.txt");
                //  Directory.Delete(@".\Envtestdata");
            }
            else if (Properties.Resources.env.Equals("PRODA External"))
            {
                Directory.Delete(@".\Envtestdata\PRODA External.txt");
            }
            else if (Properties.Resources.env.Equals("UAT External"))
            {
                Directory.Delete(@".\Envtestdata\UAT External.txt");
            }
        }

        #region Elements
        public static void buyBundles(IApp app, string testname)
        {
            app.WaitForElement(x => x.Text(Core.OR["Buybundles"]), timeout: TimeSpan.FromSeconds(30));
            Core.TakeScreenShot(app, "Buy bundles", testname);
            app.Tap(x => x.Text(Core.OR["Buybundles"]));
            Core.TakeScreenShot(app, "Buy bundles", testname);
            app.WaitForElement(x => x.Text(Core.OR["Buybundles"]));
            Core.TakeScreenShot(app, "Buy bundles", testname);
            app.WaitForElement(x => x.Marked(Core.OR["Backbtn"]), timeout: TimeSpan.FromSeconds(30));
            app.Tap(x => x.Marked(Core.OR["Backbtn"]));

            Core.Menu(app, testname);
        }

        public static void detailBalances(IApp app, string testname)
        {
            //DATA BALANCES
            app.WaitForElement(x => x.Text(Core.OR["DetailBal"]), timeout: TimeSpan.FromSeconds(30));
            Core.TakeScreenShot(app, "Detail balances", testname);
            app.WaitForElement(x => x.Text(Core.OR["DetailBal"]), timeout: TimeSpan.FromSeconds(30));
            app.Tap(x => x.Text(Core.OR["DataBal"]));

            app.WaitForElement(x => x.Text(Core.OR["DataBal"]), timeout: TimeSpan.FromSeconds(30));
            Core.TakeScreenShot(app, "Data balances", testname);

            app.WaitForElement(x => x.Marked(Core.OR["Backbtn"]), timeout: TimeSpan.FromSeconds(30));
            app.Tap(x => x.Marked(Core.OR["Backbtn"]));
            Core.TakeScreenShot(app, "Tap - Back Button Icon", testname);

            //VOICE BALANCES
            VodaiOS.Core.Menu(app, testname);
            app.WaitForElement(x => x.Text(Core.OR["VoiceBal"]), timeout: TimeSpan.FromSeconds(30));
            Core.TakeScreenShot(app, "Veri - Voice balances", testname);
            app.Tap(x => x.Text(Core.OR["VoiceBal"]));
            Core.TakeScreenShot(app, "Tap - Voice balances", testname);
            app.WaitForElement(x => x.Text(Core.OR["VoiceBal"]), timeout: TimeSpan.FromSeconds(30));
            Core.TakeScreenShot(app, "Veri - Voice balances", testname);
            app.WaitForElement(x => x.Marked(Core.OR["Backbtn"]), timeout: TimeSpan.FromSeconds(30));
            app.Tap(x => x.Marked(Core.OR["Backbtn"]));

            Core.Menu(app, testname);

            app.WaitForElement(x => x.Text(Core.OR["SMSBal"]), timeout: TimeSpan.FromSeconds(30));
            Core.TakeScreenShot(app, "Veri - SMS balances", testname);
            app.Tap(x => x.Text(Core.OR["SMSBal"]));
            Core.TakeScreenShot(app, "Tap - SMS balances", testname);
            app.WaitForElement(x => x.Text(Core.OR["SMSBal"]), timeout: TimeSpan.FromSeconds(30));
            Core.TakeScreenShot(app, "Veri - SMS balances", testname);
            app.WaitForElement(x => x.Marked(Core.OR["Backbtn"]), timeout: TimeSpan.FromSeconds(30));
            app.Tap(x => x.Marked(Core.OR["Backbtn"]));

            Core.Menu(app, testname);

            app.WaitForElement(x => x.Text(Core.OR["MMSBal"]), timeout: TimeSpan.FromSeconds(30));
            Core.TakeScreenShot(app, "Veri - MMS balances", testname);
            app.Tap(x => x.Text(Core.OR["MMSBal"]));
            Core.TakeScreenShot(app, "Tap - MMS balances", testname);
            app.WaitForElement(x => x.Text(Core.OR["MMSBal"]), timeout: TimeSpan.FromSeconds(30));
            Core.TakeScreenShot(app, "Veri - MMS balances", testname);
            app.WaitForElement(x => x.Marked(Core.OR["Backbtn"]), timeout: TimeSpan.FromSeconds(30));
            app.Tap(x => x.Marked(Core.OR["Backbtn"]));
        }

        public static void myBill(IApp app, string testname)
        {
            VodaiOS.Core.Menu(app, testname);
            app.WaitForElement(x => x.Text(Core.OR["Mybill"]));
            Core.TakeScreenShot(app, "Veri - My bill", testname);
            app.Tap(x => x.Text(Core.OR["Mybill"]));
            Core.TakeScreenShot(app, "Tap - My bill", testname);
            app.WaitForElement(x => x.Text(Core.OR["Mybill"]));
            Core.TakeScreenShot(app, "Veri - My bill", testname);
            app.WaitForElement(x => x.Marked(Core.OR["Backbtn"]), timeout: TimeSpan.FromSeconds(30));
            app.Tap(x => x.Marked(Core.OR["Backbtn"]));
            Core.TakeScreenShot(app, "Tap - Back Button Icon", testname);
        }

        //public static void myOffer(IApp app, string testname)
        //{
        //    Core.Menu(app,testname);
        //    app.WaitForElement(x => x.Text(Core.OR["Just4You"]));
        //    app.Tap(x => x.Marked(Core.OR["Just4You"]));
        //    Core.TakeScreenShot(app,"Veri - Just 4 You", testname);
        //    app.Tap(x => x.Marked(Core.OR["Backbtn"]));
        //}

        //public static void just4You(IApp app, string testname)
        //{
        //    Core.Menu(app, testname);
        //    app.WaitForElement(x => x.Text(Core.OR["Just4You"]));
        //    Core.TakeScreenShot(app, "Veri - Just 4 You", testname);
        //    app.Tap(x => x.Text(Core.OR["Just4You"]));
        //    Core.TakeScreenShot(app, "Tap - Just 4 You", testname);
     
        //    app.WaitForElement(x => x.Text(Core.OR["Just4You"]), timeout: TimeSpan.FromSeconds(30));
        //    Core.TakeScreenShot(app, "Veri - Just 4 You", testname);
        //    app.WaitForElement(x => x.Text(Core.OR["Selectbundle"]));
        //    Core.TakeScreenShot(app, "Veri - Select a bundle", testname);

        //    app.WaitForElement(x => x.Marked(Core.OR["Backbtn"]), timeout: TimeSpan.FromSeconds(30));
        //    app.Tap(x => x.Marked(Core.OR["Backbtn"]));
        //    Core.TakeScreenShot(app, "Tap - Back Button Icon", testname);
        //}

        //public static void shopping(IApp app, string testname)
        //{
        //    Core.Menu(app,testname);
        //    app.WaitForElement(x => x.Text(Core.OR["Shopping"]), timeout: TimeSpan.FromSeconds(30));
        //    Core.TakeScreenShot(app,"Veri - Shopping", testname);
        //    app.Tap(x => x.Text(Core.OR["Shopping"]));

        //    app.WaitForElement(x => x.Class(Core.OR["FormsImageView"]), timeout: TimeSpan.FromSeconds(30));
        //    app.WaitForElement(x => x.Text(Core.OR["Shopping"]), timeout: TimeSpan.FromSeconds(30));
        //    Core.TakeScreenShot(app,"Shopping screen is dispalying", testname);
        //    app.WaitForElement(x => x.Marked(Core.OR["Backbtn"]), timeout: TimeSpan.FromSeconds(30));
        //    app.Tap(x => x.Marked(Core.OR["Backbtn"]));
        //    //  app.Screenshot("Tap view with class: FormsImageView with marked: Back button");
        //}

        //public static void services(IApp app, string testname)
        //{
        //    Core.Menu(app,testname);
        //    //Free SMS
        //    app.ScrollDownTo(x => x.Text(Core.OR["Services"]));
        //    app.WaitForElement(x => x.Text(Core.OR["Services"]), timeout: TimeSpan.FromSeconds(30));
        //    Core.TakeScreenShot(app,"Veri - Services", testname);
        //    //app.ScrollTo(x => x.Text(Core.OR["SendFreeSMS"]));
        //    app.WaitForElement(x => x.Text(Core.OR["SendFreeSMS"]), timeout: TimeSpan.FromSeconds(30));
        //    Core.TakeScreenShot(app,"Veri - Send Free SMS", testname);
        //    app.Tap(x => x.Text(Core.OR["SendFreeSMS"]));
        //    Core.TakeScreenShot(app,"Tap - Send Free SMS", testname);
        //    //app.WaitForNoElement();
        //    //app.WaitForElement(x => x.Text("Free SMS's left for today"), timeout: TimeSpan.FromSeconds(30));
        //    app.WaitForElement(x => x.Marked(Core.OR["CellphoneNumberTextBox"]), timeout: TimeSpan.FromSeconds(30));
        //    Core.TakeScreenShot(app,"Wait - CellphoneNumberTextBox", testname);
        //    app.Tap(x => x.Marked(Core.OR["CellphoneNumberTextBox"]));
        //    Core.TakeScreenShot(app,"Tap - CellphoneNumberTextBox", testname);
        //    Core.TakeScreenShot(app,"Send Free SMS screen is displyaing", testname);
        //    app.WaitForElement(x => x.Marked(Core.OR["Backbtn"]), timeout: TimeSpan.FromSeconds(30));
        //    app.Tap(x => x.Marked(Core.OR["Backbtn"]));

        //    //RECHARGE
        //    Core.Menu(app,testname);
        //  //  app.ScrollTo(x => x.Text(Core.OR["Recharge"]));
        //    app.WaitForElement(x => x.Text(Core.OR["Recharge"]), timeout: TimeSpan.FromSeconds(30));
        //    Core.TakeScreenShot(app,"Veri - Recharge", testname);
        //    app.Tap(x => x.Text(Core.OR["Recharge"]));
        //    Core.TakeScreenShot(app,"Tap - Recharge", testname);
        //    app.WaitForElement(x => x.Marked(Core.OR["CellTextBox"]), timeout: TimeSpan.FromSeconds(30));
        //    Core.TakeScreenShot(app,"Wait - CellTextBox", testname);
        //    app.Tap(x => x.Marked(Core.OR["CellTextBox"]));
        //    Core.TakeScreenShot(app,"Tap - CellTextBox", testname);
        //    Core.TakeScreenShot(app,"Recharge screen is dispaying", testname);
        //    app.WaitForElement(x => x.Marked(Core.OR["Backbtn"]), timeout: TimeSpan.FromSeconds(30));
        //    app.Tap(x => x.Marked(Core.OR["Backbtn"]));
        //    //app.Screenshot("Tap view with class: FormsImageView with marked: Back button");

        //    //PURCHASE HISTORY
        //    Core.Menu(app,testname);
        //  //  app.ScrollTo(x => x.Text(Core.OR["PurchaseHistory"]));
        //    app.WaitForElement(x => x.Text(Core.OR["PurchaseHistory"]), timeout: TimeSpan.FromSeconds(30));
        //    Core.TakeScreenShot(app,"Veri - Purchase History", testname);
        //    app.Tap(x => x.Text(Core.OR["PurchaseHistory"]));
        //    Core.TakeScreenShot(app,"Tap - Purchase History", testname);
        //    //app.WaitForElement(x => x.Text(Core.OR["PurchaseHistory"]), timeout: TimeSpan.FromSeconds(30));
        //    app.WaitForElement(x => x.Text("Order number"), timeout: TimeSpan.FromSeconds(30));
        //    Core.TakeScreenShot(app,"Purchase History screen is dispaying", testname);
        //    app.WaitForElement(x => x.Marked(Core.OR["Backbtn"]), timeout: TimeSpan.FromSeconds(30));
        //    app.Tap(x => x.Marked(Core.OR["Backbtn"]));

    

        //    //MANAGE SERVICES
        //    Core.Menu(app,testname);
        //  //  app.ScrollTo(x => x.Text(Core.OR["Manageservices"]), timeout: TimeSpan.FromSeconds(30));
        //    app.WaitForElement(x => x.Text(Core.OR["Manageservices"]));
        //    Core.TakeScreenShot(app,"Veri - Manage services", testname);
        //    app.Tap(x => x.Text(Core.OR["Manageservices"]));
        //    Core.TakeScreenShot(app,"Tap - Manage services", testname);
        //    app.WaitForElement(x => x.Text(Core.OR["Manageservices"]), timeout: TimeSpan.FromSeconds(30));
        //    app.WaitForElement(x => x.Text(Core.OR["Manageservices"]), timeout: TimeSpan.FromSeconds(30));
        //    Core.TakeScreenShot(app,"Wait - Manage services", testname);
        //    app.WaitForElement(x => x.Marked(Core.OR["Backbtn"]), timeout: TimeSpan.FromSeconds(30));
        //    app.Tap(x => x.Marked(Core.OR["Backbtn"]));

           
        //}

        //public static void RewardsEveryDay(IApp app, string testname)
        //{
        //    Core.Menu(app,testname);
        //    app.ScrollDownTo(x => x.Text(Core.OR["Vodacomrewards"]));
        //    app.WaitForElement(x => x.Text(Core.OR["Vodacomrewards"]));
        //    app.Tap(Core.OR["Vodacomrewards"]);
        //    Core.TakeScreenShot(app,"Vodacom rewards Tap", testname);

        //    app.Tap(x => x.Marked(Core.OR["Backbtn"]));
        //    Core.Menu(app,testname);
        //    app.WaitForElement(x => x.Text(Core.OR["Partnerrewards"]));
        //    app.Tap(Core.OR["Partnerrewards"]);
        //    Core.TakeScreenShot(app,"Partner rewards Tap", testname);
        //    Core.StandardRates(testname);
        //    //app.Tap(x => x.Marked(Core.OR["Backbtn"]));
        //}

        public static void competitionandPromotion(IApp app, string testname)
        {
            Core.Menu(app, testname);
           // app.ScrollTo(x => x.Text(Core.OR["Logout"]));
            app.WaitForElement(x => x.Text(Core.OR["Logout"]), timeout: TimeSpan.FromSeconds(30));

            app.WaitForElement(x => x.Text(Core.OR["Competitionsandpromotions"]), timeout: TimeSpan.FromSeconds(30));
            Core.TakeScreenShot(app, "Veri - Competitions and promotions", testname);
            app.WaitForElement(x => x.Text(Core.OR["PlayEveryDay"]), timeout: TimeSpan.FromSeconds(30));
            Core.TakeScreenShot(app, "Veri - Play Every Day", testname);
            app.Tap(x => x.Text(Core.OR["PlayEveryDay"]));
            Core.TakeScreenShot(app, "Tap - Play Every Day", testname);
            app.WaitForElement(x => x.Text(Core.OR["PlayEveryDay"]), timeout: TimeSpan.FromSeconds(30));
            Core.TakeScreenShot(app, "Play Every Day screen is dispalying", testname);

            app.WaitForElement(x => x.Marked(Core.OR["Backbtn"]), timeout: TimeSpan.FromSeconds(30)); app.Tap(x => x.Marked(Core.OR["Backbtn"]));
  
        }

        public static void myProfile(IApp app, string testname)
        {
            Core.Menu(app, testname);
           // app.ScrollTo(x => x.Text(Core.OR["Logout"]));
            app.WaitForElement(x => x.Text(Core.OR["Logout"]), timeout: TimeSpan.FromSeconds(30));
            app.WaitForElement(x => x.Text(Core.OR["MyProfile"]), timeout: TimeSpan.FromSeconds(30));
            Core.TakeScreenShot(app, "Veri - My Profile", testname);
            app.TouchAndHold(x => x.Text(Core.OR["MyProfile"]));
            // app.Screenshot("Long press: FormsTextView with text: My Profile");
            app.WaitForElement(x => x.Text(Core.OR["AccDetails"]), timeout: TimeSpan.FromSeconds(30));
            Core.TakeScreenShot(app, "Veri - Account details", testname);
            app.Tap(x => x.Text(Core.OR["AccDetails"]));
            Core.TakeScreenShot(app, "Tap - Account details", testname);
            app.WaitForElement(x => x.Text(Core.OR["AccDetails"]), timeout: TimeSpan.FromSeconds(30));
            Core.TakeScreenShot(app, "Account Details Screen is dispalying", testname);
            app.WaitForElement(x => x.Marked(Core.OR["Backbtn"]), timeout: TimeSpan.FromSeconds(30));
            app.Tap(x => x.Marked(Core.OR["Backbtn"]));
            // app.Screenshot("Tap view with class: FormsImageView with marked: Back button");
            Core.Menu(app, testname);
           // app.ScrollTo(x => x.Text(Core.OR["Logout"]));
            app.WaitForElement(x => x.Text(Core.OR["Logout"]), timeout: TimeSpan.FromSeconds(30));
            app.WaitForElement(x => x.Text(Core.OR["Settings"]), timeout: TimeSpan.FromSeconds(30));
            Core.TakeScreenShot(app, "Veri - Settings", testname);
            app.Tap(x => x.Text(Core.OR["Settings"]));
            Core.TakeScreenShot(app, "Tap - Settings", testname);

            app.WaitForElement(x => x.Text(Core.OR["Settings"]));
            app.WaitForElement(x => x.Text(Core.OR["PrivacyPolicy"]));
            Core.TakeScreenShot(app, "Setting screen is displaying", testname);
            app.Tap(x => x.Marked(Core.OR["Backbtn"]));
        }

        public static void help(IApp app, string testname)
        {
            //HELP ME
            Core.Menu(app, testname);
         //   app.ScrollTo(x => x.Text(Core.OR["Logout"]));
            app.WaitForElement(x => x.Text(Core.OR["Logout"]), timeout: TimeSpan.FromSeconds(30));
            app.WaitForElement(x => x.Text(Core.OR["help"]), timeout: TimeSpan.FromSeconds(30));
            Core.TakeScreenShot(app, "Veri - Help", testname);
            app.WaitForElement(x => x.Text(Core.OR["HelpMe"]), timeout: TimeSpan.FromSeconds(30));
            Core.TakeScreenShot(app, "Veri - Help Me", testname);
            app.Tap(x => x.Text(Core.OR["HelpMe"]));
            Core.TakeScreenShot(app, "Help me screen is displaying", testname);
            app.WaitForElement(x => x.Marked(Core.OR["Backbtn"]), timeout: TimeSpan.FromSeconds(30));
            app.Tap(x => x.Marked(Core.OR["Backbtn"]));

            //CONTACT US
            Core.Menu( app, testname);
         //   app.ScrollTo(x => x.Text(Core.OR["Logout"]));
            app.WaitForElement(x => x.Text(Core.OR["Logout"]), timeout: TimeSpan.FromSeconds(30));
            app.WaitForElement(x => x.Text(Core.OR["ContactUs"]), timeout: TimeSpan.FromSeconds(30));
            Core.TakeScreenShot(app, "Veri - Contact Us", testname);
            app.Tap(x => x.Text(Core.OR["ContactUs"]));
            Core.TakeScreenShot(app, "Tap - Contact Us", testname);
            app.WaitForElement(x => x.Text(Core.OR["Clickchat"]), timeout: TimeSpan.FromSeconds(30));
            Core.TakeScreenShot(app, "Contact us screen is displaying", testname);
            app.WaitForElement(x => x.Marked(Core.OR["Backbtn"]), timeout: TimeSpan.FromSeconds(30));
            app.Tap(x => x.Marked(Core.OR["Backbtn"]));

            //COVERAGE MAPS
            Core.Menu(app, testname);
          //  app.ScrollTo(x => x.Text(Core.OR["Logout"]));
            app.WaitForElement(x => x.Text(Core.OR["Logout"]), timeout: TimeSpan.FromSeconds(30));
            app.WaitForElement(x => x.Text(Core.OR["Coveragemaps"]), timeout: TimeSpan.FromSeconds(30));
            Core.TakeScreenShot(app, "Wait - Coverage maps", testname);
            app.Tap(x => x.Text(Core.OR["Coveragemaps"]));
            app.WaitForElement(x => x.Text(Core.OR["Coveragemaps"]));
            Core.TakeScreenShot(app, "Tap - Coverage maps", testname);

            app.Tap(x => x.Marked(Core.OR["Backbtn"]));

            Core.Menu( app, testname);
          //  app.ScrollTo(x => x.Text(Core.OR["Logout"]));
            app.WaitForElement(x => x.Text(Core.OR["Logout"]), timeout: TimeSpan.FromSeconds(30));
         
        }
#endregion elements

        //New Core functions
        public static void Expand(IApp app, string option, string testname)
        {
            app.ScrollTo(option);
            app.WaitForElement(x => x.Text(option), timeout: TimeSpan.FromSeconds(30));
            app.Tap(option);

            Thread.Sleep(3000);
            Core.TakeScreenShot(app, option, testname);
        }

        public static void myMessages(IApp app, string testname)
        {
            Core.Menu(app, testname);
            app.WaitForElement(x => x.Text(Core.OR["Mymessages"]), timeout: TimeSpan.FromSeconds(30));
            Core.TakeScreenShot(app, "Veri - My messages", testname);
            app.Tap(x => x.Text(Core.OR["Mymessages"]));
            app.WaitForElement(x => x.Class(Core.OR["FormsImageView"]), timeout: TimeSpan.FromSeconds(30));
            app.WaitForElement(x => x.Text(Core.OR["Mymessages"]), timeout: TimeSpan.FromSeconds(30));
            Core.TakeScreenShot(app, "My messages screen is dispalying ", testname);
            app.WaitForElement(x => x.Marked(Core.OR["Backbtn"]), timeout: TimeSpan.FromSeconds(30));
            app.Tap(x => x.Marked(Core.OR["Backbtn"]));
        }

        #region datePicker
        public static void datePicker(IApp app, int option, string testname)
        {
            try
            {

                app.WaitForElement(x => x.Class("android.widget.DatePicker"), timeout: TimeSpan.FromSeconds(30));
                app.WaitForElement(x => x.Class("android.widget.NumberPicker").Index(0), timeout: TimeSpan.FromSeconds(30));
                app.WaitForElement(x => x.Class("android.widget.NumberPicker").Index(1), timeout: TimeSpan.FromSeconds(30));
                app.WaitForElement(x => x.Class("android.widget.NumberPicker").Index(2), timeout: TimeSpan.FromSeconds(30));
                Core.TakeScreenShot(app, "Veri - date picker", testname);

                app.WaitForElement(x => x.Class("android.widget.Button").Index(0), timeout: TimeSpan.FromSeconds(30));
                for (int i = 1; i <= option; i++)
                {
                    app.TapCoordinates(128, 502);
                }
                app.Tap(x => x.Text(Core.OR["Done"]));
            }
            catch
            {
                app.WaitForElement(x => x.Marked("date_picker_header"), timeout: TimeSpan.FromSeconds(30));
                app.WaitForElement(x => x.Marked("month_view"), timeout: TimeSpan.FromSeconds(30));

                app.WaitForElement(x => x.Marked("prev"), timeout: TimeSpan.FromSeconds(30));
                for (int i = 1; i <= option; i++)
                {
                    app.Tap(x => x.Marked("prev"));
                }

                app.WaitForElement(x => x.Index(14));
                app.Tap(x => x.Index(14));
                app.WaitForElement(x => x.Marked("button1").Text("OK"));
                app.Tap(x => x.Marked("button1").Text("OK"));
                Thread.Sleep(5000);
            }
        }
        #endregion
        #endregion cool

        public static void TakeScreenShot(IApp app, String name, String testname)
        {
            if (Properties.Resources.Exectype == "Local")
            {
                string str = "Scr-" + nbr + "-" + testname + "-" + name + ".png";
                app.Screenshot(name).MoveTo(@".\" + str);

                nbr += 01;
                ScreenMove(app, testname, str);
            }
            else if (Properties.Resources.Exectype == "Cloud")
            {
                app.Screenshot(name);
            }
        }

        public static void feedbackPromt(IApp app, string testname)
        {
            //Survay pop up for 9.3 and 9.4 on once-off data balances screen
            if (app.Query(x => x.Text("Feedback")).Any())
            {
                Core.SurveyPopUp(app, testname);
                if (!app.Query(x => x.Text(Core.OR["Onceoffbalances"])).Any())
                {
                    app.Tap(x => x.Marked(Core.OR["Backbtn"]));
                    TakeScreenShot(app, "Tapped on - Back button", testname);
                }
                if (app.Query(x => x.Text("Buy more")).Any())
                {
                    app.Tap(x => x.Marked(Core.OR["Backbtn"]));
                    TakeScreenShot(app, "Tapped on - Back button", testname);
                }
            }
        }

        #region move file
        public static void MoveFiles(string fromPath, string toPath, string className, String str)
        {
            string[] files = System.IO.Directory.GetFiles(fromPath, str);//"*.png"
            string currentDateFolder = DateTime.Now.ToLongDateString();

            string tmStamp = DateTime.Now.ToString("hh-mm-ss tt");//For whole Date and Time, change to DateTime.Now.ToString("yyyyMMddHHmmssfff")
                                                                  //  string fname = "Scr"
                                                                  //    string toPath1 = Directory.CreateDirectory(toPath + "\\" + fname);
            DirectoryInfo dirInfo = null;
            if (!Directory.Exists(toPath))
            {
                dirInfo = Directory.CreateDirectory(toPath);
                // You can use this info if you need it  
            }
            else
            {
                dirInfo = new DirectoryInfo(toPath);
            }

            if (dirInfo != null)
            {
                dirInfo = dirInfo.CreateSubdirectory(className);//+ " " + tmStamp
            }

            var newPath = dirInfo.FullName;
            foreach (var file in files)
            {
                File.Move(file, newPath + "\\" + Path.GetFileName(file));
            }
        }
        #endregion file move
    }
}