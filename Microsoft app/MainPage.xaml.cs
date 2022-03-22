using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Notifications;
using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Microsoft_app
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class MainPage : Application
    {
        public MainPage()
        {

            var a = new ToastContentBuilder()
    .AddArgument("conversationId", 9813)

    .AddText("Some text")

    .AddButton(new ToastButton()
        .SetContent("Archive")
        .AddArgument("action", "archive")
        .SetBackgroundActivation());

            a.SetBackgroundActivation();
            a.Show();

            //ButtonSendToast_Click(null, null);
        }

        private void Application_Startup(object sender, System.Windows.StartupEventArgs e)
        {
            // Create the startup window
            MainWindow wnd = new MainWindow();
            // Do stuff here, e.g. to the window
            wnd.Title = "Something else";
            // Show the window
            wnd.Show();
        }

        private void ButtonSendToast_Click(object sender, RoutedEventArgs e)
        {
            // In a real app, these would be initialized with actual data
            string title = "Windows Setting Notification";
            string content = "Congratulations, your subscription is upgraded to Microsoft 365 family!";
            string image = "https://www.bing.com/images/search?q=Microsoft%20Logo&FORM=IQFRBA&id=E8DB3B45CA8AF3D466642D11006811E44F4CD260";
            string logo = "ms-appdata:///local/Square44x44Logo.scale-200.png";
            int conversationId = 384928;

            // Construct the visuals of the toast
            ToastVisual visual = new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = title
                        },

                        new AdaptiveText()
                        {
                            Text = content
                        },

                        new AdaptiveImage()
                        {
                            Source = image
                        }
                    },

                    AppLogoOverride = new ToastGenericAppLogo()
                    {
                        Source = logo,
                        HintCrop = ToastGenericAppLogoCrop.Circle
                    }
                }
            };

            // Construct the actions for the toast (inputs and buttons)
            ToastActionsCustom actions = new ToastActionsCustom()
            {

                Buttons =
                {
                    new ToastButton("Reply", new QueryString()
                    {
                        { "action", "reply" },
                        { "conversationId", conversationId.ToString() }

                    }.ToString())
                    {
                        ActivationType = ToastActivationType.Background,
                        ImageUri = "Assets/Reply.png",

                        // Reference the text box's ID in order to
                        // place this button next to the text box
                        TextBoxId = "tbReply"
                    },

                    new ToastButton("Like", new QueryString()
                    {
                        { "action", "like" },
                        { "conversationId", conversationId.ToString() }

                    }.ToString())
                    {
                        ActivationType = ToastActivationType.Background
                    },

                    new ToastButton("View", new QueryString()
                    {
                        { "action", "viewImage" },
                        { "imageUrl", image }

                    }.ToString())
                }
            };


            // Now we can construct the final toast content
            ToastContent toastContent = new ToastContent()
            {
                Visual = visual,
                Actions = actions,

                // Arguments when the user taps body of toast
                Launch = new QueryString()
                {
                    { "action", "viewConversation" },
                    { "conversationId", conversationId.ToString() }

                }.ToString()
            };


            // And create the toast notification
            ToastNotification notification = new ToastNotification(toastContent.GetXml());


            // And then send the toast
            ToastNotificationManager.CreateToastNotifier().Show(notification);
        }
    }
}
