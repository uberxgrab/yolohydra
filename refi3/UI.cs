using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.IO;

namespace refi3
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class UI : IExternalApplication
    {
        public static TextBox item3;
        public static SplitButton comboBoxFam;
        public static SplitButtonData comboBoxDataFam;
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
        public Result OnStartup(UIControlledApplication application)
        {
            // Create panel
            RibbonPanel panel = application.CreateRibbonPanel("yolo");
            //Create textbox
            TextBoxData itemData3 = new TextBoxData("itemName3");
            item3 = panel.AddItem(itemData3) as TextBox;
            item3.Width = 50;
            item3.Value = "1000";
            item3.ToolTip = itemData3.Name;
            item3.ShowImageAsButton = true;
            //Create button split
            string path = Assembly.GetExecutingAssembly().Location;
            PushButtonData buttonData = new PushButtonData("btn00", "Split", path, "refi3.SplitCommand");
            panel.AddItem(buttonData);
            //Create button splitsys
            PushButtonData buttonDatasys = new PushButtonData("btn01", "SplitSYS", path, "refi3.SplitSystem");
            PushButton btn01 = panel.AddItem(buttonDatasys) as PushButton;
            System.Windows.Media.Imaging.BitmapImage imagesys = new System.Windows.Media.Imaging.BitmapImage();
            Uri urisys = new Uri(@"D:\must_have_icon_set\Cut\Cut_16x16.png"); 
            btn01.LargeImage = imagesys;
            //Create button group
            PushButtonData buttonDatagr = new PushButtonData("btn02", "Group", path, "refi3.GroupCommand");
            panel.AddItem(buttonDatagr);
            //Create button changetype
            PushButtonData buttonDataChange = new PushButtonData("btn03", "Change", path, "refi3.CommandChange");
            panel.AddItem(buttonDataChange);
            //Create button Test
            PushButtonData buttonDataTest = new PushButtonData("btn04", "Test", path, "refi3.Test");
            panel.AddItem(buttonDataTest);
            //Create ComboboxFam
            comboBoxDataFam = new SplitButtonData("Family", "Family");
            comboBoxFam = panel.AddItem(comboBoxDataFam) as SplitButton;
            DirectoryInfo d = new DirectoryInfo(@"D:\Family Duct Fitting");
            DirectoryInfo[] listDirect= d.GetDirectories();
            foreach (DirectoryInfo folder in listDirect)
            {
                PushButtonData combo = new PushButtonData(folder.Name, folder.Name, path, "refi3.CommandChange");
                comboBoxFam.AddPushButton(combo);
            }
            return Result.Succeeded;
        }
    }
}
