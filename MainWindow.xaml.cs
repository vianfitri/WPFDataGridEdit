using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace WPFDataGridEdit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public class cPerson
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public bool IsRelated { get; set; }
            public string Gender { get; set; }
        }

        private ObservableCollection<cPerson> _People = new ObservableCollection<cPerson>();
        public ObservableCollection<cPerson> People
        {
            get { return _People; }
            set
            {
                if (!value.Equals(_People))
                {
                    _People = value;
                    NotifyPropertyChanged("People");
                }
            }
        }

        private ObservableCollection<string> _Genders = new ObservableCollection<string>() { "Male", "Female", "Don't know" };

        public ObservableCollection<String> Genders { get { return _Genders; } }

        public MainWindow()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        private void DataGrid_Selected(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(DataGridCell))
            {
                DataGrid g = (DataGrid)sender;
                g.BeginEdit(e);
                DataGridCell Cell = e.OriginalSource as DataGridCell;
                List<TextBox> tbl = FindChildrenByType<TextBox>(Cell);
                if (tbl.Count > 0)
                {
                    tbl[0].Focus();
                    tbl[0].SelectAll();
                }
            }
        }

        public static List<T> FindChildrenByType<T>(DependencyObject depObj) where T : DependencyObject
        {
            List<T> Children = new List<T>();
            if (depObj != null)
            {
                for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(depObj) - 1; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        Children.Add((T)child);
                    }
                    Children.AddRange(FindChildrenByType<T>(child));
                }
            }
            return Children;
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource.GetType() != typeof(DataGridCell))
            {
                People.Add(new cPerson());
            }
        }
    }
}
