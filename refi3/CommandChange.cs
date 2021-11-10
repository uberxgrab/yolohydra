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
    public partial class CommandChange : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get document
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            //ref
            IList<Reference> listReference = uidoc.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element);
            foreach(Reference reference in listReference)
            {
                Change(doc, uidoc, reference);
            }
            //Reference reference = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);
            //Getconnectorconect(doc, uidoc, reference);
            return Result.Succeeded;
        }
    }
}
