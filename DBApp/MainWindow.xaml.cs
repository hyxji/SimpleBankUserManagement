// Izhary Pauline Rodriguez Fortun
// 21486144
// Prac: Thursday 8 AM - 10 AM

using System;
using System.Drawing;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using BusinessTier;
using CommonContracts;

namespace DBApp
{
    public partial class MainWindow : Window
    {
        private BusinessServerInterface foob;
        private delegate SearchResult SearchDelegate(string query);

        public MainWindow()
        {
            InitializeComponent();

            var tcp = new NetTcpBinding();
            var URL = "net.tcp://localhost:8101/BusinessService"; 
            var chanFactory = new ChannelFactory<BusinessServerInterface>(tcp, URL);
            foob = chanFactory.CreateChannel();

            NoItems.Content = "Total Entries: " + foob.GetNumEntries();
            LoadData(0);
            IndexBox.Text = "0";
        }

        private void IndexButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(IndexBox.Text, out var index))
            {
                LoadData(index);
            }
            else
            {
                MessageBox.Show($"\"{IndexBox.Text}\" is not a valid integer...");
            }
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var searchQuery = SearchBox.Text;
            var searchDelegate = new SearchDelegate(SearchDB);
            var callback = new AsyncCallback(OnSearchCompletion);

            SetWaitingState(true);

            searchDelegate.BeginInvoke(searchQuery, callback, null);
        }

        private SearchResult SearchDB(string query)
        {
            uint accNo = 0;
            uint pin = 0;
            int balance = 0;
            string firstName = null;
            string lastName = null;
            Bitmap icon = null;

            try
            {
                if (IsChannelFaulted())
                {
                    CreateNewChannel();
                }
                foob.GetValuesForSearch(query, out accNo, out pin, out balance, out firstName, out lastName, out icon);
                return new SearchResult
                {
                    AccountNumber = (int)accNo,
                    Pin = (int)pin,
                    Balance = balance,
                    FirstName = firstName,
                    LastName = lastName,
                    Icon = icon
                };
            }
            catch (FaultException<SearchFault> fault)
            {
                MessageBox.Show($"Search fault: {fault.Detail.Issue}");
            }
            catch (FaultException<Exception> fault)
            {
                MessageBox.Show($"FaultException: {fault.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex.Message}");
            }
            return null;
        }

        private bool IsChannelFaulted()
        {
            var channel = foob as ICommunicationObject;
            return channel != null && channel.State == CommunicationState.Faulted;
        }

        private void CreateNewChannel()
        {
            var tcp = new NetTcpBinding();
            var URL = "net.tcp://localhost:8101/BusinessService";
            var chanFactory = new ChannelFactory<BusinessServerInterface>(tcp, URL);
            foob = chanFactory.CreateChannel();
        }


        private void OnSearchCompletion(IAsyncResult result)
        {
            var asyncResult = (AsyncResult)result;
            var search = (SearchDelegate)asyncResult.AsyncDelegate;
            var searchResult = search.EndInvoke(asyncResult);

            Dispatcher.Invoke(() =>
            {
                if (searchResult != null)
                {
                    UpdateGui(searchResult);
                }

                SetWaitingState(false);
            });
        }

        private void LoadData(int index)
        {
            try
            {
                foob.GetValuesForEntry(index, out var accNo, out var pin, out var bal, out var fName, out var lName, out var icon);
                FirstName.Text = fName;
                LastName.Text = lName;
                Balance.Text = bal.ToString("C");
                AcctNo.Text = accNo.ToString();
                Pin.Text = pin.ToString("D4");

                UserIcon.Source = Imaging.CreateBitmapSourceFromHBitmap(icon.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                icon.Dispose();
            }
            catch (FaultException<IndexOutOfRangeFault> exception)
            {
                MessageBox.Show(exception.Detail.Issue);
            }
        }
        private void UpdateGui(SearchResult result)
        {
            FirstName.Text = result.FirstName;
            LastName.Text = result.LastName;
            Balance.Text = result.Balance.ToString("C");
            AcctNo.Text = result.AccountNumber.ToString();
            Pin.Text = result.Pin.ToString();

            if (UserIcon.Source != null)
            {
                UserIcon.Source = null;
            }

            if (result.Icon != null)
            {
                UserIcon.Source = Imaging.CreateBitmapSourceFromHBitmap(result.Icon.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                result.Icon.Dispose();
            }
        }

        private void SetWaitingState(bool isWaiting)
        {
            progressBar.Dispatcher.Invoke(() =>
            {
                progressBar.Visibility = isWaiting ? Visibility.Visible : Visibility.Collapsed;
            });
        }
        public class SearchResult
        {
            public int AccountNumber { get; set; }
            public int Pin { get; set; }
            public decimal Balance { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public System.Drawing.Bitmap Icon { get; set; }
        }

    }
}
