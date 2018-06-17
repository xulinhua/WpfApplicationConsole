using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Modbus.Device;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Xml;

namespace WpfApplicationConsole
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string text;
        private string text1;

        public string Text1
        {
            get
            {
                return text1;
            }

            set
            {
                if (text1 != value)
                {
                    text1 = value;
                    RaisePropertyChanged("Text1");
                }
            }
        }
        private void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(@"F:\testFile.txt", "asdf少吃点");
            Console.WriteLine("OK,文件1已成功写入");

        }

        private void btnFileTest_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllLines(@"F:\testFile.txt", new string[] { "aa", "bb", "cc" });
            Console.WriteLine("OK,文件2已成功写入");
        }

        private void btnFileTest2_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllBytes(@"F:\testFile.txt", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            Console.WriteLine("OK,文件3已成功写入");
            File.AppendAllText(@"F:\testFile.txt", "asddfdfd");
            Console.WriteLine("OK,文件4已成功写入");
            text = File.ReadAllText(@"F:\testFile.txt", Encoding.Default);
            byte[] abc = Encoding.Default.GetBytes(text);
            foreach (var item in abc)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine(text);
            Console.WriteLine("OK,已完成读取！");
        }

        private void lblText_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void btnFileCopy_Click(object sender, RoutedEventArgs e)
        {
            string sourcePath = @"F:\10、多态之虚方法.avi";
            string targetPath = @"F:\10、多态之虚方法new.avi";
            FileCopy(sourcePath, targetPath);
            Console.WriteLine("视频复制成功！");
        }
        private void FileCopy(string source, string target)
        {
            using (FileStream ReadStream = new FileStream(source, FileMode.Open, FileAccess.Read))
            {
                using (FileStream WriteStream = new FileStream(target, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    while (true)
                    {
                        byte[] copyText = new byte[1024 * 1024 * 5];
                        int length = ReadStream.Read(copyText, 0, copyText.Length);
                        if (length == 0)
                        {
                            break;
                        }
                        WriteStream.Write(copyText, 0, length);
                    }
                }

            }
            Text1 = File.ReadAllText(@"F:\testFile.txt", Encoding.Default);
            Console.WriteLine("OK,已完成读取！");
        }

        private void Multi_Click(object sender, RoutedEventArgs e)
        {
            //原始方式
            Chinese ch = new Chinese("张三");
            Janpanese ja = new Janpanese("井下");
            Korean ko = new Korean("崔柜里");
            Person[] persons = { ch, ja, ko };

            foreach (var item in persons)
            {
                if (item is Chinese)
                {
                    ((Chinese)item).SayHello();
                }
                else if (item is Janpanese)
                {
                    ((Janpanese)item).SayHello();
                }
                else
                {
                    ((Korean)item).SayHello();
                }
            }
            //虚方法
            Chinese1 ch1 = new Chinese1("张三1");
            Janpanese1 ja1 = new Janpanese1("井下1");
            Korean1 ko1 = new Korean1("崔柜里1");
            Person1[] persons1 = { ch1, ja1, ko1 };
            foreach (var item in persons1)
            {
                item.SayHello();
            }
            //抽象类
            Animal dg = new Dog();
            Animal ct = new Cat();
            dg.Bark();
            ct.Bark();

        }

        private void btnShape_Click(object sender, RoutedEventArgs e)
        {
            Shape sp = new Circle(5.0);
            //sp = new Rectangle(5,6);
            double area = sp.GetArea();
            double perimeters = sp.GetPerimeter();
            Console.WriteLine("面积为 {0:f3}，周长为 {1:f3}", area, perimeters);
            text1 = "123456";

        }
        //序列化:将对象二值化，便于信息交互
        private void btnSerialize_Click(object sender, RoutedEventArgs e)
        {
            PersonTest p = new PersonTest();
            p.Name = "张三";
            p.Sex = "male";
            p.Age = 25;
            using (FileStream fsWrite = new FileStream(@"F:\Serialize.txt", FileMode.OpenOrCreate, FileAccess.Write))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fsWrite, p);
            }
            Console.WriteLine("序列化成功！");

            //反序列化
            PersonTest pt;
            using (FileStream fsRead = new FileStream(@"F:\Serialize.txt", FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                pt = (PersonTest)bf.Deserialize(fsRead);
            }
            Console.WriteLine("{0},{1},{2}", pt.Name, pt.Sex, pt.Age);
        }

        private void btnMD5_Click(object sender, RoutedEventArgs e)
        {
            MD5 jiami = MD5.Create();
            string md = "123";
            Text1 = null;
            byte[] buffer = Encoding.Default.GetBytes(md);

            byte[] md5 = jiami.ComputeHash(buffer);

            for (int i = 0; i < md5.Length; i++)
            {
                Text1 += md5[i].ToString("X2");
            }
            //Thickness tn = new Thickness(300, 165, 0, 0);
            //btnMD5.Margin = tn;


        }
        /// <summary>
        /// 创建XML文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnXML_Click(object sender, RoutedEventArgs e)
        {
            //通过代码创建XML文件
            //1.引用命名空间
            //2.创建XML文件对象
            XmlDocument doc = new XmlDocument();
            //3.创建第一个行描述信息，并添加到doc文档中
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(dec);
            //4.创建根节点，并添加到doc文档中
            XmlElement books = doc.CreateElement("Books");
            doc.AppendChild(books);

            //5.给根节点创建子节点，并添加到根节点
            XmlElement book1 = doc.CreateElement("Book");
            books.AppendChild(book1);
            //6.给book1添加子节点
            XmlElement name1 = doc.CreateElement("Name");
            name1.InnerText = "金瓶梅";
            book1.AppendChild(name1);
            XmlElement price1 = doc.CreateElement("Price");
            price1.InnerText = "100";
            book1.AppendChild(price1);

            XmlElement book2 = doc.CreateElement("Book");
            books.AppendChild(book2);
            XmlElement name2 = doc.CreateElement("Name");
            name2.InnerText = "九尾龟";
            book2.AppendChild(name2);
            XmlElement price2 = doc.CreateElement("Price");
            price2.InnerText = "120";
            book2.AppendChild(price2);

            //添加带属性标签
            XmlElement book3 = doc.CreateElement("Book");
            books.AppendChild(book3);
            XmlElement item1 = doc.CreateElement("Item");
            item1.SetAttribute("Name","数学");
            item1.SetAttribute("Price","130");
            book3.AppendChild(item1);
            XmlElement item2 = doc.CreateElement("Item");
            item2.SetAttribute("Name", "语文");
            item2.SetAttribute("Price", "280");
            book3.AppendChild(item2);

            //直接添加标签
            XmlElement item3 = doc.CreateElement("Item3");
            item3.InnerXml = ("<Name>英语</Name>");
            book3.AppendChild(item3);
            //item3.InnerXml = ("<Price>250</Price>");
            //book3.AppendChild(item3); //添加两个时以后一个为主


            //保存该XML文档
            doc.Save(@"XML文件.xml");
            Console.WriteLine("保存成功！");
        }
        /// <summary>
        /// 追加XML内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnXML_Append_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement books;
            if (File.Exists(@"XML文件1.xml"))
            {
                doc.Load(@"XML文件1.xml");
                books = doc.DocumentElement;
                doc.AppendChild(books);

                XmlElement book1 = doc.CreateElement("Book");
                books.AppendChild(book1);
                XmlElement name1 = doc.CreateElement("Name");
                name1.InnerText = "C++";
                book1.AppendChild(name1);
                XmlElement price1 = doc.CreateElement("Price");
                price1.InnerText = "10";
                book1.AppendChild(price1);
            }
            else
            {
                XmlDeclaration dec = doc.CreateXmlDeclaration("1.0","utf-8",null);
                doc.AppendChild(dec);
                books = doc.CreateElement("Books");
                doc.AppendChild(books);

                XmlElement book1 = doc.CreateElement("Book");
                books.AppendChild(book1);
                XmlElement name1 = doc.CreateElement("Name");
                name1.InnerText = "C++";
                book1.AppendChild(name1);
                XmlElement price1 = doc.CreateElement("Price");
                price1.InnerText = "10";
                book1.AppendChild(price1);

            }
            doc.Save(@"XML文件1.xml");
            Console.WriteLine("追加成功！");
        }

        private void btnXML_Read_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("XML文件.xml");
            XmlElement books = doc.DocumentElement;
            XmlNodeList xnl = books.ChildNodes;
            foreach (XmlNode item in xnl)
            {
                Console.WriteLine(item.InnerText);
            }
        }
        /// <summary>
        /// 读取带属性的标签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnXML_ReadAttributes_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("XML文件.xml");
            XmlNodeList xnl = doc.SelectNodes("/Books/Book1/Item");
            foreach (XmlNode node in xnl)
            {
                Console.WriteLine(node.Attributes["Name"].Value);
                Console.WriteLine(node.Attributes["Price"].Value);
            }

        }
    }

    //多态的三种方式：虚方法 抽象类 接口
    #region 原始方式
    public class Person
    {
        private string name;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">姓名</param>
        public Person(string name)
        {
            this.name = name;
        }
        /// <summary>
        /// 打招呼函数
        /// </summary>
        public void SayHello()
        {
            Console.WriteLine("我是人类");
        }
        //属性
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }
    }
    public class Chinese : Person
    {
        public Chinese(string name) : base(name)
        {
            this.Name = name;
        }
        public void SayHello()
        {
            Console.WriteLine("我是中国人，我叫{0}", this.Name);
        }
    }
    public class Janpanese : Person
    {
        public Janpanese(string name) : base(name)
        {
            this.Name = name;
        }
        public void SayHello()
        {
            Console.WriteLine("我是日本人，我叫{0}", this.Name);
        }
    }
    public class Korean : Person
    {
        public Korean(string name) : base(name)
        {
            this.Name = name;
        }
        public void SayHello()
        {

            Console.WriteLine("我是韩国人，我叫{0}", this.Name);
        }


    }
    #endregion

    #region 虚方法
    public class Person1
    {
        private string name;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">姓名</param>
        public Person1(string name)
        {
            this.name = name;
        }
        /// <summary>
        /// 打招呼函数
        /// </summary>
        public virtual void SayHello()
        {
            Console.WriteLine("我是人类");
        }
        //属性
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }
    }
    public class Chinese1 : Person1
    {
        public Chinese1(string name) : base(name)
        {
            this.Name = name;
        }
        public override void SayHello()
        {
            Console.WriteLine("我是中国人，我叫{0}", this.Name);
        }
    }
    public class Janpanese1 : Person1
    {
        public Janpanese1(string name) : base(name)
        {
            this.Name = name;
        }
        public override void SayHello()
        {
            Console.WriteLine("我是日本人，我叫{0}", this.Name);
        }
    }
    public class Korean1 : Person1
    {
        public Korean1(string name) : base(name)
        {
            this.Name = name;
        }
        public override void SayHello()
        {

            Console.WriteLine("我是韩国人，我叫{0}", this.Name);
        }


    }
    #endregion

    #region 抽象类
    //抽象类
    public abstract class Animal
    {
        public abstract string Name
        {
            get;
            set;
        }

        public abstract void Bark();
    }
    //子类
    public class Dog : Animal
    {
        public override string Name
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void Bark()
        {
            Console.WriteLine("狗叫");
        }
    }
    public class Cat : Animal
    {
        public override string Name
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void Bark()
        {
            Console.WriteLine("猫叫");
        }
    }
    #endregion

    #region 抽象类练习
    public abstract class Shape
    {
        public abstract double GetArea();
        public abstract double GetPerimeter();
    }
    public class Circle : Shape
    {
        private double radius;

        public double Radius
        {
            get
            {
                return radius;
            }

            set
            {
                radius = value;
            }
        }
        public Circle(double radius)
        {
            this.radius = radius;
        }
        public override double GetArea()
        {
            return Math.PI * this.radius * this.radius;
        }

        public override double GetPerimeter()
        {
            return Math.PI * this.radius * 2;
        }
    }
    public class Rectangle : Shape
    {
        private double height;
        private double width;

        public double Height
        {
            get
            {
                return height;
            }

            set
            {
                height = value;
            }
        }

        public double Width
        {
            get
            {
                return width;
            }

            set
            {
                width = value;
            }
        }


        public Rectangle(double height, double width)
        {
            this.height = height;
            this.width = width;
        }
        public override double GetArea()
        {
            return this.width * this.height;
        }

        public override double GetPerimeter()
        {
            return 2 * (this.height + this.width);
        }
    }
    #endregion

    #region 序列化
    [Serializable]
    public class PersonTest
    {
        private string name;
        private string sex;
        private int age;
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Sex
        {
            get
            {
                return sex;
            }

            set
            {
                sex = value;
            }
        }

        public int Age
        {
            get
            {
                return age;
            }

            set
            {
                age = value;
            }
        }
    }
    #endregion
}
