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
    class SplitSystem : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Get document
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            IList<Reference> listre = uidoc.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element);
            List<Element> liste = new List<Element>();
            foreach(Reference re in listre)
            {
                Element element = doc.GetElement(re.ElementId); 
                liste.Add(element);
            }
            using (Transaction trans = new Transaction(doc, "SplitSystem"))
            {
                TaskDialog taskDialog = new TaskDialog("Split Side");
                taskDialog.MainContent = "Chia từ điểm xuất phát";
                taskDialog.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No;
                if (taskDialog.Show() == TaskDialogResult.Yes)
                {
                    foreach (Reference refelement in listre)
                    {
                        trans.Start();
                        SplitFunc.SplitFromOrigin(doc, uidoc, refelement);
                        trans.Commit();
                    }
                }
                else
                {
                    foreach (Element element in liste)
                    {
                        trans.Start();
                        SplitFunc.SplitOpposite(doc, uidoc, element);
                        trans.Commit();
                    }
                }
            }
            return Result.Succeeded;
        }
    }
}
