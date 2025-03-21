// Izhary Pauline Rodriguez Fortun
// 21486144
// Prac: Thursday 8 AM - 10 AM

using System;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using BusinessTier;
using CommonContracts;

namespace AsyncDBApp
{
    public partial class MainWindow : Window
    {
        private BusinessServerInterface foob;

        public MainWindow()
        {
            InitializeComponent();
            InitializeService();
        }

        private void InitializeService()
        {
            NetTcpBinding tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8101/BusinessService";
            ChannelFactory<BusinessServerInterface> foobFactory = new ChannelFactory<BusinessServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string query = SearchBox.Text;

            if (!IsValidQuery(query))
            {
                MessageBox.Show("Invalid search query. Please enter a valid name.");
                return;
            }

            SetSearchInProgress(true);

            try
            {
                await SearchAndUpdateGuiAsync(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during search: {ex.Message}");
            }
            finally
            {
                SetSearchInProgress(false);
            }
        }
        private bool IsValidQuery(string query)
        {
            return !string.IsNullOrWhiteSpace(query) && query.All(char.IsLetter);
        }
        private async void IndexButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(IndexBox.Text, out var index))
            {
                try
                {
                    SetWaitingState(true);
                    await LoadDataAsync(index);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading data: {ex.Message}");
                }
                finally
                {
                    SetWaitingState(false);
                }
            }
            else
            {
                MessageBox.Show($"\"{IndexBox.Text}\" is not a valid integer.");
            }
        }

        private async Task<SearchResult> SearchDBAsync(string query)
        {
            return await Task.Run(() =>
            {
                uint accNo = 0;
                uint pin = 0;
                int balance = 0;
                string firstName = null;
                string lastName = null;
                Bitmap icon = null;

                try
                {
                    foob.GetValuesForSearch(query, out accNo, out pin, out balance, out firstName, out lastName, out icon);

                    if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
                    {
                        return null;
                    }

                    return new SearchResult
                    {
                        AccountNumber = accNo,
                        Pin = pin,
                        Balance = balance,
                        FirstName = firstName,
                        LastName = lastName,
                        Icon = icon
                    };
                }
                catch (FaultException<SearchFault> fault)
                {
                    MessageBox.Show($"Search fault: {fault.Detail.Issue}");
                    return null;
                }
                catch (FaultException<Exception> fault)
                {
                    MessageBox.Show($"FaultException: {fault.Message}");
                    return null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Exception: {ex.Message}");
                    return null;
                }
            });
        }


        private async Task LoadDataAsync(int index)
        {
            await Task.Run(() =>
            {
                try
                {
                    foob.GetValuesForEntry(index, out var accNo, out var pin, out var bal, out var fName, out var lName, out var icon);
                    Dispatcher.Invoke(() =>
                    {
                        FirstName.Text = fName;
                        LastName.Text = lName;
                        Balance.Text = bal.ToString("C");
                        AcctNo.Text = accNo.ToString();
                        Pin.Text = pin.ToString("D4");

                        if (UserIcon.Source != null)
                        {
                            UserIcon.Source = null;
                        }

                        if (icon != null)
                        {
                            UserIcon.Source = Imaging.CreateBitmapSourceFromHBitmap(icon.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                            icon.Dispose();
                        }
                    });
                }
                catch (FaultException<IndexOutOfRangeFault> exception)
                {
                    Dispatcher.Invoke(() => MessageBox.Show(exception.Detail.Issue));
                }
            });
        }
        private async Task SearchAndUpdateGuiAsync(string query)
        {
            try
            {
                SearchResult result = await SearchDBAsync(query);

                if (result != null)
                {
                    UpdateGui(result);
                }
                else
                {
                    MessageBox.Show("No results found.");
                }
            }
            catch (FaultException<SearchFault> fault)
            {
                MessageBox.Show($"Search fault: {fault.Detail.Issue}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during search: {ex.Message}");
            }
        }

        private void UpdateGui(SearchResult result)
        {
            Dispatcher.Invoke(() =>
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
            });
        }
        private void SetWaitingState(bool isWaiting)
        {
            progressBar.Dispatcher.Invoke(() =>
            {
                progressBar.Visibility = isWaiting ? Visibility.Visible : Visibility.Collapsed;
            });
        }
        private void SetSearchInProgress(bool inProgress)
        {
            Dispatcher.Invoke(() =>
            {
                bool isSearching = inProgress;

                FirstName.IsReadOnly = isSearching;
                LastName.IsReadOnly = isSearching;
                AcctNo.IsReadOnly = isSearching;
                Pin.IsReadOnly = isSearching;
                Balance.IsReadOnly = isSearching;

                SearchButton.IsEnabled = !isSearching;
                IndexButton.IsEnabled = !isSearching;

                progressBar.Visibility = isSearching ? Visibility.Visible : Visibility.Collapsed;

                progressBar.IsIndeterminate = isSearching;
            });
        }

        public class SearchResult
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public decimal Balance { get; set; }
            public uint AccountNumber { get; set; }
            public uint Pin { get; set; }
            public Bitmap Icon { get; set; }
        }

    }
}
