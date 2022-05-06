using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;

namespace McDonald_v2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            McDonaldInterface mcDonald = new McDonaldInterface();
            mcDonald.Start();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

    public class McDonaldInterface
    {
        public McDonaldInterface()
        {

        }
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Google Sheets API .NET Quickstart";
        static SheetsService service = null;
        public void Start()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("./credential.json", FileMode.Open, FileAccess.ReadWrite))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Debug.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Sheets API service.
            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define request parameters.
            String spreadsheetId = "12p5CYuxOtGLpxwFdDU5ZQ0fx389mgPTrfc8zUtOhAZk";
            String range = "A1:B";
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);

            // Prints the names and majors of students in a sample spreadsheet:
            // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
            


            UserHandler uH = new UserHandler(request);
        }
    }


    public class User
    {
        string log;
        float id;
        bool isServed;


        public User(string nLog, float nId)
        {
            log = nLog;
            id = nId;
            isServed = false;
        }

        public void UserServed()
        {
            isServed = true;
        }

        public string GetLog()
        {
            return log;
        }

        public bool GetStatus()
        {
            return isServed;
        }

        public float GetId()
        {
            return id;
        }

    }

    public class UserHandler
    {
        List<User> userList = new List<User>();
        float id;
         
        public UserHandler(SpreadsheetsResource.ValuesResource.GetRequest request)
        {
            id = 1;

            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;

            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    // Print columns A and E, which correspond to indices 0 and 4.
                    Debug.WriteLine(row[0] + ", " + row[1]);

                    User nUser = new User(row[0].ToString(), id);
                    id++;

                    userList.Add(nUser);
                }
            }
            else
            {
                Debug.WriteLine("No data found.");
            }
        }

        public int getLastId()
        {

            return -1;
        }


    }
}
