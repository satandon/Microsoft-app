using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Win32;
using Windows.Storage;
using Windows.UI.Notifications;

namespace Microsoft_app
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
                private String imageFilePath = String.Empty;

		private void Application_Startup(object sender, StartupEventArgs e)
		{

           //ButtonSendToast_Click("dunning");
            //ButtonSendToast_Click("upgrade");
            ButtonSendToast_Click("adminStopped");


        }

        private void ButtonSendToast_Click(string flag)
        {
            var notification = new ToastContentBuilder();
            var imageUri = Path.GetFullPath(@"Assets\Microsoft.jpg");
            var xboxImageUri = Path.GetFullPath(@"Assets\Xbox.png");

            switch (flag)
            {
                case "dunning":
                    notification.AddText("Xbox Subscription").AddText("Hey, your subscription is about to expire. Please pay immediately to enjoy the benefits!")
                                .AddButton(new ToastButton()
                                .SetContent("Pay now")
                                .AddArgument("action", "pay")
                                .SetBackgroundActivation())
                                .AddAppLogoOverride(new Uri(xboxImageUri));
                    break;

                case "upgrade":
                    notification.AddText("M365 Subscription").AddText("Congratulations, your subscription is upgraded to Microsoft 365 family!")
                                .AddButton(new ToastButton()
                                .SetContent("View")
                                .AddArgument("action", "view")
                                .SetBackgroundActivation())
                                .AddAppLogoOverride(new Uri(imageUri));
                    break;

                case "adminStopped":
                    notification.AddText("M365 Subscription").AddText("Hey, Your sharing subscription is about to expire. Please check with admin")
                                .AddButton(new ToastButton()
                                .SetContent("View")
                                .AddArgument("action", "view")
                                .SetBackgroundActivation())
                                .AddAppLogoOverride(new Uri(imageUri));
                    break;
            }

            notification.Show();
        }

        public async Task ReadFromRegistryAndFindDiff()
        {
            while (true)
            {
                JObject targetJObject = new JObject();
                try
                {
                    // Initial Data
                    var initialData = File.ReadAllText(@"Responses\Data.json");
                    var sourceJObject = JObject.Parse(initialData);

                    // Read from registry
                    try
                    {
                        //opening the subkey  
                        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\OurSettings");

                        //If it does exist, retrieve the stored values  
                        if (key != null)
                        {
                            var registryValue = key.GetValue("Setting1");
                            var convertToStringRegistryValue = Convert.ToString(registryValue);
                            targetJObject = JObject.Parse(convertToStringRegistryValue);
                            key.Close();
                        }
                    }
                    catch (Exception ex)  //just for demonstration...it's always best to handle specific exceptions
                    {
                        //react appropriately
                    }

                    var jsondiffPatch = new JsonDiffPatch();
                    JToken diffResult = jsondiffPatch.Diff(sourceJObject, targetJObject);
                    var formatter = new JsonDeltaFormatter();
                    var operations = formatter.Format(diffResult);

                    if (operations.Count > 0)
                    {
                        // Send Notifications
                        break;
                    }

                    // Wait for 3 seconds
                    Thread.Sleep(3000);
                }
                catch (Exception e)
                {
                }
            }
        }
    }
}
