using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Microsoft.Win32;
using NetworkService.ViewModel;

namespace NetworkService.Views
{
    /// <summary>
    /// Interaction logic for NetworkEntitiesView.xaml
    /// </summary>
    public partial class NetworkEntitiesView : UserControl
    {
        public NetworkEntitiesView()
        {
            InitializeComponent();
        }

        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select Profile Image";
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.gif;*.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog.InitialDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Resources\Images"));

            bool? response = openFileDialog.ShowDialog();

            if (response == true)
            {
                string selectedImageFilePath = openFileDialog.FileName;
                (this.DataContext as NetworkEntitiesViewModel)?.SetImage(selectedImageFilePath);
                PictureDisplay.Source = new BitmapImage(new Uri(selectedImageFilePath));
            }
        }

        TextBox focusedTextBox;
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // KeyboardGrid.Visibility = Visibility.Visible;
            focusedTextBox = GetFocusedTextBox();
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
           // if (!preventFocusLoss && !IdTextBox.IsFocused && !NameTextBox.IsFocused && !SearchTextBox.IsFocused)
           // {
           //     KeyboardGrid.Visibility = Visibility.Collapsed;
           // }
        }

        private bool preventFocusLoss = false;
        private void KeyboardButton_Click(object sender, RoutedEventArgs e)
        {
            // Add the content of the clicked button to the focused text box
            if (sender is Button button)
            {
                if (focusedTextBox != null)
                {
                    if (button.Content.ToString().Equals("SPACE"))
                        button.Content = " ";
                    else if(button.Content.ToString().Equals("DELETE"))
                        Console.WriteLine("");
                    else if(button.Content.ToString().Equals("ENTER"))
                        KeyboardGrid.Visibility = Visibility.Collapsed;

                    // Append content to the text box
                    focusedTextBox.Text += button.Content.ToString();

                    // Set focus back to the text box
                    focusedTextBox.Focus();
                }
            }
        }

        private TextBox GetFocusedTextBox()
        {
            // Helper method to get the currently focused text box
            if (IdTextBox.IsFocused)
                return IdTextBox;
            else if (NameTextBox.IsFocused)
                return NameTextBox;
            else if (SearchTextBox.IsFocused)
                return SearchTextBox; // Assuming you don't want to add letters to a combo box
            else
                return null;
        }
    }
}
