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
    public partial class GroupCommand : IExternalCommand
    {
        
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Get document
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            IList<Reference> listReference = uidoc.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element);
            List<Element> listDuct = new List<Element>();
            foreach(Reference re in listReference)
            {
                listDuct.Add(doc.GetElement(re.ElementId));
            }
            List<Element> listDuct1 = new List<Element>();
            foreach (Reference re in listReference)
            {
                listDuct1.Add(doc.GetElement(re.ElementId));
            }

            using (Transaction trans = new Transaction(doc, "Group"))
            {
                if (EvaConnector(listDuct)==true)
                {                
                    trans.Start();
                    //try
                    //{
                        Group(doc, uidoc, listDuct);
                        trans.Commit();
                    //}
                    //catch (Exception e)
                    //{
                    //    trans.RollBack();
                    //}
                }
                else
                {
                    trans.Start();
                    GroupOppo(doc, uidoc, listDuct);
                    trans.Commit();
                }
            }
            return Result.Succeeded;
        }
    }
}
