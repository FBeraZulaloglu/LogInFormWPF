using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
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

namespace GroundStationWeek1
{
    /// <summary>
    /// 1) XML CS VE APP.xml in mantığı anlatılacak
    /// 2) XML DEKİ WPF bileşenleri ve grid (column/row) stack panel margin padding horizontal,vertical alignment dan bahsedilecek
    /// 3) BAŞVURULARDAN KÜTÜPHANE EKLEME ÖRNEĞİ YAP
    /// 4) ÖDEV VE KAYNAK PAYLAŞIMI
    /// </summary>
    public partial class MainWindow : Window
    {
        //List<Student> students = new List<Student>();
        public ObservableCollection<Student> students { get; set; }
        Student newStudent;
        ImageSource selectedIcon;
        int numberOfPre;
        int numberOf1st;
        int numberOf2st;
        int numberOf3st;
        int numberOf4st;
        int numberOfAfter;
        StringBuilder studentsCSV;

        public MainWindow()
        {
            InitializeComponent();
            studentsCSV = new StringBuilder();
            students = new ObservableCollection<Student>();
            students.CollectionChanged += this.StudentHasAdded;
            
        }

        private void kayıtYap_Click(object sender, RoutedEventArgs e)
        {
            // öğreniclerin kayıt olmasını sağlayan fonksiyon
            newStudent = new Student();

            if(adSoyad.Text.Equals("")) 
            {
                MessageBox.Show("Ad alanı boş bırakılamaz");
            }
            else
            {
                newStudent.name = adSoyad.Text;
                try
                {

                    newStudent.number = int.Parse(no.Text); // sonrasında eğer no 5 hane değilse message box çıkar 
                    ComboBoxItem item = (ComboBoxItem)classes.SelectedItem;
                    if (item != null)
                    {
                        newStudent.ilgiAlanı = ilgiAlanı.Text;
                        newStudent.studentClass = item.Content.ToString();
                        if(selectedIcon != null)
                        {
                            newStudent.iconSource = selectedIcon;
                        }
                        MessageBox.Show("Kaydın başarılı.");
                        addStudentToList(newStudent);
                        updateStudentNumbers(newStudent);// öğrencileri sınıflarına göre ayırır.
                        adSoyad.Text = "";
                        no.Text = "";
                        ilgiAlanı.Text = "";
                        classes.Text = "Sınıflar";
                    }
                    else
                    {
                        MessageBox.Show("Sınıf seçimi zorunludur.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    MessageBox.Show("öğrenci No yu düzgün giriniz.");
                }

            }

        }

        private void addStudentToList(Student s)
        {
            // öğrencileri listeye ve tabloya kaydeder ardından scroll barı aşağı indirir.
            students.Add(s);
            foreach (var item in students)
            {
                Console.WriteLine(item.name);
            }
            studentsCSV.AppendLine(s.name + "," + s.number + "," + s.ilgiAlanı);

        }

        private void updateStudentNumbers(Student newStudent)
        {
            if (newStudent.studentClass.Equals("Hazırlık"))
            {
                // hazırlıktaki öğrenci textbox sayısı değiştir.
                numberOfPre++;
                hazirlik.Text = numberOfPre + " öğrenci";
            }
            else if (newStudent.studentClass.Equals("1. sınıf"))
            {
                numberOf1st++;
                sinif1.Text = numberOf1st + " öğrenci";
            }
            else if (newStudent.studentClass.Equals("2. sınıf"))
            {
                numberOf2st++;
                sinif2.Text = numberOf2st + " öğrenci";
            }
            else if (newStudent.studentClass.Equals("3. sınıf"))
            {
                numberOf3st++;
                sinif3.Text = numberOf3st + " öğrenci";
            }
            else if (newStudent.studentClass.Equals("4. sınıf"))
            {
                numberOf4st++;
                sinif4.Text = numberOf4st + " öğrenci";
            }
            else
            {
                numberOfAfter++;
                mezun.Text = numberOfAfter + " öğrenci"; 
            }
        }

       

        private void StudentHasAdded(object sender, NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<Student> obsSender = sender as ObservableCollection<Student>;

            Dispatcher.BeginInvoke((Action)(() =>
            {

                //telemetry tablosunu yenile
                studentTable.Items.Refresh();
                // tablodaki scroll barı en aşşağı götür
                if (obsSender != null)
                    studentTable.ItemsSource = obsSender;

                var border = VisualTreeHelper.GetChild(studentTable, 0) as Decorator;
                if (border != null)
                {
                    var scroll = border.Child as ScrollViewer;
                    if (scroll != null) scroll.ScrollToEnd();
                }

            }));
        }

        private void writeToCSV_Click(object sender, RoutedEventArgs e)
        {
            
           File.WriteAllText("Ogrenciler.csv",studentsCSV.ToString());
            
        }


        private void iconComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // change the image preview
            ComboBoxItem cbi = (ComboBoxItem)iconComboBox.SelectedItem;
            StackPanel st = (StackPanel)cbi.Content;

            Image childname = null;
            foreach (object child in st.Children)
            {
                if (child is Image)
                {
                    childname = (child as Image);
                }

            }
            Console.WriteLine(childname);
            if(childname == null)
            {
                MessageBox.Show("Resmi tekrar seçiniz.");
            }
            else
            {
                imagePreview.Source = childname.Source;
            }
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(imagePreview.Source != null)
            {

                selectedIcon = imagePreview.Source;
                MessageBox.Show("ikonunuz seçildi");
            }
        }

        private void ProgramClosed(object sender, EventArgs e)
        {
            // programı kapatırken csv yi sıfırla
        }
    }
}
