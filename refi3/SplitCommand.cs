using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;


namespace refi3
{
    [TransactionAttribute(TransactionMode.Manual)]
    public partial class SplitCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
            
            // Get document
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            // Tham chiếu đến elemnt đã chọn
            Reference refelement = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);
            // Get element
            Element element = doc.GetElement(refelement.ElementId);
            using (Transaction trans = new Transaction(doc, "Split"))
            {
                trans.Start();
                TaskDialog taskDialog = new TaskDialog("Split Side");
                taskDialog.MainContent = "Chia từ điểm xuất phát";
                taskDialog.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No;
                if(taskDialog.Show() == TaskDialogResult.Yes)
                {
                    SplitFunc.SplitFromOrigin(doc, uidoc, refelement);
                    trans.Commit();
                }
                else
                {
                    SplitFunc.SplitOpposite(doc, uidoc, element);
                    trans.Commit();
                }
            }
            return Result.Succeeded;
        }
    }
}
